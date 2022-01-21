using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace RichDomainStore.Core.Extensions
{
    public static class ServiceProviderExtension
    {
        public static void ApplyMigrations<TContext>(this IServiceProvider serviceProvider) where TContext : DbContext
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            try
            {
                var dbContext = serviceProvider.GetService<TContext>();

                if (dbContext != null)
                {
                    var stopwatch = Stopwatch.StartNew();

                    Debug.WriteLine("Applying migrations.");

                    dbContext.Database.Migrate();

                    Debug.WriteLine($"Migrations have been applied successfully. Time taken: {stopwatch.ElapsedMilliseconds}ms.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error has occurred while applying migrations. Exception -> {ex}");
            }
        }
    }
}