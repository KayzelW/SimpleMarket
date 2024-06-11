using Microsoft.AspNetCore.Mvc;
using SimpleMarket.MarketClasses;

namespace SimpleMarket.Controllers;

/*
Вариант 1: Реализовать сервис с JSON - сервис для предоставления информации о списке товаров,
а также возможностью заказа/удаления товара и заказа.

В качестве БД можно использовать статический объект в контроллере asp.net или любую известную вам реляционную СУБД
В качестве клиента - любой инструмент для отправки и получения запросов. Ex: Insomnia или Restlet плагин для Хрома

Как минимум, должны быть реализованы сущности "Товар" и "Заказ", и методы работы с этими сущностями:
  добавление нового товара в список, удаление существующего товара,
  просмотр списка существующих товаров,
  создание заказа на товар (или товары - тут реализация на усмотрение кандидата),
  удаление заказа,
  просмотр списка заказов.
 */

[ApiController, Route("api/[controller]")]
public class MarketController : Controller
{
    private static List<Product> _products = [];
    private static List<Order> _orders = [];
    private static int _productIdCounter = 1;
    private static int _orderIdCounter = 1;

    #region Products

    // GET: api/products
    [HttpGet("products")]
    public IActionResult GetProducts()
    {
        return Ok(_products);
    }

    // POST: api/products
    [HttpPost("products")]
    public IActionResult AddProduct([FromBody] Product product)
    {
        product.Id = _productIdCounter++;
        _products.Add(product);
        return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
    }

    // DELETE: api/products/{id}
    [HttpDelete("products/{id:int}")]
    public IActionResult DeleteProduct(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        _products.Remove(product);
        return NoContent();
    }

    #endregion


    #region Orders

    // GET: api/orders
    [HttpGet("orders")]
    public IActionResult GetOrders()
    {
        return Ok(_orders);
    }

    // POST: api/orders
    [HttpPost("orders")]
    public IActionResult CreateOrder([FromBody] List<int> productIds)
    {
        var products = _products.Where(p => productIds.Contains(p.Id)).ToList();
        if (products.Count != productIds.Count)
        {
            return BadRequest("One or more products are not found.");
        }

        var order = new Order
        {
            Id = _orderIdCounter++,
            Products = products,
            OrderDate = DateTime.UtcNow
        };
        _orders.Add(order);

        return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
    }

    // DELETE: api/orders/{id}
    [HttpDelete("orders/{id:int}")]
    public IActionResult DeleteOrder(int id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            return NotFound();
        }

        _orders.Remove(order);
        return NoContent();
    }

    #endregion
}