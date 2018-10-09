using System;
using Microsoft.EntityFrameworkCore;

namespace ServiceGovernance.Registry.EntityFramework.Options
{
    /// <summary>
    /// Options for configuring the registry store
    /// </summary>
    public class RegistryStoreOptions
    {
        /// <summary>
        /// Callback to configure the EF DbContext.
        /// </summary>
        /// <value>
        /// The configure database context.
        /// </value>
        public Action<DbContextOptionsBuilder> ConfigureDbContext { get; set; }

        /// <summary>
        /// Callback in DI resolve the EF DbContextOptions. If set, ConfigureDbContext will not be used.
        /// </summary>
        /// <value>
        /// The configure database context.
        /// </value>
        public Action<IServiceProvider, DbContextOptionsBuilder> ResolveDbContextOptions { get; set; }

        /// <summary>
        /// Gets or sets the default schema.
        /// </summary>
        /// <value>
        /// The default schema.
        /// </value>
        public string DefaultSchema { get; set; }

        /// <summary>
        /// Gets or sets the service table configuration.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        public string Service { get; set; } = "Services";

        /// <summary>
        /// Gets or sets the service endpoint table configuration.
        /// </summary>
        /// <value>
        /// The service endpoint.
        /// </value>
        public string ServiceEndpoint { get; set; } = "ServiceEndpoints";
    }
}
