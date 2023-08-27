namespace FormulaOne.DataService.Repositories.Interfaces;

public interface IUnitOfWork
{
    IDriverRepository Drivers { get; init; }
    IAchievementRepository Achievements { get; init; }

    Task<bool> CompleteAsync();
}