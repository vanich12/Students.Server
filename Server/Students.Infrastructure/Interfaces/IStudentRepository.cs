using Students.Infrastructure.DTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Models;


namespace Students.Infrastructure.Interfaces;

/// <summary>
/// Интерфейс репозитория студентов.
/// </summary>
public interface IStudentRepository : IGenericRepository<Student>
{
    /// <summary>
    /// Пагинация писка студентов.
    /// </summary>
    /// <param name="page">Номер страницы.</param>
    /// <param name="pageSize">Размер страницы.</param>
    /// <returns>Список студентов с пагинацией.</returns>
    Task<PagedPage<StudentDTO>>GetStudentsByPage(int page, int pageSize, StudentFilterDto? filters);

  /// <summary>
  /// Поиск студента (с подгрузкой данных о группах и заявках) по идентификатору.
  /// </summary>
  /// <param name="studentId">Идентификатор студента.</param>
  /// <returns>Студент.</returns>
  Task<Students.Models.Student?> GetStudentWithGroupsAndRequests(Guid studentId);

  //Task<PagedPage<StudentDTO>> GetFilteredStudentByDateOfBirth(int page, int pageSize, DateOnly date);
}
