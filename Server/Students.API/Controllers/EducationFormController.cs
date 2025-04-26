﻿using Microsoft.AspNetCore.Mvc;
using Students.Infrastructure.Interfaces;
using Students.Models.ReferenceModels;

namespace Students.API.Controllers;

/// <summary>
/// Контроллер формы образования.
/// </summary>
[ApiController]
[Route("[controller]")]
[ApiVersion("1.0")]
public class EducationFormController : GenericAPiController<EducationForm>
{
  /// <summary>
  /// Конструктор.
  /// </summary>
  /// <param name="repository">Репозиторий форм образований.</param>
  /// <param name="logger">Логгер.</param>
  public EducationFormController(IGenericRepository<EducationForm> repository, ILogger<EducationForm> logger) : base(
    repository, logger)
  {
  }
}