using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Data;
using StoreApi.Dtos;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly BikestoreContext _context;

        public OrderController(BikestoreContext bikestoreContext)
        {
            _context = bikestoreContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var order = await _context.Orders.ToListAsync();

            return Ok(order);

        }


        [HttpGet("{order_id}")]
        public async Task<IActionResult> GetById(int order_id)
        {
            var order = _context.Orders.Where(o => o.OrderId == order_id)
                .Select(o => new OrderDto
                {
                    OrderDate = o.OrderDate,
                    OrderStatus = o.OrderStatus,
                    ShippedDate = o.ShippedDate,
                    CustomerId = o.CustomerId,
                    RequiredDate = o.RequiredDate,
                    StaffId = o.StaffId,
                    StoreId = o.StoreId
                }
                ).SingleOrDefault();

            if (order == null)
                return NotFound();

            return Ok(order);
        }




        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto o)
        {
            var isvalidCust = await _context.Customers.AnyAsync(c => c.CustomerId == o.CustomerId);

            var isvalidStaff = await _context.Staffs.AnyAsync(s => s.StaffId == o.StaffId);
            var isvalidStore = await _context.Stores.AnyAsync(s => s.StoreId == o.StoreId);
            if (!isvalidCust)
                return NotFound();
            if (!isvalidStore)
                return NotFound();
            if (!isvalidStaff)
                return NotFound();
            var order = new Order
            {
                OrderDate = o.OrderDate,
                StaffId = o.StaffId,
                StoreId = o.StoreId,
                RequiredDate = o.RequiredDate,
                CustomerId = o.CustomerId,
                OrderStatus = o.OrderStatus,
                ShippedDate = o.ShippedDate
            };

            _context.AddAsync(order);
            _context.SaveChanges();

            return Ok(order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderDto o)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.OrderDate = o.OrderDate;
            order.StoreId = o.StoreId;
            order.ShippedDate = o.ShippedDate;
            order.RequiredDate = o.RequiredDate;
            order.OrderStatus = o.OrderStatus;
            order.StaffId = o.StaffId;
            order.CustomerId = o.CustomerId;

            _context.SaveChanges();
            return Ok(order);


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            var orderItems = await _context.OrderItems
        .Where(oi => oi.OrderId == id)
    .ToListAsync();

            _context.OrderItems.RemoveRange(orderItems);
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();
            return Ok();

        }
    }
}
