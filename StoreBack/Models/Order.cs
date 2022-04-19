namespace Models;

using System.Text.Json.Serialization;

public class Order {

    public int Id { get; set; }
    public DateTime DateOrdered { get; set; }
    public int customerId { get; set; }
    public double TotalCost { get; set; } = 0.00;
    public int storeId { get; set; }

    [JsonInclude]
    public List<Product> cartContents = new List<Product>();

    [JsonInclude]
    public List<Product> storeInventory = new List<Product>();
}