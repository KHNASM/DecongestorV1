using Decongestor.DataAccess.Config;
using Decongestor.Domain;
using Decongestor.ViewModels;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Decongestor.Business
{
    public class DecongestorBusiness : IDecongestorBusiness
    {
        private readonly DataContext _dataContext;
        private readonly ITollChargeCalculator _chargeCalculator;

        public DecongestorBusiness(DataContext dataContext, ITollChargeCalculator chargeCalculator)
        {
            if (chargeCalculator is null)
            {
                throw new ArgumentNullException(nameof(chargeCalculator));
            }

            _dataContext = dataContext;
            _chargeCalculator = chargeCalculator;
        }

        public void ApplyCharge(string vehicleId)
        {
            var utcNow = DateTime.UtcNow;

            TollCharge charge = _chargeCalculator.CalculateCharge(vehicleId, utcNow);

            var tollEntry = new TollEntry
            {
                EnteredAtUtc = charge.EntryDateTimeUtc,
                VehicleId = charge.VehicleId,
                Charge = charge.Charge,
                Remarks = charge.Remarks
            };

            _dataContext.TollEntries.Add(tollEntry);
            _dataContext.SaveChanges();
        }

        public VehicleViewModel[] GetAllVehicles()
        {
            var vehicles = _dataContext.Vehicles
                .Include(v => v.TollEntries)
                .Include(v => v.VehicleType)
                .ToArray();

            return vehicles
                .OrderBy(v => v.Id)
                .Select(v => new VehicleViewModel
                {
                    Id = v.Id,
                    Description = v.Description,
                    VehicleTypeDescription = v.VehicleType.Description,
                    TollEntries = v.TollEntries
                        .OrderBy(te => te.Id)
                        .Select(te => new TollEntryViewModel
                        {
                            EnteredAtUtc = te.EnteredAtUtc.ToLocalTime().ToString("ddd dd/MM/yyyy HH:mm:ss"),
                            Charge = te.Charge.ToString("C"),
                            Remarks = te.Remarks
                        })
                        .ToArray()
                })
                .ToArray();
        }
    }
}
