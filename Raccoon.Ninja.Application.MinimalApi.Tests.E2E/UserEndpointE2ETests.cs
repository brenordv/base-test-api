using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Raccoon.Ninja.Application.MinimalApi.Tests.E2E;

public class UserEndpointE2ETests
{
    private const string ApiUsersEndpoint = "/api/users";
    
    [Fact]
    public async Task GetUsers_ReturnsOk()
    {
        // Arrange
        await using var factory = new WebApplicationFactory<Program>();
        await factory.PopulateUsers(10, 0);
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync(ApiUsersEndpoint);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}