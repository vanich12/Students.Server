using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Studens.Application.Report.Interfaces;

namespace Students.API.Controllers;

/// <summary>
/// Контроллер отчетов.
/// </summary>
[ApiController]
[Route("[controller]")]
public class  ReportController : ControllerBase
{
  #region Поля и свойства

  private readonly ILogger<ReportController> _logger;
  private readonly IReport<XLWorkbook> _report;

  #endregion

  #region Методы

  /// <summary>
  /// Получить отчет для Росстата.
  /// </summary>
  /// <returns>Отчет.</returns>
  [HttpPost("GetRostatReport")]
  public async Task<FileResult> GetRosstatReport(DateOnly startDate, DateOnly endDate)
  {
    var workbook = await _report.GenerateRosstatReport(startDate,endDate) ?? throw new ArgumentNullException("Нет данных.");
    return CreateFileReport(workbook, "Росстат");
  }

  /// <summary>
  /// Получить отчет ФРДО.
  /// </summary>
  /// <returns>Отчет.</returns>
  [HttpPost("GetPFDOReport")]
  public async Task<FileResult> GetPFDOReport([FromQuery] DateOnly startDate, DateOnly endDate)
  {
      try
      {
          var workbook = await _report.GenerateFRDOReport(startDate, endDate)
                         ?? throw new ArgumentNullException("Нет данных.");
          return CreateFileReport(workbook, "ФРДО");
      }
      catch (Exception e)
      {
          Console.WriteLine(e);
          throw;
      }
 
  }

  /// <summary>
  /// Создание файла.
  /// </summary>
  /// <param name="workbook">Книга.</param>
  /// <param name="nameReport">Название отчета.</param>
  /// <returns>Файл.</returns>
  private FileContentResult CreateFileReport(XLWorkbook workbook, string nameReport)
  {
    FileContentResult result;
    using var stream = new MemoryStream();
    workbook.SaveAs(stream);
    result = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Отчет {nameReport} {DateTime.Now}.xlsx");
    stream.Close();
    return result;
  }

  #endregion

  #region Конструкторы

  /// <summary>
  /// Конструктор.
  /// </summary>
  /// <param name="report">Отчет.</param>
  /// <param name="logger">Логгер.</param>
  public ReportController(IReport<XLWorkbook> report, ILogger<ReportController> logger)
  {
    _report = report;
    _logger = logger;
  }

  #endregion
}