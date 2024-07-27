﻿using Haulage.Model;
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
            if (customerlogin == null) { throw new ArgumentNullException("Please provide a user login to view orders"); }
            List<CustomerOrder> orders = OrderController.GetAllOrdersForCustomer(customerlogin);
            if (orders == null || orders.Count == 0) { { throw new Exception("No orders available for the user"); } }
            else { return orders; }
        }

        public static CustomerOrder GetCustomerOrder(string id)
        {
            try
            {
                if (id == null || id == "") { throw new Exception("No id provided"); }
                CustomerOrder order = OrderController.GetSpecificOrderDetailsForACustomer(id);
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

        public static Handover ScheduleHandover(CustomerOrder order, DateTime handoverDate, bool pickup)
        {
            if(handoverDate < DateTime.Now)
            {
                throw new Exception("Cannot pick date less than the current day");
            }
           return order.ScheduleHandover(order, handoverDate, pickup);
        }
    }
}
