using Haulage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Haulage.Control
{
    public class CustomerController
    {
        private string customerlogin;

        public CustomerController(string customerLogin) {
            this.customerlogin = customerLogin;
        }
        public List<CustomerOrder> GetAllOrders()
        {
            if (customerlogin == null || customerlogin == "") { throw new ArgumentNullException("Please provide a user login to view orders"); }
            List<CustomerOrder> orders = OrderController.GetAllOrdersForCustomer(customerlogin);
            if (orders == null || orders.Count == 0) { { throw new Exception("No orders available for the user"); } }
            else { return orders; }
        }

        public CustomerOrder GetCustomerOrder(string id)
        {
            try
            {
                if (id == null || id == "") { throw new Exception("No id provided"); }
                CustomerOrder order = OrderController.GetSpecificOrderWithDetails(id);
                if (order == null || order.Manifest == null)
                {
                    throw new Exception("No order or manifest found");
                }
                return order;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Handover ScheduleHandover(CustomerOrder order, DateTime handoverDate, bool pickup)
        {
            // Make sure the call to ScheduleHandover matches the new method signature
            return order.ScheduleHandover(handoverDate, pickup);
        }
    }
}
