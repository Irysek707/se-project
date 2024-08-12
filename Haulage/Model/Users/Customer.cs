using Haulage.Model.Constants;
using Haulage.Model.Helpers;

namespace Haulage.Model.Users
{
    public class Customer : User
    {
        public Customer(string login, string name, string surname)
            : base(Role.CUSTOMER, login, name, surname)
        {
        }
    }
}
