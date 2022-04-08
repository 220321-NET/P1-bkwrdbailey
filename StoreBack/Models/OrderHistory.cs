namespace Models;

public class OrderHistory
{
    public int OrderId { get; set; }
    public int StoreId { get; set; }
    public double TotalCost { get; set; }
    public int ItemQty { get; set; }
    public double ItemPrice { get; set; }
    public DateTime DateOrdered { get; set; }
    public User customer = new User();
    public Store store = new Store();
    public string ProductName { get; set; } = "";
}