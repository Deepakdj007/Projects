using Backend.Data;
using Backend.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Backend.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderRepository(MongoDbService mongoDbService)
        {
            _orders = mongoDbService.Database?.GetCollection<Order>("Orders");
        }

        public async Task<List<Order>> GetAllAsync() => await _orders.Find(_ => true).ToListAsync();

        public async Task<Order> GetByIdAsync(string id) => await _orders.Find(o => o.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Order order) => await _orders.InsertOneAsync(order);
    }
}