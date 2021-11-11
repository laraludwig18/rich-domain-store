using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Core.Messages.CommonMessages.Notifications;
using RichDomainStore.Sales.Application.Queries;
using RichDomainStore.Sales.Application.Queries.Dtos;

namespace RichDomainStore.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IGetCustomerOrdersQuery _getCustomerOrdersQuery;

        public OrderController(IGetCustomerOrdersQuery getCustomerOrdersQuery, 
                                INotificationHandler<DomainNotification> notifications, 
                                IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            _getCustomerOrdersQuery = getCustomerOrdersQuery;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetCustomerOrdersAsync()
        {
            var orders = await _getCustomerOrdersQuery.HandleAsync(CustomerId);

            return Ok(orders);
        }
    }
}