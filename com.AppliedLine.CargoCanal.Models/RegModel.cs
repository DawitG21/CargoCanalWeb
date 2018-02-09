using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class RegModel
    {
        public Person Person = new Person();
        public Company Company = new Company();
        public Login Login = new Login();
        //public UserTypes UserType = new UserTypes();
        public DefaultBroker DefaultBroker = new DefaultBroker();
        //used to indicate errors during registration process
        //Error message is passed back to client
        public string ErrorMessage = "";
    }
}
