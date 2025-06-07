using Microsoft.AspNetCore.Mvc;
using Students.Application.Services.Interfaces;
using Students.Infrastructure.DTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;
using Exception = System.Exception;

namespace Students.API.Controllers;

/// <summary>
/// Контроллер приказов.
/// </summary>
[ApiController]
[Route("[controller]")]
[ApiVersion("1.0")]
public class OrderController : GenericAPiController<Order>
{
    #region Поля и свойства

    private readonly IOrderRepository _orderRepository;
    private readonly IOrderService _orderService;
    private readonly ILogger _logger;

    #endregion

    #region Методы

    [HttpPost("PostDTO")]
    public async Task<IActionResult> PostDTO(OrderDTO form)
    {
        try
        {
            await this._orderService.CreateOrder(form);
            return this.StatusCode(StatusCodes.Status201Created, form);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Error while creating new Entity");
            return this.Exception();
        }
    }
    /// <summary>
    /// Пагинация приказов
    /// </summary>
    /// <param name="pageable"></param>
    /// <returns></returns>
    [HttpGet("paged")]
    public async Task<IActionResult> ListAllPage([FromQuery] Pageable pageable)
    {
        try
        {
            var orders = await this._orderRepository.GetOrdersByPage(pageable.PageNumber, pageable.PageSize);
            return this.Ok(orders);
        }
        catch (Exception e)
        {
            this._logger.LogError(e.Message, "Error while tried get list of orders");
            return this.Exception();
        }
    }

    /// <summary>
    /// Список приказов с информацией о студентах.
    /// </summary>
    /// <returns>Приказы.</returns>
    [HttpGet("GetListOrdersWithStudent")]
    public async Task<IActionResult> GetListOrdersWithStudentAsync()
    {
        return StatusCode(StatusCodes.Status200OK,
            await _orderRepository.GetListOrdersWithStudentAsync());
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="orderКepository">Репозиторий приказов.</param>
    /// <param name="logger">Логгер.</param>
    public OrderController(IOrderRepository orderКepository, ILogger<Order> logger, IOrderService orderService) : base(
        orderКepository, logger)
    {
        this._orderRepository = orderКepository;
        this._orderService = orderService;
        this._logger = logger;
    }

    #endregion
}