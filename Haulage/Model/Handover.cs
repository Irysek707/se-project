using Haulage.Model.Constants;
using Haulage.Model.Helpers;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Haulage.Model
{
    public class Handover
    {
        public DateTime expectedHandover { get; set; }
        public DateTime actualHandover { get; set; }

        public bool Pickup ;

        [PrimaryKey]
        public Guid id { get; set; }

        [ForeignKey(typeof(CustomerOrder))]
        public Guid orderId { get; set; }

        public Handover(Guid orderId, DateTime expectedHandover, bool pickup)
        {
            this.expectedHandover = expectedHandover;
            this.Pickup = pickup;
            this.id = Guid.NewGuid();
            this.orderId = orderId;
            DBHelpers.EnterToDB(this);
        }

        public Handover() { }

        public bool changeActualTime(DateTime actualHandover)
        {
            this.actualHandover = actualHandover;
            return DBHelpers.EnterToDB(this);
        }

  

    }
}
