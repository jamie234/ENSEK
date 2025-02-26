using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class AppDatabaseContext : DbContext
{
    public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<Account> Account { get; set; }
    public DbSet<MeterReading> MeterReading { get; set; }
}
