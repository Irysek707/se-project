using Haulage.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haulage.Model.Users
{
    public class Admin: User
    {
        public Admin(string login) : base(Constants.Role.ADMIN, login)
        {
        }
    }
}
