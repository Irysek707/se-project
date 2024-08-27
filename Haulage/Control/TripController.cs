using Haulage.Model;
using Haulage.Model.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haulage.Control
{
   class TripController
    {

        public static List<Trip> GetAllTrips()
        {
            try
            {
                DB.connection.BeginTransaction();
                SQLiteCommand comm = new SQLiteCommand(DB.connection);
                comm.CommandText = "SELECT [Id],[ScheduledDuration] ,[TripStatus],[NumberOfStops],[Driver],[VehicleId]  FROM [Trip] ;";
                List<Trip> trips = comm.ExecuteQuery<Trip>().ToList();
                DB.connection.Commit();
                return trips;
            }
            catch (Exception e)
            {
                DB.connection.Rollback();
                throw e;
            }
        }
        public static List<Trip> GetAllTripsForDriver(string driver)
        {
            try
            {
                DB.connection.BeginTransaction();
                SQLiteCommand comm = new SQLiteCommand(DB.connection);
                comm.CommandText = DBHelpers.FormatSQL("SELECT [Id],[ScheduledDuration] ,[TripStatus],[NumberOfStops],[Driver],[VehicleId]  FROM [Trip]  WHERE [Driver] = '", driver);
                List<Trip> trips = comm.ExecuteQuery<Trip>().ToList();
                DB.connection.Commit();
                return trips;
            }
            catch (Exception e)
            {
                DB.connection.Rollback();
                throw e;
            }
        }

        public static Trip GetTripWithDetails(string tripId)
        {
            try
            {
                DB.connection.BeginTransaction();
                SQLiteCommand comm = new SQLiteCommand(DB.connection);
                comm.CommandText = DBHelpers.FormatSQL("SELECT [Id],[ScheduledDuration],[TripStatus],[NumberOfStops],[Driver]  FROM [Trip]  WHERE Id = '", tripId);
                List<Trip> trips = comm.ExecuteQuery<Trip>().ToList();
                if(trips.Count > 1)
                {
                    throw new Exception("Too many trips with the same id");
                }
                if (trips.Count == 0)
                {
                    throw new Exception("No trip found");
                }
                Trip trip = trips[0];
                comm.CommandText = DBHelpers.FormatSQL("SELECT [Id],[OrderId] ,[TripId] ,[DeliveryAddressId] FROM [TripStop]  WHERE TripId = '", tripId);
                List<TripStop> stops = comm.ExecuteQuery<TripStop>().ToList();
                if (stops.Count == 0)
                {
                    throw new Exception("No stops for this trip found");
                }
                stops.ForEach(stop =>
                {
                    comm.CommandText = DBHelpers.FormatSQL("SELECT [Id],[OrderId],[Longitude] ,[Latitude] FROM [DeliveryAddress]  WHERE OrderId = '", stop.OrderId.ToString());
                    List<DeliveryAddress> deliveryAddress = comm.ExecuteQuery<DeliveryAddress>().ToList();
                    if (deliveryAddress.Count > 1)
                    {
                        throw new Exception("Too many addresses for a single order");
                    }
                    stop.setDeliveryAddress(deliveryAddress[0]);
                    CustomerOrder order = OrderController.getCustomerOrderContinueTransaction(stop.OrderId.ToString());
                    stop.setOrder(order);
                });
                trip.setStops(stops);
                DB.connection.Commit();
                return trip;
            }
            catch (Exception e)
            {
                DB.connection.Rollback();
                throw e;
            }
        }
    }
}
