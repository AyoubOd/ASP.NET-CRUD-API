using System.Net;

namespace FormulaOne.Tests;

public class DriverTests : IClassFixture<AppTestsFixtures>
{
    private readonly AppTestsFixtures _fixture;

    public DriverTests(AppTestsFixtures fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Should_ReturnOk_When_AskingForListOfDrivers()
    {
        var response = await _fixture.HttpClient.GetAsync("api/drivers");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}