using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Students.Application.Services.Interfaces;
using Students.Infrastructure.DTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.Application.Services
{
    public class PersonService(
        IPersonRepository personRepository, ILogger<Person> logger) : GenericService<Person>(personRepository, logger), IPersonService
    {
        /// <summary>
        /// Обновление данных персоны через заявку(вебхук)
        /// </summary>
        /// <param name="pendingRequestId">Идентфикатор неподтвержденной заявки</param>
        /// <param name="personId">Идентификатор персоны</param>
        /// <param name="form">DTO Персоны</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Person> UpdatePersonFromPendingRequestData(Guid pendingRequestId, Guid personId,
            NewPersonDTO form)
        {
            try
            {
                var oldPerson = await personRepository.FindById(personId);
                if (oldPerson is null)
                    throw new ArgumentException($"персона по: {personId} не найдена");

                var middlewarePerson = await Mapper.NewPersonDTOToPerson(form);

                var newPerson = await personRepository.Update(personId, middlewarePerson);

                return newPerson;
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }

        }

        /// <summary>
        /// Создание персоны на основе заявки (вебхука)
        /// </summary>
        /// <param name="pendingRequestId">Идентфикатор неподтвержденной заявки</param>
        /// <param name="form">DTO Персоны</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Person> CreatePersonFromPendingRequest(Guid pendingRequestId, NewPersonDTO form)
        {
            try
            {
                var person = await Mapper.NewPersonDTOToPerson(form);
                if (person is null)
                    throw new ArgumentException($"не удалось создать персону по DTO");

                var newPerson = await personRepository.Create(person);
                if (newPerson is null)
                    throw new ArgumentException("Не удалось создать персоны");

                return newPerson;
            }

            catch (ArgumentException ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }
    }
}