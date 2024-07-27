using Haulage.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haulage.Model
{
    public class Driver : User
    {
        public Driver(string login) : base(Role.DRIVER, login)
        {
        }
    }
}
