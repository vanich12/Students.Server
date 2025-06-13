using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Students.Application.Services.Interfaces;
using Students.Infrastructure.Interfaces;
using Students.Models.ReportsModel;

namespace Students.Application.Services
{
    public class FRDOService(IReportRepository<FRDOModel> FRDOrepository, ILogger<FRDOModel> logger): IFRDOService
    {
        /// <summary>
        /// Получить данные для отчета ФРДО.
        /// </summary>
        /// <param name="startDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Список данных для отчета.</returns>
        public async Task<IEnumerable<FRDOModel>> GetReportDataForFRDO(DateOnly startDate, DateOnly endDate)
        {
            try
            {
                var listReportData = await FRDOrepository.Get(startDate, endDate);
                if (listReportData == null || !listReportData.Any())
                    return null;
                return listReportData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}