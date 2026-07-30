using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApi.Data;
using StoreApi.Dtos;
using StoreApi.Models;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly BikestoreContext _context;

        public StoreController(BikestoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var stores = await _context.Stores.ToListAsync();

            return Ok(stores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var store = await _context.Stores
                .Where(s => s.StoreId == id)
                .Select(s => new StoreDto
                {
                    StoreId = s.StoreId,
                    StoreName = s.StoreName,
                    Phone = s.Phone,
                    Email = s.Email,
                    Street = s.Street,
                    City = s.City,
                    State = s.State,
                    ZipCode = s.ZipCode
                })
                .SingleOrDefaultAsync();

            if (store == null)
                return NotFound();

            return Ok(store);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStore(StoreDto dto)
        {
            var store = new Store
            {
                StoreName = dto.StoreName,
                Phone = dto.Phone,
                Email = dto.Email,
                Street = dto.Street,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode
            };

            await _context.Stores.AddAsync(store);
            await _context.SaveChangesAsync();

            return Ok(store);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStore(int id, StoreDto dto)
        {
            var store = await _context.Stores.FindAsync(id);

            if (store == null)
                return NotFound();

            store.StoreName = dto.StoreName;
            store.Phone = dto.Phone;
            store.Email = dto.Email;
            store.Street = dto.Street;
            store.City = dto.City;
            store.State = dto.State;
            store.ZipCode = dto.ZipCode;

            await _context.SaveChangesAsync();

            return Ok(store);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var store = await _context.Stores.FindAsync(id);

            if (store == null)
                return NotFound();

            _context.Stores.Remove(store);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}