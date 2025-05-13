using Microsoft.AspNetCore.Mvc;
using Students.Infrastructure.DTO;
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
                Console.WriteLine(e);
                throw;
            }
        }

        public PersonController(IGenericRepository<Person> repository, IPersonRepository personRepository,
            ILogger<Person> logger) : base(repository, logger)
        {
            this._logger = logger;
            this._personRepository = personRepository;
        }
    }
}