using Haulage.Model.Helpers;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace Haulage.Model
{
    public class TripStop
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [ForeignKey(typeof(CustomerOrder))]
        public Guid OrderId { get; set; }

        [ForeignKey(typeof(Trip))]
        public Guid TripId { get; set; }

        [ForeignKey(typeof(DeliveryAddress))]
        public Guid DeliveryAddressId { get; set; }

        public CustomerOrder Order { get { return this.order; } }
        private CustomerOrder order;

        public DeliveryAddress Address { get { return this.address; } }
        private DeliveryAddress address;

        public DateTime? ExpectedAt { get; private set; }

        public StopStatus Status { get; internal set; } = StopStatus.Pending;  // Track the status of the stop

        public TripStop(CustomerOrder order, DeliveryAddress address)
        {
            this.Id = Guid.NewGuid();
            this.order = order;
            this.OrderId = order.Id;
            this.address = address;
            this.DeliveryAddressId = address.Id;
            DBHelpers.EnterToDB(this);
        }

        public TripStop() { }

        // Method to set the expected time for the stop
        public void SetExpected(DateTime expectedAt)
        {
            if (expectedAt < DateTime.Now)
            {
                throw new ArgumentException("Cannot set expected time in the past");
            }
            this.ExpectedAt = expectedAt;
            DBHelpers.UpdateDB(this);
        }

        // Method to update the delivery address
        public void SetDeliveryAddress(DeliveryAddress deliveryAddress)
        {
            this.address = deliveryAddress;
            this.DeliveryAddressId = deliveryAddress.Id;
            DBHelpers.UpdateDB(this);
        }

        // Method to update the order associated with this stop
        public void SetOrder(CustomerOrder order)
        {
            this.order = order;
            this.OrderId = order.Id;
            DBHelpers.UpdateDB(this);
        }

        // Method to associate the stop with a trip
        public bool SetTripId(Guid tripId)
        {
            this.TripId = tripId;
            return DBHelpers.UpdateDB(this);
        }

        // Method to confirm pickup or delivery
        public bool ConfirmPickupOrDelivery()
        {
            if (this.Status == StopStatus.Completed)
            {
                return false;  // Already completed, no need to update
            }

            this.Status = StopStatus.Completed;
            return DBHelpers.UpdateDB(this);
        }

        // Enum to track the status of the stop
        public enum StopStatus
        {
            Pending,
            Completed
        }
    }
}
