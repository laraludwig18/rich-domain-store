using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RichDomainStore.Core.Data.EventSourcing;

namespace RichDomainStore.API.Controllers
{
    [ApiController]
    [Route("api/v1/events")]
    public class EventController : Controller
    {
        private readonly IEventSourcingRepository _eventSourcingRepository;

        public EventController(IEventSourcingRepository eventSourcingRepository)
        {
            _eventSourcingRepository = eventSourcingRepository;
        }

        [HttpGet("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetEventsByAggregateIdAsync(Guid id)
        {
            var events = await _eventSourcingRepository.GetEventsByAggregateIdAsync(id);

            return Ok(events);
        }
    }
}