using Haulage.Model.Users;
using Haulage.Model.Vehicle;
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
        public static Customer mockCustomer = new Customer("customer1");
        public static Driver mockDriver = new Driver("driver1");

        // In real system mapping for inheritance would be necessary to support sqlite as the database 
        // However for the mocks it is easier to enter them to DB with their default type
        public static User mockDriver1 = new(Role.DRIVER,"driver1");
        public static User mockDriver2 = new(Role.DRIVER, "driver2");

        //Same as for drivers but for transport
        public Transport transport1 = new Transport("Volvo", 500);
        public Transport transport2 = new Transport("Nissan",500);
        public Transport transport3 = new Transport("Solaris", 18.000);
        public static Admin mockAdmin = new Admin("admin1");
        public static Warehouse warehouse = new Warehouse(12.12, 12.12);
        public static Car car1 = new Car("Volvo");
        public static Car car2 = new Car("Nissan");
        public static Truck truck1 = new Truck("Solaris");

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
            CustomerOrder order4 = new CustomerOrder(new Manifest(ManifestItems), "AnotherCustomer", warehouse.Id);
            DeliveryAddress deliveryAddress1 = new DeliveryAddress(order1.Id, 13.13, 13.14);
            DeliveryAddress deliveryAddress2 = new DeliveryAddress(order2.Id, 14.14, 14.15);
            DeliveryAddress deliveryAddress3 = new DeliveryAddress(order3.Id, 15.15, 15.16);
            DeliveryAddress deliveryAddress4 = new DeliveryAddress(order4.Id, 16.16, 16.17);

            TripStop[] stopArr1 = [new TripStop(order1, deliveryAddress1)];

            TripStop[] stopArr2 = [new TripStop(order2, deliveryAddress2),
            new TripStop(order3, deliveryAddress3),
             new TripStop(order1, deliveryAddress3)];

            TripStop[] stopArr3 = [new TripStop(order4, deliveryAddress4)];
            Trip trip1 = new Trip(stopArr1, warehouse.Longitude, warehouse.Latitude);
            Trip trip2 = new Trip(stopArr2, warehouse.Longitude, warehouse.Latitude);
            Trip trip3 = new Trip(stopArr3, warehouse.Longitude, warehouse.Latitude);
        }
    }
}
