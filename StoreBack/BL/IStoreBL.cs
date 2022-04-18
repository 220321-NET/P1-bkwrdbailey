using Models;
namespace BL;

public interface IStoreBL
{
    Task<List<User>> GetAllUsersAsync();
    User AddUser(User userToAdd);
    void AddOrder(Order order);
    void AddProduct(int storeId, Product newProduct);
    Task<List<Store>> GetAllStoresAsync();
    List<Product> GetStoreInventory(int currStoreId);
    List<OrderHistory> GetOrderHistoryByStoreAsync(int storeId, int sortOrder);
    List<OrderHistory> GetOrderHistoryByUserAsync(int userId, int sortOrder);
    void UpdateStoreInventory(int storeId, Product productToUpdate);
}