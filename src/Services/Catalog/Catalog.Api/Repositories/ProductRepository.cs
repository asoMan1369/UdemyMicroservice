using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;
        public ProductRepository(ICatalogContext contex)
        {
            _context = contex ?? throw new ArgumentException(nameof(contex));

        }

        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            //FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
           var deletedItem= await _context.Products.DeleteOneAsync(filter: p => p.Id == id);
            return deletedItem.IsAcknowledged &&
                   deletedItem.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context
            .Products
            .Find(p => p.Id==id)
            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
            return await _context
                        .Products
                        .Find(filter)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Name, name);
            return await _context
                        .Products
                        .Find(filter)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context
                        .Products
                        .Find(p =>true)
                        .ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updataResult = await _context.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return  updataResult.IsAcknowledged 
                && updataResult.ModifiedCount > 0;
        }
    }
}
