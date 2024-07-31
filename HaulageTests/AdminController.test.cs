using Haulage;
using Haulage.Model;
using Haulage.Control;
using Xunit.Sdk;
using System.Reflection;
using Haulage.Model.Vehicle;
using Haulage.Model.Constants;
using SQLite;


namespace UnitTests
{

    public class AdminControllerTests
    {
        private DB DB = new DB(true);
        private AdminController controller = new AdminController();
        private string insertTrip1 = "INSERT INTO [Trip]([Id],[Driver],[ScheduledDuration],[VehicleId],[TripStatus],[NumberOfStops])VALUES('123','driver1',5000,'12',0,3);";
        private string insertTrip2 = "INSERT INTO [Trip]([Id],[Driver],[ScheduledDuration],[VehicleId],[TripStatus],[NumberOfStops])VALUES('1234','driver1',5000,'12',0,3);";
        private string insertDriver1 = "INSERT INTO [User] ([Role],[Login]) VALUES (1, 'driver1');";
        private string insertDriver2 = "INSERT INTO [User] ([Role],[Login]) VALUES (1, 'driver2');";
        private string insertVehicles1 = "INSERT INTO [Transport] ([Id],[Name],[CarryCapacityKg],[Booked]) VALUES ('123','Volga',500.00,false);";
        private string insertVehicles2 = "\"INSERT INTO [Transport] ([Id],[Name],[CarryCapacityKg],[Booked]) VALUES ('1234','Volga',500.00,false);";

        [Fact]
        public void GetsEmptyTripArray()
        {
            List<Trip> trips = controller.GetAllTrips();
            Assert.Equal([], trips);
        }

        [Fact]
        public void GetTripArrayWithOneTrip()
        {
            DB.connection.Query<Trip>(insertTrip1);
            List<Trip> trips = controller.GetAllTrips();
            Assert.Single(trips);
        }
        [Fact]
        public void getTripArrayWithTwoTrips()
        {
            DB.connection.Query<Trip>(insertTrip2);
            List<Trip> trips = controller.GetAllTrips();
            Assert.Equal(2, trips.Count);
        }

        [Fact]
        public void getTripFailNoTripId()
        {
            Assert.Throws<Exception>(() => AdminController.GetTrip(""));
        }


        [Fact]
        public void GetTripFailNoTrip()
        {
            Assert.Throws<Exception>(() => AdminController.GetTrip("1"));
        }

        [Fact]
        public void GetTripSuccess()
        {
            Trip trip = AdminController.GetTrip("123");
            Assert.NotNull(trip);
        }

        [Fact]
        public void AllocateDriverSuccess()
        {
            Trip trip = AdminController.GetTrip("123");
            AdminController.AllocateDriver(new Driver("driver1"), trip);
            Assert.Equal("driver1", trip.Driver);
        }

        [Fact]
        public void DeallocateDriverSuccess()
        {
            Trip trip = AdminController.GetTrip("123");
            AdminController.DeallocateDriver(trip);
            Assert.Equal(Constants.NO_DRIVER_ALLOCATED, trip.Driver);
        }

        [Fact]
        public void AllocateVehicleSuccess()
        {
            Trip trip = AdminController.GetTrip("123");
            Truck truck = new Truck("Solaris");
            AdminController.AllocateVehicle(truck, trip);
            Assert.Equal(truck.Id, trip.VehicleId);
        }

        [Fact]
        public void DeallocateVehicleSuccess()
        {
            Trip trip = AdminController.GetTrip("123");
            Truck truck = new Truck("Solaris");
            AdminController.AllocateVehicle(truck, trip);
            AdminController.DeallocateVehicle(trip);
            Assert.Equal(Guid.Empty, trip.VehicleId);
        }
        [Fact]
        public void DeallocateVehicleNullVehicle()
        {
            Trip trip = AdminController.GetTrip("123");
            Transport vehicle = AdminController.DeallocateVehicle(trip);
            Assert.Equal(Guid.Empty, trip.VehicleId);
            Assert.Equal(null, vehicle);
        }

        [Fact]
        public void GetAllDriversEmptyList()
        {
            List<Driver> drivers = AdminController.GetAllDriver();
            Assert.Equal([], drivers);
        }

        [Fact]
        public void GetAllDriversSingleEntryList()
        {
            DB.connection.Query<Driver>(insertDriver1);
            List<Driver> drivers = AdminController.GetAllDriver();
            Assert.Single(drivers);
        }

        [Fact]
        public void GetAllDriversTwoEntriesInList()
        {
            DB.connection.Query<Driver>(insertDriver2);
            List<Driver> drivers = AdminController.GetAllDriver();
            Assert.Equal(2, drivers.Count);
        }

        [Fact]
        public void GetAllVehiclesEmptyList()
        {
            List<Transport> vehicles = AdminController.GetAllVehicles();
            Assert.Equal([], vehicles);
        }

        [Fact]
        public void GetAllVehiclesSingleEntryInList()
        {
            DB.connection.Query<Transport>(insertVehicles1);
            List<Transport> vehicles = AdminController.GetAllVehicles();
            Assert.Equal([], vehicles);
        }

        [Fact]
        public void GetAllVehiclesTwoEntriesInList()
        {
            DB.connection.Query<Transport>(insertVehicles2);
            List<Transport> vehicles = AdminController.GetAllVehicles();
            Assert.Equal(2, vehicles.Count);
        }

    }
}
    

