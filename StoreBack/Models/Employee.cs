namespace Models;
using System.Text.RegularExpressions;
using Serilog;

public class Employee : User
{
    // public void AddProduct()
    // {
    // ProductName:
    //     Product newProduct = new Product();
    //     string regularExpression = "^[a-zA-Z ]+$";
    //     Regex regex = new Regex(regularExpression);

    //     Console.WriteLine("What is the name of the product:");
    //     string productName = Console.ReadLine();

    //     if (regex.IsMatch(productName))
    //     {
    //         newProduct.Name = productName;
    //     }
    //     else
    //     {
    //         Console.WriteLine("Invalid Input, only letters allowed");
    //         goto ProductName;
    //     }

    // ProductDesc:
    //     Console.WriteLine("Give a description of the product:");
    //     string productDesc = Console.ReadLine();

    //     if (regex.IsMatch(productDesc))
    //     {
    //         newProduct.Description = productDesc;
    //     }
    //     else
    //     {
    //         Console.WriteLine("Invalid Input, only letters allowed");
    //         goto ProductDesc;
    //     }

    // ProductPrice:
    //     Console.WriteLine("How much will this product cost:");

    //     Log.Logger = new LoggerConfiguration()
    //         .MinimumLevel.Debug()
    //         .WriteTo.File("../Logs/ExceptionLogging.txt", rollingInterval: RollingInterval.Day)
    //         .CreateLogger();

    //     try
    //     {
    //         double productPrice = Convert.ToDouble(Console.ReadLine());
    //     }
    //     catch (Exception e)
    //     {
    //         Log.Information($"Exception Caught: {e}");
    //         goto ProductPrice;
    //     }
    // }

    // public void RemoveProduct()
    // {
    //     // Completely remove a product from the store
    // }

    // public void AdjustProductQuantity()
    // {
    //     // Search through products to adjust quantity of a specific product
    // }
}