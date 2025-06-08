using Microsoft.AspNetCore.Mvc;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.API.Controllers;

/// <summary>
/// Контроллер Документы повышения квалификации.
/// </summary>
[ApiController]
[Route("controller")]
[ApiVersion("1.0")]
public class DocumentRiseQualifacationController : GenericAPiController<DocumentRiseQualification>
{
    private readonly IDocumentRiseQualificationRepository _documentRiseQualificationRepository;
    private readonly ILogger _logger;
    // TODO: доделать отправку POST
    [HttpGet("paged")]
    public async Task<IActionResult> ListAllPaged([FromQuery] Pageable pageable)
    {
        try
        {
            var items = await _documentRiseQualificationRepository.GetPagedRiseQualifications(pageable.PageNumber, pageable.PageSize);
            return this.Ok(items);
        }
        catch (Exception e)
        {
            this._logger.LogError(e.Message, "Error while tried get list of documents");
            return this.Exception();
        }
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="repository">Репозиторий документов повышения квалификации.</param>
    /// <param name="logger">Логгер.</param>
    public DocumentRiseQualifacationController(IDocumentRiseQualificationRepository repository,
        ILogger<DocumentRiseQualification> logger) : base(repository, logger)
    {
        _documentRiseQualificationRepository = repository;
        _logger = logger;
    }
}