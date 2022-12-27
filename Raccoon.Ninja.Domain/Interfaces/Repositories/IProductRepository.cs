using Raccoon.Ninja.Domain.Entities;

namespace Raccoon.Ninja.Domain.Interfaces.Repositories;

public interface IProductRepository
{
    IList<Product> Get();
    Product Get(Guid id);
    Product Add(Product newProduct);
    Product Update(Product product);
}