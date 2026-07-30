using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Data;
using StoreApi.Dtos;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly BikestoreContext _context;

        public ProductController(BikestoreContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts(int page=1,int page_size=10)
        {
            var products = await _context.Products.OrderBy(p=>p.ProductId).Skip((page-1)*page_size).Take(10).ToListAsync();
            return Ok(products);
        }



        [HttpGet("{year}")]
        public async Task<IActionResult> GetProductsModelYear(int year)
        {
            var product = await _context.Products.Where(x => x.ModelYear > year).OrderBy(p => p.ModelYear).ToListAsync();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto dto)
        {
            var product = new ProductDto
            {
                ProductName = dto.ProductName,
                BrandId = dto.BrandId,
                CategoryId = dto.CategoryId,
                ListPrice = dto.ListPrice,
                ModelYear = dto.ModelYear

            };
            _context.Add(product);
            _context.SaveChanges();

            return Ok(product);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateProduct(int id, ProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();
            product.ProductName = dto.ProductName;
            product.BrandId = dto.BrandId;
            product.ModelYear = dto.ModelYear;
            product.CategoryId = dto.CategoryId;
            product.ListPrice = dto.ListPrice;

            _context.SaveChanges();
            return Ok(product);


        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();
            return
                Ok(product);

        }
    }
}
