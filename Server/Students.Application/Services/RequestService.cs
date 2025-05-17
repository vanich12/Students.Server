using Students.Application.Exceptions;
using Students.Application.Services.Interfaces;
using Students.Infrastructure.DTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;
using Students.Models.ReferenceModels;

namespace Students.Application.Services
{
    /// <summary>
    /// Сервис обработки заявок
    /// </summary>
    /// <param name="requestRepository"></param>
    /// <param name="studentRepository"></param>
    /// <param name="statusRequestRepository"></param>
    /// <param name="pendingRequestRepository"></param>
    /// <param name="personRepository"></param>
    public class RequestService(
        IRequestRepository requestRepository,
        IStudentRepository studentRepository,
        IGenericRepository<StatusRequest> statusRequestRepository,
        IGenericRepository<EducationProgram> educationProgramRepository,
        IPendingRequestRepository pendingRequestRepository,
        IPersonRepository personRepository)
        : GenericService<Request>(requestRepository), IRequestService
    {
        /// <summary>
        /// Получение заявок, которые есть у студента
        /// </summary>
        /// <param name="studentId">Id студента</param>
        /// <returns></returns>
        /// <exception cref="StudentNotFoundException"></exception>
        public async Task<IEnumerable<RequestsDTO>?> GetListRequestsOfStudentExists(Guid studentId)
        {
            var student = await studentRepository.GetStudentWithInitPerson(studentId);
            if (student is null) throw new StudentNotFoundException(studentId);

            var requestsEntities = await requestRepository.Get(p => p.Id == studentId);
            if (requestsEntities == null || !requestsEntities.Any())
                return Enumerable.Empty<RequestsDTO>();

            return requestsEntities.Select(entity => Mapper.RequestToRequestDTO(entity).Result).ToList();
        }

        public async Task<PagedPage<RequestsDTO>> GetRequestsDTOByPage(int page, int pageSize, RequestFilterDTO filters)
        {
            return await requestRepository.GetRequestsDTOByPage(page, pageSize, filters);
        }

        public async Task<PagedPage<RequestsDTO>> GetPendingRequestsDTOByPage(int page, int pageSize)
        {
            return await pendingRequestRepository.GetRequestPendingByPage(page, pageSize);
        }


        /// <summary>
        /// Создание и привязка отвалидированной заявки на основе "сырой"
        /// </summary>
        /// <param name="pRequestId">Id "сырой заявк"</param>
        /// <param name="personId">Id персоны</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Request> CreateRequestFromPendingRequest(Guid pRequestId, Guid personId)
        {
            var pendingReq = await pendingRequestRepository.FindById(pRequestId);
            if (pendingReq is null)
                throw new ArgumentException($"по pendingRequest : {pRequestId} ничего не найдено");

            var person = await personRepository.FindById(personId);
            person.Family = pendingReq.Family;
            person.Name = pendingReq.Name;
            person.Patron = pendingReq.Patron;
            person.Email = pendingReq.Email;
            person.Phone = pendingReq.Phone;

            var newPerson = await personRepository.Update(personId, person);
            if (newPerson is null)
                throw new InvalidOperationException("Ошибка при обновлении пользователя");

            var newRequest = await Mapper.PendingRequestToRequest(pendingReq, educationProgramRepository, statusRequestRepository);

            newRequest.PersonId = personId;

            return await requestRepository.Create(newRequest);
        }

        public async Task BindRequestToPerson(Guid requestId, Guid personId)
        {
            var requestOld = await requestRepository.FindById(requestId);
            if (requestOld is null)
                throw new ArgumentException($"заявка по: {requestId} не найдена");

            requestOld.PersonId = personId;

            var newRequest = await requestRepository.Update(requestOld.Id, requestOld);
        }

        public async Task<Guid?> AddOrderToRequest(Guid requestId, Order order)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Создание новой заявки с фронта, создание персоны
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task CreateRequestFromClient(NewRequestDTO form)
        {
            try
            {
                var request = await Mapper.NewRequestDTOToRequest(form, statusRequestRepository);
                var fio = $"{form.family} {form.name} {form.patron}";
                var date = form.birthDate;

                // ищем студента полностью совпадающего с данными заявки
                var person = await personRepository.GetOne(x =>
                    x.Name == form.name && x.Family == form.family && x.Patron == form.patron && x.BirthDate == date &&
                    x.Email == form.email && x.Phone == form.phone);

                if (person is null)
                {
                    request.IsAlreadyStudied = false;
                    var matchingStudentsList = await personRepository.Get(x =>
                        x.FullName == fio || x.BirthDate == date || x.Email == form.email || x.Phone == form.phone
                    );
                    // если не нашлась персона с такими данными, то тогда создаем новую
                    if (matchingStudentsList is null || !matchingStudentsList.Any())
                    {
                        var requestStatus =
                            await statusRequestRepository.GetOne(x => x.Name == "Требует создание персоны");
                        request.Status = requestStatus;
                        await requestRepository.Create(request);
                        return;
                    }

                    var requestStatus2 = await statusRequestRepository.GetOne(x => x.Name == "Ожидает подтверждения");
                    request.Status = requestStatus2;
                    await requestRepository.Create(request);
                }
                else
                {
                    // если совпадение точное
                    request.IsAlreadyStudied = true;
                    request.PersonId = person.Id;
                    var statusMatched = await statusRequestRepository.GetOne(x => x.Name == "Новая заявка");
                    request.Status = statusMatched;
                    var newPerson = await Mapper.NewRequestDTOToPerson(form);

                    await personRepository.Update(person.Id, newPerson);
                    await requestRepository.Create(request);
                }
            }
            catch (StudentNotFoundException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task UpdateRequestData(Guid id, RequestsDTO form)
        {
            try
            {
                var resultOld = await requestRepository.FindById(id);
                if (resultOld is null)
                    throw new RequestNotFoundExceptions(id);
                var person = await personRepository.FindById(form.StudentId.Value);

                if (person is not null)
                {
                    var tempNewStudent = await personRepository.GetOne(x => x.Phone == form.phone &&
                                                                            x.Email == form.Email &&
                                                                            x.Family == form.family &&
                                                                            x.Name == form.name! &&
                                                                            x.Patron == form.patron!);
                    // вот этот момент жиденький, надо переделать, пока так
                    if (tempNewStudent is not null && person.Id != tempNewStudent.Id)
                    {
                        //throw new Exception("Попытка задублировать студентов");
                        person = tempNewStudent;
                    }

                    var newPerson = await Mapper.RequestDTOToPerson(form);

                    await personRepository.Update(person.Id, newPerson);
                }
                else
                {
                    person = await personRepository.GetOne(x => x.Phone == form.phone &&
                                                                x.Email == form.Email &&
                                                                x.Family == form.family! &&
                                                                x.Name == form.name! &&
                                                                x.Patron == form.patron!);

                    if (person is null)
                    {
                        person = await Mapper.RequestDTOToPerson(form);

                        person = await personRepository.Create(person);
                        resultOld.StudentId = person.Id;
                    }
                }

                resultOld.StatusRequestId = form!.StatusRequestId;
                resultOld.StatusEntrancExams = form.statusEntrancExams;
                resultOld.Email = form.Email ?? "";
                resultOld.Phone = form.phone ?? "";
                resultOld.Agreement = form.agreement;
                resultOld.EducationProgramId = form.EducationProgramId;

                await requestRepository.Update(id, resultOld);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}