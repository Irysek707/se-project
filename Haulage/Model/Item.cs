using Haulage.Model.Helpers;
using SQLite;

namespace Haulage.Model
{
    public class Item
    {
       
        public string Name { get; set; }
        [PrimaryKey]
        public string Code { get; set; }
        public double Price { get; set; }

        public Item(string code, string name, double price)
        {
            this.Code = code;
            this.Name = name;
            this.Price = price;
            if (!DBHelpers.EnterToDB(this))
            {
            //    throw new Exception("Something went wrong adding an item");
            }
        }

        public Item() { }
 

    }
}
