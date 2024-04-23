// ReSharper disable ClassNeverInstantiated.Global

#pragma warning disable CS8618

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using WebApiTemplate.Api;
using Xunit;

namespace WebApiTemplate.IntegrationTests;

public class AppWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public async Task InitializeAsync()
    {
    }

    public new async Task DisposeAsync()
    {
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var typesToRemove = new[] { typeof(DbContextOptions) };

            var toRemove = services.Where(e => typesToRemove.Contains(e.ServiceType)).ToList();
            foreach (var descriptor in toRemove)
            {
                services.Remove(descriptor);
            }

            Program.Container.Options.AllowOverridingRegistrations = true;
        });
    }

    public async Task ResetCache()
    {
    }
}
