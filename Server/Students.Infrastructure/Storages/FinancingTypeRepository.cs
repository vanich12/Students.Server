using Students.DBCore.Contexts;
using Students.Infrastructure.Interfaces;
using Students.Models.ReferenceModels;

namespace Students.Infrastructure.Storages;

/// <summary>
/// Репозиторий типов финансирования.
/// </summary>
public class FinancingTypeRepository : GenericRepository<FinancingType>, IFinancingTypeRepository
{
  #region Поля и свойства

  private readonly StudentContext _context;

  #endregion

  #region Методы



  #endregion

  #region Конструкторы

  /// <summary>
  /// Конструктор.
  /// </summary>
  /// <param name="context"></param>
  public FinancingTypeRepository(StudentContext context) : base(context)
  {
    _context = context;
  }

  #endregion
}