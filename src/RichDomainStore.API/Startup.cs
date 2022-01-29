using System;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RichDomainStore.API.Configurations;
using RichDomainStore.Catalog.Data;
using RichDomainStore.Core.Extensions;
using RichDomainStore.Payments.Data;
using RichDomainStore.Sales.Data;

namespace RichDomainStore.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddMvcConfiguration();

            services.AddDatabaseConfiguration(Configuration);

            services.AddAutoMapperConfiguration();

            services.AddSwaggerConfiguration();

            services.AddMediatR(typeof(Startup));

            services.AddDependencyInjectionConfiguration();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerSetup();

            ApplyMigrations(app.ApplicationServices);
        }

        private static void ApplyMigrations(IServiceProvider applicationServices)
        {
            var serviceScopeFactory = applicationServices.GetRequiredService<IServiceScopeFactory>();

            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                serviceScope.ServiceProvider.ApplyMigrations<SalesContext>();
                serviceScope.ServiceProvider.ApplyMigrations<CatalogContext>();
                serviceScope.ServiceProvider.ApplyMigrations<PaymentContext>();
            }
        }
    }
}
