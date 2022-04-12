using System.Net.Http;
using System.Text.Json;
using System.Text;
using Serilog;
using Models;

namespace UI;
public class HttpService
{
    private HttpClient client = new HttpClient();
    private readonly string _apiBaseURL = "https://localhost:7250/api/";

    public HttpService()
    {
        client.BaseAddress = new Uri(_apiBaseURL);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        List<User> users = new List<User>();
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("../../Logs/ExceptionLogging.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            users = await JsonSerializer.DeserializeAsync<List<User>>(await client.GetStreamAsync("Store/GetUsers")) ?? new List<User>();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("OOPS! Looks like something went wrong.");
            Log.Information($"Exception Caught: {e}");
        }
        finally
        {
            Log.CloseAndFlush();
        }

        return users;
    }

    public async Task<List<Store>> GetAllStoresAsync()
    {
        List<Store> stores = new List<Store>();
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("../../Logs/ExceptionLogging.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            stores = await JsonSerializer.DeserializeAsync<List<Store>>(await client.GetStreamAsync("Store/GetStores")) ?? new List<Store>();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("OOPS! Looks like something went wrong.");
            Log.Information($"Exception Caught: {e}");
        }
        finally
        {
            Log.CloseAndFlush();
        }

        return stores;

    }

    public async Task<List<OrderHistory>> GetOrderHistoryByStoreAsync(int storeId)
    {
        List<OrderHistory> storeOrderHistory = new List<OrderHistory>();
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("../../Logs/ExceptionLogging.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            storeOrderHistory = await JsonSerializer.DeserializeAsync<List<OrderHistory>>(await client.GetStreamAsync("Store")) ?? new List<OrderHistory>();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("OOPS! Looks like something went wrong.");
            Log.Information($"Exception Caught: {e}");
        }
        finally
        {
            Log.CloseAndFlush();
        }

        return storeOrderHistory;

    }

    public async Task<Store> GetStoreInventoryAsync(Store currStore)
    {
        string serializedStore = JsonSerializer.Serialize(currStore.Id);
        StringContent content = new StringContent(serializedStore, Encoding.UTF8, "application/json");
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("../../Logs/ExceptionLogging.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            HttpResponseMessage response = await client.PatchAsync("Store/GetInventory", content);
            currStore.Inventory = await JsonSerializer.DeserializeAsync<List<Product>>(await response.Content.ReadAsStreamAsync()) ?? new List<Product>();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("Welp, there seems to have been an issue. :>");
            Log.Information($"Exception Caught: {e}");
        }
        finally
        {
            Log.CloseAndFlush();
        }

        return currStore;
    }

    public async Task<List<OrderHistory>> GetOrderHistoryByUserAsync(int userId)
    {
        List<OrderHistory> userOrderHistory = new List<OrderHistory>();
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("../../Logs/ExceptionLogging.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            userOrderHistory = await JsonSerializer.DeserializeAsync<List<OrderHistory>>(await client.GetStreamAsync("Store")) ?? new List<OrderHistory>();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("OOPS! Looks like something went wrong.");
            Log.Information($"Exception Caught: {e}");
        }
        finally
        {
            Log.CloseAndFlush();
        }

        return userOrderHistory;

    }

    public async Task AddOrderAsync(Order newOrder)
    {
        string serializedOrder = JsonSerializer.Serialize(newOrder);
        StringContent content = new StringContent(serializedOrder, Encoding.UTF8, "application/json");
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("../../Logs/ExceptionLogging.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            HttpResponseMessage response = await client.PostAsync("Store", content);
            response.EnsureSuccessStatusCode();
            await JsonSerializer.DeserializeAsync<User>(await response.Content.ReadAsStreamAsync());
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("Welp, there seems to have been an issue. :>");
            Log.Information($"Exception Caught: {e}");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public async Task<User> AddUserAsync(User newUser)
    {
        string serializedUser = JsonSerializer.Serialize(newUser);
        StringContent content = new StringContent(serializedUser, Encoding.UTF8, "application/json");
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("../../Logs/ExceptionLogging.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            HttpResponseMessage response = await client.PostAsync("Store", content);
            response.EnsureSuccessStatusCode();
            return await JsonSerializer.DeserializeAsync<User>(await response.Content.ReadAsStreamAsync()) ?? new User();
        }
        catch (HttpRequestException)
        {
            throw;
        }
    }
}