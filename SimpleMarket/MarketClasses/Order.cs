﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SimpleMarket.MarketClasses.Models;

namespace SimpleMarket.MarketClasses;

public class Order
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public List<ProductOrder> Products { get; set; } = [];
    public DateTime OrderDate { get; set; } = DateTime.Now;


}