using ClosedXML.Excel;
using Students.Models.ReportsModel;

namespace Studens.Application.Report.Interfaces
{
    /// <summary>
    /// Интерфейс.
    /// </summary>
    public interface IReport<T>
    {
        /// <summary>
        /// Генерирование отчета для Росстата.
        /// </summary>
        /// <returns>Книга.</returns>
        Task<T?> GenerateRosstatReport(DateOnly startDate, DateOnly endDate);

        /// <summary>
        /// Генерирование отчета ФРДО.
        /// </summary>
        /// <returns>Книга.</returns>
        public Task<T?> GenerateFRDOReport(DateOnly startDate, DateOnly endDate);

        /// <summary>
        /// Получить данные для отчета ФРДО c клиента.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<T?> GenerateFRDOReportFromData(List<FRDOModel> data);
    }
}
