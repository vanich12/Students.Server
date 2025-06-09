using System.Reflection;
using ClosedXML.Excel;
using ClosedXML.Report;
using Studens.Application.Report.Interfaces;
using Studens.Application.Report.Services;
using Students.Infrastructure.Interfaces;
using Students.Models.ReportsModel;

namespace Studens.Application.Report
{
    /// <summary>
    /// Генератор отчетов.
    /// </summary>
    public class GenerateReports : IReport<XLWorkbook>
    {
        private readonly IReportRepository<RosstatModel> _reportRosstatRepository;
        private readonly IReportRepository<FRDOModel> _reportPFDORepository;

        /// <summary>
        /// Генерировать отчет для Росстата.
        /// </summary>
        /// <returns>Книга.</returns>
        public async Task<XLWorkbook?> GenerateRosstatReport(DateOnly startDate, DateOnly endDate)
        {
            var listReportData = await _reportRosstatRepository.Get(startDate, endDate) ?? throw new ArgumentNullException("Нет данных.");
            var template = new XLTemplate(Directory.GetCurrentDirectory() + @"\Report\Templates\Form1-PK.xlsx");
            template.AddVariable(listReportData.FirstOrDefault());
            template.Generate();

            return template.Workbook as XLWorkbook;
        }

        /// <summary>
        /// Генерировать отчет ФРДО.
        /// </summary>
        /// <returns>Книга.</returns>
        public async Task<XLWorkbook?> GenerateFRDOReport(DateOnly startDate, DateOnly endDate)
        {
            // 1. Получаем данные для отчета
            var listReportData = await _reportPFDORepository.Get(startDate, endDate);
            if (listReportData == null || !listReportData.Any())
                return null;
            

            string templateDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Report", "Templates");
            string templatePath = Path.Combine(templateDirectory, "FRDO.xlsx");

       
            if (!Directory.Exists(templateDirectory))
            {
                Directory.CreateDirectory(templateDirectory);
                // ВАЖНО: После создания папки, шаблон там не появится сам.
                // Вы должны либо скопировать его туда при развертывании приложения,
                // либо программа должна сообщить об ошибке.
            }

            // 4. Проверяем, существует ли сам файл шаблона
            if (!File.Exists(templatePath))
                throw new FileNotFoundException("Файл шаблона отчета не найден по пути: " + templatePath);


            // 5. Открываем шаблон и заполняем его
      
            var workbook = new XLWorkbook(templatePath);

            var worksheet = workbook.Worksheet("Шаблон"); // Убедитесь, что лист называется именно так
            if (worksheet == null)
            {
                throw new InvalidOperationException("В шаблоне отчета не найден лист с названием 'Шаблон'.");
            }

            FillingCells(worksheet, listReportData);

            return workbook;
        }

        /// <summary>
        /// Заполнение отчета ПФДО.
        /// </summary>
        /// <param name="xLWorksheet">Лист.</param>
        /// <param name="list">Список сущностей.</param>
        /// <returns>Лист.</returns>
        private IXLWorksheet FillingCells(IXLWorksheet xLWorksheet, List<FRDOModel> list)
        {
            int charCounter = 0;
            int cellCounter = 2;

            foreach (var row in list)
            {
                PropertyInfo[] cells = row.GetType().GetProperties();
                foreach (PropertyInfo cell in cells)
                {
                    var prop = cell.GetValue(row)?.ToString();
                    var cellValue = cell.GetValue(row) is null ? string.Empty : prop;
                    xLWorksheet.Cell(ExcelMetadata.ExcelColumnName[charCounter].ToString() + cellCounter).Value = cellValue;
                    charCounter++;
                }
                charCounter = 0;
                cellCounter++;
            }
            return xLWorksheet;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="reportPFDORepository">Репозиторий.</param>
        /// <param name="reportRosstatRepository">Репозиторий.</param>
        public GenerateReports(IReportRepository<FRDOModel> reportPFDORepository, IReportRepository<RosstatModel> reportRosstatRepository)
        {
            _reportRosstatRepository = reportRosstatRepository;
            _reportPFDORepository = reportPFDORepository;
        }
    }
}