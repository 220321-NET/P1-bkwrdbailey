namespace Models;

public class Store
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public List<Product> Inventory { get; set; } = new List<Product>();

    public void DisplayStock()
    {
        int i = 1;
        foreach (Product product in Inventory)
        {
            Console.WriteLine($"[{i}] {product.Name} | ${product.Price} | {product.Quantity} QTY.\n{product.Description}");
            i++;
        }
    }

}