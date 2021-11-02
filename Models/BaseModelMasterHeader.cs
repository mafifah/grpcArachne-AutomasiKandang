using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace grpcArachne.Models
{
    public class BaseModelMasterHeader
    {
        public bool? IsDefault { get; set; }
        public bool? IsRVisible { get; set; }
        public bool? IsDVisible { get; set; }
        public string Tag { get; set; }
        public bool? Status { get; set; }
        public DateTimeOffset? WaktuInsert { get; set; }
        public DateTimeOffset? WaktuUpdate { get; set; }
        public long? IdLogHeader { get; set; }
        public long? IdCreator { get; set; }
        public long? IdOperator { get; set; }
        public long? IdValidator { get; set; }
        public string State { get; set; }
        public string Synchronise { get; set; }
    }
}
