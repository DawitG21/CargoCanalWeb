using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class LoginInfo
    {
        //info about the person logged in
        public string _username;
        public string _tick;
        public string _guid;
        public DateTime lastSeenDateTime = DateTime.Now;

        //the payload
        public Company Company = new Company();
        public Person Person = new Person();
        public Login Login = new Login();

        //added for CustomsBroker branch
        public UserTypes UserType = new UserTypes();
        public DefaultBroker DefaultBroker = new DefaultBroker();
    }
}
