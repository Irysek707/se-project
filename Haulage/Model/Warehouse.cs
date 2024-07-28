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

    //The current implementation of application uses a single warehouse,
    //however it is a good idea to leave space for future expansions
    public class Warehouse
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        //numbers from -180 to 180
        public double Longitude { get; set ; }

        public string Name { get; set; }

        
        // numbers from -90 to 90
        
        public double Latitude { get; set; }

        //There may be need to add additional columns in future

        public Warehouse ()
        {

        }

        public Warehouse(double longitude, double latitude, string name = "Warehouse")
        {
            if(OtherHelpers.CheckLongitudeAndLatitude(longitude, latitude))
            {
                this.Id = Guid.NewGuid();
                this.Longitude = longitude;
                this.Latitude = latitude;
                this.Name = name;
                DBHelpers.EnterToDB (this);
            }
        }
    }
}
