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
        public Task<T?> GenerateRosstatReport();

        /// <summary>
        /// Генерирование отчета ФРДО.
        /// </summary>
        /// <returns>Книга.</returns>
        public Task<T?> GenerateFRDOReport();
    }
}
