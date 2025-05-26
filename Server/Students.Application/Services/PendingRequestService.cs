using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Application.Services.Interfaces;
using Students.Infrastructure.DTO;
using Students.Infrastructure.DTO.FilterDTO;
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
        IGenericRepository<StatusRequest> statusRequestRepository,
        IGenericRepository<EducationProgram> educationProgramRepository,
        IPersonRepository personRepository,
        IPendingRequestRepository pendingRequestRepository)
        : GenericService<PendingRequest>(repository), IPendingRequestService
    {
        public async Task<PagedPage<RequestsDTO>> GetPendingRequestsDTOByPage(int page, int pageSize,
            PendingRequestFilterDTO filters)
        {
            return await pendingRequestRepository.GetRequestPendingByPage(page, pageSize, filters);
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
                throw new ArgumentException($"по pRequestId : {pRequestId} заявки не найдено");

            var newRequest =
                await Mapper.PendingRequestToRequest(pendingReq, educationProgramRepository, statusRequestRepository);

            newRequest.PersonId = personId;
            // архивируем "сырую" заявку, по хорошему это делать бы после создания уже подтвержденной заявки

            pendingReq.IsArchive = true;
            var updatePendingRequest = await pendingRequestRepository.Patch(pRequestId, pendingReq);
            if (updatePendingRequest is null)
                throw new InvalidOperationException(
                    $"Ошибка при обновлении статуса неподтвержденной заявки по id:{pRequestId}");

            return await requestRepository.Create(newRequest);
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
                await Mapper.RequestDTOToPendingRequest(form);

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