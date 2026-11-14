using Asp.Versioning;
using Fileforge.Application.Features.Documents.GetDocument;
using Fileforge.Application.Features.Documents.UploadDocument;
using Microsoft.AspNetCore.Mvc;

namespace Fileforge.Api.Controllers.V1.Features;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]")]
public class DocumentController : Controller
{
    private const long MaxFileSizeBytes = 10 * 1024 * 1024;

    [HttpPost(Name = "Upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadDocumentAsync(IFormFile? file, [FromServices] UploadDocumentHandler handler)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest(new { Error = "A non-empty file is required" });
        }

        if (file.Length > MaxFileSizeBytes)
        {
            return Problem(statusCode: StatusCodes.Status413PayloadTooLarge, title: "File too large", detail: "The maximun file size is 10 MB");
        }

        await using var stream = file.OpenReadStream();

        var document = await handler.HandleAsync(stream, file.FileName, file.ContentType, file.Length);

        return CreatedAtRoute(
            routeName: "GetDocument",
            routeValues: new { id = document.Id },
            value: new
            {
                document.Id,
                document.FileName,
                Status = document.Status.ToString(),
                document.SizeInBytes,
                document.CreatedAtUtc
            });
    }



    [HttpGet(Name = "GetDocument/{id:guid}")]
    public async Task<IActionResult> GetDocumentByIdAsync(Guid id, [FromServices] GetDocumentHandler handler)
    {
        var document = await handler.HandleAsync(id);

        return document is null ? NotFound(new { Error = "Document not found"}) : Ok(document);
    }
}
