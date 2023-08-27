using FomulaOne.DataService.Data;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FormulaOne.DataService.Repositories;

public class DriverRepository : GenericRepository<Driver>, IDriverRepository
{
    public DriverRepository(ILogger logger, AppDbContext context) : base(logger, context)
    {
    }

    public override async Task<IEnumerable<Driver>> All()
    {
        try
        {
            return await _dbSet.Where(d => d.DeletedAt == null)
                .AsNoTracking()
                .OrderBy(d => d.CreatedAt)
                .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} All function error", typeof(DriverRepository));
            throw;
        }
    }

    public override async void Add(Driver driver)
    {
        _context.Drivers.Add(driver);
    }

    public override Driver? GetById(Guid id)
    {
        return _context
            .Drivers
            .Where(d => d.Id == id)
            .FirstOrDefault(
                d => d.DeletedAt == null
            );
    }

    public override void Update(Driver driver)
    {
        _context.Entry(driver).State = EntityState.Modified;
    }

    public override void Delete(Driver driver)
    {
        driver.DeletedAt = DateTime.UtcNow;
    }
}