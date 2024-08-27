using System;
using NUnit.Framework;
using Haulage;
using Haulage.Model;
using Haulage.Model.Constants;
using Haulage.Control;
using System.Collections.Generic;
using NSubstitute;

namespace Haulage.Tests
{
    public class UnitTests
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

        [Test]
        public void ReportDelay_ShouldUpdateTripStatusToDelayed()
        {
            // Arrange
            var trip = new Trip
            {
                Id = Guid.NewGuid(),
                TripStatus = TripStatus.ONTIME
            };
            DB.connection.Insert(trip);

            // Act
            trip.DelayTrip();
            DB.connection.Update(trip);

            // Assert
            var updatedTrip = DB.connection.Find<Trip>(trip.Id);
            Assert.AreEqual(TripStatus.DELAYED, updatedTrip.TripStatus);
        }

        [Test]
        public void TestAddDriver_Success()
        {
            // Arrange
            var driver = new Driver("testDriver", "John", "Doe");

            // Act
            AdminController.AddDriver(driver);
            var insertedDriver = DB.connection.Find<Driver>(driver.Login);

            // Assert
            Assert.NotNull(insertedDriver);
            Assert.AreEqual(driver.Login, insertedDriver.Login);
            Assert.AreEqual(driver.Name, insertedDriver.Name);
            Assert.AreEqual(driver.Surname, insertedDriver.Surname);
        }

        [Test]
        public void TestAddDriver_Failure()
        {
            // Arrange
            var driver = new Driver(null, "John", "Doe");

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => AdminController.AddDriver(driver));
            Assert.AreEqual("Error adding driver: Driver cannot be null", ex.Message);
        }

        [Test]
        public void TestUpdateDriver_Success()
        {
            // Arrange
            var driver = new Driver("testDriver", "John", "Doe");
            AdminController.AddDriver(driver);

            // Act
            driver.Name = "Jane";
            driver.Surname = "Smith";
            AdminController.UpdateDriver(driver);
            var updatedDriver = DB.connection.Find<Driver>(driver.Login);

            // Assert
            Assert.AreEqual("Jane", updatedDriver.Name);
            Assert.AreEqual("Smith", updatedDriver.Surname);
        }

        [Test]
        public void TestUpdateDriver_Failure()
        {
            // Arrange
            var driver = new Driver("testDriver", "John", "Doe");
            AdminController.AddDriver(driver);

            // Modify to an invalid driver data
            driver.Name = "";
            driver.Surname = "";

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => AdminController.UpdateDriver(driver));
            Assert.AreEqual("Error updating driver: No records were updated. Check if the driver exists and has the correct Login.", ex.Message);
        }

        [Test]
        public void TestDeleteDriver_Success()
        {
            // Arrange
            var driver = new Driver("testDriver", "John", "Doe");
            AdminController.AddDriver(driver);

            // Act
            AdminController.DeleteDriver(driver);
            var deletedDriver = DB.connection.Find<Driver>(driver.Login);

            // Assert
            Assert.IsNull(deletedDriver);
        }

        [Test]
        public void TestDeleteDriver_Failure()
        {
            // Arrange
            var driver = new Driver("nonExistentDriver", "John", "Doe");

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => AdminController.DeleteDriver(driver));
            Assert.AreEqual("Error deleting driver: Driver with the provided Login does not exist.", ex.Message);
        }



    }

}
