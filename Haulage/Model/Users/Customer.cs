using Haulage.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haulage.Model.Users
{
    public class Customer : User
    {
        public Customer(string login) : base(Role.CUSTOMER, login)
        {
        }
    }
}
