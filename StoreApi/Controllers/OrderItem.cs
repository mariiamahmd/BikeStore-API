using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApi.Data;
using StoreApi.Dtos;
using StoreApi.Models;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly BikestoreContext _context;

        public OrderItemController(BikestoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var items = await _context.OrderItems.ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.OrderItems
                .Where(i => i.ItemId == id)
                .Select(i => new OrderItemDto
                {
                    ItemId = i.ItemId,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    ListPrice = i.ListPrice,
                    Discount = i.Discount,
                    OrderId = i.OrderId
                })
                .SingleOrDefaultAsync();

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderItem(OrderItemDto dto)
        {
            var validOrder = await _context.Orders.AnyAsync(o => o.OrderId == dto.OrderId);
            var validProduct = await _context.Products.AnyAsync(p => p.ProductId == dto.ProductId);

            if (!validOrder || !validProduct)
                return NotFound();

            var item = new OrderItem
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                ListPrice = dto.ListPrice,
                Discount = dto.Discount,
                OrderId = dto.OrderId
            };

            await _context.OrderItems.AddAsync(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, OrderItemDto dto)
        {
            var item = await _context.OrderItems.FindAsync(id);

            if (item == null)
                return NotFound();

            item.ProductId = dto.ProductId;
            item.Quantity = dto.Quantity;
            item.ListPrice = dto.ListPrice;
            item.Discount = dto.Discount;
            item.OrderId = dto.OrderId;

            await _context.SaveChangesAsync();

            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var item = await _context.OrderItems.FindAsync(id);

            if (item == null)
                return NotFound();

            _context.OrderItems.Remove(item);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}