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
        public static List<Trip> getAllTripsForDriver(string driver)
        {
            try
            {
                DB.connection.BeginTransaction();
                SQLiteCommand comm = new SQLiteCommand(DB.connection);
                comm.CommandText = DBHelpers.FormatSQL("SELECT [Id],[ScheduledDuration] ,[TripStatus],[NumberOfStops],[Driver]  FROM [Trip]  WHERE [Driver] = '", driver);
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
    }
}
