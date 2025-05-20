using Microsoft.AspNetCore.Mvc;
using Students.Infrastructure.DTO;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.API.Controllers;

/// <summary>
/// Контроллер студентов.
/// </summary>
[ApiController]
[Route("[controller]")]
[ApiVersion("1.0")]
public class StudentController : GenericAPiController<Student>
{
  #region Поля и свойства

  private readonly IStudentRepository _studentRepository;
  private readonly ILogger<Student> _logger;

  #endregion

  #region Методы

  /// <summary>
  /// Список студентов с разделением по страницам (кажется это можно вынести в абстрактный класс).
  /// </summary>
  [HttpGet("paged")]
  public async Task<IActionResult> ListAllPaged([FromQuery] Pageable pageable, StudentFilterDto? filters)
  {
    try
    {
      var items = await this._studentRepository.GetStudentsByPage(pageable.PageNumber, pageable.PageSize, filters);
      return this.Ok(items);
    }
    catch(Exception e)
    {
      this._logger.LogError(e, "Error while getting Entities");
      return this.Exception();
    }
  }


  /// <summary>
  /// Получить студента с заявками и группами(не работает).
  /// </summary>
  /// <param name="studentId">Идентификатор студент.а</param>
  /// <returns>Студент с подгруженными заявками и группами.</returns>
  [HttpPost("GetStudentWithGroupsAndRequests")]
  public async Task<IActionResult> GetStudentWithGroupsAndRequests(Guid studentId)
  {
    try
    {
      var form = await this._studentRepository.GetStudentWithGroupsAndRequests(studentId);
      return form is null ? this.NotFoundException() : this.Ok(form);
    }
    catch(Exception e)
    {
      this._logger.LogError(e, "Error while getting Entity by Id");
      return this.Exception();
    }
  }

  #endregion

  #region Конструкторы

  /// <summary>
  /// Конструктор.
  /// </summary>
  /// <param name="repository">Репозиторий студентов.</param>
  /// <param name="logger">Логгер.</param>
  /// <param name="studentRepository">Репозиторий студентов.</param>
  public StudentController(IGenericRepository<Student> repository, ILogger<Student> logger,
    IStudentRepository studentRepository) : base(repository, logger)
  {
    this._studentRepository = studentRepository;
    this._logger = logger;
  }

  #endregion
}
