namespace Models;

public class Product {

    public int Id { get; set; }
    public string Name { get; set; } = "";
    public double Price { get; set; }
    public string Description { get; set; } = "";
    public int Quantity { get; set; } = 0;

    public void IncreaseQty(int amtToAdd) 
    {
        Quantity += amtToAdd;
    }

    public void DecreaseQty(int amtToDecrease) 
    {
        Quantity -= amtToDecrease;
    }

    public void ChangePrice(float newPrice) {
        Price = newPrice;
    }
}