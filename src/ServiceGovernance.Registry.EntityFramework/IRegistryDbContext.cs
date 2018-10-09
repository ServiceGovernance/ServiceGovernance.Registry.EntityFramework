using Microsoft.EntityFrameworkCore;
using ServiceGovernance.Registry.EntityFramework.Entities;
using System.Threading.Tasks;

namespace ServiceGovernance.Registry.EntityFramework
{
    /// <summary>
    /// Abstraction for the registry db context
    /// </summary>
    public interface IRegistryDbContext
    {
        /// <summary>
        /// Gets or sets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        DbSet<Service> Services { get; set; }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}
