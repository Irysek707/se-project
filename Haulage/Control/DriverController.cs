using Haulage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haulage.Control
{
    class DriverController
    {
        public static List<Trip> GetAllTrips(string login)
        {
            if (login == null) { throw new ArgumentNullException("Please provide a user login to view trips"); }
            List<Trip> trips = TripController.getAllTripsForDriver(login);
            if (trips == null || trips.Count == 0) { { throw new Exception("No trips available for the user"); } }
            else { return trips; }
        }
    }
}
