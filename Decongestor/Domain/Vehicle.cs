using System.Collections.Generic;

namespace Decongestor.Domain
{
    public class Vehicle
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public int VehicleTypeId { get; set; }

        public virtual VehicleType VehicleType { get; set; }

        public virtual ICollection<TollEntry> TollEntries { get; set; } = new HashSet<TollEntry>();
    }
}
