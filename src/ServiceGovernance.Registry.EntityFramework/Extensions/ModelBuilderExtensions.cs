using Microsoft.EntityFrameworkCore;
using ServiceGovernance.Registry.EntityFramework.Entities;
using ServiceGovernance.Registry.EntityFramework.Options;

namespace ServiceGovernance.Registry.EntityFramework.Extensions
{
    /// <summary>
    /// Extension methods to define the database schema for the registry stores
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Configures the service registry model
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <param name="storeOptions">The store options.</param>
        public static void ConfigureServiceRegistry(this ModelBuilder modelBuilder, RegistryStoreOptions storeOptions)
        {
            if (!string.IsNullOrWhiteSpace(storeOptions.DefaultSchema))
                modelBuilder.HasDefaultSchema(storeOptions.DefaultSchema);

            modelBuilder.Entity<Service>(service =>
            {
                service.ToTable(storeOptions.Service);
                service.HasKey(x => x.Id);

                service.Property(x => x.ServiceId).HasMaxLength(200).IsRequired();
                service.Property(x => x.DisplayName).HasMaxLength(1000);

                service.HasIndex(x => x.ServiceId).IsUnique();

                service.HasMany(x => x.Endpoints).WithOne(x => x.Service).HasForeignKey(x => x.ServiceId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ServiceEndpoint>(endpoint =>
            {
                endpoint.ToTable(storeOptions.ServiceEndpoint);
                endpoint.Property(x => x.EndpointUri).HasMaxLength(2000).IsRequired();
            });
        }
    }
}
