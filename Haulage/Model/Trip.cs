using Haulage.Model.Constants;
using Haulage.Model.Helpers;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace Haulage.Model
{
    public class Trip
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Driver { get; set; }
        // Define a one-to-many relationship with TripStop
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<TripStop> Stops { get; set; }
        public TimeSpan ScheduledDuration { get; set; }
        [ForeignKey(typeof(Transport))]
        public Guid VehicleId { get; set; }
        public TripStatus TripStatus { get; set; }
        public int NumberOfStops { get; set; }
        public double StartLongitude { get; set; }
        public double StartLatitude { get; set; }

        // Public parameterless constructor
        public Trip()
        {
            Stops = new List<TripStop>();
        }

        public Trip(TripStop[] stopArr1, double longitude, double latitude)
        {
            Stops = new List<TripStop>(stopArr1);
            StartLongitude = longitude;
            StartLatitude = latitude;
        }

        public Trip(List<TripStop> stops, double startLongitude, double startLatitude)
        {
            Stops = stops;
            StartLongitude = startLongitude;
            StartLatitude = startLatitude;
        }

        public void AllocateDriver(string driver)
        {
            Driver = driver;
        }

        public bool AllocateVehicle(Transport vehicle)
        {
            VehicleId = vehicle.Id;
            return true;
        }

        public void DeallocateDriver()
        {
            Driver = null;
        }

        public bool DeallocateVehicle(Transport vehicle)
        {
            if (VehicleId == vehicle.Id)
            {
                VehicleId = Guid.Empty;
                return true;
            }
            return false;
        }

        public void SetStops(List<TripStop> stops)
        {
            Stops = stops;
        }

        // Method to delay the trip
        public void DelayTrip()
        {
            // Implementation here
        }

        // Method to set the trip on time
        public void OnTimeTrip()
        {
            // Implementation here
        }

        public bool ConfirmPickupDelivery(Guid stopId)
        {
            // Implementation here
            return true;
        }
    }
}

