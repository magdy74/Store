using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Magdy.APIs.Attributes;
using Store.Magdy.APIs.Errors;
using Store.Magdy.Core.Dtos.Products;
using Store.Magdy.Core.Helper;
using Store.Magdy.Core.Services.Contract;
using Store.Magdy.Core.Specifications.Products;

namespace Store.Magdy.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [ProducesResponseType(typeof(PaginationResponse<ProductDto>), StatusCodes.Status200OK)]
        [HttpGet] // Get BaseUrl/api/Products?sort
        [Cached(100)]
        // Name, PriceAsc, PriceDesc
        public async Task<ActionResult<PaginationResponse<ProductDto>>> GetAllProdcuts([FromQuery] ProductSpecParams productSpec) //endpoint
        {
            var result = await _productService.GetAllProductsAsync(productSpec);

            return Ok(result); // 200
        }

        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("brands")] // Get BaseUrl/api/Products/brands 
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllBrands()
        {
            var result = await _productService.GetAllBrandsAsync();

            return Ok(result);
        }

        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("types")] // Get BaseUrl/api/Products/types 
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllTypes()
        {
            var result = await _productService.GetAllTypesAsync();

            return Ok(result);
        }

        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] // BaseUrl/api/Products/ 
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(400));

            var result = await _productService.GetProductByIdAsync(id.Value);

            if (result is null) return NotFound(new ApiErrorResponse(404,$"The Product With Id: {id} not found at DB :("));

            return Ok(result);
        }


    }
}
