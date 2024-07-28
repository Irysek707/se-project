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
    class TransportController
    {
        public static List<Transport> GetAllVehicles()
        {
            try
            {
                DB.connection.BeginTransaction();
                SQLiteCommand comm = new SQLiteCommand(DB.connection);
                comm.CommandText = "SELECT [Id],[Name] ,[CarryCapacityKg],[Booked]  FROM [Transport] WHERE Booked = 0;";
                List<Transport> vehicles = comm.ExecuteQuery<Transport>().ToList();
                DB.connection.Commit();
                return vehicles;
            }
            catch (Exception e)
            {
                DB.connection.Rollback();
                throw e;
            }
        }

        public static Transport GetVehicle(string id)
        {
            try
            {
                DB.connection.BeginTransaction();
                SQLiteCommand comm = new SQLiteCommand(DB.connection);
                comm.CommandText = DBHelpers.FormatSQL("SELECT [Id],[Name],[CarryCapacityKg],[Booked]  FROM [Transport]  WHERE Id = '", id);
                List<Transport> vehicles = comm.ExecuteQuery<Transport>().ToList();
                if (vehicles.Count == 0)
                {
                    return null;
                }
                if (vehicles.Count > 1)
                {
                    throw new Exception("Too many vehicles with the same id");
                }
                DB.connection.Commit();
                return vehicles[0];
            }
            catch (Exception e)
            {
                DB.connection.Rollback();
                throw e;
            }
        }
    }
}
