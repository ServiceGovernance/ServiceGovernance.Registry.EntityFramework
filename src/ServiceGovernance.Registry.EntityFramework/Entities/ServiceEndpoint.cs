using System.Diagnostics;

namespace ServiceGovernance.Registry.EntityFramework.Entities
{
    [DebuggerDisplay("{Id} - {EndpointUri}")]
    public class ServiceEndpoint
    {
        public int Id { get; set; }
        public string EndpointUri { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
