using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RichDomainStore.Catalog.Application.Dtos;
using RichDomainStore.Catalog.Application.Services;

namespace RichDomainStore.API.Controllers
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController : Controller
    {
        private readonly IProductAppService _productAppService;

        public ProductController(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _productAppService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetProductByIdAsync")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            var result = await _productAppService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpGet("categories/code/{categoryCode}")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetByCategoryCodeAsync(int categoryCode)
        {
            var result = await _productAppService.GetByCategoryCodeAsync(categoryCode);

            return Ok(result);
        }

        [HttpGet("categories")]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            var result = await _productAppService.GetCategoriesAsync();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AddProductAsync(AddProductDto createProductDTO)
        {
            var result = await _productAppService.AddProductAsync(createProductDTO);

            return CreatedAtRoute("GetProductByIdAsync", new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> UpdateProductAsync(Guid id, [FromBody] UpdateProductDto updateProductDTO)
        {
            var result = await _productAppService.UpdateProductAsync(id, updateProductDTO);

            return Ok(result);
        }

        [HttpPut("{id}/debit-stock")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> DebitStockAsync(Guid id, [FromBody] UpdateStockDto updateStockDTO)
        {
            var result = await _productAppService.DebitStockAsync(id, updateStockDTO);

            return Ok(result);
        }

        [HttpPut("{id}/re-stock")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ReStockAsync(Guid id, [FromBody] UpdateStockDto updateStockDTO)
        {
            var result = await _productAppService.ReStockAsync(id, updateStockDTO);

            return Ok(result);
        }
    }
}
