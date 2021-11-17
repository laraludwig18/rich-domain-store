using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RichDomainStore.Catalog.Application.Services;
using RichDomainStore.Catalog.Data;
using RichDomainStore.Catalog.Data.Repositories;
using RichDomainStore.Catalog.Domain.Events;
using RichDomainStore.Catalog.Domain.Interfaces;
using RichDomainStore.Catalog.Domain.Services;
using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Core.Messages.CommonMessages.Notifications;
using RichDomainStore.Sales.Application.Commands;
using RichDomainStore.Sales.Application.Handlers;
using RichDomainStore.Sales.Application.Queries;
using RichDomainStore.Sales.Data;
using RichDomainStore.Sales.Data.Repositories;
using RichDomainStore.Sales.Domain.Interfaces;

namespace RichDomainStore.API.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            // Mediator
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // Notifications
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Catalog
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductAppService, ProductAppService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<CatalogContext>();

            services.AddScoped<INotificationHandler<LowProductInStockEvent>, LowProductInStockEventHandler>();

            // Sales
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IGetCustomerOrdersQuery, GetCustomerOrdersQuery>();
            services.AddScoped<IGetCustomerCartQuery, GetCustomerCartQuery>();
            services.AddScoped<SalesContext>();

            services.AddScoped<IRequestHandler<AddOrderItemCommand, bool>, AddOrderItemCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateOrderItemCommand, bool>, UpdateOrderItemCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveOrderItemCommand, bool>, RemoveOrderItemCommandHandler>();
            services.AddScoped<IRequestHandler<ApplyVoucherCommand, bool>, ApplyVoucherCommandHandler>();
        }
    }
}