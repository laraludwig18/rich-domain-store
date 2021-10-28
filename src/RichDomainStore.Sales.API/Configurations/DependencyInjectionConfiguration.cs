using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RichDomainStore.Core.Bus;
using RichDomainStore.Sales.Application.Commands;
using RichDomainStore.Sales.Application.Handlers;

namespace RichDomainStore.Sales.API.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            services.AddScoped<IRequestHandler<AddOrderItemCommand, bool>, AddOrderItemCommandHandler>();
        }
    }
}