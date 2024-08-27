using System;
using NUnit.Framework;
using Haulage;
using Haulage.Model;
using Haulage.Model.Constants;
using Haulage.Control;

namespace Haulage.Tests
{
    public class CustomerControllerIntegrationTests
    {
        private CustomerController _controller;
        private DB _database;

        [SetUp]
        public void Setup()
        {
            _database = new DB(); // Initialize the database
            _controller = new CustomerController("testCustomer");

            // Optionally, you can clear tables or insert necessary test data here
            DB.connection.DeleteAll<CustomerOrder>();
            DB.connection.DeleteAll<Handover>();
        }


        [Test]
        public void ScheduleHandover_WithValidInputs_ShouldUpdateHandover()
        {
            // Arrange
            var order = new CustomerOrder
            {
                Id = Guid.NewGuid(),
                Status = Status.PENDING
            };
            DB.connection.Insert(order);

            DateTime handoverDate = DateTime.Now.AddDays(1);
            bool pickup = true;

            // Act
            var result = _controller.ScheduleHandover(order, handoverDate, pickup);

            // Assert
            var updatedOrder = DB.connection.Find<CustomerOrder>(order.Id);
            Assert.AreEqual(Status.AWAITING_PICKUP, updatedOrder.Status);
            Assert.NotNull(result);
            Assert.AreEqual(handoverDate, result.ExpectedHandover);
            Assert.AreEqual(pickup, result.Pickup);
        }

        [Test]
        public void ScheduleHandover_WithPastDate_ShouldThrowException()
        {
            // Arrange
            var order = new CustomerOrder
            {
                Id = Guid.NewGuid(),
                Status = Status.PENDING
            };
            DB.connection.Insert(order);

            DateTime pastDate = DateTime.Now.AddDays(-1);
            bool pickup = true;

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _controller.ScheduleHandover(order, pastDate, pickup));
            Assert.AreEqual("Cannot pick date less than the current day", ex.Message);
        }

        [Test]
        public void ConfirmPickup_ShouldUpdateStatusToExpected_WhenHandoverIsScheduled()
        {
            // Arrange
            var order = new CustomerOrder
            {
                Id = Guid.NewGuid(),
                Status = Status.AWAITING_PICKUP
            };
            var handover = new Handover(order.Id, DateTime.Now.AddDays(1), true);
            order.AddHandover(handover);

            DB.connection.Insert(order);
            DB.connection.Insert(handover);

            // Act
            order.ConfirmPickup();
            DB.connection.Update(order);

            // Assert
            var updatedOrder = DB.connection.Find<CustomerOrder>(order.Id);
            Assert.AreEqual(Status.EXPECTED, updatedOrder.Status);
        }

        [Test]
        public void ConfirmDelivery_ShouldUpdateStatusToCollected_WhenConditionsAreMet()
        {
            // Arrange
            var order = new CustomerOrder
            {
                Id = Guid.NewGuid(),
                Status = Status.EXPECTED
            };
            var handover = new Handover(order.Id, DateTime.Now.AddDays(1), false);
            order.AddHandover(handover);

            DB.connection.Insert(order);
            DB.connection.Insert(handover);

            // Act
            order.ConfirmDelivery();
            DB.connection.Update(order);

            // Assert
            var updatedOrder = DB.connection.Find<CustomerOrder>(order.Id);
            Assert.AreEqual(Status.COLLECTED, updatedOrder.Status);
        }
    }
}
