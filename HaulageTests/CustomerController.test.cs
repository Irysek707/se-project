using Haulage;
using Haulage.Control;
using Haulage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaulageTests
{
    public class CustomerControllerTests
    {
        DB DB = new DB(true);
        private CustomerController controller = new CustomerController("customer1");
        private string order1 = "INSERT INTO [CustomerOrder]([ManifestId],[Customer],[Id],[Status],[WarehouseId]) VALUES ('123','customer1','123',0,'123');";
        private string order2 = "INSERT INTO [CustomerOrder]([ManifestId],[Customer],[Id],[Status],[WarehouseId]) VALUES ('1234','customer1','123',0,'123');";

        [Fact]

        public void GetAllOrdersNoUserThrowsException()
        {
            Assert.Throws<Exception>(() => new CustomerController("").GetAllOrders());
        }

        [Fact]
        public void GetAllOrdersEmptyArray()
        {
            Assert.Throws<Exception>(() => controller.GetAllOrders());
        }

        [Fact]
         public void GetAllOrdersSingleEntryInArray()
        {
            DB.connection.Query<CustomerOrder>(order1);
            List<CustomerOrder> orders = controller.GetAllOrders();
            Assert.Single(orders);
        }

        [Fact]
        public void GetAllOrdersTwoOrdersInArray()
        {
            DB.connection.Query<CustomerOrder>(order2);
            List<CustomerOrder> orders = controller.GetAllOrders();
            Assert.Equal(2, orders.Count);
        }

        [Fact]
        public void GetCustomerOrderNoId()
        {
            Assert.Throws<Exception>(() => controller.GetCustomerOrder(""));
        }

        [Fact]
        public void GetCustomerOrderNoOrderFound()
        {
            Assert.Throws<Exception>(() => controller.GetCustomerOrder("12"));
        }

        [Fact]
        public void GetCustomerOrderSuccess()
        {
            CustomerOrder order = controller.GetCustomerOrder("123");
            Assert.NotNull(order);
            Assert.Equal("123", order.Id.ToString());
        }

        [Fact]
        public void ScheduleAHandoverFailDateInPast ()
        {
            Assert.Throws<Exception>(() => controller.ScheduleHandover(null, DateTime.Parse("2009-06-15T13:45:30"), false));
        }

        [Fact]

        public void ScheduleHandoverSuccessPickup()
        {
            CustomerOrder order = controller.GetCustomerOrder("123");
            Handover Handover = controller.ScheduleHandover(order, DateTime.Now, true);
            Assert.NotNull(order.Handover);
            Assert.True(order.Handover.Pickup);
        }
    }
}

