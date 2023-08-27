using FormulaOne.Entities.DbSet;
using Microsoft.EntityFrameworkCore;

namespace FomulaOne.DataService.Data;

public class AppDbContext : DbContext
{
    public virtual DbSet<Driver> Drivers { get; set; }
    public virtual DbSet<Achievement> Achievements { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Driver>(entity =>
            {
                entity.HasMany(e => e.Achievements)
                    .WithOne(e => e.Driver)
                    .HasForeignKey(e => e.DriverId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_Achievements_Driver");
            }
        );
    }
}