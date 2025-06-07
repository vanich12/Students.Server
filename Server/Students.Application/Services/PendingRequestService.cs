using Microsoft.Extensions.Logging;
using Students.Application.Services.Interfaces;
using Students.Infrastructure.DTO;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;
using Students.Models.ReferenceModels;

namespace Students.Application.Services
{
    public class PendingRequestService(
        IRequestRepository requestRepository,
        IGenericRepository<PendingRequest> repository,
        IGenericRepository<TypeEducation> typeEducationRepository,
        IGenericRepository<StatusRequest> statusRequestRepository,
        IGenericRepository<EducationProgram> educationProgramRepository,
        IPendingRequestRepository pendingRequestRepository,
        IPersonRepository personRepository,
        ILogger<PendingRequest> logger)
        : GenericService<PendingRequest>(repository, logger), IPendingRequestService
    {
        public async Task<PagedPage<RequestsDTO>> GetPendingRequestsDTOByPage(int page, int pageSize,
            PendingRequestFilterDTO filters)
        {
            var items = await pendingRequestRepository.GetRequestPendingByPage(page, pageSize, filters);
            return items;
        }

        /// <summary>
        /// Создание и привязка отвалидированной заявки на основе "сырой" если персона повторно подает заявку
        /// </summary>
        /// <param name="pRequestId">Id "сырой заявк"</param>
        /// <param name="personId">Id персоны</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Request> CreateRequestFromPendingRequestAndPerson(Guid pRequestId, Guid personId)
        {
            try
            {
                var pendingReq = await pendingRequestRepository.FindById(pRequestId);
                if (pendingReq is null)
                    throw new ArgumentException($"по pRequestId : {pRequestId} заявки не найдено");

                var newRequest =
                    await Mapper.PendingRequestToRequest(pendingReq, educationProgramRepository,
                        statusRequestRepository);

                newRequest.PersonId = personId;
                // архивируем "сырую" заявку, по хорошему это делать бы после создания уже подтвержденной заявки

                pendingReq.IsArchive = true;
                var updatePendingRequest = await pendingRequestRepository.Patch(pRequestId, pendingReq);
                if (updatePendingRequest is null)
                    throw new InvalidOperationException(
                        $"Ошибка при обновлении статуса неподтвержденной заявки по id:{pRequestId}");

                return await requestRepository.Create(newRequest);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Создание отвалидированной заявки и персоны на основе "сырой" если, раньше персоной не подавалась заявка
        /// </summary>
        /// <param name="pRequestId"></param>
        /// <returns></returns>
        public async Task<Request> CreateRequestFromPendingRequest(Guid pRequestId)
        {
            try
            {
                var pendingReq = await pendingRequestRepository.FindById(pRequestId);
                if (pendingReq is null)
                    throw new ArgumentException($"по pRequestId : {pRequestId} заявки не найдено");

                var newRequest =
                    await Mapper.PendingRequestToRequest(pendingReq, educationProgramRepository,
                        statusRequestRepository);

                var newPerson = await Mapper.PendingRequestToPerson(pendingReq);

                var person = await personRepository.Create(newPerson);
                newRequest.PersonId = person.Id;

                var request = await requestRepository.Create(newRequest);

                return request;
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }
        /// <summary>
        /// Поиск заявки по id
        /// </summary>
        /// <param name="id">id неподтвержденной заявки</param>
        /// <returns></returns>
        public async Task<RequestsDTO?> FindById(Guid id)
        {
            try
            {
                var pendingRequest = await pendingRequestRepository.FindById(id);
                if (pendingRequest is null)
                    throw new InvalidOperationException(
                        $"Ошибка, по Id: {id} не найдено заявки, ожидающей пожтверждения");

                return await Mapper.PendingRequestToRequestDTO(pendingRequest,
                    educationProgramRepository, typeEducationRepository);
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }
        /// <summary>
        /// Обновление заявки
        /// </summary>
        /// <param name="id">id неподтвержденной заявки</param>
        /// <param name="form"> данные с клиента</param>
        /// <returns></returns>
        public async Task<PendingRequest> UpdatePendingRequestData(Guid id, RequestsDTO form)
        {
            try
            {
                // ToDo убрать это говно
                var pendingReq =
                    await Mapper.RequestDTOToPendingRequest(form);

                var newPendingReq = await pendingRequestRepository.Update(id, pendingReq);
                if (newPendingReq is null)
                    throw new InvalidOperationException(
                        $"Ошибка, не удалось обновить неподтвержденную заявку по Id: {id}");

                return newPendingReq;
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }
        /// <summary>
        /// Привязка заявки к персонге
        /// </summary>
        /// <param name="requestId">id заявки</param>
        /// <param name="personId">id персоны</param>
        /// <returns></returns>
        public async Task BindRequestToPerson(Guid requestId, Guid personId)
        {
            try
            {
                var requestOld = await requestRepository.FindById(requestId);
                if (requestOld is null)
                    throw new ArgumentException($"заявка по: {requestId} не найдена");

                requestOld.PersonId = personId;

                var newRequest = await requestRepository.Update(requestOld.Id, requestOld);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }
    }
}