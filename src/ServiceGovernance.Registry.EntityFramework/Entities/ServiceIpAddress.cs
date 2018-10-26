using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ServiceGovernance.Registry.EntityFramework.Entities
{
    [DebuggerDisplay("{Id} - {IpAddress}")]
    public class ServiceIpAddress
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
