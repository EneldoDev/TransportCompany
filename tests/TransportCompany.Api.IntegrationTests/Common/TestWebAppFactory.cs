using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using TransportCompany.Api;
using TransportCompany.Infrastructure.Common.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TransportCompany.Application.Interfaces;
using TransportCompany.Infrastructure.Trucks;

namespace TransportCompany.Api.IntegrationTests.Common
{
    public class TestWebAppFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
    {
        private TestDatabase _testDb = null;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            _testDb = TestDatabase.CreateAndInitialize();
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<DbContextOptions<CompanyDbContext>>()
                .AddDbContext<CompanyDbContext>((sp, options) => options.UseSqlite(_testDb.Connection));
                services.AddMediatR(options =>
            options.RegisterServicesFromAssembly(typeof(TransportCompany.Application.DependencyInjection).Assembly));
            });
        }

        public void ResetDatabase()
        {
            _testDb.ResetDb();
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public new Task DisposeAsync()
        {
            _testDb.Dispose();

            return Task.CompletedTask;
        }
    }
}