using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RichDomainStore.Catalog.Application.Services;
using RichDomainStore.Catalog.Data;
using RichDomainStore.Catalog.Data.Repositories;
using RichDomainStore.Catalog.Domain.Events;
using RichDomainStore.Catalog.Domain.Interfaces;
using RichDomainStore.Catalog.Domain.Services;
using RichDomainStore.Core.Bus;

namespace RichDomainStore.Catalog.API.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductAppService, ProductAppService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<CatalogContext>();
            
            services.AddScoped<INotificationHandler<LowProductInStockEvent>, ProductEventHandler>();
        }
    }
}