using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RichDomainStore.API.Models;
using RichDomainStore.Catalog.Application.Services;
using RichDomainStore.Core.Bus;
using RichDomainStore.Sales.Application.Commands;

namespace RichDomainStore.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IProductAppService _productAppService;
        private readonly IMediatorHandler _mediatorHandler;
        public CartController(IProductAppService productAppService, IMediatorHandler mediatorHandler)
        {
            _productAppService = productAppService;
            _mediatorHandler = mediatorHandler;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AddItemAsync([FromBody] AddOrderItemRequest request)
        {
            var product = await _productAppService.GetByIdAsync(request.ProductId);

            if (product.StockQuantity < request.Quantity)
            {
                return BadRequest("Insufficient stock");
            }

            var command = new AddOrderItemCommand(CustomerId, product.Id, product.Name, request.Quantity, product.Value);
            await _mediatorHandler.SendCommandAsync(command);

            return Ok();
        }
    }
}