using Asp.Versioning;
using Fileforge.Application.Features.Documents.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Nito.Disposables;
using System.Diagnostics;

namespace Fileforge.Api.Controllers.V1.Features;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]")]
public class SearchController : Controller
{
    [HttpGet(Name = "SearchDocuments")]
    public async Task<IActionResult> Search([FromQuery] string? query, int? topK, [FromServices] SearchDocumentsHandler handler)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest(new { Error = "A non-empty search query is requierd"});
        }

        var resultCount = topK ?? 5;

        if (resultCount is < 1 or > 20)
        {
            return BadRequest(new { Error = "TopK must be between 1 and 20"});
        }

        var results = await handler.HandleAsync(query, resultCount);

        return Ok(new 
        {
            Query = query,
            Count = results.Count,
            Results = results
        });
    }
}
