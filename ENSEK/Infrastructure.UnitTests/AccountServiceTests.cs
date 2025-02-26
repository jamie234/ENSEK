using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.UnitTests
{
    public class AccountServiceTests
    {
        private AccountService _accountService;
        private AppDatabaseContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;

            _context = new AppDatabaseContext(options);
            _context.Database.EnsureCreated();

            _accountService = new AccountService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureCreated();
            _context.Dispose();
        }

        [Test]
        public async Task GetAccount_ShouldReturnAccount_WhenItExists()
        {
            // Arrange
            Account account = new() { AccountId = 1, FirstName = "Tommy", LastName = "Test" };
            await _context.Account.AddAsync(account);
            await _context.SaveChangesAsync();

            // Act
            var result = await _accountService.GetAccount(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.FirstName, Is.EqualTo(account.FirstName));
        }

        [Test]
        public async Task GetAccount_ShouldNotReturnAccount_WhenItDoesNotExists()
        {
            // Arrange
            Account account = new() { AccountId = 1, FirstName = "Tommy", LastName = "Test" };
            await _context.Account.AddAsync(account);
            await _context.SaveChangesAsync();

            // Act
            var result = await _accountService.GetAccount(2);

            // Assert
            Assert.IsNull(result);
        }
    }
}