using Haulage.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haulage.Control
{
    class AdminController
    {
        public AdminController() { }

        public  List<Trip> GetAllTrips()
        {
            try
            {
             List<Trip> trips = TripController.GetAllTrips();
                return trips;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Trip GetTrip(string id)
        {
            try
            {
                if (id == null || id == "") { throw new Exception("No id provided"); }
                Trip trip = TripController.GetTripWithDetails(id);
                if (trip == null || trip.Stops == null || trip.Stops.Length == 0)
                {
                    throw new Exception("No trip or stops for the trip were found");
                }
                return trip;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // These two methods would benefit from more work given more time to focus on correctly blocking both
        // drivers and vehicles from getting overbooked or double booked
        public static string AllocateDriver(Model.Driver driver, Trip trip)
        {
            try
            {
                trip.AllocateDriver(driver.Login);
                return driver.Login;
            }
            catch (Exception e) { 
               throw e;
            }
        }

        public static void DeallocateDriver(Trip trip)
        {
            try
            {
                trip.DeallocateDriver();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Transport AllocateVehicle(Transport transport, Trip trip)
        {
            try
            {
                trip.AllocateVehicle(transport);
                return transport;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Transport DeallocateVehicle(Trip trip)
        {
            try
            {
                Transport transport = TransportController.GetVehicle(trip.VehicleId.ToString());
                trip.DeallocateVehicle(transport);
                return transport;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<Driver> GetAllDriver()
        {
            try
            {
                DB.connection.BeginTransaction();
                SQLiteCommand comm = new SQLiteCommand(DB.connection);
                comm.CommandText = "SELECT [Role],[Login]  FROM [User]  WHERE Role = 1;";
                List<Driver> drivers = comm.ExecuteQuery<Driver>().ToList();
                DB.connection.Commit();
                return drivers;
            }
            catch (Exception e)
            {
                DB.connection.Rollback();
                throw e;
            }
        }

        public static List<Transport> GetAllVehicles()
        {
            try
            {
                return TransportController.GetAllVehicles();
            }
            catch (Exception e)
            {
                DB.connection.Rollback();
                throw e;
            }
        }
    }
}
