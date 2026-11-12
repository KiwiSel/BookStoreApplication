using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookstoreWebApplication1.Models;
using BookstoreWebApplication1.Data;
using BookstoreWebApplication1.DTOs;
using BookstoreWebApplication1.Services;
using BookstoreWebApplication1.Enums;

namespace BookstoreWebApplication1.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

         public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders(int? userId) {
        var orders = await _orderService.GetAllOrders(userId);
        if (orders == null) return NotFound();
        return Ok(orders);
        }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id) {
        var order = await _orderService.GetOrder(id);
        if (order is null) return NotFound();
        return Ok(order);
    }


    [HttpPost("{basketId}")]
    public async Task<ActionResult<Order>> CreateOrder(int basketId) {

        var result = await _orderService.CreateOrder(basketId);
        switch (result)
            {
                case ServiceError.None:
                    return Created();
                case ServiceError.BasketNotFound:
                        return NotFound("User does not have a configured basket");
                case ServiceError.EmptyBasket:
                    return BadRequest("No items selected to purchase");
                case ServiceError.UserNotFound:
                    return NotFound("User invalid");
                case ServiceError.BookNotFound:
                    return NotFound("Book does not exist");
                case ServiceError.InsufficientStock:
                    return BadRequest("No longer in stock");
                default:
                    return BadRequest("Something went wrong");

            }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            bool deleted = await _orderService.DeleteOrder(id);
            if (deleted) return NoContent();
            return NotFound();
        }

    [HttpPut("{id}")]
    public async Task<ActionResult<Order>> PutOrder(int id, OrderDTO inputOrder) {
        var result = await _orderService.UpdateOrder(id, inputOrder);
        if (result.Equals(ServiceError.OrderNotFound)) return NotFound();
        return NoContent();
        }

    }
}
