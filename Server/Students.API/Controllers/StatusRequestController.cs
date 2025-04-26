﻿using Microsoft.AspNetCore.Mvc;
using Students.Infrastructure.Interfaces;
using Students.Models.ReferenceModels;

namespace Students.API.Controllers;

/// <summary>
/// Контроллер статусов заявок.
/// </summary>
[ApiController]
[Route("[controller]")]
[ApiVersion("1.0")]
public class StatusRequestController : GenericAPiController<StatusRequest>
{
  /// <summary>
  /// Конструктор.
  /// </summary>
  /// <param name="repository">Репозиторий стусов заявок.</param>
  /// <param name="logger">Логгер.</param>
  public StatusRequestController(IGenericRepository<StatusRequest> repository, ILogger<StatusRequest> logger) : base(
    repository, logger)
  {
  }
}
