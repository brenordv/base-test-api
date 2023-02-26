using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Raccoon.Ninja.Domain.Models;

namespace Raccoon.Ninja.Application.MinimalApi.Tests.E2E;

/// <summary>
/// Couple of helper methods to populate the database, add products, etc.
/// </summary>
internal static class WebAppExtensions
{
    public static async Task PopulateUsers(this WebApplicationFactory<Program> factory, int quantity, int archive)
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync($"/api/users/dev/populate-db?quantity={quantity}&archive={archive}");
        response.EnsureSuccessStatusCode();
    }

    public static async Task PopulateProducts(this WebApplicationFactory<Program> factory, int quantity, int archive)
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync($"/api/products/dev/populate-db?quantity={quantity}&archive={archive}");
        response.EnsureSuccessStatusCode();
    }

    public static async Task<ProductModel> AddProduct(this WebApplicationFactory<Program> factory,
        StringContent payload, AddProductModel model)
    {
        var client = factory.CreateClient();
        var response = await client.PostAsync("/api/products", payload);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var createdProd = JsonConvert.DeserializeObject<ProductModel>(content);
        return createdProd;
    }
}