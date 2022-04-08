namespace Models;

public class Order {

    public int Id { get; set; }
    public DateTime DateOrdered { get; set; }
    public User customer = new User();
    public Cart cart = new Cart();
    public Store store = new Store();
}