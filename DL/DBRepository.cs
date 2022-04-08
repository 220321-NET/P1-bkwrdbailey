using Microsoft.Data.SqlClient;
using System.Data;
using Models;

namespace DL;
public class DBRepository : IRepository
{
    private readonly string _connectionString;

    public DBRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Creates a product list that acts as the store's inventory to hold the returned record results from the SQL call
    /// that is read through
    /// </summary>
    /// <param name="currentStore">The instance of the current store the user selected</param>
    /// <returns>List of the current store's inventory</returns>
    public Store GetStoreInventory(Store currentStore)
    {

        List<Product> storeInventory = new List<Product>();

        DataSet inventorySet = new DataSet();

        SqlConnection connection = new SqlConnection(_connectionString);
        SqlCommand cmd = new SqlCommand("SELECT ProductId, StoreFrontId, Product.Name as ProductName, Description, Price, Quantity FROM Inventory JOIN Product ON (Product.Id = ProductId) JOIN StoreFront ON (StoreFront.Id = StoreFrontId) WHERE StoreFrontId = @id;", connection);
        cmd.Parameters.AddWithValue("@id", currentStore.Id);

        SqlDataAdapter inventoryAdapter = new SqlDataAdapter(cmd);

        inventoryAdapter.Fill(inventorySet, "StoreInventoryTable");
        DataTable? storeInventoryTable = inventorySet.Tables["StoreInventoryTable"];
        if (storeInventoryTable != null && storeInventoryTable.Rows.Count > 0)
        {
            foreach (DataRow row in storeInventoryTable.Rows)
            {
                Product product = new Product();

                product.Id = (int)row["ProductId"];
                product.Name = (string)row["ProductName"];
                product.Description = (string)row["Description"];
                product.Price = (double)row["Price"];
                product.Quantity = (int)row["Quantity"];

                storeInventory.Add(product);
            }
        }
        currentStore.Inventory = storeInventory;

        return currentStore;
    }

    /// <summary>
    /// Creates a User list that holds instances that to each individual user record in the database
    /// acquired from a sql call that is read through
    /// Also adds either a User instance or an Employee instance to the return list based on the IsEmployed boolean value
    /// </summary>
    /// <returns>A list of all the users in the Azure Database</returns>
    public List<User> GetAllUsers()
    {

        List<User> users = new List<User>();

        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM Users", connection);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string userName = reader.GetString(1);
            string password = reader.GetString(2);
            bool _isEmployed = reader.GetBoolean(3);

            User user = new User
            {
                Id = id,
                UserName = userName,
                Password = password,
                IsEmployed = _isEmployed
            };
            users.Add(user);
        }

        reader.Close();
        connection.Close();

        return users;
    }

    /// <summary>
    /// Adds every store record in the database to a stores list to be returned individual store interaction
    /// </summary>
    /// <returns>A list of all the stores from the Azure Database</returns>
    public List<Store> GetAllStores()
    {
        List<Store> stores = new List<Store>();

        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM StoreFront", connection);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            string address = reader.GetString(2);

            Store store = new Store
            {
                Id = id,
                Name = name,
                Address = address
            };

            stores.Add(store);
        }

        reader.Close();
        connection.Close();

        return stores;
    }


    /// <summary>
    /// The order history for a specific user is acquired from the Azure Database and passed into new individual OrderHistory objects
    /// that show that user's overall order history for every store
    /// </summary>
    /// <param name="user">The current User object that is interacting with the application</param>
    /// <returns>A list of OrderHistory objects populated from a SQL call</returns>
    public List<OrderHistory> GetOrderHistoryByUser(User user)
    {
        List<OrderHistory> userOrderHistory = new List<OrderHistory>();
        List<Store> stores = GetAllStores();

        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand("SELECT Orders.Id, StoreFrontId, CustomerId, DateOrdered, Quantity, Product.Name, Product.Price, TotalCost FROM Orders JOIN Cart ON (Cart.OrderId = Orders.Id) JOIN StoreFront ON (StoreFront.Id = Orders.StoreFrontId) JOIN Product ON (Product.Id = Cart.ProductId) WHERE CustomerId = @customerId ORDER BY StoreFrontId;", connection);
        cmd.Parameters.AddWithValue("@customerId", user.Id);

        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            int storeId = reader.GetInt32(1);
            DateTime date = reader.GetDateTime(3);
            int itemQty = reader.GetInt32(4);
            string itemName = reader.GetString(5);
            double itemPrice = reader.GetDouble(6);
            double totalCost = reader.GetDouble(7);

            OrderHistory order = new OrderHistory
            {
                OrderId = id,
                ProductName = itemName,
                StoreId = storeId,
                TotalCost = totalCost,
                ItemPrice = itemPrice,
                ItemQty = itemQty,
                DateOrdered = date,
            };

            foreach (Store store in stores)
            {
                if (store.Id == storeId)
                {
                    order.store = store;
                    break;
                }
            }

            userOrderHistory.Add(order);
        }
        connection.Close();

        return userOrderHistory;
    }

    /// <summary>
    /// Uses a Store object to acquire that store's order history data from the Azure Database to be be added to individual OrderHistory objects
    /// that will show that single store's overall order history
    /// </summary>
    /// <param name="_store">The current Store object thats order history is being requested</param>
    /// <returns>Gives back a list of OrderHistory objects based on a specific store</returns>
    public List<OrderHistory> GetOrderHistoryByStore(Store _store)
    {
        List<OrderHistory> storeOrderHistory = new List<OrderHistory>();
        List<User> users = GetAllUsers();

        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand("SELECT Orders.Id, CustomerId, DateOrdered, Quantity, Product.Name, TotalCost, StoreFrontId, Price FROM Orders JOIN Cart ON (Cart.OrderId = Orders.Id) JOIN StoreFront ON (StoreFront.Id = Orders.StoreFrontId) JOIN Product ON (Product.Id = Cart.ProductId) WHERE StoreFrontId = @storeId ORDER BY StoreFrontId;", connection);
        cmd.Parameters.AddWithValue("@storeId", _store.Id);

        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            int customerId = reader.GetInt32(1);
            DateTime date = reader.GetDateTime(2);
            int itemQty = reader.GetInt32(3);
            string itemName = reader.GetString(4);
            double totalCost = reader.GetDouble(5);
            int storeId = reader.GetInt32(6);
            double itemPrice = reader.GetDouble(7);

            OrderHistory order = new OrderHistory
            {
                OrderId = id,
                ProductName = itemName,
                StoreId = storeId,
                TotalCost = totalCost,
                ItemQty = itemQty,
                DateOrdered = date,
                ItemPrice = itemPrice
            };

            foreach (User user in users)
            {
                if (customerId == user.Id)
                {
                    order.customer = user;
                    break;
                }
            }
            storeOrderHistory.Add(order);
        }

        reader.Close();
        connection.Close();

        return storeOrderHistory;
    }

    /// <summary>
    /// Adds a user to the Azure database and gives the generated id from the database to the user object for identification
    /// </summary>
    /// <param name="userToAdd">Instance made from a user registering for a new account</param>
    /// <returns>The user instance that was added to the database</returns>
    public User CreateUser(User userToAdd)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand("INSERT INTO Users(UserName, Password) OUTPUT INSERTED.Id VALUES(@username, @password)", connection);

        cmd.Parameters.AddWithValue("@username", userToAdd.UserName);
        cmd.Parameters.AddWithValue("@password", userToAdd.Password);

        try
        {
            userToAdd.Id = (int)cmd.ExecuteScalar();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        connection.Close();

        return userToAdd;
    }

    /// <summary>
    /// Takes a new created order instance and inserts a record of it into the Azure database
    /// </summary>
    /// <param name="order">The instance of order that will be added to the database</param>
    public void CreateOrder(Order order)
    {
        DateTime currentDate = DateTime.Now;
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand("INSERT INTO Orders(StoreFrontId, CustomerId, TotalCost, DateOrdered) OUTPUT INSERTED.Id VALUES (@storeId, @customerId, @totalCost, @dateOrdered)", connection);

        cmd.Parameters.AddWithValue("@storeId", order.store.Id);
        cmd.Parameters.AddWithValue("@customerId", order.customer.Id);
        cmd.Parameters.AddWithValue("@totalCost", order.cart.GetTotalCost());
        cmd.Parameters.AddWithValue("@dateOrdered", currentDate);

        try
        {
            order.Id = (int)cmd.ExecuteScalar();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        connection.Close();
        CreateCart(order);
    }

    /// <summary>
    /// Each product ordered by the user is added individually as a record to the Azure database and tied
    /// together by their shared order id
    /// </summary>
    /// <param name="order">An instance of Order used for cart creation</param>
    public void CreateCart(Order order)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        SqlCommand cmd;

        foreach (Product product in order.cart.AllProducts())
        {
            cmd = new SqlCommand("INSERT INTO Cart (ProductId, Quantity, OrderId) OUTPUT INSERTED.Id VALUES (@productId, @quantity, @orderId)", connection);
            cmd.Parameters.AddWithValue("@productId", product.Id);
            cmd.Parameters.AddWithValue("@quantity", product.Quantity);
            cmd.Parameters.AddWithValue("@orderId", order.Id);

            int id = (int)cmd.ExecuteScalar();
        }

        connection.Close();

        UpdateInventory(order);
    }

    public void AddProduct(Product product)
    {
        // Add new product to database
    }

    /// <summary>
    /// The inventory tied to a specific store is updated based on the Store instance field in the order instance
    /// which has the updated inventory to be used for updating the inventory table in the Azure database
    /// </summary>
    /// <param name="order">Instance of order passed along for inventory updating</param>
    public void UpdateInventory(Order order)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        SqlCommand cmd;

        foreach (Product product in order.store.Inventory)
        {
            cmd = new SqlCommand("UPDATE Inventory SET Quantity = @quantity OUTPUT INSERTED.Id WHERE ProductId = @productId AND StoreFrontId = @storeFront", connection);
            cmd.Parameters.AddWithValue("@quantity", product.Quantity);
            cmd.Parameters.AddWithValue("@productId", product.Id);
            cmd.Parameters.AddWithValue("@storeFront", order.store.Id);

            int id = (int)cmd.ExecuteScalar();
        }

        connection.Close();
    }
}