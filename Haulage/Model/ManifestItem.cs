
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
    public class ManifestItem
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [ForeignKey(typeof(Manifest))]
        public Guid ManifestId { get; set; }

        [ForeignKey(typeof(Item))]

        public string ItemCode { get; set; }
        public double Total { get { return this.GetTotal(); } }
        public Item Item { get { return item; } }
        private Item item { get; set; }
        public int Quantity { get; set; }

        public ManifestItem(Item item, int count)
        {
            this.item = item;
            this.ItemCode = item.Code;
            this.Quantity = count;
            this.Id = Guid.NewGuid ();
            DBHelpers.EnterToDB (this);

        }

        public ManifestItem() { }

        public bool setManifestId(Guid id)
        {
            this.ManifestId = id;
            return DBHelpers.UpdateDB (this);
        }

        public void setItem(Item item)
        {
            this.item = item;
        }

        public double GetTotal()
        {
            return Math.Round(Quantity * this.item.Price,2);
        }
    }
}
