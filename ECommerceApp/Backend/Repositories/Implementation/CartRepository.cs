using Backend.Data;
using Backend.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Backend.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IMongoCollection<Cart> _carts;

        public CartRepository(MongoDbService mongoDbService)
        {
            _carts = mongoDbService.Database?.GetCollection<Cart>("Carts");
        }

        public async Task<Cart> GetByUserIdAsync(string userId)
        {
            return await _carts.Find(c => c.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task AddOrUpdateAsync(Cart cart)
        {
            var existing = await GetByUserIdAsync(cart.UserId);
            if (existing != null)
                await _carts.ReplaceOneAsync(c => c.UserId == cart.UserId, cart);
            else
                await _carts.InsertOneAsync(cart);
        }
    }
}