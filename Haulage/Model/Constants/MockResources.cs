using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Haulage.Model.Constants
{
    public class MockResources
    { 

        //Placeholder until user implementation
        public static User mockUser = new User(Model.Constants.Role.CUSTOMER, "customer1");
        
        public void CreateMockOrderResources()
        {
            new CustomerOrder(new Manifest(ManifestItem.createSomeItemsForDebug()), mockUser.login);
            new CustomerOrder(new Manifest(ManifestItem.createSomeItemsForDebug()), mockUser.login);
            new CustomerOrder(new Manifest(ManifestItem.createSomeItemsForDebug()), mockUser.login);
        }
    }
}
