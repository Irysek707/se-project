using Haulage.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Haulage.Model.Constants
{
    public class MockResources
    { 

        //Placeholder until user implementation
        public static User mockCustomer = new Customer("customer1");
        public static User mockDriver = new Driver("driver1");
        public static Warehouse warehouse = new Warehouse(12.12, 12.12);

        //For testing purposes only, delete later
         static ManifestItem[] ManifestItems =
        [ new ManifestItem(new Item("4005556151097", "Ravensburger Jigsaw, 100Pieces", 25.39),2),
            new ManifestItem(new Item("4005556151096", "Ravensburger Jigsaw, 100Pieces", 25.39),3),
            new ManifestItem(new Item("4005556151095", "Ravensburger Jigsaw, 100Pieces", 25.39),1),
            new ManifestItem(new Item("1845678901001", "Hobby Paint 250ml", 5.99),1),
            new ManifestItem(new Item("1845678901002", "Hobby Paint 1L", 13.55), 1),
            new ManifestItem(new Item("1845678901003", "Hobby Paints 500ml", 8.80), 1)];

         
      

        public void CreateMockResources()
        {
            CustomerOrder order1 = new CustomerOrder(new Manifest(ManifestItems), mockCustomer.Login, warehouse.Id);
            CustomerOrder order2 =  new CustomerOrder(new Manifest(ManifestItems), mockCustomer.Login, warehouse.Id);
            CustomerOrder order3 = new CustomerOrder(new Manifest(ManifestItems), mockCustomer.Login, warehouse.Id);
            DeliveryAddress deliveryAddress1 = new DeliveryAddress(order1.Id, 13.13, 13.14);
            DeliveryAddress deliveryAddress2 = new DeliveryAddress(order1.Id, 14.14, 14.15);
            DeliveryAddress deliveryAddress3 = new DeliveryAddress(order1.Id, 15.15, 15.16);
            TripStop stop1 = new TripStop(order1, deliveryAddress1);
            TripStop stop2 = new TripStop(order2, deliveryAddress2);
            TripStop stop3 = new TripStop(order3, deliveryAddress3);
            TripStop altStop1 = new TripStop(order2, deliveryAddress2);
            TripStop altStop2 = new TripStop(order3, deliveryAddress1);
            TripStop altStop3 = new TripStop(order1, deliveryAddress3);
            TripStop otherAltStop1 = new TripStop(order2, deliveryAddress2);
            TripStop otherAltStop2 = new TripStop(order3, deliveryAddress3);
            TripStop otherAltStop3 = new TripStop(order1, deliveryAddress1);
            Trip trip1 = new Trip([stop1, stop2, stop3], warehouse.Longitude, warehouse.Latitude);
            Trip trip2 = new Trip([altStop1, altStop2, altStop3], warehouse.Longitude, warehouse.Latitude);
            Trip trip3 = new Trip([otherAltStop1, otherAltStop2, otherAltStop3], warehouse.Longitude, warehouse.Latitude);
            trip1.AllocateDriver(mockDriver.Login);
            trip2.AllocateDriver(mockDriver.Login);
            trip3.AllocateDriver(mockDriver.Login);
        }
    }
}
