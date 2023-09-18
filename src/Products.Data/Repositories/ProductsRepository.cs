using Microsoft.EntityFrameworkCore;
using Products.Data.Persistence;
using Products.Data.Persistence.Entities;

namespace Products.Data
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ProductsDbContext _dbContext;

        public ProductsRepository(ProductsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int?> Add(Product product)
        {
            product.Id = Guid.NewGuid();
            _dbContext.Products.Add(product);

            int affectedRecords = await _dbContext.SaveChangesAsync();

            if(affectedRecords > 0)
            {
                return product.Sku;
            }

            return null;
        }

        public async Task<Product?> Get(int sku)
        {
            return await _dbContext.Products
                .FirstOrDefaultAsync(x => x.Sku == sku);
        }

        public async Task<Product?> Get(string name)
        {
            return await _dbContext.Products
                .FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<List<Product>> GetAll()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<bool> Update(Product product)
        {
            _dbContext.Update(product);

            int affectedRecords = await _dbContext.SaveChangesAsync();
            if (affectedRecords > 0)
            {
                return true;
            }

            return false;
        }
    }
}