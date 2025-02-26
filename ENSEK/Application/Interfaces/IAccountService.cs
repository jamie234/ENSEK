using Domain.Entities;

namespace Application.Interfaces;

public interface IAccountService
{
    /// <summary>
    /// Get Account record if it exists
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    Task<Account?> GetAccount(int accountId);
}
