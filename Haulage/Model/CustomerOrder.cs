using Haulage.Model.Constants;
using Haulage.Model.Helpers;
using SQLite;
using System.ComponentModel.DataAnnotations.Schema;
using ForeignKeyAttribute = SQLiteNetExtensions.Attributes.ForeignKeyAttribute;

namespace Haulage.Model
{
    public class CustomerOrder
    {
        [ForeignKey(typeof(Manifest))]
        public Guid ManifestId { get; set; }

        [ForeignKey(typeof(User))]
        public string Customer { get; set; }

        public Manifest Manifest { get { return this.manifest; } }
        private Manifest manifest { get; set; }

        public Handover Handover { get { return this.handover; } }
        private Handover handover { get; set; } = null;

        [PrimaryKey]
        public Guid Id { get { return id; } set { this.id = value; } }
        private Guid id { get; set; }

        public Status Status { get { return status; } set { this.status = value; } }

        [ForeignKey(typeof(Warehouse))]
        public Guid WarehouseId { get; set; }

        private Status status { get; set; }

        public CustomerOrder(Manifest manifest, string user, Guid warehouseId)
        {
            this.manifest = manifest;
            this.Customer = user;
            this.ManifestId = manifest.Id;
            this.id = Guid.NewGuid();
            this.status = Status.PENDING;
            this.WarehouseId = warehouseId;
            DBHelpers.EnterToDB(this);
        }

        public CustomerOrder() { }

        public void AddManifest(Manifest manifest)
        {
            this.manifest = manifest;
        }

        public void AddHandover(Handover handover)
        {
            this.handover = handover;
        }

        public Handover ScheduleHandover(DateTime handoverDate, bool pickup)
        {
            this.handover = new Handover(this.Id, handoverDate, pickup);
            this.status = pickup ? Status.AWAITING_PICKUP : Status.EXPECTED;
            DB.connection.Update(this);
            return this.handover;
        }

        public void ConfirmPickup()
        {
            this.status = Status.EXPECTED;
            DB.connection.Update(this);
        }

        public void ConfirmDelivery()
        {
            this.status = Status.COLLECTED;
            DB.connection.Update(this);
        }
    }
}
