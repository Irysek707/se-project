using Haulage.Model.Constants;
using Haulage.Model.Helpers;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace Haulage.Model
{
    public class Trip
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Driver { get; set; } = Constants.Constants.NO_DRIVER_ALLOCATED;
        public TripStop[] Stops { get { return this.stops; } }
        private TripStop[] stops;
        public TimeSpan ScheduledDuration { get; set; }

        [ForeignKey(typeof(Transport))]
        public Guid VehicleId { get; set; }
        public TripStatus TripStatus { get; set; }

        public int NumberOfStops { get; set; }

        //numbers from -180 to 180
        double StartLongitude { get; set; }

        // numbers from -90 to 90
        double StartLatitude { get; set; }

        public Trip(TripStop[] stops, double startLongitude, double startLatitude)
        {
            OtherHelpers.CheckLongitudeAndLatitude(startLongitude, startLatitude);
            this.Id = Guid.NewGuid();
            this.stops = stops;
            foreach (TripStop stop in this.stops)
            {
                stop.setTripId(this.Id);
            }
            this.NumberOfStops = stops.Length;
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

        public bool AllocateVehicle(Transport vehicle)
        {
            this.VehicleId = vehicle.Id;
            vehicle.BookVehicle();
            return DBHelpers.UpdateDB(this);
        }

        public void DeallocateDriver()
        {
            this.Driver = Constants.Constants.NO_DRIVER_ALLOCATED;
            DBHelpers.UpdateDB(this);
        }

        public bool DeallocateVehicle(Transport vehicle)
        {
            if (vehicle != null)
            {
                this.VehicleId = Guid.Empty;
                vehicle.UnbookVehicle();
                return DBHelpers.UpdateDB(this);
            }
            return false;
        }

        public void setStops(List<TripStop> stops)
        {
            this.stops = stops.ToArray();
        }

        // Method to delay the trip
        public void DelayTrip()
        {
            this.TripStatus = TripStatus.DELAYED;
            DBHelpers.UpdateDB(this);
        }

        // Method to set the trip on time
        public void OnTimeTrip()
        {
            this.TripStatus = TripStatus.ONTIME;
            DBHelpers.UpdateDB(this);
        }
    }
}
