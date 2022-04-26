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
    private readonly string _apiBaseURL = "https://computerstoreapi.azurewebsites.net/api/";
    private HttpClient client = new HttpClient();

    [Fact]
    public void UserShouldSetValidUsernames()
    {
        User user = new User();
        user.UserName = "Bailey";

        Assert.Equal("Bailey", user.UserName);
    }

    [Fact]
    public void QuantityProperlyUpdated()
    {
        Product product = new Product {
            Name = "Lollipop",
            Quantity = 12
        };

        product.Quantity -= 10;

        Assert.Equal(2, product.Quantity);
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