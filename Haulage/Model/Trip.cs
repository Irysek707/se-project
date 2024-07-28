using Haulage.Model.Constants;
using Haulage.Model.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haulage.Model
{
    class Trip
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Driver { get; set; } = Constants.Constants.NO_DRIVER_ALLOCATED;
        public List<TripStop> Stops { get { return this.stops; } }
        private List<TripStop> stops;
        public TimeSpan ScheduledDuration { get; set; }
        public TripStatus TripStatus { get; set; }

        public int NumberOfStops { get; set; }

        //numbers from -180 to 180
        double StartLongitude { get; set; }

        // numbers from -90 to 90

        double StartLatitude { get; set; }

        public Trip(List<TripStop> stops, double startLongitude, double startLatitude) {
            OtherHelpers.CheckLongitudeAndLatitude(startLongitude, startLatitude);
            this.Id = Guid.NewGuid();
            this.stops = stops;
            this.NumberOfStops = stops.Count;
            this.TripStatus = TripStatus.SCHEDULED;
            this.StartLongitude = startLongitude;
            this.StartLatitude = startLatitude;
            this.ScheduledDuration = OtherHelpers.CalculateTripDuration(startLongitude, startLatitude, stops);
            DBHelpers.EnterToDB(this);
        }

        public Trip() { }

        public void AllocateDriver(string driver)
        {
            this.Driver = driver;
            DBHelpers.UpdateDB(this);
        }

    }
}
