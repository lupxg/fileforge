using Fileforge.Application.Features.Documents.GetDocument;
using Fileforge.Application.Features.Documents.Search;
using Fileforge.Application.Features.Documents.UploadDocument;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fileforge.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddScoped<UploadDocumentHandler>();
        services.AddScoped<GetDocumentHandler>();
        services.AddScoped<SearchDocumentsHandler>();

        return services;
    }
}
