using Students.Models.ReportsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Application.Services.Interfaces
{
    public interface IFRDOService
    {
        Task<IEnumerable<FRDOModel>> GetReportDataForFRDO(DateOnly startDate, DateOnly endDate);
    }
}