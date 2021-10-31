using System;
using Microsoft.AspNetCore.Mvc;

namespace RichDomainStore.API.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected Guid CustomerId = Guid.Parse("4885e451-b0e4-4490-b959-04fabc806d32");
    }
}