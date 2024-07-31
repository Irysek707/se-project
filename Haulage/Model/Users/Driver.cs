using Haulage.Model.Constants;
using Haulage.Model.Helpers;

namespace Haulage.Model
{
    public class Driver : User
    {
        public Driver(string login, string name, string surname)
            : base(Role.DRIVER, login, name, surname)
        {
            DBHelpers.EnterToDB(this);
        }

        public Driver() { }
    }
}
