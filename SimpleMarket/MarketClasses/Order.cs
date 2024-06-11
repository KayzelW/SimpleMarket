namespace SimpleMarket.MarketClasses;

public class Order
{
    public int Id { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
    public DateTime OrderDate { get; set; }
}