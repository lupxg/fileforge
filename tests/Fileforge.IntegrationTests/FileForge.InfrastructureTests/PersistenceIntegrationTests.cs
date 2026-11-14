using Fileforge.Infrastructure.Persistence;
using Fileforge.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fileforge.IntegrationTests.FileForge.InfrastructureTests;

public sealed class PersistenceIntegrationTests(PostgresFixture fixture) : IClassFixture<PostgresFixture>
{
    [Fact]
    public async Task Database_ShouldBeAccessible()
    {
        using var scope = fixture.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<FileForgeDbContext>();

        var canConnect = await db.Database.CanConnectAsync(TestContext.Current.CancellationToken);

        Assert.True(canConnect);
    }
}