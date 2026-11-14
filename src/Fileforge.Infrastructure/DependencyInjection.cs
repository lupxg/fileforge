using Fileforge.Application.Abstractions;
using Fileforge.Application.Features.Documents.Processing;
using Fileforge.Application.Interfaces;
using Fileforge.Infrastructure.Embeddings;
using Fileforge.Infrastructure.FileProcessing;
using Fileforge.Infrastructure.FileStorage;
using Fileforge.Infrastructure.Persistence;
using Fileforge.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fileforge.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
    {
        var connectionString = configuration.GetConnectionString("FileForgeConnection") ?? throw new InvalidOperationException("Connection string not found");

        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IChunkRepository, ChunkRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IDocumentTextExtractor, PlainTextDocumentExtractor>();
        services.AddSingleton<ITextChunker, FixedSizeTextChunker>();
        services.AddDbContext<FileForgeDbContext>(options => ConfigureDatabase(options, connectionString));
        var storageRoot = configuration["FileStorage:RootPath"] ?? Path.Combine(AppContext.BaseDirectory, "App_Data", "uploads");
        services.AddSingleton<IFileStorage>(_ => new LocalFileStorage(storageRoot));

        return services;
    }


    public static void ConfigureDatabase(DbContextOptionsBuilder options, string connectionString)
    {
        options.UseNpgsql(connectionString, npgsql => 
        {
            npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "fileforge");
            npgsql.UseVector(); 
        });
    }

    public static IServiceCollection AddOpenAIEmbeddings(this IServiceCollection services, IConfiguration configuration)
    {
        var apiKey = configuration["OPENAI_API_KEY"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("The OPENAI_API_KEY environment variable is not configured.");
        }

        #pragma warning disable

            services.AddOpenAIEmbeddingGenerator(
                modelId: OpenAIEmbeddingService.ModelName,
                apiKey: apiKey,
                dimensions: OpenAIEmbeddingService.Dimensions
            );

        #pragma warning restore

        services.AddTransient<IEmbeddingService, OpenAIEmbeddingService>();

        return services;
    }
}
