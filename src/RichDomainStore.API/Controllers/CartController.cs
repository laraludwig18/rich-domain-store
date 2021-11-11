using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RichDomainStore.API.Models;
using RichDomainStore.Catalog.Application.Services;
using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Core.Messages.CommonMessages.Notifications;
using RichDomainStore.Sales.Application.Commands;
using RichDomainStore.Sales.Application.Queries;
using RichDomainStore.Sales.Application.Queries.Dtos;

namespace RichDomainStore.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IProductAppService _productAppService;
        private readonly IGetCustomerCartQuery _getCustomerCartQuery;
        private readonly IMediatorHandler _mediatorHandler;

        public CartController(INotificationHandler<DomainNotification> notifications,
                                IGetCustomerCartQuery getCustomerCartQuery,
                                IProductAppService productAppService,
                                IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            _productAppService = productAppService;
            _getCustomerCartQuery = getCustomerCartQuery;
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetCustomerCartAsync()
        {
            var cart = await _getCustomerCartQuery.HandleAsync(CustomerId);

            return Ok(cart);
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

            if (IsValidOperation())
            {
                return Ok();
            }

            var errors = GetErrorMessages();
            return BadRequest(errors);
        }
    }
}