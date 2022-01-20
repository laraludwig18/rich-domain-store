using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using RichDomainStore.Core.Communication.Mediator;
using RichDomainStore.Core.Messages.CommonMessages.Notifications;
using RichDomainStore.Sales.Application.Commands;
using RichDomainStore.Sales.Application.Handlers;
using RichDomainStore.Sales.Domain.Entities;
using RichDomainStore.Sales.Domain.Interfaces;
using RichDomainStore.Sales.UnitTests.Fixtures;
using Xunit;

namespace RichDomainStore.Sales.UnitTests.Application.Handlers
{
    [Collection(nameof(AddOrderItemCommandCollection))]
    public class AddOrderItemCommandHandlerTests : IClassFixture<OrderFixture>
    {
        private readonly AddOrderItemCommandFixture _addOrderItemCommandFixture;
        private readonly OrderFixture _orderFixture;
        private readonly AutoMocker _mocker;
        private readonly AddOrderItemCommandHandler _addOrderItemCommandHandler;
        private readonly AddOrderItemCommand _validCommand;
        private readonly Order _draftOrder;
        private readonly OrderItem _validOrderItem;
        public AddOrderItemCommandHandlerTests(
            AddOrderItemCommandFixture addOrderItemCommandFixture,
            OrderFixture orderFixture)
        {
            _addOrderItemCommandFixture = addOrderItemCommandFixture;
            _orderFixture = orderFixture;

            _mocker = new AutoMocker();
            _addOrderItemCommandHandler = _mocker.CreateInstance<AddOrderItemCommandHandler>();

            _mocker.GetMock<IOrderRepository>()
                .Setup(r => r.UnitOfWork.CommitAsync())
                .ReturnsAsync(true);

            _validCommand = _addOrderItemCommandFixture.GenerateValidCommand();

            _draftOrder = _orderFixture.GenerateDraftOrder();
            _validOrderItem = _orderFixture.GenerateValidOrderItem();
            _draftOrder.AddItem(_validOrderItem);
        }

        [Fact]
        public async Task Handle_NewOrder_ShouldExecuteSuccessfully()
        {
            // Act
            var result = await _addOrderItemCommandHandler.Handle(_validCommand, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _mocker.GetMock<IOrderRepository>().Verify(r => r.Add(It.IsAny<Order>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_NewItemToExistingDraftOrder_ShouldExecuteSuccessfully()
        {
            // Arrange
            _mocker.GetMock<IOrderRepository>()
                .Setup(r => r.GetDraftOrderByCustomerIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(_draftOrder);

            // Act
            var result = await _addOrderItemCommandHandler.Handle(_validCommand, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _mocker.GetMock<IOrderRepository>().Verify(r => r.AddItem(It.IsAny<OrderItem>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Order>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ExistingItemOnDraftOrder_ShouldExecuteSuccessfully()
        {
            // Arrange
            var command = _addOrderItemCommandFixture.GenerateValidCommand(_validOrderItem.ProductId);

            _mocker.GetMock<IOrderRepository>()
                .Setup(r => r.GetDraftOrderByCustomerIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(_draftOrder);

            // Act
            var result = await _addOrderItemCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _mocker.GetMock<IOrderRepository>().Verify(r => r.UpdateItem(It.IsAny<OrderItem>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Order>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ShouldReturnFalseAndSendNotifications()
        {
            // Arrange
            var command = _addOrderItemCommandFixture.GenerateInvalidCommand(itemQuantity: OrderItem.MinItemQuantity - 1);

            // Act
            var result = await _addOrderItemCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _mocker.GetMock<IMediatorHandler>()
                .Verify(m => m.PublishNotificationAsync(It.IsAny<DomainNotification>()), Times.Exactly(5));
        }
    }
}