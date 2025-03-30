using Backend.Entities;

namespace Backend.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(string id);
        Task CreateAsync(Order order);
    }
}