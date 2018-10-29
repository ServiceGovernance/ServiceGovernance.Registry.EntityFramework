using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ServiceGovernance.Registry.EntityFramework.Entities;
using System.Collections.Generic;

namespace ServiceGovernance.Registry.EntityFramework
{
    /// <summary>
    /// Extensions to provide helper functions for <see cref="IRegistryDbContext"/>
    /// </summary>
    public static class RegistryDbContextExtensions
    {
        /// <summary>
        /// Creates a query for the services
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IIncludableQueryable<Service, List<ServicePublicUrl>> CreateServiceQuery(this IRegistryDbContext context)
        {
            return context.Services
                            .Include(s => s.Endpoints)
                            .Include(s => s.IpAddresses)
                            .Include(s => s.PublicUrls);
        }
    }
}
