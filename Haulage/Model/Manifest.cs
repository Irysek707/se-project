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
        public Guid Id { get; set; }

        public ManifestItem[] Items { get { return items; } }
        private ManifestItem[] items { get; set; }
        public double Total { get; set; }

        public Manifest(ManifestItem[] items)
        {
            this.Id = Guid.NewGuid();
            this.items = items;
            foreach (ManifestItem item in items)
            {
                item.setManifestId(this.Id);
            }
            this.Total = this.GetTotal();
            DBHelpers.EnterToDB(this);
        }

        public Manifest(Guid id, double total)
        {
            new Manifest(id, total, null);
        }

        public Manifest(Guid id, double total, ManifestItem[] items)
        {
            this.Id = id;
            this.items = items;
            this.Total = total;
        }


        public Manifest() { }

        private double GetTotal()
        {           
           return Math.Round(items.Select(x => x.GetTotal()).Sum(),2);
        }
    }
}
