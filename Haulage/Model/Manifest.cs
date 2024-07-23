using Haulage.Model.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haulage.Model
{
    public class Manifest
    {
        [PrimaryKey]
        public Guid id { get; set; }

        public ManifestItem[] Items { get { return items; } }
        private ManifestItem[] items { get; set; }
        public double total { get; set; }

        public Manifest(ManifestItem[] items)
        {
            this.id = Guid.NewGuid();
            this.items = items;
            foreach (ManifestItem item in items)
            {
                item.setManifestId(this.id);
            }
            this.total = this.getTotal();
            DBHelpers.EnterToDB(this);
        }

        public Manifest(Guid id, double total)
        {
            new Manifest(id, total, null);
        }

        public Manifest(Guid id, double total, ManifestItem[] items)
        {
            this.id = id;
            this.items = items;
            this.total = total;
        }


        public Manifest() { }

        private double getTotal()
        {           
           return Math.Round(items.Select(x => x.getTotal()).Sum(),2);
        }
    }
}
