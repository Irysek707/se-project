using Haulage.Model.Helpers;
using SQLite;

namespace Haulage.Model
{
    public class Item
    {
       
        public string name { get; set; }
        [PrimaryKey]
        public string code { get; set; }
        public double price { get; set; }

        public Item(string code, string name, double price)
        {
            this.code = code;
            this.name = name;
            this.price = price;
            if (!DBHelpers.EnterToDB(this))
            {
            //    throw new Exception("Something went wrong adding an item");
            }
        }

        public Item() { }
 

    }
}
