using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;
using System.Text.Json;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductApiController> _logger;
        private readonly IDistributedCache _cache;

        public ProductApiController(AppDbContext context, ILogger<ProductApiController> logger, IDistributedCache cache)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var cached = await _cache.GetStringAsync("product_list");

            if (!string.IsNullOrEmpty(cached))
            {
                var products = JsonSerializer.Deserialize<List<Product>>(cached);
                return Ok(products);
            }

            var data = await _context.Products.ToListAsync();
            Console.WriteLine("DB'den veri çekildi");
            var serialized = JsonSerializer.Serialize(data);

            await _cache.SetStringAsync("product_list", serialized, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            return Ok(data);
        }

        // PUT: api/ProductApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;

            await _context.SaveChangesAsync();
            await _cache.RemoveAsync("product_list");

            _logger.LogInformation("ProductApi PUT çağrıldı. ID: {ProductId}", id);

            return Ok(product);
        }

        // GET: api/ProductApi/5
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                _logger.LogInformation("API Productı bulamadı");
                return NotFound();
            }

            _logger.LogInformation("ProductApi GET çağrıldı. ID: {ProductId}", id);
            return Ok(product);
        }

        // POST: api/ProductApi
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product newProduct)
        {
            if (newProduct == null)
            {
                _logger.LogInformation("ProductApi POST BadRequest");
                return BadRequest();
            }

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync("product_list");
            _logger.LogInformation("ProductApi POST çağrıldı.");
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
        }

        // DELETE: api/ProductApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                _logger.LogInformation("API Productı bulamadı");
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync("product_list");
            _logger.LogInformation("API Productı sildi ID: {ProductId}", id);
            return NoContent();
        }
    }
}