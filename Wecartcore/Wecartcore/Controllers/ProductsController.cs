using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wecartcore.Models;
using Wecartcore.DTO;
using System.IO;
using System.Linq;

namespace WeCartCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ProductsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            // Include the Category when fetching products
            var products = _context.Products.Include(p => p.Category).ToList();
            return Ok(products);
        }

        // GET: api/Products/{id}
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID must be greater than 0.");
            }

            var product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // GET: api/Products/byname/{name}
        [HttpGet("byname/{name}")]
        public IActionResult GetProductByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name cannot be null or empty.");
            }

            var product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Name == name);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // GET: api/Products/orderedbyprice
        [HttpGet("orderedbyprice")]
        public IActionResult GetProductsOrderedByPrice()
        {
            var products = _context.Products.Include(p => p.Category).OrderByDescending(p => p.Price).ToList();
            return Ok(products);
        }

        // DELETE: api/Products?id={id}
        [HttpDelete("id")]
        public IActionResult DeleteProduct([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID must be greater than 0.");
            }

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return NoContent();
        }

        // POST: api/Products
        [HttpPost]
        public IActionResult AddProduct([FromForm] ProductDTO productDto)
        {
            string fileName = null;

            if (productDto.ProductImage != null)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/images");

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                fileName = Guid.NewGuid().ToString() + Path.GetExtension(productDto.ProductImage.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    productDto.ProductImage.CopyTo(stream);
                }
            }

            var product = new Product
            {
                Name = productDto.ProductName,
                Description = productDto.Description,
                Price = productDto.Price,
                CategoryId = productDto.CategoryId,
                ImageUrl = fileName != null ? "/uploads/images/" + fileName : null
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return Ok(product);
        }

        // PUT: api/Products/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromForm] ProductDTO productDto)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            if (productDto.ProductImage != null)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/images");

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productDto.ProductImage.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    productDto.ProductImage.CopyTo(stream);
                }

                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                product.ImageUrl = "/uploads/images/" + fileName;
            }

            product.Name = productDto.ProductName;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.CategoryId = productDto.CategoryId;

            _context.SaveChanges();

            return Ok(product);
        }

        // GET: api/Products/filtered
        [HttpGet("filtered")]
        public IActionResult GetFilteredProducts(int? categoryId, double? minPrice, double? maxPrice)
        {
            var products = _context.Products.Include(p => p.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId);
            }

            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }

            var result = products.ToList();
            return Ok(result);
        }

        // GET: api/Products/categories
        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            var categories = _context.Categories.ToList();
            return Ok(categories);
        }


        // GET: api/Products/latest
        [HttpGet("latest")]
        public IActionResult GetLatestProducts()
        {
            var latestProducts = _context.Products
                                         .Include(p => p.Category)
                                         .OrderByDescending(p => p.ProductId)  // Assuming ProductId represents the order of insertion
                                         .Take(5)
                                         .ToList();

            return Ok(latestProducts);
        }



    }
}
