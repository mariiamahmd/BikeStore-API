using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApi.Data;
using StoreApi.Dtos;
using StoreApi.Models;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly BikestoreContext _context;

        public BrandController(BikestoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var brands = await _context.Brands.ToListAsync();

            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _context.Brands
                .Where(b => b.BrandId == id)
                .Select(b => new BrandDto
                {
                    BrandId = b.BrandId,
                    BrandName = b.BrandName
                })
                .SingleOrDefaultAsync();

            if (brand == null)
                return NotFound();

            return Ok(brand);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrand(BrandDto dto)
        {
            var brand = new Brand
            {
                BrandName = dto.BrandName
            };

            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();

            return Ok(brand);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(int id, BrandDto dto)
        {
            var brand = await _context.Brands.FindAsync(id);

            if (brand == null)
                return NotFound();

            brand.BrandName = dto.BrandName;

            await _context.SaveChangesAsync();

            return Ok(brand);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var brand = await _context.Brands.FindAsync(id);

            if (brand == null)
                return NotFound();

            _context.Brands.Remove(brand);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}