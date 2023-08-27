using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FormulaOne.Entities.DbSet;
using FormulaOne.Entities.DTOs;
using FormulaOne.Entities.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace FormulaOne.Tests.Tests;

public class DriverTests : IClassFixture<AppTestsFixtures>
{
    private readonly AppTestsFixtures _fixture;
    private readonly ITestOutputHelper _testOutput;

    public DriverTests(AppTestsFixtures fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _testOutput = output;
    }

    [Fact]
    public async Task Should_ReturnOk_When_AskingForListOfDrivers()
    {
        var response = await _fixture.HttpClient.GetAsync("api/drivers");
        string responseString = await response.Content.ReadAsStringAsync();
        List<Driver>? jsonResponse = (JsonSerializer.Deserialize<List<Driver>>(responseString));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Single(jsonResponse!);
    }

    [Theory]
    [InlineData("Louis", "Hamilton", 3, "1991-02-07")]
    public async Task Should_CreateDriver(string firstName, string lastName, int driverNumber, string dateOfBirth)
    {
        var response = await _fixture
            .HttpClient
            .PostAsJsonAsync(
                "api/drivers",
                new CreateDriverRequest
                {
                    FirstName = firstName,
                    LastName = lastName,
                    DriverNumber = driverNumber,
                    DateOfBirth = DateTime.Parse(dateOfBirth)
                }
            );

        var createdModel = _fixture
            .Context
            .Drivers
            .OrderByDescending(
                d => d.CreatedAt
            ).FirstOrDefault();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(createdModel);
        Assert.Equal(firstName, createdModel.FirstName);
    }

    [Fact]
    public async void Should_ReturnDriverDetails()
    {
        var driver = _fixture.Context.Drivers.FirstOrDefault();

        var response = await _fixture.HttpClient.GetAsync($"api/Drivers/{driver!.Id}");
        var responseString = await response.Content.ReadAsStringAsync();
        var responseJson = JsonSerializer.Deserialize<DriverResource>(responseString,
            new JsonSerializerOptions() { PropertyNamingPolicy = null }
        );

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseJson);
        Assert.Equal(driver.FirstName, responseJson.firstName);
    }

    [Theory]
    [InlineData("Louis", "Hamilton", 3, "1991-02-07")]
    public async Task Should_UpdateDriver(string newFirstName, string newLastName, int newDriverNumber,
        string newDateOfBirth)
    {
        var driver = _fixture.Context.Drivers.FirstOrDefault();

        var response = await _fixture.HttpClient.PatchAsJsonAsync($"api/drivers/{driver!.Id}", new
        {
            FirstName = newFirstName,
            LastName = newLastName,
            DateOfBirth = DateTime.Parse(newDateOfBirth),
            DriverNumber = newDriverNumber
        });
        await _fixture.Context.Entry(driver).ReloadAsync();

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(driver.FirstName, newFirstName);
        Assert.Equal(driver.LastName, newLastName);
    }

    [Fact]
    public async Task Should_DeleteDriver()
    {
        var driver = await _fixture.Context.Drivers.Where(d => d.DeletedAt == null).FirstOrDefaultAsync();
        var oldDriverCount = await _fixture.Context.Drivers.Where(d => d.DeletedAt == null).CountAsync();

        var response = await _fixture.HttpClient.DeleteAsync($"api/drivers/{driver!.Id}");

        var newDriverCount = await _fixture.Context.Drivers.Where(d => d.DeletedAt == null).CountAsync();
        await _fixture.Context.Entry(driver).ReloadAsync();


        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(oldDriverCount - 1, newDriverCount);
        Assert.NotNull(driver.DeletedAt);
    }
}