﻿namespace Students.Infrastructure.Interfaces;

/// <summary>
/// Репозиторий отчетов.
/// </summary>
public interface IReportRepository<TEntity> where TEntity : class
{
  /// <summary>
  /// Получить список сущностей.
  /// </summary>
  /// <returns>Список сущностей.</returns>
  public Task<List<TEntity>> Get(DateOnly startDate, DateOnly endDate);


}