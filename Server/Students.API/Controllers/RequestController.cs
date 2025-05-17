using Microsoft.AspNetCore.Mvc;
using Students.Application.Services.Interfaces;
using Students.Infrastructure.DTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;
using Students.Models.ReferenceModels;

namespace Students.API.Controllers;

/// <summary>
/// Контроллер заявок.
/// </summary>
[ApiController]
[Route("[controller]")]
[ApiVersion("1.0")]
public class RequestController : GenericAPiController<Request>
{
    #region Поля и свойства

    private readonly ILogger<Request> _logger;
    private readonly IRequestRepository _requestRepository;
    private readonly IRequestService _requestService;

    #endregion

    #region Методы

    /// <summary>
    /// Получение заявки по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор заявки.</param>
    /// <returns>Заявка.</returns>
    public override async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var form = await this._requestRepository.FindById(id);
            if (form is null)
            {
                return this.NotFoundException();
            }

            var requestsDTO = await Mapper.RequestToRequestDTO(form);

            return this.Ok(requestsDTO);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Error while getting Entity by Id");
            return this.Exception();
        }
    }


    /// <summary>
    /// Создание новой заявки с фронта.
    /// </summary>
    /// <param name="form">DTO заявки с данными о потенциальном студенте.</param>
    /// <returns>Новая заявка (попутно создается новый студент, если не был найден).</returns>
    [HttpPost("NewRequest")]
    public async Task<IActionResult> Post([FromBody] NewRequestDTO form)
    {
        try
        {
            await this._requestService.CreateRequestFromClient(form);
            return this.Ok(form);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Error while creating Entity");
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
    [HttpPut("EditRequest/{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] RequestsDTO form)
    {
        try
        {
            await _requestService.UpdateRequestData(id, form);
            return this.Ok(form);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Error while updating Entity");
            return this.Exception();
        }
    }

    /// <summary>
    /// Создание новой заявки.
    /// </summary>
    /// <param name="request">Заявка.</param>
    /// <returns>Состояние запроса + Заявка.</returns>
    public override async Task<IActionResult> Post(Request request)
    {
        try
        {
            if (request.StudentId is not null)
            {
                var existingStudentRequests =
                    await this._requestRepository.Get(r => r.StudentId == request.StudentId);
                request.IsAlreadyStudied = existingStudentRequests.Any();
            }

            var form = await this._requestRepository.Create(request);

            return this.Ok(form);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Error while creating Entity");
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
            await _requestService.BindRequestToPerson(requestId, personId);
            return this.Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return this.Exception();
        }
    }

    /// <summary>
    /// Добавление приказа.
    /// </summary>
    /// <param name="requestId">Идентификатор заявки.</param>
    /// <param name="order">Приказ.</param>
    /// <returns>Состояние запроса.</returns>
    [HttpPost("AddOrderToRequest")]
    public async Task<IActionResult> AddOrderToRequest(Guid requestId, [FromBody] Order order)
    {
        try
        {
            var request = await this._requestRepository.AddOrderToRequest(requestId, order);
            return request is null ? this.NotFoundException() : this.Ok();
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Error while creating Entity");
            return this.Exception();
        }
    }

    /// <summary>
    /// Список с заявками, которые подавал студент.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <returns>Список заявок.</returns>
    [HttpGet("GetListRequestsOfStudentExists")]
    public async Task<IActionResult> GetListRequestsOfStudentExists(Guid studentId)
    {
        try
        {
            var requests = await this._requestRepository.GetListRequestsOfStudentExists(studentId);
            return requests is null ? this.NotFoundException() : this.Ok(requests);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Error while getting Entities");
            return this.Exception();
        }
    }

    /// <summary>
    /// Список заявок с разделением по страницам.
    /// </summary>
    /// <returns>Состояние запроса + список заявок с разделением по страницам.</returns>
    [HttpGet("paged")]
    public async Task<IActionResult> ListAllPagedDTO([FromQuery] Pageable pageable, RequestFilterDTO filters)
    {
        try
        {
            var items = await this._requestRepository.GetRequestsDTOByPage(pageable.PageNumber, pageable.PageSize,
                filters);
            return this.Ok(items);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Error while getting Entities");
            return this.Exception();
        }
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="repository">Репозиторий заявок.</param>
    /// <param name="logger">Логгер.</param>
    /// <param name="requestRepository">Репозиторий заявок (как будто лучше использовать этот параметр вместо двух???).</param>
    /// <param name="statusRequestRepository">Репозиторий состояний заявок.</param>
    /// <param name="studentRepository">Репозиторий студентов).</param>
    public RequestController(IGenericRepository<Request> repository, ILogger<Request> logger,
        IRequestRepository requestRepository,
        IRequestService requestService) : base(repository, logger)
    {
        this._requestRepository = requestRepository;
        this._requestService = requestService;
        this._logger = logger;
    }

    #endregion
}