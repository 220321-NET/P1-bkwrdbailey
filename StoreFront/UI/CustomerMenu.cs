// using BL;
using Models;
using Serilog;

namespace UI;

public class CustomerMenu
{

    private readonly HttpService _httpService;
    private User _user = new User();
    private Cart cart = new Cart();
    private Store currentStore = new Store();

    public CustomerMenu(HttpService httpService, User user)
    {
        _httpService = httpService;
        _user = user;
    }

    public async Task StoreMenu()
    {

        List<Store> stores = await _httpService.GetAllStoresAsync();

        Console.WriteLine("==================================================================");

    StoreLocation:
        Console.WriteLine("Select a store to shop at or View your order history");

        int i = 1;
        foreach (Store store in stores)
        {
            Console.WriteLine($"[{i}] {store.Name} | {store.Address}");
            i++;
        }

        Console.WriteLine($"[{i}] View Your Order History");
        Console.WriteLine($"[x] Logout");

        string storeAnswer = Console.ReadLine().Trim();
        Console.WriteLine("==================================================================");

        if (storeAnswer == "1")
        {
            currentStore = stores[0];
        }
        else if (storeAnswer == "2")
        {
            currentStore = stores[1];
        }
        else if (storeAnswer == "3")
        {
            await ViewOrderHistory();
            goto StoreLocation;
        }
        else if (storeAnswer.ToLower() == "x")
        {
            return;
        }
        else
        {
            Console.WriteLine("Invalid Input");
            goto StoreLocation;
        }

        currentStore.Inventory = await _httpService.GetStoreInventoryAsync(currentStore);
        string result = Menu();

        if (result == "6")
        {
            goto StoreLocation;
        }
    }

    private string Menu()
    {
    MenuChoices:
        Console.WriteLine("[1] See inventory");
        Console.WriteLine("[2] Add product to cart");
        Console.WriteLine("[3] Remove product from cart");
        Console.WriteLine("[4] Show cart's contents");
        Console.WriteLine("[5] Checkout");
        Console.WriteLine("[6] Change store location");
        Console.WriteLine("[x] Logout");

        string choice = Console.ReadLine().Trim().ToLower();
        Console.WriteLine("==================================================================");

        switch (choice)
        {
            case "1":
                Inventory();
                Console.WriteLine("==================================================================");

                break;

            case "2":
                AddProductToCart();
                Console.WriteLine("==================================================================");
                break;

            case "3":
                if (cart.IsCartEmpty())
                {
                    Console.WriteLine("There is nothing in the cart to remove");
                    Console.WriteLine("==================================================================");
                }
                else
                {
                    RemoveProduct();
                    Console.WriteLine("==================================================================");
                }
                break;

            case "4":
                if (cart.IsCartEmpty())
                {
                    Console.WriteLine("There is nothing in the cart");
                    Console.WriteLine("==================================================================");

                }
                else
                {
                    cart.CartContents();
                    Console.WriteLine("==================================================================");

                }
                break;

            case "5":
                if (cart.IsCartEmpty())
                {
                    Console.WriteLine("There is nothing in the cart to checkout");
                    Console.WriteLine("==================================================================");
                }
                else
                {
                    bool HasCheckedOut = Checkout();

                    if (HasCheckedOut)
                    {
                        return "x";
                    }
                    Console.WriteLine("==================================================================");
                }
                break;

            case "6":
                if (cart.IsCartEmpty())
                {
                    return choice;
                }
                else
                {
                ChangeStore:
                    Console.WriteLine("There are items in your cart\nAre you sure you want to change the store? [Y/N]");
                    string decision = Console.ReadLine().Trim().ToUpper();

                    if (decision == "Y")
                    {
                        cart.ClearCart();
                        Console.WriteLine("Your cart has been cleared!");
                        Console.WriteLine("==================================================================");

                        return choice;
                    }
                    else if (decision == "N")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input");
                        goto ChangeStore;
                    }
                }

            case "x":
                return choice;

            default:
                Console.WriteLine("Invalid Input");
                break;
        }
        goto MenuChoices;
    }

    private void AddProductToCart()
    {
    ItemToAdd:
        Inventory();
        Console.WriteLine("==================================================================");

        Console.WriteLine("Which item would you like to add:");
        string choice = Console.ReadLine().Trim();
        int productId;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("../Logs/ExceptionLogging.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            productId = Convert.ToInt32(choice);
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid Input");
            Log.Information($"Exception Caught: {e}");
            goto ItemToAdd;
        }
        finally
        {
            Log.CloseAndFlush();
        }

        if (productId > currentStore.Inventory.Count || productId < 0)
        {
            Console.WriteLine("Invalid Input");
            goto ItemToAdd;
        }

        foreach (Product product in currentStore.Inventory)
        {
            if (product.Id == productId)
            {
            AmtToAdd:
                Console.WriteLine("How many of this item would you like:");
                int amount;


                try
                {
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.File("../Logs/ExceptionLogging.txt", rollingInterval: RollingInterval.Day)
                        .CreateLogger();

                    amount = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Invalid Input");
                    Log.Information($"Exception Caught: {e}");
                    goto AmtToAdd;
                }
                finally
                {
                    Log.CloseAndFlush();
                }

                if (amount > product.Quantity)
                {
                    Console.WriteLine("Requested amount is higher than its quantity");
                    goto AmtToAdd;
                }

                Product item = new Product();

                item.Id = product.Id;
                item.Name = product.Name;
                item.Price = product.Price;
                item.Description = product.Description;
                item.Quantity = amount;

                for (int i = 0; i < currentStore.Inventory.Count; i++)
                {
                    if (productId == currentStore.Inventory[i].Id)
                    {
                        currentStore.Inventory[i].Quantity -= amount;
                    }
                }

                cart.AddItem(item);
            }
        }
    }

    private void RemoveProduct()
    {
    ItemToRemove:
        cart.CartContents();
        Console.WriteLine("==================================================================");

        Console.WriteLine("Which item would you like removed:");
        string choice = Console.ReadLine().Trim();
        int numChoice;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("../Logs/ExceptionLogging.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            numChoice = Convert.ToInt32(choice);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Invalid Input");
            Log.Information($"Exception Caught: {e}");
            goto ItemToRemove;
        }
        finally
        {
            Log.CloseAndFlush();
        }

        Product product;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("../Logs/ExceptionLogging.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            product = cart.RemoveItem(numChoice - 1);

        }
        catch (Exception e)
        {
            Console.WriteLine($"Invalid Input");
            Log.Information($"Exception Caught: {e}");
            goto ItemToRemove;
        }
        finally
        {
            Log.CloseAndFlush();
        }


        foreach (Product item in currentStore.Inventory)
        {
            if (item.Name == product.Name)
            {
                item.Quantity += product.Quantity;
            }
        }
    }

    private bool Checkout()
    {
    CheckoutChoice:
        cart.CartContents();
        Console.WriteLine("==================================================================");

        Console.WriteLine("Are you sure you are ready to checkout? [Y/N]");
        string choice = Console.ReadLine().Trim().ToUpper();

        if (choice == "Y")
        {
            Console.WriteLine("Finalizing Order...");
            Order order = new Order();
            order.customer = _user;
            order.cart = cart;
            order.store = currentStore;
            _httpService.AddOrderAsync(order);
            return true;
        }
        else if (choice == "N")
        {
            return false;
        }
        else
        {
            Console.WriteLine("Invalid Input");
            goto CheckoutChoice;
        }
    }
    private void Inventory()
    {
        currentStore.DisplayStock();
    }

    private async Task ViewOrderHistory()
    {
        int sortOrder = 5;
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("../Logs/ExceptionLogging.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();


        List<OrderHistory> userOrderHistory = await _httpService.GetOrderHistoryByUserAsync(_user.Id, sortOrder);

        if (userOrderHistory.Count == 0)
        {
            Console.WriteLine("No Order history to show");
            Console.WriteLine("==================================================================");
            return;
        }

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

        userOrderHistory = await _httpService.GetOrderHistoryByUserAsync(_user.Id, sortOrder);

        Console.WriteLine("===========================\n====== Order History ======\n===========================");

        foreach (OrderHistory order in userOrderHistory)
        {
            Console.WriteLine($"{order.OrderId} | {order.StoreName} | {order.StoreAddress} | ${order.TotalCost}:\n -Product Info:${order.ItemPrice} | {order.ProductName} | {order.ItemQty} QTY. | {order.DateOrdered}");
        }

        Console.WriteLine("==================================================================");
    }
}