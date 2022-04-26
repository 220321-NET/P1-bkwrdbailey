using Xunit;
using Models;
using DL;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Tests;

public class UnitTests
{
    // private readonly string _apiBaseURL = "https://computerstoreapi.azurewebsites.net/api/";
    private HttpClient client = new HttpClient();

    [Fact]
    public void UserShouldSetValidUsernames()
    {
        User user = new User();
        user.UserName = "Bailey";

        Assert.Equal("Bailey", user.UserName);
    }

    [Fact]
    public void UserShouldSetValidPassword()
    {
        User user = new User();
        user.Password = "Password123";

        Assert.Equal("Password123", user.Password);
    }

    [Fact]
    public void QuantityProperlyUpdated()
    {
        Product product = new Product
        {
            Name = "Lollipop",
            Quantity = 12
        };

        product.Quantity -= 10;

        Assert.Equal(2, product.Quantity);
    }

    [Fact]
    public void StoreHasValidName()
    {
        Store store = new Store();

        store.Name = "Boopity and More";

        Assert.Equal("Boopity and More", store.Name);
    }

    [Fact]
    public void StoreHasValidAddress()
    {
        Store store = new Store();

        store.Address = "Down the road";

        Assert.Equal("Down the road", store.Address);
    }

    [Fact]
    public void StoreHasCorrectInventory()
    {
        List<Product> InventoryTest = new List<Product>();

        InventoryTest.Add(new Product { Id = 1, Name = "Object", Price = 10.00, Description = "Just an Object", Quantity = 7 });

        Store store = new Store();
        store.Inventory = InventoryTest;

        Assert.Equal(InventoryTest, store.Inventory);
    }

    [Fact]
    public void ProductNameIsValid()
    {
        Product product = new Product();

        product.Name = "Item";

        Assert.Equal("Item", product.Name);
    }

    [Fact]
    public void ProductDescriptionIsValid()
    {
        Product product = new Product();

        product.Description = "Item is good";

        Assert.Equal("Item is good", product.Description);
    }

    [Fact]
    public void ProductPriceIsValid()
    {
        Product product = new Product();

        product.Price = 9.99;

        Assert.Equal(9.99, product.Price);
    }

    [Fact]
    public void ProductQuantityIsValid()
    {
        Product product = new Product();

        product.Quantity = 25;

        Assert.Equal(25, product.Quantity);
    }

    [Fact]
    public void OrderTotalCostIsValid()
    {
        Order order = new Order();

        order.TotalCost = 99.99;

        Assert.Equal(99.99, order.TotalCost);
    }

    // [Fact]
    // public async void AllUsersObtainedFromDB() {
    //     client.BaseAddress = new Uri(_apiBaseURL);
    //     List<User> users = new List<User>();
    //     string connectionString = File.ReadAllText("./connectionString.txt");

    //     DBRepository dbRepo = new DBRepository(connectionString);

    //     users = await JsonSerializer.DeserializeAsync<List<User>>(await client.GetStreamAsync("Store/GetUsers")) ?? new List<User>();
    //     List<User> actualUsers = await dbRepo.GetAllUsersAsync();

    //     Assert.Equal(actualUsers.Count, users.Count);
    // }

    // [Fact]
    // public async void CorrectInventoryObtainedFromDB() {
    //     client.BaseAddress = new Uri(_apiBaseURL);
    //     List<Product> inventory = new List<Product>();
    //     string connectionString = File.ReadAllText("./connectionString.txt");

    //     DBRepository dbRepo = new DBRepository(connectionString);

    //     inventory = await JsonSerializer.DeserializeAsync<List<Product>>(await client.GetStreamAsync($"Store/GetInventory/{2}")) ?? new List<Product>();
    //     List<Product> actualInventory = dbRepo.GetStoreInventory(2);

    //     Assert.Equal(actualInventory.Count, inventory.Count);
    // }
}