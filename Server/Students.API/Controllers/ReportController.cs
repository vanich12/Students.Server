using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Studens.Application.Report.Interfaces;
using Students.Application.Services.Interfaces;
using Students.Models.ReportsModel;

namespace Students.API.Controllers;

/// <summary>
/// Контроллер отчетов.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ReportController : ControllerBase
{
    #region Поля и свойства

    private readonly ILogger<ReportController> _logger;
    private readonly IReport<XLWorkbook> _report;
    private readonly IFRDOService _frdoService;

    #endregion

    #region Методы

    /// <summary>
    /// Получить отчет для Росстата.
    /// </summary>
    /// <returns>Отчет.</returns>
    [HttpPost("GetRostatReport")]
    public async Task<FileResult> GetRosstatReport(DateOnly startDate, DateOnly endDate)
    {
        var workbook = await _report.GenerateRosstatReport(startDate, endDate) ??
                       throw new ArgumentNullException("Нет данных.");
        return CreateFileReport(workbook, "Росстат");
    }

    /// <summary>
    /// Получить данные для предпросмотра отчета ФРДО в формате JSON.
    /// </summary>
    /// <returns>Список данных для отчета.</returns>
    [HttpGet("PreviewPFDOReport")] 
    public async Task<IActionResult> PreviewPFDOReport([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
    {
     
        var reportData =
            await _frdoService.GetReportDataForFRDO(startDate,
                endDate); 

        if (reportData == null || !reportData.Any())
            return NotFound("Данные для отчета за указанный период не найдены.");


        return Ok(reportData);
    }

    /// <summary>
    /// Сгенерировать и скачать отчет ФРДО на основе отредактированных данных из предпросмотра.
    /// </summary>
    /// <param name="editedReportData">Список отредактированных строк отчета.</param>
    /// <returns>Отчет в формате .xlsx.</returns>
    [HttpPost("GenerateEditedPFDOReport")]
    public async Task<FileResult> GenerateEditedPFDOReport([FromBody] List<FRDOModel> editedReportData)
    {
        if (editedReportData == null || !editedReportData.Any())
            throw new BadHttpRequestException("Данные для отчета не предоставлены.");
        
        try
        {
            var workbook = await _report.GenerateFRDOReportFromData(editedReportData);

            if (workbook == null) throw new InvalidOperationException("Не удалось сгенерировать книгу Excel.");

            return CreateFileReport(workbook, "ФРДО_отредактированный");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при генерации отредактированного отчета ФРДО");
            throw; 
        }
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
            _logger.LogError(e, "Ошибка при генерации отредактированного отчета ФРДО");
            throw; // повзоляет middleware обработать ошибку
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
        result = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Отчет {nameReport} {DateTime.Now}.xlsx");
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
    public ReportController(IReport<XLWorkbook> report, IFRDOService frdoService, ILogger<ReportController> logger)
    {
        this._frdoService = frdoService;
        this._report = report;
        this._logger = logger;
    }

    #endregion
}