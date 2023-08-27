using FomulaOne.DataService.Data;
using FormulaOne.Entities.DbSet;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace FormulaOne.Tests;

public class AppTestsFixtures : IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    public readonly HttpClient HttpClient;
    public AppDbContext Context;

    public AppTestsFixtures()
    {
        _factory = new AppFactory();
        HttpClient = _factory.CreateClient();
        Context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Drivers.Add(new Driver()
        {
            FirstName = "Driver 1",
            LastName = "Driver 1",
            DriverNumber = 1,
            DateOfBirth = DateTime.UtcNow,
        });
        dbContext.SaveChanges();
    }

    public void Dispose()
    {
        HttpClient.Dispose();
        _factory.Dispose();
    }
}