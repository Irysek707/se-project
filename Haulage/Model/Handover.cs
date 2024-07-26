using Haulage.Model.Constants;
using Haulage.Model.Helpers;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Haulage.Model
{
    public class Handover
    {
        public DateTime ExpectedHandover { get; set; }
        public DateTime ActualHandover { get; set; }

        public bool Pickup { get; set; }

        [PrimaryKey]
        public Guid Id { get; set; }

        [ForeignKey(typeof(CustomerOrder))]
        public Guid OrderId { get; set; }

        public Handover(Guid orderId, DateTime expectedHandover, bool pickup)
        {
            this.ExpectedHandover = expectedHandover;
            this.Pickup = pickup;
            this.Id = Guid.NewGuid();
            this.OrderId = orderId;
            DBHelpers.EnterToDB(this);
        }

        public Handover() { }

        public bool ChangeActualTime(DateTime actualHandover)
        {
            this.ActualHandover = actualHandover;
            return DBHelpers.EnterToDB(this);
        }

  

    }
}
