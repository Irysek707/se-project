
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

        //More fields can be added here to ensure secure login, however they are out of scope for this application
        /// This could also include an id to allow multiple users with the same name, <summary>
        /// This could also include an id to allow multiple users with the same name,
        ///however would then require different identification system so out of scope

        public User(Role role, string login)
        {
            this.Role = role;
            this.Login = login;
            DBHelpers.EnterToDB(this);
        }

        public User() { }

    }
}
