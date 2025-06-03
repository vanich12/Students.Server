using Students.DBCore.Contexts;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.Infrastructure.Storages;

// TODO: тоже нужно создать сервис для группы
/// <summary>
/// Репозиторий групп.
/// </summary>
public class GroupRepository : GenericRepository<Group>, IGroupRepository
{
    #region Поля и свойства

    private readonly StudentContext _ctx;
    private readonly IGroupStudentRepository _studentInGroupRepository;
    private readonly IRequestRepository _requestRepository;

    #endregion

    #region Методы

    /// <summary>
    /// Добавление студентов по заявкам в группу.
    /// </summary>
    /// <param name="requestsList">Список идентификаторов заявок.</param>
    /// <param name="groupId">Идентификатор группы.</param>
    /// <returns>Идентификаторы заявок которые не были добавлены.</returns>
    public async Task<IEnumerable<Guid>?> AddStudentsToGroupByRequest(IEnumerable<Guid> requestsList, Guid groupId)
    {
        var group = await base.FindById(groupId);
        if (group is null)
            return null;

        // требуется проверять нет ли такого студента уже в группе у этой заявки

        var bagRequestsIds = new List<Guid>();
        foreach (var requestId in requestsList)
        {
            var request = await this._requestRepository.FindById(requestId);
            var groupStudent = await _studentInGroupRepository.FindByStudentInGroup(groupId,request.StudentId.Value);
            if (groupStudent is not null)
                throw new ArgumentException("Такой студент уже есть в группе");
            if (request?.StudentId is null || request.EducationProgramId != group.EducationProgramId ||
                await this._studentInGroupRepository.Create(request, groupId) is null)
                bagRequestsIds.Add(requestId);
        }

        return bagRequestsIds;
    }

    /// <summary>
    /// Удаление студентпа из группы
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public async Task<Guid?> RemoveStudentFromGroupByRequest(Guid studentId, Guid groupId)
    {
        var group = await base.FindById(groupId);
        if (group is null)
            return null;

        var student = await this._ctx.Students.FindAsync(studentId);
        if (student is null)
            return null;

        var note = await this._studentInGroupRepository.GetOne(
            x => x.GroupId == groupId && x.StudentId == student.Id);

        if (note is null)
            return null;

        await this._studentInGroupRepository.Remove(note);

        return studentId;
    }


    /// <summary>
    /// Список групп, в которых состоит студент.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <returns>Список групп.</returns>
    public async Task<IEnumerable<Group?>?> GetListGroupsOfStudentExists(Guid studentId)
    {
        var student = await this._ctx.FindAsync<Student>(studentId);

        if (student is null)
            return null;

        await this._ctx.Entry(student).Collection(s => s.Groups!).LoadAsync();

        return student.Groups;
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    /// <param name="studInGroupRep">Репозиторий групп студентов.</param>
    public GroupRepository(StudentContext context, IGroupStudentRepository studInGroupRep,
        IRequestRepository requestRepository) : base(context)
    {
        this._ctx = context;
        this._studentInGroupRepository = studInGroupRep;
        this._requestRepository = requestRepository;
    }

    #endregion
}