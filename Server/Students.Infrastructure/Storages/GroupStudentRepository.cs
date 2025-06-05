using Microsoft.EntityFrameworkCore;
using Students.DBCore.Contexts;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.Infrastructure.Storages;

/// <summary>
/// Репозиторий групп студентов.
/// </summary>
public class GroupStudentRepository : GenericRepository<GroupStudent>, IGroupStudentRepository
{
    #region Поля и свойства

    private readonly StudentContext _ctx;

    #endregion

    #region

    public override async Task<IEnumerable<GroupStudent>> GetAll()
    {
        return await this._ctx.GroupStudent.Include(s => s.Student).ToListAsync();
    }
    // TODO вынести в сервисы и проверять на ошибки
    public async Task<GroupStudent> FindByStudentInGroup(Guid groupId, Guid studentId)
    {
        var item = await this._ctx.GroupStudent.FirstOrDefaultAsync(x => x.GroupId == groupId && x.StudentId == studentId);
        return item;
    }

    /// <summary>
    /// Добавление студента по заявке в группу.
    /// </summary>
    /// <param name="request">Заявка.</param>
    /// <param name="groupId">Идентификатор группы.</param>
    /// <returns>Группа студентов.</returns>
    public async Task<GroupStudent?> Create(Request request, Guid groupId)
    {
        //делает именно яв­ную под­груз­ку навигационного свойства Student у уже отслеживаемого объекта
        if (request.Student is null)
            return null;

        return await base.Create(new GroupStudent
        {
            StudentId = request.Student.Id,
            GroupId = groupId,
            RequestId = request.Id
        });
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public GroupStudentRepository(StudentContext context) : base(context)
    {
        this._ctx = context;
    }

    #endregion
}