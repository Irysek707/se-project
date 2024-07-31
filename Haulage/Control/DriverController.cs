using Haulage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haulage.Control
{
    public class DriverController
    {
        private string driverLogin;

        public DriverController(string driverLogin)
        {

        this.driverLogin = driverLogin; }
    
        public List<Trip> GetAllTrips()
        {
            if (driverLogin == null || driverLogin == "") { throw new ArgumentNullException("Please provide a user login to view trips"); }
            List<Trip> trips = TripController.GetAllTripsForDriver(driverLogin);
            if (trips == null || trips.Count == 0) { { throw new Exception("No trips available for the user"); } }
            else { return trips; }
        }
    }
}
