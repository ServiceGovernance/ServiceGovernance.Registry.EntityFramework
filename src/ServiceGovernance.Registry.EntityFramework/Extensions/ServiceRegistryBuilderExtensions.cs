using Microsoft.EntityFrameworkCore;
using ServiceGovernance.Registry.Configuration;
using ServiceGovernance.Registry.EntityFramework;
using ServiceGovernance.Registry.EntityFramework.Options;
using ServiceGovernance.Registry.EntityFramework.Stores;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add EntityFramework database persistence to service registry
    /// </summary>
    public static class ServiceRegistryBuilderExtensions
    {
        /// <summary>
        /// Configures EntityFramework implementation of IServiceStore
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IServiceRegistryBuilder AddRegistryStore(this IServiceRegistryBuilder builder,
            Action<RegistryStoreOptions> storeOptionsAction = null)
        {
            return builder.AddRegistryStore<RegistryDbContext>(storeOptionsAction);
        }

        /// <summary>
        /// Configures EntityFramework implementation of IServiceStore.
        /// </summary>
        /// <typeparam name="TContext">The IConfigurationDbContext to use.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IServiceRegistryBuilder AddRegistryStore<TContext>(this IServiceRegistryBuilder builder, Action<RegistryStoreOptions> storeOptionsAction = null)
            where TContext : DbContext, IRegistryDbContext
        {
            builder.Services.AddRegistryDbContext<TContext>(storeOptionsAction);

            builder.AddServiceStore<ServiceStore>();

            return builder;
        }

        /// <summary>
        /// Configures caching for IServiceStore
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IServiceRegistryBuilder AddRegistryStoreCache(this IServiceRegistryBuilder builder)
        {
            builder.AddInMemoryCaching();

            // add the caching decorators
            builder.AddServiceStoreCache<ServiceStore>();

            return builder;
        }

        /// <summary>
        /// Add Registry DbContext to the DI system.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IServiceCollection AddRegistryDbContext(this IServiceCollection services,
            Action<RegistryStoreOptions> storeOptionsAction = null)
        {
            return services.AddRegistryDbContext<RegistryDbContext>(storeOptionsAction);
        }

        /// <summary>
        /// Add Registry DbContext to the DI system.
        /// </summary>        
        /// <typeparam name="TContext">The IRegistryDbContext to use.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IServiceCollection AddRegistryDbContext<TContext>(this IServiceCollection services, Action<RegistryStoreOptions> storeOptionsAction = null)
            where TContext : DbContext, IRegistryDbContext
        {
            var options = new RegistryStoreOptions();
            services.AddSingleton(options);
            storeOptionsAction?.Invoke(options);

            if (options.ResolveDbContextOptions != null)
            {
                services.AddDbContext<TContext>(options.ResolveDbContextOptions);
            }
            else
            {
                services.AddDbContext<TContext>(dbCtxBuilder => options.ConfigureDbContext?.Invoke(dbCtxBuilder));
            }

            services.AddScoped<IRegistryDbContext, TContext>();

            return services;
        }
    }
}
