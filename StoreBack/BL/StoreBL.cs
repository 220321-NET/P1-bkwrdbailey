using DL;
using Models;

namespace BL;
public class StoreBL : IStoreBL
{
    private readonly IRepository _repo;
    public StoreBL(IRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _repo.GetAllUsersAsync();
    }

    public User AddUser(User userToAdd)
    {
        return _repo.CreateUser(userToAdd);
    }

    public void AddOrder(Order order)
    {
        _repo.CreateOrder(order);
    }

    public async Task<List<Store>> GetAllStoresAsync()
    {
        return await _repo.GetAllStoresAsync();
    }

    public List<Product> GetStoreInventory(int currStoreId)
    {
        return _repo.GetStoreInventory(currStoreId);
    }

    public async Task<List<OrderHistory>> GetOrderHistoryByStoreAsync(int storeId)
    {
        List<OrderHistory> storeOrderHistory = await _repo.GetOrderHistoryByStoreAsync(storeId);
        return storeOrderHistory;
    }

    public async Task<List<OrderHistory>> GetOrderHistoryByUserAsync(int userId)
    {
        List<OrderHistory> userOrderHistory = await _repo.GetOrderHistoryByUserAsync(userId);
        return userOrderHistory;
    }
}
