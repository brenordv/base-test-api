using Raccoon.Ninja.AppServices.Interfaces;
using Raccoon.Ninja.Domain.Models;

namespace Raccoon.Ninja.Application.MinimalApi.Endpoints;

public static class ProductEndpoints
{
    public static IResult GetProducts(IProductsAppService productsAppService)
    {
        var products = productsAppService.Get();
        return products.Any()
            ? Results.Ok(products)
            : Results.NoContent();
    }

    public static IResult GetProductById(IProductsAppService productsAppService, Guid id)
    {
        var product = productsAppService.Get(id);
        return product is not null
            ? Results.Ok(product)
            : Results.NotFound();
    }

    public static IResult CreateProduct(IProductsAppService productsAppService, AddProductModel product)
    {
        var createdProduct = productsAppService.Add(product);
        return createdProduct is not null
            ? Results.Created(createdProduct.Id.ToString(), createdProduct)
            : Results.BadRequest();
    }

    public static IResult UpdateProduct(IProductsAppService productsAppService, UpdateProductModel product)
    {
        var updatedProduct = productsAppService.Update(product);
        return updatedProduct is not null
            ? Results.Ok(updatedProduct)
            : Results.NotFound();
    }
}