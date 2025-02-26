using Application.Interfaces;
using Domain.Entities;
using Moq;
using WebAPI.Controllers;

namespace WebAPI.UnitTests
{
    public class MeterReadingControllerTests
    {
        private MeterReadingController _meterReadingController;
        private readonly Mock<IAccountService> _accountServiceMock = new();
        private readonly Mock<IMeterReadingService> _meterReadingServiceMock = new();

        [SetUp]
        public void Setup()
        {
            _meterReadingController = new MeterReadingController(_accountServiceMock.Object, _meterReadingServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void TryParse_ShouldReturnTrue_WhenInputValid()
        {
            // Arrange
            string input = "2345,22/04/2019 12:25,45522,";
            MeterReading meterReading;

            // Act
            bool validMeterReadingEntry = _meterReadingController.ValidMeterReadingEntry(input, out meterReading);

            // Assert
            Assert.IsTrue(validMeterReadingEntry);
        }

        [Test]
        public void TryParse_ShouldReturnTrue_WhenInputHasTrailingCharactersAfterValidInput()
        {
            // Arrange
            string input = "1241,11/04/2019 09:24,12436,X";
            MeterReading meterReading;

            // Act
            bool validMeterReadingEntry = _meterReadingController.ValidMeterReadingEntry(input, out meterReading);

            // Assert
            Assert.IsTrue(validMeterReadingEntry);
        }

        [Test]
        public void TryParse_ShouldReturnFalse_WhenAccountIsAString()
        {
            // Arrange
            string input = "dasdasd,22/04/2019 12:25,45522,";
            MeterReading meterReading;

            // Act
            bool validMeterReadingEntry = _meterReadingController.ValidMeterReadingEntry(input, out meterReading);

            // Assert
            Assert.IsFalse(validMeterReadingEntry);
        }


        [Test]
        public void TryParse_ShouldReturnFalse_WhenDateProvidedIsInvalid()
        {
            // Arrange
            string input = "2345,22/24/2019 12:25,45522,";
            MeterReading meterReading;

            // Act
            bool validMeterReadingEntry = _meterReadingController.ValidMeterReadingEntry(input, out meterReading);

            // Assert
            Assert.IsFalse(validMeterReadingEntry);
        }


        [Test]
        public void TryParse_ShouldReturnFalse_WhenMeterReadingValueIsNegative()
        {
            // Arrange
            string input = "2345,22/04/2019 12:25,-45522,";
            MeterReading meterReading;

            // Act
            bool validMeterReadingEntry = _meterReadingController.ValidMeterReadingEntry(input, out meterReading);

            // Assert
            Assert.IsFalse(validMeterReadingEntry);
        }
    }
}