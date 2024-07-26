using Haulage.Model;
using Haulage.Model.Constants;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haulage
{

    public class DB
    {
        public static SQLiteConnection connection;

        public DB() {
            this.Init();
        }

        //This will init the database with mockData (from MockResources in Constants)
        //Disable the code in those resources if
        public void Init()
        {
            string databasePath = Constants.DatabasePath;
            if (connection is not null)
            {
                return;
            }
            connection = new SQLiteConnection(databasePath, Constants.Flags);
            //Enable only if you would like a clean install of all database tables
            cleanup();

            connection.CreateTable<User>();
            connection.CreateTable<Warehouse>();

            connection.CreateTable<CustomerOrder>();
            connection.CreateTable<Manifest>();
            connection.CreateTable<ManifestItem>();
            connection.CreateTable<Item>();
            connection.CreateTable<Handover>();

            connection.CreateTable<Trip>();
            connection.CreateTable<DeliveryAddress>();
            connection.CreateTable<TripStop>();
            /// Remember to add new tables for each model class created here

            /// Adding mockOrder resources here, disable for production
            new MockResources().CreateMockResources();
        }
        private void cleanup()
        {
            connection.DropTable<Manifest>();
            connection.DropTable<ManifestItem>();
            connection.DropTable<Item>();
            connection.DropTable<Handover>();
            connection.DropTable<CustomerOrder>();
            connection.DropTable<User>();
        }
    }


}

