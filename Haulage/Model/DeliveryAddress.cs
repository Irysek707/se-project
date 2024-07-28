using Haulage.Model.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForeignKeyAttribute = SQLiteNetExtensions.Attributes.ForeignKeyAttribute;

namespace Haulage.Model
{
    class DeliveryAddress
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [ForeignKey(typeof(CustomerOrder))]
        public Guid OrderId { get; set; }

        //numbers from -180 to 180
        double Longitude { get; set; }

        // numbers from -90 to 90

        double Latitude { get; set; }

        public DeliveryAddress() { }

        public DeliveryAddress(Guid orderId, double longitude, double latitude)
        {
            this.Id = Guid.NewGuid();
            this.OrderId = orderId;
            this.Longitude = longitude;
            this.Latitude = latitude;
            DBHelpers.EnterToDB(this);
        }
    }
}
