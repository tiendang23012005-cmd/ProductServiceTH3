using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("products")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public ProductsController(
            ProductDbContext context)
        {
            _context = context;
        }


        // =========================================
        // GET /products
        // Lấy tất cả sản phẩm
        // =========================================
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products
                .OrderBy(x => x.id)
                .ToListAsync();

            return Ok(products);
        }


        // =========================================
        // GET /products/{id}
        // Lấy sản phẩm theo ID
        // =========================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(
            int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.id == id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy sản phẩm"
                });
            }

            return Ok(product);
        }


        // =========================================
        // POST /products
        // Thêm sản phẩm
        // =========================================
        [HttpPost]
        public async Task<IActionResult> CreateProduct(
            [FromBody] Product product)
        {
            if (string.IsNullOrWhiteSpace(product.name))
            {
                return BadRequest(new
                {
                    message = "Tên sản phẩm không được để trống"
                });
            }

            if (product.price < 0)
            {
                return BadRequest(new
                {
                    message = "Giá sản phẩm không được âm"
                });
            }

            if (product.quantity < 0)
            {
                return BadRequest(new
                {
                    message = "Số lượng không được âm"
                });
            }

            product.created_at = DateTime.Now;
            product.updated_at = DateTime.Now;

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProduct),
                new { id = product.id },
                product
            );
        }


        // =========================================
        // PUT /products/{id}
        // Cập nhật sản phẩm
        // =========================================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(
            int id,
            [FromBody] Product request)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.id == id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy sản phẩm"
                });
            }

            if (string.IsNullOrWhiteSpace(request.name))
            {
                return BadRequest(new
                {
                    message = "Tên sản phẩm không được để trống"
                });
            }

            if (request.price < 0)
            {
                return BadRequest(new
                {
                    message = "Giá sản phẩm không được âm"
                });
            }

            if (request.quantity < 0)
            {
                return BadRequest(new
                {
                    message = "Số lượng không được âm"
                });
            }

            product.name = request.name;

            product.description = request.description;

            product.price = request.price;

            product.quantity = request.quantity;

            product.updated_at = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Cập nhật sản phẩm thành công",
                product = product
            });
        }


        // =========================================
        // DELETE /products/{id}
        // Xóa sản phẩm
        // =========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(
            int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.id == id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy sản phẩm"
                });
            }

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Xóa sản phẩm thành công"
            });
        }
    }
}