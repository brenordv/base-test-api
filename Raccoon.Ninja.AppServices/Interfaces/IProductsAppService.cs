using Raccoon.Ninja.Domain.Models;

namespace Raccoon.Ninja.AppServices.Interfaces;

public interface IProductsAppService
{
    IList<ProductModel> Get();
    ProductModel Get(Guid id);
    ProductModel Add(AddProductModel newProductRequest);
    ProductModel Update(UpdateProductModel product);
    void PopulateDevDb(int? quantity, int? toArchive);
}