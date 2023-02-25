using AutoMapper;
using Raccoon.Ninja.AppServices.Interfaces;
using Raccoon.Ninja.Domain.Entities;
using Raccoon.Ninja.Domain.Extensions;
using Raccoon.Ninja.Domain.Interfaces.Services;
using Raccoon.Ninja.Domain.Models;

namespace Raccoon.Ninja.AppServices.AppServices;

public class ProductsAppService : IProductsAppService
{
    private readonly IMapper _mapper;
    private readonly IProductsService _productsService;

    public ProductsAppService(IMapper mapper, IProductsService productsService)
    {
        _mapper = mapper;
        _productsService = productsService;
    }

    public IList<ProductModel> Get()
    {
        var products = _productsService.Get();
        return _mapper.Map<IList<Product>, IList<ProductModel>>(products);
    }

    public ProductModel Get(Guid id)
    {
        var product = _productsService.Get(id);
        return _mapper.Map<Product, ProductModel>(product);
    }

    public ProductModel Add(AddProductModel newProductRequest)
    {
        var addedProduct = _productsService.Add(newProductRequest);
        return _mapper.Map<Product, ProductModel>(addedProduct);
    }

    public ProductModel Update(UpdateProductModel product)
    {
        var parsed = product.Parse();
        var updatedProduct = _productsService.Update(product.Id, parsed);
        return _mapper.Map<Product, ProductModel>(updatedProduct);
    }

    public void PopulateDevDb(int? quantity, int? toArchive)
    {
        _productsService.PopulateDevDb(quantity, toArchive);
    }
}