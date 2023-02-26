using Raccoon.Ninja.Domain.Entities;
using Raccoon.Ninja.Domain.Enums;
using Raccoon.Ninja.Domain.Interfaces.Managers;
using Raccoon.Ninja.Domain.Interfaces.Repositories;
using Raccoon.Ninja.Domain.Interfaces.Services;
using Raccoon.Ninja.Domain.Validators;
using Raccoon.Ninja.Test.Helpers.Generators;
using Raccoon.Ninja.Test.Helpers.Helpers;

namespace Raccoon.Ninja.Services.Services;

public class ProductsService : IProductsService
{
    private readonly IEventManager _eventManager;
    private readonly IProductRepository _productRepository;

    public ProductsService(IEventManager eventManager, IProductRepository productRepository)
    {
        _eventManager = eventManager;
        _productRepository = productRepository;
    }

    public IList<Product> Get()
    {
        return _productRepository.Get();
    }

    public Product Get(Guid id)
    {
        id.EnsureIsValidForId();
        return _productRepository.Get(id);
    }

    public Product Add(Product newProduct)
    {
        newProduct.EnsureIsValidForInsert();
        var inserted = _productRepository.Add(newProduct);
        _eventManager.Notify(EventType.ProductsChanged);
        return inserted;
    }

    public Product Update(Guid productId, IDictionary<string, object> parsed)
    {
        var product = _productRepository.Get(productId);

        if (product == null) return null;

        var prepareForUpdate = new Product(product, parsed);
        var updated = _productRepository.Update(prepareForUpdate);
        _eventManager.Notify(EventType.ProductsChanged);
        return updated;
    }

    public void PopulateDevDb(int? quantity, int? toArchive)
    {
        var qty = quantity ?? 100;
        var toArchiveQty = toArchive ?? 10;

        var inserted = new List<Product>();
        foreach (var product in ProductGenerator.Generate(qty, false))
        {
            var insertedProduct = _productRepository.Add(product);
            inserted.Add(insertedProduct);
            Console.WriteLine($"Inserted: {insertedProduct}");
        }

        for (var i = 0; i < toArchiveQty; i++)
        {
            var prodToArchive = inserted.RandomPick();
            prodToArchive.ArchiveProduct();
            _productRepository.Update(prodToArchive);
        }
    }
}