using Microsoft.AspNetCore.Mvc;
using Students.Application.Services.Interfaces;
using Students.Infrastructure.DTO;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiVersion("1.0")]
    public class PersonController : GenericAPiController<Person>
    {
        private readonly ILogger<Person> _logger;
        private readonly IPersonRepository _personRepository;
        private readonly IPersonService _personService;

        /// <summary>
        /// Пагинация Персон
        /// </summary>
        /// <param name="pageable">параметры пагинации</param>
        /// <param name="filters">фильтры персон</param>
        /// <returns></returns>
        [HttpGet("paged")]
        public async Task<IActionResult> ListAllPAged([FromQuery] Pageable pageable, PersonFilterDTO? filters)
        {
            try
            {
                var items = await _personRepository.GetStudentsByPage(pageable.PageNumber, pageable.PageSize, filters);
                return this.Ok(items);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message, "Error while tried get list of items");
                return this.Exception();
            }
        }

        /// <summary>
        /// Создаине персоны на основе заявки (вебхука) 
        /// </summary>
        /// <param name="pendingRequestId">Id заявки (вебхука)</param>
        /// <param name="form">данные персоны</param>
        /// <returns></returns>
        [HttpPost("CreatePersonBasedOnPRequest")]
        public async Task<IActionResult> CreatePersonBasedOnPRequest(Guid pendingRequestId,
            [FromBody] NewPersonDTO form)
        {
            try
            {
                var item = await _personService.CreatePersonFromPendingRequest(pendingRequestId, form);
                return this.Ok(item);
            }
            catch (ArgumentException ex)
            {
                this._logger.LogError(ex.Message, "Error while tried some item");
                return this.Exception();
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message, "Error while getting Item by Id");
                return this.Exception();
            }
        }

        ///// <summary>
        ///// Обновление студента
        ///// </summary>
        ///// <param name="id">Id студнта</param>
        ///// <param name="form"></param>
        ///// <returns></returns>
        //[HttpPut("EditStudent/{studentId}")]
        //public async Task<IActionResult> Put(Guid personId, [FromBody] PersonDTO form)
        //{
        //    try
        //    {
        //        var student = await this._personService.UpdateStudent(studentId, form);
        //        return student is null ? this.NotFoundException() : this.Ok(student);
        //    }
        //    catch (Exception e)
        //    {
        //        this._logger.LogError(e.Message, "Error while trying update student by id");
        //        throw;
        //    }
        //}

        /// <summary>
        /// Обновление персоны на основе заявки (вебхука)
        /// </summary>
        /// <param name="pendingRequestId">Id заявки (вебхука)</param>
        /// <param name="personId">Id персоны</param>
        /// <param name="form">данные персоны</param>
        /// <returns></returns>
        [HttpPut("UpdatePersonBasedOnPRequest")]
        public async Task<IActionResult> UpdatePersonBasedOnPRequest([FromQuery] Guid pendingRequestId, Guid personId,
            [FromBody] NewPersonDTO form)
        {
            try
            {
                var item = await _personService.UpdatePersonFromPendingRequestData(pendingRequestId, personId, form);
                return this.Ok(item);
            }
            catch (ArgumentException ex)
            {
                this._logger.LogError(ex.Message, "Error while tried get person by Id");
                return this.Exception();
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message, "Error while getting Item by Id");
                return this.Exception();
            }
        }


        public PersonController(IGenericRepository<Person> repository, IPersonRepository personRepository,
            IPersonService personService,
            ILogger<Person> logger) : base(repository, logger)
        {
            this._logger = logger;
            this._personRepository = personRepository;
            this._personService = personService;
        }
    }
}