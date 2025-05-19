using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Application.Services.Interfaces;
using Students.Infrastructure.DTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Infrastructure.Storages;
using Students.Models;
using Students.Models.ReferenceModels;

namespace Students.Application.Services
{
    public class PendingRequestService(
        IRequestRepository requestRepository,
        IGenericRepository<PendingRequest> repository,
        IStudentRepository studentRepository,
        IGenericRepository<StatusRequest> statusRequestRepository,
        IGenericRepository<EducationProgram> educationProgramRepository,
        IPendingRequestRepository pendingRequestRepository,
        IPersonRepository personRepository) : GenericService<PendingRequest>(repository), IPendingRequestService
    {
        public async Task<PagedPage<RequestsDTO>> GetPendingRequestsDTOByPage(int page, int pageSize)
        {
            return await pendingRequestRepository.GetRequestPendingByPage(page, pageSize);
        }

        public async Task<RequestsDTO?> FindById(Guid id)
        {
            var pendingRequest = await pendingRequestRepository.FindById(id);
            if (pendingRequest is null)
                throw new InvalidOperationException($"Ошибка, по Id: {id} не найдено заявки, ожидающей пожтверждения");

            return await Mapper.PendingRequestToRequestDTO(pendingRequest,
                statusRequestRepository, educationProgramRepository);
        }

        public async Task<PendingRequest> UpdatePendingRequestData(Guid id, RequestsDTO form)
        {
            var educationProgram = await educationProgramRepository.FindById(form.EducationProgramId.Value);

            var pendingReq =
                await Mapper.RequestDTOToPendingRequest(form, statusRequestRepository, educationProgramRepository);

            if (educationProgram is not null)
                pendingReq.Education = educationProgram.Name;

            var newPendingReq = await pendingRequestRepository.Update(id, pendingReq);
            if (newPendingReq is null)
                throw new InvalidOperationException($"Ошибка, не удалось обновить неподтвержденную заявку по Id: {id}");

            return newPendingReq;
        }

        public async Task BindRequestToPerson(Guid requestId, Guid personId)
        {
            var requestOld = await requestRepository.FindById(requestId);
            if (requestOld is null)
                throw new ArgumentException($"заявка по: {requestId} не найдена");

            requestOld.PersonId = personId;

            var newRequest = await requestRepository.Update(requestOld.Id, requestOld);
        }
    }
}