using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{

    public class Customer
    {
        public Customer()
        {
            //consturctor
        }
        //constructor with all prameters
        public Customer(long _id, string _customerID, string _personID, DateTime _registeredDate, string _photoPath)
        {
            ID = _id;
            CustomerID = _customerID;
            PersonID = _personID;
            RegisteredDate = _registeredDate;
            PhotoPath = _photoPath;
        }

        public long ID { get; set; }
        public string CustomerID { get; set; }
        public string PersonID { get; set; }
        public DateTime RegisteredDate { get; set; }
        public string PhotoPath { get; set; }

        #region " Class Members Code Starts Here ..........."
        // Class Members code....................
        public int SaveCustomer(Customer clsCustomerObj)
        {
            return new DataAccessLayer().SaveCustomerEntry(clsCustomerObj);
        }

        public int UpdateCustomer(Customer clsCustomerObj)
        {
            return new DataAccessLayer().UpdateCustomerEntry(clsCustomerObj);
        }

        public Customer GetCustomer(string customerID)
        {
            return new DataAccessLayer().GetCustomerObj(customerID);
        }

        public int DeleteCustomer(string customerID)
        {
            return new DataAccessLayer().DeleteCustomerEntry(customerID);
        }

        // -------------------- end of Class Members Code
        #endregion

    }
}
