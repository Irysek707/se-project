using Haulage.Model.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Haulage.Model
{
    // A basic struct from which all other vehicle classes can inherit from (In case there is a need for specific information)
    public class Transport
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double CarryCapacityKg  { get; set; }

        public bool Booked { get; set; } = false;

        public Transport(string name, double carryCapacityKg)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.CarryCapacityKg = carryCapacityKg;
            DBHelpers.EnterToDB(this);
        }

        public Transport() { }

        public bool BookVehicle()
        {
            this.Booked = true;
            return DBHelpers.UpdateDB(this);
        }

        public bool UnbookVehicle()
        {
            this.Booked = false;
            return DBHelpers.UpdateDB(this);
        }
    }
}
