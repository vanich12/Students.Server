﻿using Microsoft.AspNetCore.Mvc;
using Students.Application.Services;
using Students.Application.Services.Interfaces;
using Students.Infrastructure.DTO;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiVersion("1.0")]
    public class PendingRequestController : GenericAPiController<PendingRequest>
    {
        private readonly IPendingRequestService _pendingRequestService;
        private readonly ILogger<PendingRequest> _logger;

        [HttpGet("paged")]
        public async Task<IActionResult> GetPendingRequestByPage([FromQuery] Pageable pageable,
            PendingRequestFilterDTO filters)
        {
            try
            {
                var items = await this._pendingRequestService.GetPendingRequestsDTOByPage(pageable.PageNumber,
                    pageable.PageSize, filters);
                return this.Ok(items);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                return this.Exception();
            }
        }


        /// <summary>
        /// Получение заявки по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор заявки.</param>
        /// <returns>Заявка.</returns>
        public override async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var requestsDto = await this._pendingRequestService.FindById(id);
                return this.Ok(requestsDto);
            }
            catch (InvalidOperationException ex)
            {
                this._logger.LogError(ex, "Error while getting Entity by Id");
                return this.Exception();
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "Error while getting Entity by Id");
                return this.Exception();
            }
        }

        [HttpPost("CreateRequestFromPendingRequestAndPerson")]
        public async Task<IActionResult> CreateRequestFromPendingRequestAndPerson(Guid pRequestId, Guid personId)
        {
            try
            {
                var request = await _pendingRequestService.CreateRequestFromPendingRequestAndPerson(pRequestId, personId);
                return this.Ok(request);
            }
            catch (ArgumentException ex)
            {
                this._logger.LogError(ex.Message, "Error while getting Item by Id");
                return this.Exception();
            }
            catch (InvalidOperationException ex)
            {
                this._logger.LogError(ex.Message, "Error while trying Update Person");
                return this.Exception();
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message,
                    $"Error while trying create Request by PendingRequest with pendingRequestId:{pRequestId}, and with personId: {personId} ");
                return this.Exception();
            }
        }
        /// <summary>
        /// Создание отвалидированной заявки на основе "сырой"
        /// </summary>
        /// <param name="pRequestId"></param>
        /// <returns></returns>
        [HttpPost("CreateRequestFromPendingRequest")]
        public async Task<IActionResult> CreateRequestFromPendingRequest(Guid pRequestId)
        {
            try
            {
                var request = await _pendingRequestService.CreateRequestFromPendingRequest(pRequestId);
                return this.Ok(request);
            }
            catch (ArgumentException ex)
            {
                this._logger.LogError(ex.Message, "Error while getting Item by Id");
                return this.Exception();
            }
            catch (InvalidOperationException ex)
            {
                this._logger.LogError(ex.Message, "Error while trying Update Person");
                return this.Exception();
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message,
                    $"Error while trying create Request by PendingRequest with pendingRequestId:{pRequestId} ");
                return this.Exception();
            }
        }

        /// <summary>
        /// Обновить объект.
        /// Пизда, а не мокап, студента выбирать нужно из списка блять
        /// </summary>
        /// <param name="id">Id объекта.</param>
        /// <param name="form">Объект.</param>
        /// <returns>Объект.</returns>
        [HttpPut("EditPendingRequest/{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] RequestsDTO form)
        {
            try
            {
                await _pendingRequestService.UpdatePendingRequestData(id, form);
                return this.Ok(form);
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "Error while updating Entity");
                return this.Exception();
            }
        }

        /// <summary>
        /// Привязка заявкуи к персоне
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        /// Todo: доделать и протестировать логику
        [HttpPut("BindRequestToPerson")]
        public async Task<IActionResult> BindRequestToPerson(Guid requestId, Guid personId)
        {
            try
            {
                await _pendingRequestService.BindRequestToPerson(requestId, personId);
                return this.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return this.Exception();
            }
        }

        public PendingRequestController(IGenericRepository<PendingRequest> repository, ILogger<PendingRequest> logger,
            IPendingRequestService requestService) :
            base(repository, logger)
        {
            this._pendingRequestService = requestService;
            this._logger = logger;
        }
    }
}