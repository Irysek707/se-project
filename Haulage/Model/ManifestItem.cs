
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
        public Guid id { get; set; }

        [ForeignKey(typeof(Manifest))]
        public Guid manifestId { get; set; }

        [ForeignKey(typeof(Item))]

        public string itemCode { get; set; }
        public double Total { get { return this.getTotal(); } }
        public Item Item { get { return item; } }
        private Item item { get; set; }
        public int quantity { get; set; }

        public ManifestItem(Item item, int count)
        {
            this.item = item;
            this.itemCode = item.code;
            this.quantity = count;
            this.id = Guid.NewGuid ();
            DBHelpers.EnterToDB (this);

        }

        public ManifestItem() { }

        public bool setManifestId(Guid id)
        {
            this.manifestId = id;
            return DBHelpers.UpdateDB (this);
        }

        public void setItem(Item item)
        {
            this.item = item;
        }

        public double getTotal()
        {
            return Math.Round(quantity * this.item.price,2);
        }

        //For testing purposes only, delete later
        public static ManifestItem[] createSomeItemsForDebug()
        {
            return [ new ManifestItem(new Item("4005556151097", "Ravensburger Jigsaw, 100Pieces", 25.39),2),
            new ManifestItem(new Item("4005556151096", "Ravensburger Jigsaw, 100Pieces", 25.39),3),
            new ManifestItem(new Item("4005556151095", "Ravensburger Jigsaw, 100Pieces", 25.39),1),
            new ManifestItem(new Item("1845678901001", "Hobby Paint 250ml", 5.99),1),
            new ManifestItem(new Item("1845678901002", "Hobby Paint 1L", 13.55), 1),
            new ManifestItem(new Item("1845678901003", "Hobby Paints 500ml", 8.80), 1)
                ];
        }
    }
}
