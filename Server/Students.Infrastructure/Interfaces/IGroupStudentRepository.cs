using Students.Models;

namespace Students.Infrastructure.Interfaces;

/// <summary>
/// Интерфейс репозитория групп студентов.
/// </summary>
public interface IGroupStudentRepository : IGenericRepository<GroupStudent>
{
  /// <summary>
  /// Добавление студента по заявке в группу.
  /// </summary>
  /// <param name="request">Заявка.</param>
  /// <param name="groupId">Идентификатор группы.</param>
  /// <returns>Группа студентов.</returns>
  Task<GroupStudent?> Create(Request request, Guid groupId);

  /// <summary>
  /// Нахождение записи в группу по Id заявки
  /// </summary>
  /// <param name="requestId"></param>
  /// <returns></returns>
  Task<GroupStudent> FindByStudentInGroup(Guid groupId,Guid studentId);
}