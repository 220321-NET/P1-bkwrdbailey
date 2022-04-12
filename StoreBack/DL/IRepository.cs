using Models;
namespace DL;

public interface IRepository
{
    List<Product> GetStoreInventory(int currentStoreId);
    Task<List<User>> GetAllUsersAsync();
    Task<List<Store>> GetAllStoresAsync();
    Task<List<OrderHistory>> GetOrderHistoryByUserAsync(int userId);
    Task<List<OrderHistory>> GetOrderHistoryByStoreAsync(int storeId);
    User CreateUser(User userToAdd);
    void CreateOrder(Order order);
    void CreateCart(Order order);
    void AddProduct(Product product);
    void UpdateInventory(Order order);

}