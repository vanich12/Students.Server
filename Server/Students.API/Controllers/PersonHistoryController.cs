using Microsoft.AspNetCore.Mvc;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [ApiVersion("1.0")]
    public class PersonHistoryController : GenericAPiController<PersonHistory>
    {
        private readonly IPersonHistoryRepository _personRepository;
        private readonly ILogger<PersonHistory> _logger;

        /// <summary>
        /// пагинация элеменов
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("paged")]
        public async Task<IActionResult> ListAllPaged([FromQuery] Pageable pageable, PersonHistoryFilterDTO? filters)
        {
            try
            {
                var items = await _personRepository.GetPersonHistoryByPage(pageable.PageNumber, pageable.PageSize, filters);
                return this.Ok(items);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                return this.Exception();
            }
        }

        public PersonHistoryController(IPersonHistoryRepository personHistoryRepository, ILogger<PersonHistory> logger)
            : base(personHistoryRepository, logger)
        {
            this._personRepository = personHistoryRepository;
            this._logger = logger;
        }
    }
}