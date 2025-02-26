using Application.Interfaces;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class MeterReadingService : IMeterReadingService
{
    private readonly AppDatabaseContext _context;

    public MeterReadingService(AppDatabaseContext context)
    {
        _context = context;
    }

    public async Task AddMeterReading(MeterReading meterReading)
    {
        await _context.MeterReading.AddAsync(meterReading);
    }

    public async Task<List<MeterReading>> GetMeterReadingsByAccountId(int accountId)
    {
        return await _context.MeterReading.Where(m => m.AccountId == accountId).ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public UploadMeterReadingsResponse UploadMeterReadings(UploadMeterReadingsRequest req)
    {
        throw new NotImplementedException();
    }
}
