using Haulage.Model.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ForeignKeyAttribute = SQLiteNetExtensions.Attributes.ForeignKeyAttribute;

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

        [ForeignKey(typeof (DeliveryAddress))]
        public Guid DeliveryAddressId { get; set; }
        public CustomerOrder Order{ get { return this.order; }  }
        private CustomerOrder order;
        public DeliveryAddress Address { get { return this.address; } }
        private DeliveryAddress address;

        public DateTime? ExpectedAt;

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

        public void SetExpected(DateTime expectedAt)
        {
            if(expectedAt < DateTime.Now)
            {
                throw new ArgumentException("Cannot set expected time in the past");
            }
            this.ExpectedAt = expectedAt;
            DBHelpers.UpdateDB(this);
        }

        public void  setDeliveryAddress(DeliveryAddress deliveryAddress) { 
            this.address = deliveryAddress;
        }

        public void setOrder(CustomerOrder order) { 
            this.order = order;
        }

        public bool setTripId(Guid tripId)
        {
            this.TripId = tripId;
            return DBHelpers.UpdateDB(this);
        }
       
    }
}
