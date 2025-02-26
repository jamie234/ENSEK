using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly AppDatabaseContext _context;

    public AccountService(AppDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetAccount(int accountId)
    {
        return await _context.Account.FindAsync(accountId);
    }
}
