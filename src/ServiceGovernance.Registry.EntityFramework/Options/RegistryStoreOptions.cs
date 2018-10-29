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
        public Action<DbContextOptionsBuilder> ConfigureDbContext { get; set; }

        /// <summary>
        /// Callback in DI resolve the EF DbContextOptions. If set, ConfigureDbContext will not be used.
        /// </summary>        
        public Action<IServiceProvider, DbContextOptionsBuilder> ResolveDbContextOptions { get; set; }

        /// <summary>
        /// Gets or sets the default schema.
        /// </summary>        
        public string DefaultSchema { get; set; }

        /// <summary>
        /// Gets or sets the service table configuration.
        /// </summary>        
        public string Service { get; set; } = "Services";

        /// <summary>
        /// Gets or sets the service endpoint table configuration.
        /// </summary>        
        public string ServiceEndpoint { get; set; } = "ServiceEndpoints";

        /// <summary>
        /// Gets or sets the service ipaddress table configuration.
        /// </summary>        
        public string ServiceIpAddress { get; set; } = "ServiceIpAddresses";

        /// <summary>
        /// Gets or sets the service url table configuration.
        /// </summary>        
        public string ServicePublicUrl { get; set; } = "ServicePublicUrls";
    }
}
