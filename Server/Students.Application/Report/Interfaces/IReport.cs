using ClosedXML.Excel;

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
    }
}
