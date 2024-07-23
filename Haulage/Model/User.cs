
using Haulage.Model.Constants;
using Haulage.Model.Helpers;
using SQLite;


namespace Haulage.Model
{
    public class User
    {
        public Role role { get; set; }

        [PrimaryKey]
        public string login { get; set; }

        public User(Role role, string login)
        {
            this.role = role;
            this.login = login;
            DBHelpers.EnterToDB(this);
        }

        public User() { }



        //More fields can be added here to ensure secure login, however they are out of scope for this application
        /// This could also include an id to allow multiple users with the same name, <summary>
        /// This could also include an id to allow multiple users with the same name,
        ///however would then require different identification system so out of scope
    }
}
