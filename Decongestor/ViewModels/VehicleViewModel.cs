using System;

namespace Decongestor.ViewModels
{
    public class VehicleViewModel
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string VehicleTypeDescription { get; set; }

        public TollEntryViewModel[] TollEntries { get; set; } = Array.Empty<TollEntryViewModel>();
    }
}
