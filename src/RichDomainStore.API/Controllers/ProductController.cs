using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RichDomainStore.Catalog.Application.DTOS;
using RichDomainStore.Catalog.Application.Services;

namespace RichDomainStore.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductAppService _productAppService;

        public ProductController(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _productAppService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetProductByIdAsync")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            var result = await _productAppService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpGet("CategoryCode/{categoryCode}")]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetByCategoryCodeAsync(int categoryCode)
        {
            var result = await _productAppService.GetByCategoryCodeAsync(categoryCode);

            return Ok(result);
        }

        [HttpGet("Categories")]
        [ProducesResponseType(typeof(IEnumerable<CategoryDTO>), StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            var result = await _productAppService.GetCategoriesAsync();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status201Created)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AddProductAsync(AddProductDTO createProductDTO)
        {
            var result = await _productAppService.AddProductAsync(createProductDTO);

            return CreatedAtRoute("GetProductByIdAsync", new { id = result.Id }, result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> UpdateProductAsync(UpdateProductDTO updateProductDTO)
        {
            var result = await _productAppService.UpdateProductAsync(updateProductDTO);

            return Ok(result);
        }

        [HttpPut("{id}/DebitStock")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> DebitStockAsync(Guid id, [FromBody] UpdateStockDTO updateStockDTO)
        {
            var result = await _productAppService.DebitStockAsync(id, updateStockDTO);

            return Ok(result);
        }

        [HttpPut("{id}/ReStock")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ReStockAsync(Guid id, [FromBody] UpdateStockDTO updateStockDTO)
        {
            var result = await _productAppService.ReStockAsync(id, updateStockDTO);

            return Ok(result);
        }
    }
}
