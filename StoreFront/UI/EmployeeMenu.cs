using Models;
using BL;
using Serilog;

namespace UI;

public class EmployeeMenu
{

    private readonly HttpService _httpService;
    private Employee _user = new Employee();
    private Store currentStore = null;

    public EmployeeMenu(HttpService httpService, Employee user)
    {
        _httpService = httpService;
        _user = user;
    }

    public async Task AdminMenu()
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
            Console.WriteLine("[4] Check Store Inventory");
            Console.WriteLine("[5] Update Store Inventory");
        }
        Console.WriteLine("[x] Logout");

        string adminChoice = Console.ReadLine().Trim().ToLower();

        switch (adminChoice)
        {
            case "1":
                Console.WriteLine("==================================================================");
                await SelectAStore();
                Console.WriteLine("==================================================================");
                break;

            case "2":
                Console.WriteLine("==================================================================");
                await StoreOrderHistory();
                Console.WriteLine("==================================================================");
                break;

            case "3":
            Console.WriteLine("==================================================================");
                AddProduct(_user.NewProduct());
                Console.WriteLine("==================================================================");
                break;

            case "4":
                await CheckInventory();
                Console.WriteLine("==================================================================");
                break;

            case "5":
                Console.WriteLine("==================================================================");
                AdjustProductQuantity();
                Console.WriteLine("==================================================================");
                break;

            case "x":
                return;

            default:
                Console.WriteLine("Invalid Input");
                goto AdminChoices;
        }

        goto AdminChoices;
    }

    private async Task SelectAStore()
    {
        List<Store> allStores = await _httpService.GetAllStoresAsync();

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
            currentStore.Inventory = await _httpService.GetStoreInventoryAsync(currentStore);
        }
        else if (storeSelection == "2")
        {
            currentStore = allStores[1];
            currentStore.Inventory = await _httpService.GetStoreInventoryAsync(currentStore);
        }
        else
        {
            Console.WriteLine("Invalid Input");
            goto StoreSelection;
        }
    }

    private async Task StoreOrderHistory()
    {
        int sortOrder;
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("../Logs/ExceptionLogging.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

    SortDecision:
        Console.WriteLine("Would you like to sort the order history:");
        Console.WriteLine("[1] Sort by Low-High Total Cost");
        Console.WriteLine("[2] Sort by High-Low Total Cost");
        Console.WriteLine("[3] Sort by Old-New Date Ordered");
        Console.WriteLine("[4] Sort by New-Old Date Ordered");
        Console.WriteLine("[5] No Sorting Filter");

        try
        {
            sortOrder = Convert.ToInt32(Console.ReadLine());
            if (sortOrder > 5 || sortOrder < 1)
            {
                Console.WriteLine("Invalid Choice entered");
                goto SortDecision;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid Input");
            Log.Information($"Exception Caught: {e}");
            goto SortDecision;
        }

        Log.CloseAndFlush();

        List<OrderHistory> storeOrderHistory = await _httpService.GetOrderHistoryByStoreAsync(currentStore.Id, sortOrder);

        Console.WriteLine("===============================\n======== Order History ========\n===============================");

        foreach (OrderHistory order in storeOrderHistory)
        {
            Console.WriteLine($"Order: {order.OrderId} | {order.CustomerName} | ${order.TotalCost} | {order.DateOrdered}:\n -Product Info: ${order.ItemPrice} | {order.ProductName} | Bought {order.ItemQty}");
        }
    }

    private async Task CheckInventory()
    {
        currentStore.Inventory = await _httpService.GetStoreInventoryAsync(currentStore);
        Console.WriteLine("==================================================================");
        currentStore.DisplayStock();
    }

    private void AddProduct(Product newProduct)
    {

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("../Logs/ExceptionLogging.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            _httpService.AddProduct(currentStore.Id, newProduct);
        }
        catch (HttpRequestException e)
        {
            Log.Information($"Exception Caught: {e}");
            Console.WriteLine("Yikes, there seems to have been an issue adding that product!");
            return;
        }
        Console.WriteLine("Product Added Successfully!");
    }

    public void AdjustProductQuantity()
    {
        Product productToUpdate = new Product();
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("../Logs/ExceptionLogging.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();



    UpdateProduct:
        Console.WriteLine("==================================================================");
        currentStore.DisplayStock();
        Console.WriteLine("Select a product to update:");

        try
        {
            int choice = Convert.ToInt32(Console.ReadLine());
            productToUpdate = currentStore.Inventory[choice - 1];
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid Choice");
            Log.Information($"Exception Caught: {e}");
            goto UpdateProduct;
        }

    UpdateDecision:
        Console.WriteLine("What would you like to do with this product\n[1] Update Quantity\n[2] Update Price\n[3] Exit");

        try
        {
            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice == 1)
            {
                try
                {
                    Console.WriteLine("What is the new quantity for this product:");
                    int newQuantity = Convert.ToInt32(Console.ReadLine());
                    productToUpdate.Quantity = newQuantity;
                    _httpService.UpdateStoreInventory(currentStore.Id, productToUpdate);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid Input");
                    Log.Information($"Exception Caught: {e}");
                    goto UpdateDecision;
                }
            }
            else if (choice == 2)
            {
                try
                {
                    Console.WriteLine("What is the new price for this product:");
                    double newPrice = Convert.ToDouble(Console.ReadLine());
                    productToUpdate.Price = newPrice;
                    _httpService.UpdateStoreInventory(currentStore.Id, productToUpdate);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid Input");
                    Log.Information($"Exception Caught: {e}");
                    goto UpdateDecision;
                }
            }
            else if (choice == 3)
            {
                Console.WriteLine("Returning to Menu...");
                Log.CloseAndFlush();
                return;
            }
            else
            {
                Console.WriteLine("Invalid Selection");
                goto UpdateDecision;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid Input");
            Log.Information($"Exception Caught: {e}");
            goto UpdateDecision;
        }
        Log.CloseAndFlush();
    }
}