using Microsoft.AspNetCore.Mvc;
using SimpleMarket.MarketClasses;
using SimpleMarket.MarketClasses.Models;

namespace SimpleMarket.Controllers;

public partial class MarketController
{
    // GET: api/products
    [HttpGet("products/{id:int}")]
    public IActionResult GetProduct(int id)
    {
        return Ok(_dbContext.Products.FirstOrDefault(x => x.Id == id));
    }

    // GET: api/products/all
    [HttpGet("products/all/{count:int?}")]
    public IActionResult GetProducts(int? count)
    {
        return Ok(_dbContext.Products.Take(count ?? 20));
    }
    
    // POST: api/products
    [HttpPost("products")]
    public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
    {
        var product = productDto.ToProduct();
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
    }

    // DELETE: api/products/{id}
    [HttpDelete("products/{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}