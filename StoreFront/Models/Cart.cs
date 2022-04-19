namespace Models;

public class Cart
{

    private List<Product> Contents { get; set; } = new List<Product>(){};

    private double TotalCost { get; set; } = 0.00;

    public void AddItem(Product item)
    {   
        double price = item.Price * item.Quantity;
        Contents.Add(item);
        TotalCost += Math.Round(price, 2);
    }

    public Product RemoveItem(int item)
    {
        int amtOfItem = Contents[item].Quantity;
        TotalCost -= Math.Round((Contents[item].Price * amtOfItem), 2);
        Product product = Contents[item];
        Contents.RemoveAt(item);

        return product;
    }

    public double GetTotalCost() {
        return TotalCost;
    }

    public List<Product> AllProducts() {
        return Contents;
    }

    public void ClearCart()
    {
        Contents = new List<Product>() {};
        TotalCost = 0.00;
    }

    public bool IsCartEmpty()
    {
        bool isEmpty;

        if (Contents.Count > 0)
        {
            isEmpty = false;
        }
        else
        {
            isEmpty = true;
        }

        return isEmpty;
    }

    public void CartContents()
    {
        int i = 1;
        foreach (Product product in Contents)
        {
            Console.WriteLine($"[{i}] {product.Name} | ${product.Price} | {product.Quantity} QTY.");
            i++;
        }
        Console.WriteLine($"Total Cost: ${TotalCost}");
    }
}