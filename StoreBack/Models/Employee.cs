namespace Models;
using System.Text.RegularExpressions;
using Serilog;

public class Employee : User
{
    public Product NewProduct()
    {
    ProductName:
        Product newProduct = new Product();
        string regularExpression = "^[a-zA-Z ]+$";
        double productPrice;
        int productQty;
        Regex regex = new Regex(regularExpression);

        Console.WriteLine("What is the name of the product:");
        string productName = Console.ReadLine();

        if (regex.IsMatch(productName))
        {
            newProduct.Name = productName;
        }
        else
        {
            Console.WriteLine("Invalid Input, only letters allowed");
            goto ProductName;
        }

    ProductDesc:
        Console.WriteLine("Give a description of the product:");
        string productDesc = Console.ReadLine();

        if (regex.IsMatch(productDesc))
        {
            newProduct.Description = productDesc;
        }
        else
        {
            Console.WriteLine("Invalid Input, only letters allowed");
            goto ProductDesc;
        }

    ProductPrice:
        Console.WriteLine("How much will this product cost:");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("../Logs/ExceptionLogging.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            productPrice = Convert.ToDouble(Console.ReadLine());
        }
        catch (Exception e)
        {
            Log.Information($"Exception Caught: {e}");
            Console.WriteLine("Invalid Input");
            goto ProductPrice;
        }

    ProductQuantity:
        Console.WriteLine("How much of this product will be available:");

        try
        {
            productQty = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception e)
        {
            Log.Information($"Exception Caught: {e}");
            Console.WriteLine("Invalid Input");
            goto ProductQuantity;
        }

        Log.CloseAndFlush();

        newProduct.Quantity = productQty;
        newProduct.Price = productPrice;
        newProduct.Name = productName;
        newProduct.Description = productDesc;

        return newProduct;
    }
}