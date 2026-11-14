using Fileforge.Application;
using Fileforge.Infrastructure;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

# region custom services
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddOpenAIEmbeddings(builder.Configuration);
#endregion

builder.Services.AddApiVersioning(options => 
{
    options.DefaultApiVersion = new ApiVersion(1,0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
    .AddApiExplorer(config => 
    {
        config.GroupNameFormat = "'v'VVV";
        config.SubstituteApiVersionInUrl = true;
    });

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => 
    {
        IReadOnlyList<ApiVersionDescription> descriptors = app.DescribeApiVersions();

        foreach (ApiVersionDescription description in descriptors)
        {
            string url = $"/swagger/{description.GroupName}/swagger.json";
            string name = description.GroupName.ToUpperInvariant();

            options.SwaggerEndpoint(url, name);
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
