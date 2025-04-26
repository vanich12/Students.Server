﻿using Microsoft.AspNetCore.Mvc;
using Students.Infrastructure.Interfaces;
using Students.Models.ReferenceModels;

namespace Students.API.Controllers;

/// <summary>
/// Контроллер типов образований.
/// </summary>
[ApiController]
[Route("[controller]")]
[ApiVersion("1.0")]
public class TypeEducationController : GenericAPiController<TypeEducation>
{
  /// <summary>
  /// Конструктор.
  /// </summary>
  /// <param name="repository">Репозиторий типов образований.</param>
  /// <param name="logger">Логгер.</param>
  public TypeEducationController(IGenericRepository<TypeEducation> repository, ILogger<TypeEducation> logger) : base(
    repository, logger)
  {
    logger.LogInformation($"Start {logger.GetType()}");
  }
}