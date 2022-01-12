using GoodNoodle.Domain.Interfaces;
using GoodNoodle.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace GoodNoodle.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly GoodNoodleContext _context;

    public UnitOfWork(GoodNoodleContext context)
    {
        _context = context;
    }

    public async Task<bool> CommitAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException dbUpdateException)
        {
            Console.WriteLine(dbUpdateException);
            return false;
        }
    }

    public async void Dispose()
    {
        await DisposeAsync(true);
    }

    protected virtual async Task DisposeAsync(bool disposing)
    {
        if (disposing)
        {
            await _context.DisposeAsync();
        }
    }
}
