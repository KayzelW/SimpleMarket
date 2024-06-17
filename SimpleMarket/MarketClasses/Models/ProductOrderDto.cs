namespace SimpleMarket.MarketClasses.Models;

public class ProductOrderDto
{
    public int ProductId { get; set; }
    public int Count { get; set; }

    public ProductOrder ToProductOrder() => new ProductOrder()
    {
        ProductId = this.ProductId,
        Count = this.Count
    };
}