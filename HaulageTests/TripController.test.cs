using Haulage;
using Haulage.Control;
using Haulage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaulageTests
{
    public class TripControllerTests
    {
        private DB DB = new DB(true);
        private string insertTrip1 = "INSERT INTO [Trip]([Id],[Driver],[ScheduledDuration],[VehicleId],[TripStatus],[NumberOfStops])VALUES('123','driver1',5000,'12',0,3);";
        private string insertTrip2 = "INSERT INTO [Trip]([Id],[Driver],[ScheduledDuration],[VehicleId],[TripStatus],[NumberOfStops])VALUES('1234','driver1',5000,'12',0,3);";
        private string insertTrip3 = "INSERT INTO [Trip]([Id],[Driver],[ScheduledDuration],[VehicleId],[TripStatus],[NumberOfStops])VALUES('1234','driver2',5000,'12',0,3);";
        private string insertTripStop = "INSERT INTO [TripStop] ([Id],[OrderId],[TripId],[DeliveryAddressId]) VALUES ('123','123','123','123');";
        private string insertOrder1 = "INSERT INTO [CustomerOrder]([ManifestId],[Customer],[Id],[Status],[WarehouseId]) VALUES ('123','customer1','123',0,'123');";
        private string insertDeliveryAddress1 = "INSERT INTO [DeliveryAddress]([Id] ,[OrderId],[Longitude],[Latitude]) VALUES ('123','123',12.1,11.1);";
        private string insertDeliveryAddress2 = "INSERT INTO [DeliveryAddress]([Id] ,[OrderId],[Longitude],[Latitude]) VALUES ('124','123',12.1,11.1);";

        [Fact]
        public void GetAllTripsEmptyArray()
        {
            List<Trip> trips = TripController.GetAllTrips();
            Assert.Equal([], trips);
        }

        [Fact]
        public void GetAllTripsSingleTrip()
        {
            DB.connection.Query<Trip>(insertTrip1);
            List<Trip> trips = TripController.GetAllTrips();
            Assert.Single(trips);
        }

        [Fact]
        public void GetAllTripsTwoTrips()
        {
            DB.connection.Query<Trip>(insertTrip2);
            List<Trip> trips = TripController.GetAllTrips();
            Assert.Equal(2, trips.Count);
        }

        [Fact]
        public void GetAllTripsForDriverEmptyArray()
        {
            List<Trip> trips = TripController.GetAllTripsForDriver("123");
            Assert.Equal([], trips);
        }

        [Fact]
        public void GetAllTripsForDriverSingleTrip()
        {
            DB.connection.Query<Trip>(insertTrip3);
            List<Trip> trips = TripController.GetAllTripsForDriver("driver2");
            Assert.Single(trips);
        }

        [Fact]
        public void GetAllTripsForDriverTwoTrips()
        {
            List<Trip> trips = TripController.GetAllTripsForDriver("driver1");
            Assert.Equal(2, trips.Count);
        }

        [Fact]

        public void GetTripWithDetailsNoStopsError()
        {
            Assert.Throws<Exception>(() => TripController.GetTripWithDetails("123"));
        }

        [Fact]

        public void GetTripWithDetailsNoOrdersFound()
        {
            DB.connection.Query<TripStop>(insertTripStop);
            Assert.Throws<Exception>(() => TripController.GetTripWithDetails("123"));
        }

        [Fact]

        public void GetTripWithDetailsNoDeliveryAddressFound()
        {
            DB.connection.Query<CustomerOrder>(insertOrder1);
            Assert.Throws<Exception>(() => TripController.GetTripWithDetails("123"));
        }

        [Fact]

        public void GetTripWithDetailsSuccess()
        {
            DB.connection.Query<CustomerOrder>(insertDeliveryAddress1);
            Trip trip = TripController.GetTripWithDetails("123");
            Assert.Equal("123", trip.Stops[0].Id.ToString());
            Assert.Equal("123", trip.Stops[0].Address.Id.ToString());
            Assert.Equal("123", trip.Stops[0].Order.Id.ToString());
        }

        [Fact]
        public void GetTripDetailsFailTooManyAddresses()
        {
            DB.connection.Query<CustomerOrder>(insertDeliveryAddress2);
            Assert.Throws<Exception>(() => TripController.GetTripWithDetails("123"));
        }
    }
}
