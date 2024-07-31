using Haulage.Model.Constants;
using Haulage.Model.Helpers;
using SQLite;

namespace Haulage.Model
{
    // This class is intended for basic functionality for the users, so that specific functionality would be added
    // to individual classes but basic ones, such as login would be here
    public class User
    {
        public Role Role { get; set; }

        [PrimaryKey]
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public User(Role role, string login, string name, string surname)
        {
            this.Role = role;
            this.Login = login;
            this.Name = name;
            this.Surname = surname;
            DBHelpers.EnterToDB(this);
        }

        public User() { }
    }
}
