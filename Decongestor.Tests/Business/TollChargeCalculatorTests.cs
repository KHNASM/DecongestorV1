using Decongestor.Business;
using Decongestor.Configuration;
using Decongestor.DataAccess;
using Decongestor.Domain;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using TollCharge = Decongestor.Business.TollCharge;

namespace Decongestor.Tests.Business
{
    [TestClass]
    public class TollChargeCalculatorTests
    {
        private Mock<IOptionsMonitor<ApplicationSettings>> _appSettingMonitorMock;
        private Mock<IChargeCalculatorSourceDataAccess> _dataAccessMock;
        private Mock<IDateTimeHelper> _dateTimeHelperMock;

        private TollChargeCalculator _calculator;
        private ApplicationSettings _mockSettings;

        [TestInitialize]
        public void InitializeUnitTest()
        {
            _mockSettings = new ApplicationSettings();
            _appSettingMonitorMock = new Mock<IOptionsMonitor<ApplicationSettings>>();
            _appSettingMonitorMock.Setup(m => m.CurrentValue).Returns(_mockSettings);

            _dataAccessMock = new Mock<IChargeCalculatorSourceDataAccess>();

            _dateTimeHelperMock = new Mock<IDateTimeHelper>();
            _dateTimeHelperMock.Setup(m => m.UtcToLocalTime(It.IsAny<DateTime>())).Returns<DateTime>(d => d);

            _calculator = new TollChargeCalculator(_appSettingMonitorMock.Object, _dataAccessMock.Object, _dateTimeHelperMock.Object);
        }

        [TestMethod]
        public void CalculateCharge_WhenTodayIsExemptedDay_ChargesZero()
        {
            // Arrange
            ConfigureExemptDays(DayOfWeek.Saturday, DayOfWeek.Sunday);

            // Act
            DateTime date1 = new(2021, 9, 4, 11, 0, 0); // regional setting/locale formats agnostic date initialization
            var date2 = date1.AddDays(1);
            var vehicleId = "XYZ9999";

            TollCharge actual1 = _calculator.CalculateCharge(vehicleId, date1);
            TollCharge actual2 = _calculator.CalculateCharge(vehicleId, date2);

            // Assert
            Assert.AreEqual(0, actual1.Charge, "Assertion on Charge failed.");
            Assert.IsTrue(actual1.Remarks.Contains("Saturday is exempted from Toll Charge"), "Assertion on remarks failed");

            Assert.AreEqual(0, actual2.Charge, "Assertion on Charge failed.");
            Assert.IsTrue(actual2.Remarks.Contains("Sunday is exempted from Toll Charge"), "Assertion on remarks failed");
        }

        [TestMethod]
        public void CalculateCharge_WhenTodayIsExemptedDay_ResultHasCorrectVehicleId()
        {
            // Arrange
            ConfigureExemptDays(DayOfWeek.Monday, DayOfWeek.Tuesday);

            // Act
            var date1 = new DateTime(2021, 9, 6, 12, 0, 0);

            TollCharge actual = _calculator.CalculateCharge("XYZ9999", date1);

            // Assert
            Assert.AreEqual("XYZ9999", actual.VehicleId, "Assertion on VehicleId failed.");
        }

        [TestMethod]
        public void CalculateCharge_WhenTodayisHoliday_ChargesZero()
        {
            // Arrange
            ConfigureExemptDays();
            _dataAccessMock.Setup(m => m.IsHolidayOn(It.IsAny<DateTime>())).Returns(true);

            var date = new DateTime(2021, 9, 7, 11, 0, 0);

            // Act
            TollCharge actual = _calculator.CalculateCharge("XYZ1234", date);

            // Assert
            Assert.AreEqual(0, actual.Charge, "Assertion on Charge on Holiday failed");
            Assert.IsTrue(actual.Remarks.Contains("is Holiday"), "Assertion on remarks failed");
        }

        [TestMethod]
        public void CalculateCharge_WhenVehicleDoesNotExist_ReturnsNull()
        {
            // Arrange
            ConfigureExemptDays();
            _dataAccessMock.Setup(m => m.IsHolidayOn(It.IsAny<DateTime>())).Returns(false);
            _dataAccessMock.Setup(m => m.GetVehicleWithTypeAndLastCharge(It.IsAny<string>())).Returns((Vehicle)null);

            // Act
            var actual = _calculator.CalculateCharge("any-id", DateTime.Now);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void CalculateCharge_VehicleTypeDailyChargeCapIsZero_ChargesZero()
        {
            // Arrange
            var mockVehicle = new Vehicle
            {
                VehicleType = new VehicleType
                {
                    DailyChargeCap = 0
                }
            };

            ConfigureExemptDays();
            _dataAccessMock.Setup(m => m.IsHolidayOn(It.IsAny<DateTime>())).Returns(false);
            _dataAccessMock.Setup(m => m.GetVehicleWithTypeAndLastCharge(It.IsAny<string>())).Returns(mockVehicle);

            // Act
            var actual = _calculator.CalculateCharge("any-id", DateTime.Now);

            // Assert
            Assert.AreEqual(0, actual.Charge, "Assertion on Charge failed.");
            Assert.IsTrue(actual.Remarks.Contains("is an exempted Vehical Type"), "Assertion on remarks failed");
        }// Exempted Vehicles, exempted vehicles have 0 daily charge cap

        [TestMethod]
        public void CalculateCharge_WhenVehicleReEntersWithinExemptedReEntryPeriod_ChargesZero()
        {
            // Arrange
            var mockVehicle = new Vehicle
            {
                VehicleType = new VehicleType
                {
                    DailyChargeCap = 100
                },
                TollEntries = new[]
                {
                    new TollEntry{ EnteredAtUtc = new(2021, 9, 3, 12, 0, 0) }
                }
            };

            _mockSettings.ReEntryExemptionPeriod = TimeSpan.FromHours(1);

            ConfigureExemptDays();
            _dataAccessMock.Setup(m => m.IsHolidayOn(It.IsAny<DateTime>())).Returns(false);
            _dataAccessMock.Setup(m => m.GetVehicleWithTypeAndLastCharge(It.IsAny<string>())).Returns(mockVehicle);

            // Act
            var date = new DateTime(2021, 9, 3, 12, 30, 0);
            var actual = _calculator.CalculateCharge("xyz", date);

            // Assert
            Assert.AreEqual(0, actual.Charge, "Assertion on Charge failed.");
            Assert.IsTrue(actual.Remarks.Contains("Vehicle re-entered within the exempted re-entry time"), "Assertion on remarks failed");
        }

        [TestMethod]
        public void CalculateCharge_WhenDailyChargeCapReached_ChargesZero()
        {
            // Arrange
            var mockVehicle = new Vehicle
            {
                VehicleType = new VehicleType
                {
                    DailyChargeCap = null
                },
                TollEntries = new[]
                {
                    new TollEntry{ EnteredAtUtc = new DateTime(2021, 9, 3, 12, 0, 0) }
                }
            };

            _mockSettings.ReEntryExemptionPeriod = TimeSpan.FromHours(1);
            _mockSettings.DefaultDailyChargeCap = 60;

            ConfigureExemptDays();
            _dataAccessMock.Setup(m => m.IsHolidayOn(It.IsAny<DateTime>())).Returns(false);
            _dataAccessMock.Setup(m => m.GetVehicleWithTypeAndLastCharge(It.IsAny<string>())).Returns(mockVehicle);
            _dataAccessMock.Setup(m => m.GetTotalTollCharge(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(60);

            // Act
            var date = new DateTime(2021, 9, 3, 13, 30, 0);
            var actual = _calculator.CalculateCharge("xyz", date);

            // Assert
            Assert.AreEqual(0, actual.Charge, "Assertion on Charge failed.");
            Assert.IsTrue(actual.Remarks.Contains("Daily charge cap of"), "Assertion on remarks failed");
        }

        [TestMethod]
        public void CalculateCharge_WhenChargeWouldExceedDailyChargeCap_ChargesUptoCap()
        {
            // Arrange
            var mockVehicle = new Vehicle
            {
                VehicleType = new VehicleType
                {
                    DailyChargeCap = null
                },
                TollEntries = new[]
                {
                    new TollEntry{ EnteredAtUtc = new DateTime(2021, 9, 3, 12, 0, 0) }
                }
            };

            _mockSettings.ReEntryExemptionPeriod = TimeSpan.FromHours(1);
            _mockSettings.DefaultDailyChargeCap = 60;
            _mockSettings.DefaultTollCharges = new[]
            {
                new Configuration.TollCharge
                {
                    FromTimeOfDayInclusive =  TimeSpan.FromHours(13),
                    ToTimeOfDayExclusive = TimeSpan.FromHours(14),
                    Charge = 20
                }
            };

            ConfigureExemptDays();
            _dataAccessMock.Setup(m => m.IsHolidayOn(It.IsAny<DateTime>())).Returns(false);
            _dataAccessMock.Setup(m => m.GetVehicleWithTypeAndLastCharge(It.IsAny<string>())).Returns(mockVehicle);
            _dataAccessMock.Setup(m => m.GetTotalTollCharge(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(45);

            // Act
            var date = new DateTime(2021, 9, 3, 13, 30, 0);
            var actual = _calculator.CalculateCharge("xyz", date);

            // Assert
            Assert.AreEqual(15, actual.Charge, "Assertion on Charge failed.");

            Assert.IsTrue(actual.Remarks.Contains("to keep within daily charge cap of"), "Assertion on remarks failed");
        }

        private void ConfigureExemptDays(params DayOfWeek[] days)
        {
            _mockSettings.ExemptedWeekDays = days;
        }
    }
}
