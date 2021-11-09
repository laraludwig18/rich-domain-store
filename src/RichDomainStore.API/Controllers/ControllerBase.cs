using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Core.Messages.CommonMessages.Notifications;

namespace RichDomainStore.API.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected Guid CustomerId = Guid.Parse("4885e451-b0e4-4490-b959-04fabc806d32");
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediatorHandler;

        protected ControllerBase(INotificationHandler<DomainNotification> notifications,
                                 IMediatorHandler mediatorHandler)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediatorHandler = mediatorHandler;
        }

        protected bool IsValidOperation()
        {
            return !_notifications.HasNotification();
        }

        protected IEnumerable<string> GetErrorMessages()
        {
            return _notifications.GetNotifications().Select(c => c.Value).ToList();
        }

        protected void NotifyError(string code, string message)
        {
            _mediatorHandler.PublishNotificationAsync(new DomainNotification(code, message));
        }
    }
}