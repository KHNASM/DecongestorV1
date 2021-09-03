using Decongestor.Domain;

namespace Decongestor.DataAccess
{
    public class ChargeCalculatorSourceData
    {
        public ChargeCalculatorSourceData(ChargeMatrix[] chargeMatrices, VehicleType[] vehicleTypes)
        {
            ChargeMatrices = chargeMatrices;
            VehicleTypes = vehicleTypes;
        }

        public ChargeMatrix[] ChargeMatrices { get; }

        public VehicleType[] VehicleTypes { get; }
    }
}
