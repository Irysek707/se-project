using Haulage.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haulage.Model.Vehicle
{
    public class Car : Transport
    {
        public Car(string name) : base(name, 500)
        {
        }
    }
}
