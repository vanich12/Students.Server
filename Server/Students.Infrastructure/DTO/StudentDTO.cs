﻿using System.ComponentModel.DataAnnotations;
using Students.Models.Enums;

namespace Students.Infrastructure.DTO
{
  /// <summary>
  /// DTO студента.
  /// </summary>
  // TODO: дописать недостющие поля
  public class StudentDTO
  {
    /// <summary>
    /// Id студента
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Фамилия
    /// экспорт из заявки
    /// </summary>

    public  string? Family { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Отчество
    /// экспорт из заявки
    /// </summary>
    public string? Patron { get; set; }

    /// <summary>
    /// ФИО
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateOnly? BirthDate { get; set; }

    /// <summary>
    /// МестоПроживания. Адрес, по хорошему нужен либо справочник, либо формат стандарта ГОСТа Р 6.30-2003
    /// экспорт из заявки
    /// </summary>

    public  string? Address { get; set; }

    public SexHuman? Sex { get; set; }

    public int? Age { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? SNILS { get; set; }

    public string? Nationality { get; set;}

    public Guid? TypeEducationId { get; set; }

    public string? Speciality { get; set; }

    public string? IT_Experience { get; set; }

    /// <summary>
    /// Id заявки
    /// </summary>
    public Guid? RequestId { get; set; }

    public Guid? ScopeOfActivityLevelOneId { get; set; }
    public Guid? ScopeOfActivityLevelTwoId { get; set; }

    /// <summary>
    /// Идентификатор Статус заявки
    /// </summary>

    public Guid? StatusRequestId { get; set; }

    /// <summary>
    /// Статус заявки.
    /// </summary>
    public string? StatusRequestName { get; set; }

    /// <summary>
    /// Идентификатор программы обучения.
    /// </summary>
    public Guid? EducationProgramId { get; set; }

    /// <summary>
    /// Наименование программы.
    /// </summary>
    public string? ProgramName { get; set; }

    /// <summary>
    /// Id группы.
    /// </summary>
    public Guid? GroupId { get; set; }

    /// <summary>
    /// Имя группы.
    /// </summary>
    public string? GroupName { get; set; }

    /// <summary>
    /// Начало обучения.
    /// </summary>
    public DateOnly? GroupStartDate { get; set; }

    /// <summary>
    /// Конец обучения.
    /// </summary>
    public DateOnly? GroupEndDate { get; set; }
    
  }
}
