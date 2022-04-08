using Models;
using BL;

namespace UI;

public class EmployeeMenu
{
    private readonly IStoreBL _bl;
    private Employee _user = new Employee();
    private Store currentStore = null;

    public EmployeeMenu(IStoreBL bl, Employee user)
    {
        _bl = bl;
        _user = user;
    }

    public void AdminMenu()
    {
        Console.WriteLine("========================");
        Console.WriteLine("====== ADMIN MENU ======");
        Console.WriteLine("========================");

    AdminChoices:
        if (currentStore != null)
        {
            Console.WriteLine($"Selected Store:\n{currentStore.Name} | {currentStore.Address}\n");
        }

        Console.WriteLine("[1] Select a Store");
        if (currentStore != null)
        {
            Console.WriteLine("[2] View Store Order History");
            Console.WriteLine("[3] Add a product");
        }
        Console.WriteLine("[x] Logout");

        string adminChoice = Console.ReadLine().Trim().ToLower();

        switch (adminChoice)
        {
            case "1":
                SelectAStore();
                break;

            case "2":
                StoreOrderHistory();
                break;

            case "3":
                _user.AddProduct();
                break;

            case "x":
                return;

            default:
                Console.WriteLine("Invalid Input");
                goto AdminChoices;
        }

        goto AdminChoices;
    }

    private void SelectAStore()
    {
        List<Store> allStores = _bl.GetAllStores();

    StoreSelection:
        int i = 1;
        foreach (Store store in allStores)
        {
            Console.WriteLine($"[{i}] {store.Name} | {store.Address}");
            i++;
        }

        Console.WriteLine("Select a Store:");
        string storeSelection = Console.ReadLine().Trim();

        if (storeSelection == "1")
        {
            currentStore = allStores[0];
        }
        else if (storeSelection == "2")
        {
            currentStore = allStores[1];
        }
        else
        {
            Console.WriteLine("Invalid Input");
            goto StoreSelection;
        }
    }

    private void StoreOrderHistory()
    {
        List<OrderHistory> storeOrderHistory = _bl.GetOrderHistoryByStore(currentStore);

        foreach (OrderHistory order in storeOrderHistory)
        {
            Console.WriteLine($"{order.customer.UserName} | {order.DateOrdered}\n${order.ItemPrice} | {order.ProductName} | {order.ItemQty} Qty.");
        }
    }
}