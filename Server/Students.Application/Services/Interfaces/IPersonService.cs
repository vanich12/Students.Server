using Students.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Models;

namespace Students.Application.Services.Interfaces
{
    public interface IPersonService
    {
        /// <summary>
        /// Обновление данных персоны через заявку(вебхук)
        /// </summary>
        /// <param name="pendingRequestId">Идентфикатор неподтвержденной заявки</param>
        /// <param name="personId">Идентификатор персоны</param>
        /// <param name="form">DTO Персоны</param>
        Task<Person> UpdatePersonFromPendingRequestData(Guid pendingRequestId, Guid personId, NewPersonDTO form);

        /// <summary>
        /// Создание персоны на основе заявки (вебхука)
        /// </summary>
        /// <param name="pendingRequestId">Идентфикатор неподтвержденной заявки</param>
        /// <param name="form">DTO Персоны</param>
        Task<Person> CreatePersonFromPendingRequest(Guid pendingRequestId, NewPersonDTO form);
    }
}
