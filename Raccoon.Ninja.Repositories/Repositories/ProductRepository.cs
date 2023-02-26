using LiteDB;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Raccoon.Ninja.Domain.Config;
using Raccoon.Ninja.Domain.Entities;
using Raccoon.Ninja.Domain.Interfaces.Repositories;
using Raccoon.Ninja.Repositories.DbHelpers;

namespace Raccoon.Ninja.Repositories.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ILogger<ProductRepository> _logger;
    private readonly ILiteCollection<Product> _collection;

    public ProductRepository(ILogger<ProductRepository> logger, IOptions<AppSettings> appSettings)
    {
        var database = new LiteDatabase(DbFileHelper.FromFile(appSettings.Value.TestDbFolder));
        _collection = database.GetCollection<Product>();
        _logger = logger;
    }

    public IList<Product> Get()
    {
        var products = _collection
            .Query()
            .OrderBy(product => product.CreatedAt)
            .ToList();

        _logger.LogTrace("Returning '{ProdCount}' from database", products.Count);

        return products;
    }

    public Product Get(Guid id)
    {
        var product = _collection
            .Query()
            .Where(product => product.Id == id)
            .FirstOrDefault();

        if (product == null)
            _logger.LogTrace("Product with id '{Id}' not found", id);
        else
            _logger.LogTrace("Returning product with id '{Id}'", id);

        return product;
    }

    public Product Add(Product newProduct)
    {
        var preparedProduct = newProduct with
        {
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };
        var insertedId = (Guid)_collection.Insert(preparedProduct);

        _logger.LogTrace("New product to the database: {ProductName} by {CompanyName} ({Id})", newProduct.Company,
            newProduct.Name, insertedId);

        return preparedProduct with
        {
            Id = insertedId
        };
    }

    public Product Update(Product product)
    {
        var preparedProduct = product with
        {
            ModifiedAt = DateTime.Now,
            Version = product.Version + 1
        };

        if (_collection.Update(preparedProduct))
            _logger.LogTrace("Product updated successfully! Id: {Id}", product.Id);
        else
            _logger.LogTrace("Failed to update product! Id: {Id}", product.Id);

        return preparedProduct;
    }
}