using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleMarket.MarketClasses;
using SimpleMarket.MarketClasses.Models;

namespace SimpleMarket.Controllers;

public partial class MarketController
{
    // GET: api/order
    [HttpGet("orders/{id:int}")]
    public IActionResult GetOrder(int? id)
    {
        return Ok(_dbContext.Orders.Include(x => x.Products).FirstOrDefault(x => x.Id == id));
    }

    // GET: api/orders
    [HttpGet("orders/all/{count:int?}")]
    public IActionResult GetOrders(int? count)
    {
        return Ok(_dbContext.Orders.Include(x => x.Products).Take(count ?? 20));
    }

    // POST: api/orders
    [HttpPost("orders")]
    public async Task<IActionResult> CreateOrder([FromBody] List<ProductOrderDto> productOrderDtos)
    {
        var productOrders = new List<ProductOrder>(productOrderDtos.Select(x => x.ToProductOrder()));
        var productIds = productOrders.Select(x => x.ProductId);
        var products = _dbContext.Products.Where(p => productIds.Contains(p.Id)).ToList();

        foreach (var product in products)
        {
            var orderedCount = productOrders.First(x => x.ProductId == product.Id).Count;
            if (orderedCount > product.Count)
            {
                _logger.LogWarning(
                    $"Ordered count({orderedCount}) of {product.Id}:{product.Name} more then have: {product.Count}");
            }

            product.Count -= orderedCount;
        }

        if (products.Count != productOrders.Count)
        {
            return BadRequest("One or more products are not found.");
        }

        var order = new Order
        {
            Products = productOrders,
            OrderDate = DateTime.UtcNow
        };

        productOrders.ForEach(x => x.Order = order);

        _dbContext.Products.UpdateRange(products);
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.ProductOrders.AddRangeAsync(productOrders);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
    }

    // DELETE: api/orders/{id}
    [HttpDelete("orders/{id:int}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = _dbContext.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            return NotFound();
        }

        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}