using Models;
namespace BL;

public interface IStoreBL
{
    List<User> GetAllUsers();
    User AddUser(User userToAdd);
    void AddOrder(Order order);
    List<Store> GetAllStores();
    Store GetStoreInventory(Store currentStore);
    List<OrderHistory> GetOrderHistoryByStore(Store _store);
    List<OrderHistory> GetOrderHistoryByUser(User user);
}