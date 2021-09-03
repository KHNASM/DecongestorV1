using Decongestor.ViewModels;

namespace Decongestor.Business
{
    public interface IDecongestorBusiness
    {
        VehicleViewModel[] GetAllVehicles();

        void ApplyCharge(string vehicleId);
    }
}