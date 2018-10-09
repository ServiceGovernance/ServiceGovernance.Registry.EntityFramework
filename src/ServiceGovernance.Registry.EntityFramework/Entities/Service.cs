using System.Collections.Generic;
using System.Diagnostics;

namespace ServiceGovernance.Registry.EntityFramework.Entities
{
    [DebuggerDisplay("{Id} ({ServiceId})")]
    public class Service
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a unique service identifier
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// Gets or sets a display name of the service
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the endpoints the service is available on
        /// </summary>
        public List<ServiceEndpoint> Endpoints { get; set; }
    }
}
