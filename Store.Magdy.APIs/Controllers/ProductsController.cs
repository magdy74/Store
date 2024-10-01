using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Magdy.Core.Services.Contract;

namespace Store.Magdy.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet] // Get BaseUrl/api/Products 
        public async Task<IActionResult> GetAllProdcuts() //endpoint
        {
            var result = await _productService.GetAllProductsAsync();

            return Ok(result); // 200
        }

        [HttpGet("brands")] // Get BaseUrl/api/Products/brands 
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await _productService.GetAllBrandsAsync();

            return Ok(result);
        }

        [HttpGet("types")] // Get BaseUrl/api/Products/types 
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await _productService.GetAllTypesAsync();

            return Ok(result);
        }

        [HttpGet("{id}")] // BaseUrl/api/Products/ 
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null) return BadRequest("Invalid id !!");

            var result = await _productService.GetProductByIdAsync(id.Value);

            if (result is null) return NotFound($"The Product With Id: {id} not found at DB :(");

            return Ok(result);
        }


    }
}
