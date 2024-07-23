
using Microsoft.Maui.Controls;
using SQLite;

namespace Haulage.Model.Helpers
{
    internal class DBHelpers
    {
        // This is not a method checking for safety, so any object (even one without table can be passed)
        // In that case it will throw error
        public static bool EnterToDB(object obj)
        {
            try
            {
                DB.connection.BeginTransaction();
                DB.connection.Insert(obj);
                DB.connection.Commit();
                return true;
            }
            catch (Exception e)
            {
                DB.connection.Rollback();
                return false;
            }
        }

        public static bool UpdateDB(object obj)
        {
            try
            {
                DB.connection.BeginTransaction();
                DB.connection.Update(obj);
                DB.connection.Commit();
                return true;
            }
            catch (Exception e)
            {
                DB.connection.Rollback();
                return false;
            }
        }
        public static string FormatSQL(string query, string variable)
        {
            return query + variable + "';";
        }

        public static string FormatSQLWithGroupBy(string query, string[] variables)
        {
            return query + variables[0] + "' GROUP BY " + variables[1] + ";";
        }
    }
}
