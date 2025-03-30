using Backend.Entities;

namespace Backend.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetByUserIdAsync(string userId);
        Task AddOrUpdateAsync(Cart cart);
    }
}