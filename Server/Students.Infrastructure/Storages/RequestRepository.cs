using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Students.DBCore.Contexts;
using Students.Infrastructure.DTO;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Infrastructure.Extension.Filters;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.Infrastructure.Storages;

/// <summary>
/// Репозиторий заявок.
/// </summary>
public class RequestRepository : GenericRepository<Request>, IRequestRepository
{
    #region Поля и свойства

    private readonly StudentContext _ctx;
    private readonly IOrderRepository _orderRepository;

    #endregion

    #region Методы

    /// <summary>
    /// Добавление приказа в заявку.
    /// </summary>
    /// <param name="requestId">Идентификатор заявки.</param>
    /// <param name="order">Приказ.</param>
    /// <returns>Идентификатор заявки.</returns>
    public async Task<Guid?> AddOrderToRequest(Guid requestId, Order order)
    {
        var request = await this.FindById(requestId);
        if (request is null)
            return null;

        order.RequestId = requestId;
        await this._orderRepository.Create(order);

        return requestId;
    }

    /// <summary>
    /// Список заявок, в которые подавал студент.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <returns>Список заявок.</returns>
    public async Task<IEnumerable<RequestsDTO>?> GetListRequestsOfStudentExists(Guid studentId)
    {
        var student = await this._ctx.FindAsync<Student>(studentId);
        _ctx.Entry(student).Reference(p => p.Person);

        if (student is null) return null;
        // TODO: надо подтянуть данные о студенте
        var request = await this._ctx.Requests.Where(s => s.StudentId == studentId).Include(x => x.Student)
            .Include(x => x.Person)
            .Include(x=>x.EducationProgram)
            .Select(x => Mapper.RequestToRequestDTO(x).Result).ToListAsync();

        if (student is null)
            return null;

        await this._ctx.Entry(student).Collection(s => s.Requests!).LoadAsync();

        return request;
    }

    /// <summary>
    /// Пагинация заявок.
    /// </summary>
    /// <param name="page">Номер страницы.</param>
    /// <param name="pageSize">Размер страницы.</param>
    public async Task<PagedPage<RequestsDTO>> GetRequestsDTOByPage(int page, int pageSize, RequestFilterDTO filters)
    {
        var query = this._ctx.Requests.AsNoTracking()
            .Include(s => s.Student)
            .Include(p => p.Person)
            .ThenInclude(te => te.TypeEducation)
            .Include(ep => ep.EducationProgram)
            .Include(st => st.Status)
            .Include(o => o.Orders)
            .ThenInclude(ko => ko.KindOrder);

        var filteredQuery = query.ApplyFilters(filters);

        var dtoQuery = filteredQuery.Select(x => Mapper.RequestToRequestDTO(x).Result);

        return await PagedPage<RequestsDTO>.ToPagedPage<string>(dtoQuery, page, pageSize, x => x.StudentFullName);
    }

    /// <summary>
    /// Поиск сущности по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>Сущность.</returns>
    public override async Task<Request?> FindById(Guid id)
    {
        return await this._ctx.Requests.AsNoTracking()
            .Include(x => x.Student)
            .Include(p => p.Person)
            .ThenInclude(y => y.TypeEducation)
            .Include(x => x.EducationProgram)
            .Include(x => x.Status)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    /// <param name="studRep">Репозиторий студентов.</param>
    /// <param name="orderRepository">Репозиторий приказов.</param>
    public RequestRepository(StudentContext context, IStudentRepository studRep, IOrderRepository orderRepository) :
        base(context)
    {
        this._ctx = context;
        this._orderRepository = orderRepository;
    }

    #endregion
}