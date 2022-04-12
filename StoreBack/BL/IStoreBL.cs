using Models;
namespace BL;

public interface IStoreBL
{
    Task<List<User>> GetAllUsersAsync();
    User AddUser(User userToAdd);
    void AddOrder(Order order);
    Task<List<Store>> GetAllStoresAsync();
    List<Product> GetStoreInventory(int currStoreId);
    Task<List<OrderHistory>> GetOrderHistoryByStoreAsync(int storeId);
    Task<List<OrderHistory>> GetOrderHistoryByUserAsync(int userId);
}