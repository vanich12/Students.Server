using Microsoft.AspNetCore.Mvc;
using Students.Application.Services.Interfaces;
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
    private readonly IStudentService _studentService;

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
            var items = await this._studentRepository.GetStudentsByPage(pageable.PageNumber, pageable.PageSize,
                filters);
            return this.Ok(items);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Error while getting Entities");
            return this.Exception();
        }
    }

    /// <summary>
    /// Список истории обучений студента
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="filters"></param>
    /// <returns></returns>
    [HttpGet("GetLearningHistoryOfStudent")]
    public async Task<IActionResult> GetLearningHistoryOfStudent(Guid studentId, RequestFilterDTO? filters)
    {
        try
        {
            var items = await this._studentService.GetLearningHistoryOfStudent(studentId, filters);
            return items is null ? this.NotFoundException() : this.Ok(items);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Error while getting Learning history");
            return this.Exception();
        }
    }

    /// <summary>
    /// Получение студента по идентификатору
    /// </summary>
    /// <param name="id">Id студента</param>
    /// <returns></returns>
    public override async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var student = await this._studentRepository.FindById(id);
            if (student is null)
                return this.NotFoundException();

            var studentDTO = await Mapper.StudentToStudentDTO(student);

            return this.Ok(studentDTO);
        }
        catch (Exception e)
        {
            this._logger.LogError(e.Message, "Error while trying get student by id");
            throw;
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
        catch (Exception e)
        {
            this._logger.LogError(e.Message, "Error while trying get student by id");
            throw;
        }
    }

    /// <summary>
    /// Обновление студента
    /// </summary>
    /// <param name="id">Id студнта</param>
    /// <param name="form"></param>
    /// <returns></returns>
    [HttpPut("EditStudent/{studentId}")]
    public async Task<IActionResult> Put(Guid studentId, [FromBody] StudentDTO form)
    {
        try
        {
            var student = await this._studentService.UpdateStudent(studentId,form);
            return student is null ? this.NotFoundException() : this.Ok(student);
        }
        catch (Exception e)
        {
            this._logger.LogError(e.Message, "Error while trying update student by id");
            throw;
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
    public StudentController(ILogger<Student> logger,
        IStudentRepository studentRepository, IStudentService studentService) : base(studentRepository, logger)
    {
        this._studentRepository = studentRepository;
        this._studentService = studentService;
        this._logger = logger;
    }

    #endregion
}