using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ServiceGovernance.Registry.EntityFramework.Entities
{
    [DebuggerDisplay("{Id} - {Url}")]
    public class ServicePublicUrl
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
