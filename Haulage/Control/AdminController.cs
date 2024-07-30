using Haulage.Model;
using Haulage.Model.Constants;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Haulage.Control
{
    class AdminController
    {
        // Use the existing connection object from DB
        private static SQLiteConnection dbConnection = DB.connection;

        public AdminController() { }

        public List<Trip> GetAllTrips()
        {
            try
            {
                return TripController.GetAllTrips();
            }
            catch (Exception e)
            {
                throw new Exception("Error retrieving trips: " + e.Message, e);
            }
        }

        public static Trip GetTrip(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new ArgumentException("No id provided");
                }

                var trip = TripController.GetTripWithDetails(id);
                if (trip == null || trip.Stops == null || trip.Stops.Length == 0)
                {
                    throw new Exception("No trip or stops for the trip were found");
                }
                return trip;
            }
            catch (Exception e)
            {
                throw new Exception("Error retrieving trip: " + e.Message, e);
            }
        }

        public static void AllocateDriver(Driver driver, Trip trip)
        {
            try
            {
                if (driver == null || trip == null)
                {
                    throw new ArgumentNullException("Driver or Trip cannot be null");
                }

                trip.AllocateDriver(driver.Login);
            }
            catch (Exception e)
            {
                throw new Exception("Error allocating driver: " + e.Message, e);
            }
        }

        public static void DeallocateDriver(Trip trip)
        {
            try
            {
                if (trip == null)
                {
                    throw new ArgumentNullException("Trip cannot be null");
                }

                trip.DeallocateDriver();
            }
            catch (Exception e)
            {
                throw new Exception("Error deallocating driver: " + e.Message, e);
            }
        }

        public static Transport AllocateVehicle(Transport transport, Trip trip)
        {
            try
            {
                if (transport == null || trip == null)
                {
                    throw new ArgumentNullException("Transport or Trip cannot be null");
                }

                trip.AllocateVehicle(transport);
                return transport;
            }
            catch (Exception e)
            {
                throw new Exception("Error allocating vehicle: " + e.Message, e);
            }
        }

        public static Transport DeallocateVehicle(Trip trip)
        {
            try
            {
                if (trip == null)
                {
                    throw new ArgumentNullException("Trip cannot be null");
                }

                var transport = TransportController.GetVehicle(trip.VehicleId.ToString());
                trip.DeallocateVehicle(transport);
                return transport;
            }
            catch (Exception e)
            {
                throw new Exception("Error deallocating vehicle: " + e.Message, e);
            }
        }

        public static List<Driver> GetAllDrivers()
        {
            try
            {
                var query = "SELECT [Login] FROM [User] WHERE [Role] = ?";
                var drivers = dbConnection.Query<Driver>(query, (int)Role.DRIVER).ToList();
                return drivers;
            }
            catch (Exception e)
            {
                throw new Exception("Error retrieving drivers: " + e.Message, e);
            }
        }
        public static void UpdateDriver(Driver driver)
        {
            try
            {
                if (driver == null)
                {
                    throw new ArgumentNullException(nameof(driver), "Driver cannot be null");
                }

                string newLogin = driver.Login;

                if (string.IsNullOrWhiteSpace(newLogin))
                {
                    throw new ArgumentException("Driver Login is not valid");
                }

                // Retrieve the existing driver by the old login
                var existingDriver = dbConnection.Query<User>("SELECT * FROM [User] WHERE [Login] = ? AND [Role] = ?", newLogin, (int)Role.DRIVER).FirstOrDefault();

                if (existingDriver == null)
                {
                    throw new Exception("Driver with the provided Login does not exist.");
                }

                // Check if the new login already exists
                var existingNewLoginDriver = dbConnection.Query<User>("SELECT * FROM [User] WHERE [Login] = ? AND [Role] = ?", newLogin, (int)Role.DRIVER).FirstOrDefault();

                if (existingNewLoginDriver != null)
                {
                    throw new Exception("A driver with the new Login already exists.");
                }

                // Perform the update operation
                var updateQuery = "UPDATE [User] SET [Login] = ? WHERE [Login] = ? AND [Role] = ?";
                var rowsAffected = dbConnection.Execute(updateQuery, newLogin, existingDriver.Login, (int)Role.DRIVER);

                if (rowsAffected == 0)
                {
                    throw new Exception("No records were updated. Check if the driver exists and has the correct Login.");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error updating driver: " + e.Message, e);
            }
        }


        public static void DeleteDriver(Driver driver)
        {
            try
            {
                if (driver == null)
                {
                    throw new ArgumentNullException(nameof(driver), "Driver cannot be null");
                }

                // Ensure the driver exists before attempting to delete
                var existingDriver = dbConnection.Query<Driver>("SELECT * FROM [User] WHERE [Login] = ? AND [Role] = ?", driver.Login, (int)Role.DRIVER).FirstOrDefault();

                if (existingDriver == null)
                {
                    throw new Exception("Driver with the provided Login does not exist.");
                }

                var query = "DELETE FROM [User] WHERE [Login] = ? AND [Role] = ?";
                var rowsAffected = dbConnection.Execute(query, driver.Login, (int)Role.DRIVER);

                if (rowsAffected == 0)
                {
                    throw new Exception("No records were deleted. Check if the driver exists.");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error deleting driver: " + e.Message, e);
            }
        }


        public static void AddDriver(Driver driver)
        {
            try
            {
                if (driver == null)
                {
                    throw new ArgumentNullException(nameof(driver), "Driver cannot be null");
                }

                var query = "INSERT INTO [User] ([Role], [Login]) VALUES (?, ?)";
                dbConnection.Execute(query, (int)Role.DRIVER, driver.Login);
            }
            catch (Exception e)
            {
                throw new Exception("Error adding driver: " + e.Message, e);
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
                throw new Exception("Error retrieving vehicles: " + e.Message, e);
            }
        }
    }
}
