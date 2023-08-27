using FomulaOne.DataService.Data;
using FormulaOne.DataService.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FormulaOne.DataService.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    public IDriverRepository Drivers { get; init; }
    public IAchievementRepository Achievements { get; init; }
    private readonly AppDbContext _context;

    public UnitOfWork(ILoggerFactory loggerFactory, AppDbContext context)
    {
        _context = context;
        var logger = loggerFactory.CreateLogger("logs");

        Drivers = new DriverRepository(logger, context);
    }

    public async Task<bool> CompleteAsync()
    {
        return (await _context.SaveChangesAsync()) > 0;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}