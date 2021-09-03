using System.Collections.Generic;

namespace Decongestor.Domain
{
    public class VehicleType
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal? DailyChargeCap { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; } = new HashSet<Vehicle>();
    }
}
