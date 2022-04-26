using Models;
namespace DL;

public interface IRepository
{
    List<Product> GetStoreInventory(int currentStoreId);
    Task<List<User>> GetAllUsersAsync();
    Task<List<Store>> GetAllStoresAsync();
    List<OrderHistory> GetOrderHistoryByUserAsync(int userId, int sortOrder);
    List<OrderHistory> GetOrderHistoryByStoreAsync(int _storeId, int sortOrder);
    User CreateUser(User userToAdd);
    void CreateOrder(Order order);
    void CreateCart(Order order);
    void CreateProduct(int storeId, Product newProduct);
    void UpdateInventoryViaOrder(Order order);
    void UpdateInventoryViaNewProduct(int storeId, Product productToUpdate);
    void UpdateInventoryProduct(int storeId, Product productToUpdate);
}