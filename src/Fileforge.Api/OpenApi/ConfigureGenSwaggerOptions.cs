using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HelpDesk.Api.OpenApi;

public class ConfigureGenSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureNamedOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
        {
            var openApiInfo = new OpenApiInfo
            {
                Title = $"HelpDesk.Api v{description.ApiVersion}",
                Version = description.ApiVersion.ToString()
            };

            options.SwaggerDoc(description.GroupName, openApiInfo);
        }
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }
}
