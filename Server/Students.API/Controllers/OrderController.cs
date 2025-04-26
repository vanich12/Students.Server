﻿using Microsoft.AspNetCore.Mvc;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.API.Controllers;

/// <summary>
/// Контроллер приказов.
/// </summary>
[ApiController]
[Route("[controller]")]
[ApiVersion("1.0")]
public class  OrderController : GenericAPiController<Order>
{
  #region Поля и свойства

  private readonly IOrderRepository _orderRepository;
  private readonly ILogger _logger;

  #endregion

  #region Методы

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
  public OrderController(IOrderRepository orderКepository, ILogger<Order> logger) : base(orderКepository, logger)
  {
    _orderRepository = orderКepository;
    _logger = logger;
  }

  #endregion
}