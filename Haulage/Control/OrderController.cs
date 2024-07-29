using Haulage.Model;
using Haulage.Model.Helpers;
using Microsoft.Maui.Controls;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Haulage.Control
{
    internal class OrderController
    {

    
        
        public static List<CustomerOrder> GetAllOrdersForCustomer(string customer)
        {
            try
            {
                DB.connection.BeginTransaction();
                SQLiteCommand comm = new SQLiteCommand(DB.connection);
                comm.CommandText = DBHelpers.FormatSQL("SELECT CustomerOrder.Id,[Total], [Status]  FROM[CustomerOrder]   JOIN[Manifest] ON CustomerOrder.manifestId = Manifest.id  WHERE customer = '", customer);  
                List<CustomerOrder> orders = comm.ExecuteQuery<CustomerOrder>().ToList();
                DB.connection.Commit();
                return orders;
            }
            catch (Exception e)
            {
                DB.connection.Rollback();
                throw e;
            }
        }

        public static CustomerOrder GetSpecificOrderWithDetails(string id)
        {
            try
            {
                DB.connection.BeginTransaction();
                CustomerOrder order = OrderController.getCustomerOrderContinueTransaction(id);
                DB.connection.Commit();
                return order;
            }
            catch (Exception e)
            {
                DB.connection.Rollback();
                throw e;
            }
        }

       
         // CAUTION: Need to catch exceptions and handle transaction seperately like method above 
         // This is intended for reusability of code
        public static CustomerOrder getCustomerOrderContinueTransaction(string id)
        {
            SQLiteCommand comm = new SQLiteCommand(DB.connection);
            comm.CommandText = DBHelpers.FormatSQL("SELECT [ManifestId],[Customer],[Id],[Status]  FROM [CustomerOrder]  WHERE [Id] ='", id);
            List<CustomerOrder> list = comm.ExecuteQuery<CustomerOrder>();
            if (list.Count > 1)
            {
                throw new Exception("Too many orders with the same id");
            }
            if (list.Count == 0)
            {
                throw new Exception("No order found");
            }
            CustomerOrder order = list[0];
            comm = new SQLiteCommand(DB.connection);
            comm.CommandText = DBHelpers.FormatSQL("SELECT [ExpectedHandover],[ActualHandover],[Id],[OrderId]  FROM [Handover]  WHERE OrderId = '", id);
            List<Handover> handovers = comm.ExecuteQuery<Handover>();
            if (handovers.Count > 0)
            {
                order.AddHandover(handovers[0]);
            }
            comm = new SQLiteCommand(DB.connection);
            comm.CommandText = DBHelpers.FormatSQL("SELECT [Quantity],[Id], [ManifestId], [ItemCode]  FROM [ManifestItem] JOIN [Item] ON ManifestItem.ItemCode = Item.Code WHERE ManifestItem.ManifestId = '", order.ManifestId.ToString());
            List<ManifestItem> manifestItems = comm.ExecuteQuery<ManifestItem>();
            manifestItems.ForEach(item =>
            {
                comm = new SQLiteCommand(DB.connection);
                comm.CommandText = DBHelpers.FormatSQL("SELECT[Name], [Code], [Price] FROM[Item] WHERE Code = '", item.ItemCode);
                List<Item> items = comm.ExecuteQuery<Item>();
                item.setItem(items[0]);
            });
            comm = new SQLiteCommand(DB.connection);
            comm.CommandText = DBHelpers.FormatSQL("SELECT Total  FROM [Manifest]  WHERE Id = '", order.ManifestId.ToString());
            List<Manifest> manifests = comm.ExecuteQuery<Manifest>();
            list[0].AddManifest(new Manifest(order.ManifestId, manifests[0].Total, manifestItems.ToArray()));
            return list[0];
        }
        }
    }

