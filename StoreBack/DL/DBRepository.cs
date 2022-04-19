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
    public List<Product> GetStoreInventory(int currentStoreId)
    {

        List<Product> storeInventory = new List<Product>();

        DataSet inventorySet = new DataSet();

        SqlConnection connection = new SqlConnection(_connectionString);
        SqlCommand cmd = new SqlCommand("SELECT ProductId, StoreFrontId, Product.Name as ProductName, Description, Inventory.ProductPrice, Quantity FROM Inventory JOIN Product ON (Product.Id = ProductId) JOIN StoreFront ON (StoreFront.Id = StoreFrontId) WHERE StoreFrontId = @id;", connection);
        cmd.Parameters.AddWithValue("@id", currentStoreId);

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
                product.Price = (double)row["ProductPrice"];
                product.Quantity = (int)row["Quantity"];

                storeInventory.Add(product);
            }
        }

        return storeInventory;
    }

    /// <summary>
    /// Creates a User list that holds instances that to each individual user record in the database
    /// acquired from a sql call that is read through
    /// Also adds either a User instance or an Employee instance to the return list based on the IsEmployed boolean value
    /// </summary>
    /// <returns>A list of all the users in the Azure Database</returns>
    public async Task<List<User>> GetAllUsersAsync()
    {

        List<User> users = new List<User>();

        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM Users", connection);
        SqlDataReader reader = cmd.ExecuteReader();

        while (await reader.ReadAsync())
        {
            int id = reader.GetInt32(0);
            string userName = reader.GetString(1);
            string password = reader.GetString(2);
            bool _isEmployed = reader.GetBoolean(3);

            if (_isEmployed == true)
            {
                Employee employee = new Employee
                {
                    Id = id,
                    UserName = userName,
                    Password = password,
                    IsEmployed = _isEmployed,
                };
                users.Add(employee);
            }
            else
            {
                User customer = new User
                {
                    Id = id,
                    UserName = userName,
                    Password = password,
                    IsEmployed = _isEmployed
                };
                users.Add(customer);
            }
        }

        reader.Close();
        connection.Close();

        return users;
    }

    /// <summary>
    /// Adds every store record in the database to a stores list to be returned for individual store interaction
    /// </summary>
    /// <returns>A list of all the store records from the database's StoreFront Table</returns>
    public async Task<List<Store>> GetAllStoresAsync()
    {
        List<Store> stores = new List<Store>();

        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM StoreFront", connection);
        SqlDataReader reader = cmd.ExecuteReader();

        while (await reader.ReadAsync())
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
    /// This method takes a user's id that is unique to them and their preferred way of sorting their order history to pick a different 
    /// sqlcommand call that will return that data to be presented in the way the user was wanting it to be filtered
    /// </summary>
    /// <param name="userId">The integer value tied to the user making the request</param>
    /// <param name="sortOrder">An integer valued tied to the preferred filter the user wants the data sorted by</param>
    /// <returns>A list of sorted orderhistory objects tied to a specific user</returns>
    public List<OrderHistory> GetOrderHistoryByUserAsync(int userId, int sortOrder)
    {
        List<OrderHistory> userOrderHistory = new List<OrderHistory>();
        SqlCommand cmd;

        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        if (sortOrder == 1)
        {
            cmd = new SqlCommand("SELECT Orders.Id, StoreFront.Id, Orders.DateOrdered, Cart.Quantity, Product.Name, Inventory.ProductPrice, Orders.TotalCost, StoreFront.Name, StoreFront.Address FROM Orders JOIN Cart ON (Cart.OrderId = Orders.Id) JOIN StoreFront ON (StoreFront.Id = Orders.StoreFrontId) JOIN Inventory ON (Inventory.StoreFrontId = Orders.StoreFrontId AND Inventory.ProductId = Cart.ProductId) JOIN Product ON (Cart.ProductId = Product.Id) WHERE Orders.CustomerId = @customerId ORDER BY Orders.TotalCost ASC;", connection);
        }
        else if (sortOrder == 2)
        {
            cmd = new SqlCommand("SELECT Orders.Id, StoreFront.Id, Orders.DateOrdered, Cart.Quantity, Product.Name, Inventory.ProductPrice, Orders.TotalCost, StoreFront.Name, StoreFront.Address FROM Orders JOIN Cart ON (Cart.OrderId = Orders.Id) JOIN StoreFront ON (StoreFront.Id = Orders.StoreFrontId) JOIN Inventory ON (Inventory.StoreFrontId = Orders.StoreFrontId AND Inventory.ProductId = Cart.ProductId) JOIN Product ON (Cart.ProductId = Product.Id) WHERE Orders.CustomerId = @customerId ORDER BY Orders.TotalCost DESC;", connection);
        }
        else if (sortOrder == 3)
        {
            cmd = new SqlCommand("SELECT Orders.Id, StoreFront.Id, Orders.DateOrdered, Cart.Quantity, Product.Name, Inventory.ProductPrice, Orders.TotalCost, StoreFront.Name, StoreFront.Address FROM Orders JOIN Cart ON (Cart.OrderId = Orders.Id) JOIN StoreFront ON (StoreFront.Id = Orders.StoreFrontId) JOIN Inventory ON (Inventory.StoreFrontId = Orders.StoreFrontId AND Inventory.ProductId = Cart.ProductId) JOIN Product ON (Cart.ProductId = Product.Id) WHERE Orders.CustomerId = @customerId ORDER BY DateOrdered ASC;", connection);
        }
        else if (sortOrder == 4)
        {
            cmd = new SqlCommand("SELECT Orders.Id, StoreFront.Id, Orders.DateOrdered, Cart.Quantity, Product.Name, Inventory.ProductPrice, Orders.TotalCost, StoreFront.Name, StoreFront.Address FROM Orders JOIN Cart ON (Cart.OrderId = Orders.Id) JOIN StoreFront ON (StoreFront.Id = Orders.StoreFrontId) JOIN Inventory ON (Inventory.StoreFrontId = Orders.StoreFrontId AND Inventory.ProductId = Cart.ProductId) JOIN Product ON (Cart.ProductId = Product.Id) WHERE Orders.CustomerId = @customerId ORDER BY DateOrdered DESC;", connection);
        }
        else
        {
            cmd = new SqlCommand("SELECT Orders.Id, StoreFront.Id, Orders.DateOrdered, Cart.Quantity, Product.Name, Inventory.ProductPrice, Orders.TotalCost, StoreFront.Name, StoreFront.Address FROM Orders JOIN Cart ON (Cart.OrderId = Orders.Id) JOIN StoreFront ON (StoreFront.Id = Orders.StoreFrontId) JOIN Inventory ON (Inventory.StoreFrontId = Orders.StoreFrontId AND Inventory.ProductId = Cart.ProductId) JOIN Product ON (Cart.ProductId = Product.Id) WHERE Orders.CustomerId = @customerId ORDER BY Orders.CustomerId ASC;", connection);
        }

        cmd.Parameters.AddWithValue("@customerId", userId);

        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            int storeId = reader.GetInt32(1);
            DateTime date = reader.GetDateTime(2);
            int itemQty = reader.GetInt32(3);
            string itemName = reader.GetString(4);
            double itemPrice = reader.GetDouble(5);
            double totalCost = reader.GetDouble(6);
            string storesName = reader.GetString(7);
            string storesAddress = reader.GetString(8);

            OrderHistory order = new OrderHistory
            {
                OrderId = id,
                ProductName = itemName,
                StoreId = storeId,
                TotalCost = totalCost,
                ItemPrice = itemPrice,
                ItemQty = itemQty,
                DateOrdered = date,
                StoreName = storesName,
                StoreAddress = storesAddress
            };

            userOrderHistory.Add(order);
        }
        connection.Close();

        return userOrderHistory;
    }

    /// <summary>
    /// This method takes in a store id and the requested way to filter the information in the sql call from the user and returns a list of orderhistory objects that reflect the
    /// chosen way of filtering it be it high total cost to low or newest order dates to oldest
    /// </summary>
    /// <param name="_storeId">The integer id for the current store being checked</param>
    /// <param name="sortOrder">An integer value to be used for ordering/sorting the SQL call</param>
    /// <returns>A list of orders tied to the specified store being requested using the OrderHistory class</returns>
    public List<OrderHistory> GetOrderHistoryByStoreAsync(int _storeId, int sortOrder)
    {
        List<OrderHistory> storeOrderHistory = new List<OrderHistory>();
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd;

        if (sortOrder == 1)
        {
            cmd = new SqlCommand("SELECT Orders.Id, Users.UserName, DateOrdered, Cart.Quantity, Product.Name, TotalCost, StoreFront.Id, Inventory.ProductPrice FROM Orders JOIN Cart ON (Cart.OrderId = Orders.Id) JOIN StoreFront ON (StoreFront.Id = Orders.StoreFrontId) JOIN Product ON (Product.Id = Cart.ProductId) JOIN Inventory ON (Inventory.StoreFrontId = Orders.StoreFrontId AND Inventory.ProductId = Cart.ProductId) JOIN Users ON (Users.Id = CustomerId) WHERE Orders.StoreFrontId = @storeId ORDER BY TotalCost ASC;", connection);
        }
        else if (sortOrder == 2)
        {
            cmd = new SqlCommand("SELECT Orders.Id, Users.UserName, DateOrdered, Cart.Quantity, Product.Name, TotalCost, StoreFront.Id, Inventory.ProductPrice FROM Orders JOIN Cart ON (Cart.OrderId = Orders.Id) JOIN StoreFront ON (StoreFront.Id = Orders.StoreFrontId) JOIN Product ON (Product.Id = Cart.ProductId) JOIN Inventory ON (Inventory.StoreFrontId = Orders.StoreFrontId AND Inventory.ProductId = Cart.ProductId) JOIN Users ON (Users.Id = CustomerId) WHERE Orders.StoreFrontId = @storeId ORDER BY TotalCost DESC;", connection);
        }
        else if (sortOrder == 3)
        {
            cmd = new SqlCommand("SELECT Orders.Id, Users.UserName, DateOrdered, Cart.Quantity, Product.Name, TotalCost, StoreFront.Id, Inventory.ProductPrice FROM Orders JOIN Cart ON (Cart.OrderId = Orders.Id) JOIN StoreFront ON (StoreFront.Id = Orders.StoreFrontId) JOIN Product ON (Product.Id = Cart.ProductId) JOIN Inventory ON (Inventory.StoreFrontId = Orders.StoreFrontId AND Inventory.ProductId = Cart.ProductId) JOIN Users ON (Users.Id = CustomerId) WHERE Orders.StoreFrontId = @storeId ORDER BY DateOrdered ASC;", connection);
        }
        else if (sortOrder == 4)
        {
            cmd = new SqlCommand("SELECT Orders.Id, Users.UserName, DateOrdered, Cart.Quantity, Product.Name, TotalCost, StoreFront.Id, Inventory.ProductPrice FROM Orders JOIN Cart ON (Cart.OrderId = Orders.Id) JOIN StoreFront ON (StoreFront.Id = Orders.StoreFrontId) JOIN Product ON (Product.Id = Cart.ProductId) JOIN Inventory ON (Inventory.StoreFrontId = Orders.StoreFrontId AND Inventory.ProductId = Cart.ProductId) JOIN Users ON (Users.Id = CustomerId) WHERE Orders.StoreFrontId = @storeId ORDER BY DateOrdered DESC;", connection);
        }
        else
        {
            cmd = new SqlCommand("SELECT Orders.Id, Users.UserName, DateOrdered, Cart.Quantity, Product.Name, TotalCost, StoreFront.Id, Inventory.ProductPrice FROM Orders JOIN Cart ON (Cart.OrderId = Orders.Id) JOIN StoreFront ON (StoreFront.Id = Orders.StoreFrontId) JOIN Product ON (Product.Id = Cart.ProductId) JOIN Inventory ON (Inventory.StoreFrontId = Orders.StoreFrontId AND Inventory.ProductId = Cart.ProductId) JOIN Users ON (Users.Id = CustomerId) WHERE Orders.StoreFrontId = @storeId ORDER BY StoreFront.Id;", connection);
        }

        cmd.Parameters.AddWithValue("@storeId", _storeId);

        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string userName = reader.GetString(1);
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
                ItemPrice = itemPrice,
                CustomerName = userName
            };
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

        cmd.Parameters.AddWithValue("@storeId", order.storeId);
        cmd.Parameters.AddWithValue("@customerId", order.customerId);
        cmd.Parameters.AddWithValue("@totalCost", order.TotalCost);
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

        foreach (Product product in order.cartContents)
        {
            cmd = new SqlCommand("INSERT INTO Cart (ProductId, Quantity, OrderId) VALUES (@productId, @quantity, @orderId)", connection);
            cmd.Parameters.AddWithValue("@productId", product.Id);
            cmd.Parameters.AddWithValue("@quantity", product.Quantity);
            cmd.Parameters.AddWithValue("@orderId", order.Id);

            cmd.ExecuteNonQuery();
        }

        connection.Close();

        UpdateInventoryViaOrder(order);
    }

    /// <summary>
    /// This method takes a product created by the user to be added into the Product Table via an Insert Sql call
    /// </summary>
    /// <param name="storeId">An integer to specify the store's inventory being altered</param>
    /// <param name="newProduct">An object of Product class that will be inserted into the Product Table</param>
    public void CreateProduct(int storeId, Product newProduct)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        SqlCommand cmd = new SqlCommand("INSERT INTO Product (Name, Description) OUTPUT INSERTED.Id VALUES (@productName, @productDesc)", connection);
        cmd.Parameters.AddWithValue("@productName", newProduct.Name);
        cmd.Parameters.AddWithValue("@productDesc", newProduct.Description);

        newProduct.Id = (int)cmd.ExecuteScalar();

        connection.Close();

        UpdateInventoryViaNewProduct(storeId, newProduct);
    }

    /// <summary>
    /// The inventory tied to a specific store is updated based on the Store instance field in the order instance
    /// which has the updated inventory to be used for updating the inventory table in the Azure database
    /// </summary>
    /// <param name="order">Instance of order passed along for inventory updating</param>
    public void UpdateInventoryViaOrder(Order order)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        SqlCommand cmd;

        foreach (Product product in order.storeInventory)
        {
            cmd = new SqlCommand("UPDATE Inventory SET Quantity = @quantity OUTPUT INSERTED.Id WHERE ProductId = @productId AND StoreFrontId = @storeFront", connection);
            cmd.Parameters.AddWithValue("@quantity", product.Quantity);
            cmd.Parameters.AddWithValue("@productId", product.Id);
            cmd.Parameters.AddWithValue("@storeFront", order.storeId);

            cmd.ExecuteNonQuery();
        }

        connection.Close();
    }

    /// <summary>
    /// This method takes a new product and inserts it into the inventory table in the database, but it will only add it for one store's inventory based on
    /// the store selected by the Employee user
    /// </summary>
    /// <param name="storeId">An integer tied to the store being updated</param>
    /// <param name="productToUpdate">An Object of Product class that is to be added to the database</param>
    public void UpdateInventoryViaNewProduct(int storeId, Product productToUpdate)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand("INSERT INTO Inventory (ProductId, StoreFrontId, Quantity, ProductPrice) VALUES (@productId, @storeFrontId, @quantity, @productPrice)", connection);
        cmd.Parameters.AddWithValue("@productId", productToUpdate.Id);
        cmd.Parameters.AddWithValue("@storeFrontId", storeId);
        cmd.Parameters.AddWithValue("@quantity", productToUpdate.Quantity);
        cmd.Parameters.AddWithValue("@productPrice", productToUpdate.Price);

        cmd.ExecuteNonQuery();

        connection.Close();
    }

    /// <summary>
    /// Takes a product object made from the user to alter its column data in the inventory table be it just a quantity change or a price change for the specified product
    /// </summary>
    /// <param name="storeId">The specific store integer to be used to access only this store's inventory</param>
    /// <param name="productToUpdate">An object of Product class to be used for altering Inventory table records</param>
    public void UpdateInventoryProduct(int storeId, Product productToUpdate)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand("UPDATE Inventory SET ProductPrice = @productPrice, Quantity = @productQty WHERE ProductId = @productId AND StoreFrontId = @storeId", connection);
        cmd.Parameters.AddWithValue("@productPrice", productToUpdate.Price);
        cmd.Parameters.AddWithValue("@productQty", productToUpdate.Quantity);
        cmd.Parameters.AddWithValue("@productId", productToUpdate.Id);
        cmd.Parameters.AddWithValue("@storeId", storeId);

        cmd.ExecuteNonQuery();

        connection.Close();
    }
}