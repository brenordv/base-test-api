using Microsoft.AspNetCore.Mvc;
using Raccoon.Ninja.AppServices.Interfaces;
using Raccoon.Ninja.Domain.Models;

namespace Raccoon.Ninja.Application.OldStyleWebApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductsAppService _productsAppService;

    public ProductsController(IProductsAppService productsAppService)
    {
        _productsAppService = productsAppService;
    }

    [HttpGet(Name = "Get all products")]
    [ProducesResponseType(typeof(List<ProductModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Get()
    {
        var products = _productsAppService.Get();
        return products.Any()
            ? Ok(products)
            : NoContent();
    }

    [HttpGet("{id:Guid}", Name = "Get product by Id")]
    [ProducesResponseType(typeof(ProductModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById([FromRoute] Guid id)
    {
        var product = _productsAppService.Get(id);
        return product is not null
            ? Ok(product)
            : NotFound();
    }

    [HttpPost(Name = "Insert new Product")]
    [ProducesResponseType(typeof(ProductModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult AddNewProduct([FromBody] AddProductModel addProductModel)
    {
        var createdProduct = _productsAppService.Add(addProductModel);
        return createdProduct is not null
            ? CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct)
            : BadRequest();
    }

    [HttpPut(Name = "Update existing Product")]
    [ProducesResponseType(typeof(ProductModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateProduct([FromBody] UpdateProductModel updateProductModel)
    {
        var updatedProduct = _productsAppService.Update(updateProductModel);
        return updatedProduct is not null
            ? Ok(updatedProduct)
            : NotFound();
    }
}