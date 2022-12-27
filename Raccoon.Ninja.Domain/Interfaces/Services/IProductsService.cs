using Raccoon.Ninja.Domain.Entities;

namespace Raccoon.Ninja.Domain.Interfaces.Services;

public interface IProductsService
{
    IList<Product> Get();
    Product Get(Guid id);
    Product Add(Product newProduct);
    Product Update(Guid productId, IDictionary<string, object> parsed);
    void PopulateDevDb(int? quantity, int? toArchive);
}