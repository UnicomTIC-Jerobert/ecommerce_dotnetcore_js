using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTOs;
using ProductAPI.Models;
using ProductAPI.Repositories;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _repository.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(ProductDTO productDto)
        {
            var product = new Product
            {
                ProductName = productDto.ProductName,
                Price = productDto.Price,
                Description = productDto.Description,
                ImageURL = productDto.ImageURL
            };
            await _repository.AddProductAsync(product);
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, ProductDTO productDto)
        {
            var product = await _repository.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            product.ProductName = productDto.ProductName;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.ImageURL = productDto.ImageURL;

            await _repository.UpdateProductAsync(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var result = await _repository.DeleteProductAsync(id);
            if (result == 0) return NotFound();
            return NoContent();
        }
    }
}
