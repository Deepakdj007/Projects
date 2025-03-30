using Backend.Data;
using Backend.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Backend.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(MongoDbService mongoDbService)
        {
            _products = mongoDbService.Database?.GetCollection<Product>("Products");
        }

        public async Task<List<Product>> GetAllAsync() => await _products.Find(_ => true).ToListAsync();

        public async Task<Product> GetByIdAsync(string id) => await _products.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Product product) => await _products.InsertOneAsync(product);

        public async Task UpdateAsync(string id, Product product) => await _products.ReplaceOneAsync(p => p.Id == id, product);

        public async Task DeleteAsync(string id) => await _products.DeleteOneAsync(p => p.Id == id);
    }
}