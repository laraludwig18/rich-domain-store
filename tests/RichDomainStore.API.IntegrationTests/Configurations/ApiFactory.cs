using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using RichDomainStore.API.IntegrationTests.Fixtures;
using RichDomainStore.Catalog.Data;
using RichDomainStore.Core.Extensions;
using RichDomainStore.Payments.Data;
using RichDomainStore.Sales.Data;

namespace RichDomainStore.API.IntegrationTests.Configurations
{
    public class ApiFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private DatabaseFixture DatabaseFixture;
        public ApiFactory(DatabaseFixture databaseFixture)
        {
            DatabaseFixture = databaseFixture;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<TStartup>();
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;

                    var catalogContext = scopedServices.GetRequiredService<CatalogContext>();

                    catalogContext.Database.EnsureDeleted();

                    scopedServices.ApplyMigrations<CatalogContext>();
                    scopedServices.ApplyMigrations<SalesContext>();
                    scopedServices.ApplyMigrations<PaymentContext>();

                    try
                    {
                        CatalogSeed(catalogContext);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"An error occurred seeding the database with test messages. Error: {ex.Message}");
                    }
                }
            });
        }

        private void CatalogSeed(CatalogContext catalogContext)
        {
            catalogContext.Categories.RemoveRange(catalogContext.Categories);
            catalogContext.Products.RemoveRange(catalogContext.Products);

            catalogContext.Categories.AddRange(DatabaseFixture.Categories);
            catalogContext.Products.AddRange(DatabaseFixture.Products);

            catalogContext.SaveChanges();
        }
    }
}