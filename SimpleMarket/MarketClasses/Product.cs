using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SimpleMarket.MarketClasses.Models;

namespace SimpleMarket.MarketClasses;

public class Product
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } //Better if was Guid, but we work with our hand...

    public string? Name { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }

    
}