using Microsoft.EntityFrameworkCore;
using Students.DBCore.Contexts;
using Students.Infrastructure.DTO;
using Students.Infrastructure.Extension.Filters;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;


namespace Students.Infrastructure.Storages;

/// <summary>
/// Репозиторий студентов.
/// </summary>
public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    #region Поля и свойства

    private readonly StudentContext _ctx;
    private readonly IGroupStudentRepository _studentInGroupRepository;

    #endregion

    #region Методы

    /// <summary>
    /// Список студентов с пагинацией.
    /// </summary>
    /// <param name="page">Номер страницы.</param>
    /// <param name="pageSize">Размер страницы.</param>
    /// <returns>Список студентов с пагинацией.</returns>
    public async Task<PagedPage<StudentDTO>> GetStudentsByPage(int page, int pageSize, StudentFilterDto? filters)
    {
        // достаем студентов с подгрузкой данных о группах и заявках
        // 1. Базовый запрос + Includes
        IQueryable<Student> studentQuery = this._ctx.Students
            .Include(gs => gs.GroupStudent!)         
            .ThenInclude(r => r.Request!)         
            .ThenInclude(st => st.Status)     
            .Include(gs1 => gs1.GroupStudent!)      
            .ThenInclude(g => g.Group!)          
            .ThenInclude(e => e.EducationProgram) 
            .Include(te => te.TypeEducation!);    

        studentQuery = studentQuery.ApplyFilters(filters); 

        // 3. Проекция в DTO
        var dtoQuery = studentQuery.Select(x => Mapper.StudentToStudentDTO(x).Result);

        // 4. Пагинация
        return await PagedPage<StudentDTO>.ToPagedPage(dtoQuery, page, pageSize, x => x.StudentFullName);
    }

    /// <summary>
    /// Поиск студента (с подгрузкой данных о группах и заявках) по идентификатору.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <returns>Студент.</returns>
    public async Task<Students.Models.Student?> GetStudentWithGroupsAndRequests(Guid studentId)
    {
        var student = await base.FindById(studentId);
        if (student is null)
            return null;

        await this._ctx.Entry(student).Collection(s => s.Groups!).LoadAsync();
        await this._ctx.Entry(student).Collection(s => s.Requests!).LoadAsync();

        return student;
    }

    public async Task<PagedPage<StudentDTO>> GetFilteredStudentByDateOfBirth(int page, int pageSize, DateOnly birthDate)
    {
        IQueryable<StudentDTO> query = this._ctx.Students
            .Include(gs => gs.GroupStudent!)
            .ThenInclude(r => r.Request!)
            .ThenInclude(st => st.Status)
            .Include(gs1 => gs1.GroupStudent!)
            .ThenInclude(g => g.Group!)
            .ThenInclude(e => e.EducationProgram)
            .Include(te => te.TypeEducation!)
            .Select(x => Mapper.StudentToStudentDTO(x).Result);

        return await PagedPage<StudentDTO>.ToPagedPage(query, page, pageSize, x => x.StudentFamily);
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    /// <param name="groupStudentRepository">Репозиторий групп студентов.</param>
    public StudentRepository(StudentContext context, IGroupStudentRepository groupStudentRepository) : base(context)
    {
        this._ctx = context;
        this._studentInGroupRepository = groupStudentRepository;
    }

    #endregion

}