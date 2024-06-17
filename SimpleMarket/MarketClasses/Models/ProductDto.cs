namespace SimpleMarket.MarketClasses.Models;

public class ProductDto
{
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }
    
    public Product ToProduct() => new Product()
    {
        Name = this.Name,
        Price = this.Price,
        Count = this.Count,
    };
}