using Microsoft.EntityFrameworkCore;
using ServiceGovernance.Registry.EntityFramework.Entities;
using ServiceGovernance.Registry.EntityFramework.Extensions;
using ServiceGovernance.Registry.EntityFramework.Options;
using System;
using System.Threading.Tasks;

namespace ServiceGovernance.Registry.EntityFramework
{
    /// <summary>
    /// DbContext for service registry data
    /// </summary>
    /// <seealso cref="IRegistryDbContext" />
    public class RegistryDbContext : RegistryDbContext<RegistryDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="storeOptions">The store options.</param>
        /// <exception cref="ArgumentNullException">storeOptions</exception>
        public RegistryDbContext(DbContextOptions<RegistryDbContext> options, RegistryStoreOptions storeOptions)
            : base(options, storeOptions)
        {
        }
    }

    /// <summary>
    /// DbContext for service registry data
    /// </summary>
    /// <typeparam name="TContext">Type of the DBContext</typeparam>
    public class RegistryDbContext<TContext> : DbContext, IRegistryDbContext where TContext : DbContext, IRegistryDbContext
    {
        private readonly RegistryStoreOptions _storeOptions;

        /// <summary>
        /// Gets or sets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        public DbSet<Service> Services { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="storeOptions">The store options.</param>
        /// <exception cref="ArgumentNullException">storeOptions</exception>
        public RegistryDbContext(DbContextOptions<TContext> options, RegistryStoreOptions storeOptions)
            : base(options)
        {
            _storeOptions = storeOptions ?? throw new ArgumentNullException(nameof(storeOptions));
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureServiceRegistry(_storeOptions);

            base.OnModelCreating(modelBuilder);
        }
    }
}
