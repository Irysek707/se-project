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
    public class DriverControllerTests
    {
        DB DB = new DB(true);
        DriverController controller = new DriverController("driver1");
        private string insertTrip1 = "INSERT INTO [Trip]([Id],[Driver],[ScheduledDuration],[VehicleId],[TripStatus],[NumberOfStops])VALUES('123','driver1',5000,'12',0,3);";
        private string insertTrip2 = "INSERT INTO [Trip]([Id],[Driver],[ScheduledDuration],[VehicleId],[TripStatus],[NumberOfStops])VALUES('1234','driver1',5000,'12',0,3);";

        [Fact]

        public void GetAllTripsNoUserThrowsException()
        {
            Assert.Throws<Exception>(() => new DriverController("").GetAllTrips());
        }

        [Fact]
        public void GetAllTripsEmptyArray()
        {
            Assert.Throws<Exception>(() => controller.GetAllTrips());
        }

        [Fact]
        public void GetAllTripsSingleTrip()
        {
            DB.connection.Query<Trip>(insertTrip1);
            List<Trip> trips = controller.GetAllTrips();
            Assert.Single(trips);
        }

        [Fact]
        public void GetAllTripsTwoTrips()
        {
            DB.connection.Query<Trip>(insertTrip2);
            List<Trip> trips = controller.GetAllTrips();
            Assert.Equal(2, trips.Count);
        }
    }
}
