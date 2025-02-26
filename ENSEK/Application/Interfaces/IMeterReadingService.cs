using Domain.Entities;
using Domain.Models;

namespace Application.Interfaces;

public interface IMeterReadingService
{
    /// <summary>
    /// Return collection of MeterReading records by account
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    Task<List<MeterReading>> GetMeterReadingsByAccountId(int accountId);

    /// <summary>
    /// Add MeterReading record
    /// </summary>
    /// <param name="meterReading"></param>
    /// <returns></returns>
    Task AddMeterReading(MeterReading meterReading);

    /// <summary>
    /// Save Changes<para/>
    /// TODO - I think a better approach would be to have a repository and a generic 
    /// base repository for CRUD functionality that this service could call but I think 
    /// this is fine given time constraints for the test.
    /// </summary>
    /// <returns></returns>
    Task SaveChangesAsync();
}
