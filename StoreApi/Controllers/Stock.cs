using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApi.Data;
using StoreApi.Dtos;
using StoreApi.Models;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly BikestoreContext _context;

        public StockController(BikestoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var stocks = await _context.Stocks.ToListAsync();

            return Ok(stocks);
        }

        [HttpGet("{storeId}/{productId}")]
        public async Task<IActionResult> GetById(int storeId, int productId)
        {
            var stock = await _context.Stocks
                .Where(s => s.StoreId == storeId && s.ProductId == productId)
                .Select(s => new StockDto
                {
                    Quantity = s.Quantity,
                    StoreId = s.StoreId,
                    ProductId = s.ProductId
                })
                .SingleOrDefaultAsync();

            if (stock == null)
                return NotFound();

            return Ok(stock);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock(StockDto dto)
        {
            var validStore = await _context.Stores.AnyAsync(s => s.StoreId == dto.StoreId);
            var validProduct = await _context.Products.AnyAsync(p => p.ProductId == dto.ProductId);

            if (!validStore || !validProduct)
                return NotFound();

            var stock = new Stock
            {
                Quantity = dto.Quantity,
                StoreId = dto.StoreId,
                ProductId = dto.ProductId
            };

            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();

            return Ok(stock);
        }


        [HttpDelete("{storeId}/{productId}")]
        public async Task<IActionResult> DeleteStock(int storeId, int productId)
        {
            var stock = await _context.Stocks
                .SingleOrDefaultAsync(s => s.StoreId == storeId && s.ProductId == productId);

            if (stock == null)
                return NotFound();

            _context.Stocks.Remove(stock);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}