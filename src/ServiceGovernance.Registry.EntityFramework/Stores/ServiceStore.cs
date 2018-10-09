using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceGovernance.Registry.EntityFramework.Mapping;
using ServiceGovernance.Registry.Models;
using ServiceGovernance.Registry.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceGovernance.Registry.EntityFramework.Stores
{
    /// <summary>
    /// Implementation of <see cref="IServiceStore"/> that uses EntityFramework
    /// </summary>
    public class ServiceStore : IServiceStore
    {
        private readonly IRegistryDbContext _context;
        private readonly ILogger<ServiceStore> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceStore"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public ServiceStore(IRegistryDbContext context, ILogger<ServiceStore> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        /// <summary>
        /// Gets the service by id
        /// </summary>
        /// <param name="serviceId">The identifier to find the service</param>
        /// <returns></returns>
        public Task<Service> FindByServiceIdAsync(string serviceId)
        {
            var service = _context.Services
                .Include(s => s.Endpoints)
                .AsNoTracking()
                .FirstOrDefault(s => s.ServiceId == serviceId);
            var model = service?.ToModel();

            _logger.LogDebug("{serviceId} found in database: {serviceIdFound}", serviceId, model != null);

            return Task.FromResult(model);
        }

        /// <summary>
        /// Get all services
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Service>> GetAllAsync()
        {
            var services = _context.Services
               .Include(s => s.Endpoints)
               .AsNoTracking();

            IEnumerable<Service> models = services.ToModelList();

            return Task.FromResult(models);
        }

        /// <summary>
        /// Removes a service from the store
        /// </summary>
        /// <param name="serviceId">The service identifier.</param>
        /// <returns></returns>
        public async Task RemoveAsync(string serviceId)
        {
            var existing = _context.Services.FirstOrDefault(x => x.ServiceId == serviceId);
            if (existing != null)
            {
                _logger.LogDebug("removing {serviceId} service from database", serviceId);

                _context.Services.Remove(existing);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogInformation("exception removing {serviceId} service from database: {error}", serviceId, ex.Message);
                }
            }
            else
            {
                _logger.LogDebug("no {serviceId} service found in database", serviceId);
            }
        }

        /// <summary>
        /// Stores the service
        /// </summary>
        /// <param name="service">The service to store.</param>
        /// <returns></returns>
        public async Task StoreAsync(Service service)
        {
            var existing = _context.Services.SingleOrDefault(x => x.ServiceId == service.ServiceId);
            if (existing == null)
            {
                _logger.LogDebug("{serviceId} not found in database", service.ServiceId);

                var entity = service.ToEntity();
                _context.Services.Add(entity);
            }
            else
            {
                _logger.LogDebug("{serviceId} found in database", service.ServiceId);

                service.UpdateEntity(existing);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning("exception updating {serviceId} service in database: {error}", service.ServiceId, ex.Message);
            }
        }
    }
}
