using Fileforge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fileforge.Infrastructure.Persistence;

public class FileForgeDbContext(DbContextOptions<FileForgeDbContext> options) : DbContext(options)
{
    // DB sets.
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<DocumentChunk> DocumentChunks => Set<DocumentChunk>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<MessageSource> MessageSources => Set<MessageSource>();
    public DbSet<Conversation> Conversations => Set<Conversation>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresExtension("vector");
        modelBuilder.HasDefaultSchema("fileforge");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileForgeDbContext).Assembly);
    }
}
