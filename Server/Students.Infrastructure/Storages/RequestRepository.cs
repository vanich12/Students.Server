using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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

    public override async Task<IEnumerable<Request>> GetAll()
    {
        var items = await this._ctx.Requests.AsNoTracking().Include(x => x.Person).ToListAsync();
        return items;
    }

    public async Task<IEnumerable<RequestsDTO>> GetAllReqDTO()
    {
        var query = this._ctx.Requests.AsNoTracking();
        query = IncludeDetails(query);

        var requests = await query.ToListAsync();

        var dtoTasks = requests.Select(request => Mapper.RequestToRequestDTO(request));

        var dtos = await Task.WhenAll(dtoTasks);

        return dtos;
    }

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
    ///  Список заявок, в которые подавал студент.
    /// </summary>
    /// <param name="studentId">id студента</param>
    /// <param name="filters">фильтры</param>
    /// <returns></returns>
    public async Task<IEnumerable<RequestsDTO>?> GetListRequestsOfStudentExists(Guid studentId,
        RequestFilterDTO filters)
    {
        var student = await this._ctx.FindAsync<Student>(studentId);

        _ctx.Entry(student).Reference(p => p.Person);

        if (student is null) return null;

        var queryableRequest = this._ctx.Requests.Where(s => s.StudentId == studentId)
            .Include(x => x.Student)
            .Include(x => x.Person)
            .Include(x => x.EducationProgram)
            .Include(x => x.Status)
            .Include(x => x.GroupStudent)
            .ThenInclude(x => x.Group
            ).ApplyFilters(filters);

        var requestDto = await queryableRequest.Select(x => Mapper.RequestToRequestDTO(x).Result).ToListAsync();

        return requestDto;
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


    public override async Task<IEnumerable<Request>> Get(Expression<Func<Request, bool>> predicate,Func<IQueryable<Request>,
        IIncludableQueryable<Request,object>>? eadgerQuery = null)
    {
        var itemsQuery = this._ctx.Requests.AsNoTracking().Where(predicate);
        if (eadgerQuery is not null)
            itemsQuery = eadgerQuery(itemsQuery);
        else
            itemsQuery = IncludeDetails(itemsQuery);
        var listItems = await itemsQuery.ToListAsync();

        return listItems;
    }

    private IQueryable<Request> IncludeDetails(IQueryable<Request> query)
    {
        return query
            .Include(x => x.Student)
            .Include(x => x.Person)
            .ThenInclude(p => p.TypeEducation) // Добавим это, т.к. часто нужно
            .Include(x => x.EducationProgram)
            .Include(x => x.Status)
            .Include(x => x.GroupStudent)
            .ThenInclude(gs => gs.Group);
    }

    /// <summary>
    /// Поиск сущности по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>Сущность.</returns>
    public override async Task<Request?> FindById(Guid id)
    {
        var query = this._ctx.Requests.AsNoTracking();
        query = IncludeDetails(query);

        return await query.FirstOrDefaultAsync(x => x.Id == id);
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