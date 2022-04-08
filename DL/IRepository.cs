using Models;
namespace DL;

public interface IRepository
{
    Store GetStoreInventory(Store currentStore);
    List<User> GetAllUsers();
    List<Store> GetAllStores();
    List<OrderHistory> GetOrderHistoryByUser(User user);
    List<OrderHistory> GetOrderHistoryByStore(Store _store);
    User CreateUser(User userToAdd);
    void CreateOrder(Order order);
    void CreateCart(Order order);
    void AddProduct(Product product);
    void UpdateInventory(Order order);

}