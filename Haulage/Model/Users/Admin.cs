using Haulage.Model.Constants;
using Haulage.Model.Helpers;

namespace Haulage.Model.Users
{
    public class Admin : User
    {
        public Admin(string login, string name, string surname)
            : base(Constants.Role.ADMIN, login, name, surname)
        {
        }
    }
}
