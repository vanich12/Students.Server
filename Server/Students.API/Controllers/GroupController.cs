﻿using Microsoft.AspNetCore.Mvc;
using Students.API.EndpointsFilters;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.API.Controllers;

/// <summary>
/// Контроллер группы.
/// </summary>
[ApiController]
[Route("[controller]")]
[ServiceFilter(typeof(LogModelStateActionFilter))]
[ApiVersion("1.0")]
public class GroupController : GenericAPiController<Group>
{
    #region Поля и свойства

    private readonly IGroupRepository _groupRepository;
    private readonly ILogger<Group> _logger;

    #endregion

    #region Методы

    /// <summary>
    /// Добавление студентов по заявкам в группу.
    /// </summary>
    /// <param name="requestsList">Список идентификаторов заявок.</param>
    /// <param name="groupId">Идентификатор группы.</param>
    /// <returns>Идентификаторы заявок которые не были добавлены.</returns>
    [HttpPost("AddStudentsToGroupByRequest")]
    public async Task<IActionResult> AddStudentsToGroupByRequest(IEnumerable<Guid> requestsList, Guid groupId)
    {
        try
        {
            var badRequests = await this._groupRepository.AddStudentsToGroupByRequest(requestsList, groupId);
            return badRequests is null ? this.NotFoundException() : this.Ok(badRequests);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Error while creating Entity");
            return this.Exception();
        }
    }
    /// <summary>
    /// Удаление студента из группы
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="groupId"></param>
    /// <returns></returns>
    [HttpDelete("RemoveStudentsFromGroupByRequest")]
    public async Task<IActionResult> RemoveStudentsFromGroupByRequest(Guid studentId, Guid groupId)
    {
        try
        {
            var badRequest = await this._groupRepository.RemoveStudentFromGroupByRequest(studentId, groupId);
            return badRequest is null ? this.NotFoundException() : this.Ok(badRequest);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Error while getting Entities");
            return this.Exception();
        }
    }

    /// <summary>
    /// Список групп, в которых состоит студент.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <returns>Список групп.</returns>
    [HttpGet("GetListGroupsOfStudentExists")]
    public async Task<IActionResult> GetListGroupsOfStudentExists(Guid studentId)
    {
        try
        {
            var groups = await this._groupRepository.GetListGroupsOfStudentExists(studentId);
            return groups is null ? this.NotFoundException() : this.Ok(groups);
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
    /// <param name="groupRepository">Репозиторий групп.</param>
    /// <param name="logger">Логгер.</param>
    public GroupController(IGroupRepository groupRepository, ILogger<Group> logger) : base(groupRepository, logger)
    {
        this._groupRepository = groupRepository;
        this._logger = logger;
    }

    #endregion
}