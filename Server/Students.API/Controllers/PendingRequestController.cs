using Microsoft.AspNetCore.Mvc;
using Students.Application.Services.Interfaces;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiVersion("1.0")]
    public class PendingRequestController : GenericAPiController<PendingRequest>
    {
        private readonly IRequestService _requestService;
        private readonly ILogger<PendingRequest> _logger;

        [HttpGet("GetPendingRequestByPage")]
        public async Task<IActionResult> GetPendingRequestByPage([FromQuery] Pageable page)
        {
            try
            {
                var items = await this._requestService.GetPendingRequestsDTOByPage(page.PageNumber, page.PageSize);
                return this.Ok(items);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                return this.Exception();
            }
        }

        [HttpPost("CreateRequestFromPendingRequest")]
        public async Task<IActionResult> CreateRequestFromPendingRequest(Guid pRequestId, Guid personId)
        {
            try
            {
                var request = await _requestService.CreateRequestFromPendingRequest(pRequestId, personId);
                return this.Ok(request);
            }
            catch (ArgumentException e)
            {
                this._logger.LogError(e.Message);
                return this.Exception();
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                return this.Exception();
            }
        }

        public PendingRequestController(IGenericRepository<PendingRequest> repository, ILogger<PendingRequest> logger,
            IRequestService requestService) :
            base(repository, logger)
        {
            this._requestService = requestService;
            this._logger = logger;
        }
    }
}