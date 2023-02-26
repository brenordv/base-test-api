using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Raccoon.Ninja.Domain.Models;

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

    [Fact]
    public async Task GetUsers_WithLimit_ReturnsOk()
    {
        // Arrange
        await using var factory = new WebApplicationFactory<Program>();
        await factory.PopulateUsers(10, 0);
        var client = factory.CreateClient();
        const int expectedUserCount = 5;
        // Act
        var response = await client.GetAsync($"{ApiUsersEndpoint}?limit={expectedUserCount}");

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();

        var usersFetched = JsonConvert.DeserializeObject<IList<UserModel>>(content);
        usersFetched.Should().NotBeNull();
        usersFetched.Count.Should().Be(expectedUserCount);
    }
}