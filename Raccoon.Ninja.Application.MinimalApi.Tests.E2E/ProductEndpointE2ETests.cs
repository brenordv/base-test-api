using System.Net;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Raccoon.Ninja.Domain.Enums;
using Raccoon.Ninja.Domain.Models;
using Raccoon.Ninja.Test.Helpers.Generators;

namespace Raccoon.Ninja.Application.MinimalApi.Tests.E2E;

public class ProductEndpointE2ETests
{
    private const string ApiProductsEndpoint = "/api/products";
    private static readonly TimeSpan AcceptableDelay = TimeSpan.FromMilliseconds(500);

    [Fact]
    public async Task GetProducts_ReturnsOk()
    {
        // Arrange
        await using var factory = new WebApplicationFactory<Program>();
        await factory.PopulateProducts(10, 0);
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync(ApiProductsEndpoint);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddProduct_ReturnsCreated()
    {
        // Arrange
        await using var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        var (payload, newProd) = GenerateAddProductPayload();

        // Act
        var response = await client.PostAsync(ApiProductsEndpoint, payload);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Content.Headers.ContentType.Should().NotBeNull();
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
        response.Content.Headers.ContentType.CharSet.Should().Be("utf-8");

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();

        var createdProd = JsonConvert.DeserializeObject<ProductModel>(content);
        createdProd.Should().NotBeNull();
        createdProd.Id.Should().NotBeEmpty();
        createdProd.Name.Should().Be(newProd.Name);
        createdProd.Description.Should().Be(newProd.Description);
        createdProd.Company.Should().Be(newProd.Company);
        createdProd.SuggestedPrice.Should().Be(newProd.SuggestedPrice);
        createdProd.Tier.Should().Be(newProd.Tier);
        createdProd.ArchivedAt.Should().BeNull();
        createdProd.CreatedAt.Should().BeCloseTo(DateTime.Now, AcceptableDelay);
        createdProd.ModifiedAt.Should().BeCloseTo(DateTime.Now, AcceptableDelay);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsOk()
    {
        // Arrange
        await using var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        var (payload, newProd) = GenerateAddProductPayload();

        // Since tier is random, we need to make sure it's not the same as the updated one.
        newProd.Tier = ProductTier.A;

        var createdProd = await factory.AddProduct(payload, newProd);
        var updateProd = new UpdateProductModel
        {
            Id = createdProd.Id,
            Name = "Updated name",
            Description = "Updated description",
            Company = "Updated company",
            SuggestedPrice = 100m,
            Tier = ProductTier.C
        };

        var updatePayload =
            new StringContent(JsonConvert.SerializeObject(updateProd), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PutAsync(ApiProductsEndpoint, updatePayload);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType.Should().NotBeNull();
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
        response.Content.Headers.ContentType.CharSet.Should().Be("utf-8");

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();

        var updatedProdResponse = JsonConvert.DeserializeObject<ProductModel>(content);
        updatedProdResponse.Should().NotBeNull();
        updatedProdResponse.Id.Should().Be(createdProd.Id);
        updatedProdResponse.Name.Should().Be(updateProd.Name);
        updatedProdResponse.Description.Should().Be(updateProd.Description);
        updatedProdResponse.Company.Should().Be(updateProd.Company);
        updatedProdResponse.SuggestedPrice.Should().Be(updateProd.SuggestedPrice);
        updatedProdResponse.Tier.Should().Be(updateProd.Tier);
        updatedProdResponse.ArchivedAt.Should().BeNull();
        updatedProdResponse.CreatedAt.Should().BeCloseTo(createdProd.CreatedAt, AcceptableDelay);
        updatedProdResponse.ModifiedAt.Should().BeCloseTo(DateTime.Now, AcceptableDelay);
        updatedProdResponse.ModifiedAt.Should().BeAfter(createdProd.ModifiedAt);
    }

    [Fact]
    public async Task GetProductById_ReturnsOk()
    {
        // Arrange
        await using var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        var (payload, newProd) = GenerateAddProductPayload();

        var createdProd = await factory.AddProduct(payload, newProd);

        // Act
        var response = await client.GetAsync("/api/products/" + createdProd.Id);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType.Should().NotBeNull();
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
        response.Content.Headers.ContentType.CharSet.Should().Be("utf-8");

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();

        var productFetched = JsonConvert.DeserializeObject<ProductModel>(content);
        productFetched.Should().NotBeNull();
        productFetched.Should().BeEquivalentTo(createdProd, options => options
            .Excluding(p => p.ModifiedAt).Excluding(p => p.CreatedAt));

        // Had to do this to account for the fact that the database stores the date with a precision of 1ms.
        productFetched.CreatedAt.Should().BeCloseTo(createdProd.CreatedAt, AcceptableDelay);
        productFetched.ModifiedAt.Should().BeCloseTo(createdProd.ModifiedAt, AcceptableDelay);
    }

    #region Test Helpers

    private static (StringContent payload, AddProductModel model) GenerateAddProductPayload()
    {
        var newProd = AddProductModelGenerator.Generate(1).Single();
        var newProdJson = JsonConvert.SerializeObject(newProd);
        var payload = new StringContent(newProdJson, Encoding.UTF8, "application/json");
        return (payload, newProd);
    }

    #endregion
}