using Infrastructure.Context;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.UnitTests
{
    public class MeterReadingServiceTests
    {
        private MeterReadingService _meterReadingService;
        private AppDatabaseContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;

            _context = new AppDatabaseContext(options);
            _context.Database.EnsureCreated();

            _meterReadingService = new MeterReadingService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureCreated();
            _context.Dispose();
        }

        [Test]
        public async Task AddMeterReading_ShouldAddReading_WhenPopulated()
        {
            // Arrange
            MeterReading meterReading = new(){ 
                MeterReadingId = 1, 
                AccountId = 1, 
                MeterReadingDateTime = DateTime.UtcNow, 
                MeterReadValue = 12345 
            };

            // Act
            await _meterReadingService.AddMeterReading(meterReading);
            await _meterReadingService.SaveChangesAsync();
            var result = _context.MeterReading.Count();

            // Act & Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task AddMeterReading_ShouldThrowAnException_WhenMeterReadingNull()
        {
            // Arrange
            MeterReading meterReading = null;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await _meterReadingService.AddMeterReading(meterReading));
        }
    }
}
