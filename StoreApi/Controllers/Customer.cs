using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Data;
using StoreApi.Dtos;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly BikestoreContext _context;
        private readonly IMapper _mapper;

        public CustomerController(BikestoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetallAsync()
        {
            var cust = await _context.Customers.ToListAsync();
            return Ok(cust);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            var cust = await _context.Customers.Where(c => c.CustomerId == id).Select
                    (c => new CustomerDto
                    {
                        CustomerId = c.CustomerId,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        Phone = c.Phone,
                        Email = c.Email,
                        State = c.State,
                        Street = c.Street,
                        ZipCode = c.ZipCode,
                        City = c.City,
                        Orders = c.Orders.Select(o => new OrderDto
                        {
                            OrderStatus = o.OrderStatus,
                            OrderDate = o.OrderDate,
                            StaffId = o.StaffId,
                            StoreId = o.StoreId,
                            ShippedDate = o.ShippedDate,
                            RequiredDate = o.RequiredDate
                        }).ToList()
                    }
                ).SingleOrDefaultAsync();

            if (cust == null)
                return NotFound();

            return Ok(cust);

        }

        [HttpPost]
        public async Task<IActionResult> PostCustomer(CustomerDto dto)
        {
            var cust = new Customer { FirstName = dto.FirstName, LastName = dto.LastName,
           Phone= dto.Phone, Email = dto.Email,
           State = dto.State,
           Street=dto.Street,
           City=dto.City,
           ZipCode=dto.ZipCode
          
            };

            await _context.Customers.AddAsync(cust);
            _context.SaveChanges();

            return Ok();


        }

        [HttpPut("{customer_id}")]

        public async Task<IActionResult> UpdateCust(int customer_id, [FromBody] CustomerDto dto)
        {
            var cust = await _context.Customers.FindAsync(customer_id);
            if (cust == null)
                return NotFound($"Customer with id {customer_id} is not found");

            cust.FirstName = dto.FirstName;
            cust.LastName = dto.LastName;

            _context.SaveChanges();
            return Ok(cust);

        }

        [HttpDelete("{customer_id}")]

        public async Task<IActionResult> DeleteCust(int customer_id)
        {
            var cust = await _context.Customers.FindAsync(customer_id);
            if (cust == null)
                return NotFound($"Customer with id {customer_id} is not found");

            var orders = await _context.Orders
     .Where(o => o.CustomerId == customer_id)
     .Include(o => o.OrderItem)
     .ToListAsync();

            foreach (var order in orders)
            {
                _context.OrderItems.RemoveRange(order.OrderItem);
            }

            _context.Orders.RemoveRange(orders);

            _context.Customers.Remove(cust);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
