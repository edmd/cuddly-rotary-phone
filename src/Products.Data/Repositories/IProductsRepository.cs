using Products.Data.Persistence.Entities;

namespace Products.Data
{
    public interface IProductsRepository
    {
        Task<int?> Add(Product product);

        Task<Product?> Get(int sku);

        Task<Product?> Get(string name);

        Task<List<Product>> GetAll();

        Task<bool> Update(Product product);
    }
}