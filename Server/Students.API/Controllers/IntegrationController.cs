using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;
using Students.Models.ReferenceModels;
using Students.Models.WebModels;

namespace Students.API.Controllers;

/// <summary>
/// Контроллер для интеграции с другими системами.
/// </summary>
[ApiController]
[Route("[controller]")]
[ApiVersion("1.0")]
public class IntegrationController : ControllerBase
{
  #region Поля и свойства

  /// <summary>
  /// Логгер контроллера.
  /// </summary>
  private readonly ILogger<IntegrationController> _logger;

  /// <summary>
  /// Репозиторий заявок.
  /// </summary>
  private readonly IRequestRepository _requestRepository;

  private readonly IPersonRepository _personRepository;

  /// <summary>
  /// Репозиторий студентов.
  /// </summary>
  private readonly IGenericRepository<Student> _studentRepository;

  /// <summary>
  /// Репозиторий образовательных программ.
  /// </summary>
  private readonly IGenericRepository<EducationProgram> _educationProgramRepository;

  /// <summary>
  /// Репозиторий статусов заявок.
  /// </summary>
  private readonly IGenericRepository<StatusRequest> _statusRequestRepository;

  /// <summary>
  /// Репозиторий типов образований.
  /// </summary>
  private readonly IGenericRepository<TypeEducation> _typeEducationRepository;

  /// <summary>
  /// Репозиторий сферы деятельности.
  /// </summary>
  private readonly IGenericRepository<ScopeOfActivity> _scopeOfActivityRepository;

  #endregion

  #region Методы

  /// <summary>
  /// Создание заявки на обучение по вебхуку.
  /// </summary>
  /// <param name="form">интеграционные данные от минцифры.</param>
  /// <returns>Возвращает статус запроса.</returns>
  [HttpPost("EducationRequest")]
  public async Task<IActionResult> Post([FromBody] RequestWebhook form)
  {
    try
    {
      var request = await Mapper.WebhookToRequest(form, this._educationProgramRepository, this._statusRequestRepository);

      var person = await this._personRepository.GetOne(x =>
        x.FullName == form.Name && x.BirthDate.ToString() == form.Birthday && x.Email == form.Email);

      if(person is null)
      {
        request.IsAlreadyStudied = false;
        if(await this._personRepository.GetOne(x =>
              x.FullName == form.Name || x.BirthDate.ToString() == form.Birthday || x.Email == form.Email) is null)
        {
            person = await Mapper.WebhookToStudent(form, this._typeEducationRepository, this._scopeOfActivityRepository);
            person = await this._personRepository.Create(person);
        }
      }
      else
      {
        request.IsAlreadyStudied = true;
      }

      request.StudentId = person?.Id;

      await this._requestRepository.Create(request);
      return this.Ok(form);
    }
    catch(ValidationException e)
    {
      this._logger.LogError(e, "Error while creating new Entity");
      return BadRequest(
        (object?)new DefaultResponse
        {
            RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier
        });
    }
    catch(Exception e)
    {
      this._logger.LogError(e, "Error while creating new Entity");
      return this.StatusCode(StatusCodes.Status500InternalServerError,
        new DefaultResponse
        {
          RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier
        });
    }
  }

  #endregion

  #region Конструкторы

  /// <summary>
  /// Конструктор.
  /// </summary>
  /// <param name="logger">Логгер контроллера.</param>
  /// <param name="requestRepository">Репозиторий заявок.</param>
  /// <param name="studentRepository">Репозиторий студентов.</param>
  /// <param name="educationProgramRepository">Репозиторий образовательных программ.</param>
  /// <param name="statusRequestRepository">Репозиторий статусов заявок.</param>
  /// <param name="typeEducationRepository">Репозиторий типов образований.</param>
  /// <param name="scopeOfActivityRepository">Репозиторий сферы деятельности.</param>
  public IntegrationController(ILogger<IntegrationController> logger, IRequestRepository requestRepository,
    IGenericRepository<Models.Student> studentRepository, IGenericRepository<EducationProgram> educationProgramRepository,
    IGenericRepository<StatusRequest> statusRequestRepository,
    IGenericRepository<TypeEducation> typeEducationRepository, IGenericRepository<ScopeOfActivity> scopeOfActivityRepository)
  {
    this._logger = logger;
    this._requestRepository = requestRepository;
    this._studentRepository = studentRepository;
    this._educationProgramRepository = educationProgramRepository;
    this._statusRequestRepository = statusRequestRepository;
    this._typeEducationRepository = typeEducationRepository;
    this._scopeOfActivityRepository = scopeOfActivityRepository;
  }

  #endregion
}