using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RichDomainStore.API.Configurations
{
    public static class AutoMapperConfiguration
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var assemblies = new[] { Assembly.Load("RichDomainStore.Catalog.Application") };
            
            services.AddAutoMapper((serviceProvider, mapperConfiguration) => 
                mapperConfiguration.AddMaps(assemblies), assemblies);
        }
    }
}