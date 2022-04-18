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
    
    public void AddProduct(int storeId, Product newProduct)
    {
        _repo.CreateProduct(storeId, newProduct);
    }

    public async Task<List<Store>> GetAllStoresAsync()
    {
        return await _repo.GetAllStoresAsync();
    }

    public List<Product> GetStoreInventory(int currStoreId)
    {
        return _repo.GetStoreInventory(currStoreId);
    }

    public List<OrderHistory> GetOrderHistoryByStoreAsync(int storeId, int sortOrder)
    {
        List<OrderHistory> storeOrderHistory = _repo.GetOrderHistoryByStoreAsync(storeId, sortOrder);
        return storeOrderHistory;
    }

    public List<OrderHistory> GetOrderHistoryByUserAsync(int userId, int sortOrder)
    {
        List<OrderHistory> userOrderHistory = _repo.GetOrderHistoryByUserAsync(userId, sortOrder);
        return userOrderHistory;
    }

    public void UpdateStoreInventory(int storeId, Product productToUpdate) {
        _repo.UpdateInventoryProduct(storeId, productToUpdate);
    }
}
