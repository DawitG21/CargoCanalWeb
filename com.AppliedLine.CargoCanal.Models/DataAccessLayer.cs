using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace com.AppliedLine.CargoCanal.Models
{
    public class DataAccessLayer
    {

        #region "Connection"
        public static SqlConnection getConnectionToMarilogDB()
        {
            try
            {
                SqlConnection connz;
                //string conStr = @"Data Source=appliedline.com;Initial Catalog=CargoCanalDB;User Id=aldbuser;Password=googlechrome;Integrated Security=False;MultipleActiveResultSets=true";
                string conStr = @"Data Source=AADELEKE-PC;Initial Catalog=CargoCanalDB;Integrated Security=True;MultipleActiveResultSets=true";
                connz = new SqlConnection(conStr);
                connz.Open();
                return connz;
            }
            catch (Exception e)
            {
                e.ToString();
                return new SqlConnection();
            }
        }
        #endregion

        #region "CustomIDBuilder and DeleteImportExportDelegate"

        private static long getMaxID(string dbTableName)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT MAX(ID) FROM " + dbTableName, conx);
                var ID = cmd.ExecuteScalar();
                if (ID == DBNull.Value)
                {
                    return 1;
                }

                return Convert.ToInt64(ID) + 1;
            }
        }

        public object GetCustomerTrackings(object email, bool v)
        {
            throw new NotImplementedException();
        }

        private static string CustomIDBuilder(string dbTableName)
        {
            return getMaxID(dbTableName).ToString().PadLeft(10, '0');
        }

        private delegate int DeleteImportExportDelegate(string ImportExportID);
        #endregion

        #region Tracking
        public int SaveTrackingEntry(Tracking tracking)
        {
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Tracking VALUES('" +
                        tracking.TrackingID + "','" +
                        tracking.TrackingNumber + "','" +
                        tracking.CustomerEmail + "','" +
                        tracking.ImportExportID + "','" +
                        tracking.AgentID + "','" +
                        tracking.DateInserted + "')", conx);

                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public Tracking GetTrackingObj(string trackingID)
        {
            try
            {
                Tracking clstrackingObj = null;

                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Tracking WHERE TrackingID = '"
                        + trackingID + "'", conx);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        clstrackingObj = new Tracking(reader.GetInt64(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetString(5),
                            reader.GetDateTime(6));
                    }

                    return clstrackingObj;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Tracking> GetTrackingsByCompany(string companyID)
        {
            try
            {
                List<Tracking> trackings = new List<Tracking>();

                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    string commandText = @"SELECT t.ID, t.TrackingID, t.TrackingNumber, t.CustomerEmail
                                            , t.ImportExportID, t.AgentID, t.DateInserted 
                                            FROM Tracking t
                                            JOIN Person p ON t.AgentID = p.PersonID
                                            WHERE p.CompanyID = '" + companyID + "'";

                    SqlCommand cmd = new SqlCommand(commandText, conx);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var clstrackingObj = new Tracking(reader.GetInt64(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetString(5),
                            reader.GetDateTime(6));

                        trackings.Add(clstrackingObj);
                    }

                    return trackings;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public JArray GetCustomerTrackings(string id, bool isEmail)
        {
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand command = null;

                    switch (isEmail)
                    {
                        case true:
                            command = new SqlCommand("sp_GetTrackingsByEmail", conx);

                            SqlParameter pCustomerEmail = command.Parameters.Add("@customerEmail", SqlDbType.NVarChar);
                            pCustomerEmail.SqlValue = id;
                            break;
                        case false:
                            command = new SqlCommand("sp_GetTrackingsByNumber", conx);

                            SqlParameter pTrackingNumber = command.Parameters.Add("@trackingNumber", SqlDbType.NVarChar);
                            pTrackingNumber.SqlValue = id;
                            break;
                    }

                    command.CommandType = CommandType.StoredProcedure;

                    DataSet ds = new DataSet("dsTrackings");
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);

                    JArray result = new JArray();
                    string trackingNumber = string.Empty;

                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = ds.Tables[0].Rows[i];


                        trackingNumber = row["TrackingNumber"].ToString();
                        var uniqueTracking = ds.Tables[0].AsEnumerable().Where(x => x.Field<string>("TrackingNumber") == trackingNumber);

                        JObject jsonObject = new JObject(
                            new JProperty("TrackingNumber", row["TrackingNumber"].ToString()),
                            new JProperty("Description", row["Description"].ToString()),
                            new JProperty("DocType", row["DocType"].ToString()),
                            new JProperty("Cargo", row["Cargo"].ToString()),
                            new JProperty("SubCargo", row["SubCargo"].ToString()),
                            new JProperty("RecordNo", row["RecordNo"].ToString()),
                            new JProperty("DocumentNo", row["DocumentNo"].ToString()),
                            new JProperty("Origin", row["Origin"].ToString()),
                            new JProperty("Destination", row["Destination"].ToString()),
                            new JProperty("AgentName", row["AgentName"].ToString()),
                            new JProperty("AgentPhone", row["AgentPhone"].ToString()),
                            new JProperty("AgentEmail", row["AgentEmail"].ToString()),
                            new JProperty("AgentCompany", row["AgentCompany"].ToString()),
                            new JProperty("Completed", Convert.ToBoolean(row["Completed"].ToString())),
                            new JProperty("LastUpdated", DateTime.Now.Subtract(Convert.ToDateTime(row["LastUpdated"].ToString())).ToString("G")),
                            new JProperty("Statuses", new JArray(
                                from s in uniqueTracking
                                select new JObject(
                                    new JProperty("Status", s.Field<string>("Status")),
                                    new JProperty("StatusDate", s.Field<DateTime>("StatusDate"))
                                ))));

                        result.Add(jsonObject);

                        i += uniqueTracking.Count() - 1;

                    }

                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public int UpdateTrackingEntry(Tracking tracking)
        {
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Tracking SET TrackingNumber = '" + tracking.TrackingNumber
                        + "', CustomerEmail = '" + tracking.CustomerEmail
                        + "', ImportExportID = '" + tracking.ImportExportID
                        + "', AgentID = '" + tracking.AgentID
                        + "', DateInserted = '" + tracking.DateInserted
                        + "' WHERE TrackingID ='" + tracking.TrackingID + "'", conx);

                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int DeleteTrackingEntry(string trackingID)
        {
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Tracking WHERE TrackingID = '"
                        + trackingID + "'", conx);

                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region Customer
        public int SaveCustomerEntry(Customer customer)
        {
            try
            {
                customer.CustomerID = CustomIDBuilder("Customer");
                if (string.IsNullOrWhiteSpace(customer.PhotoPath)) customer.PhotoPath = string.Empty;

                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Customer VALUES('" +
                        customer.CustomerID + "','" +
                        customer.PersonID + "','" +
                        customer.RegisteredDate + "','" +
                        customer.PhotoPath + "')", conx);

                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public Customer GetCustomerObj(string customerID)
        {
            try
            {
                Customer customer = null;

                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Customer WHERE CustomerID = '" + customerID + "'");
                    cmd.Connection = conx;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        customer = new Customer(
                            reader.GetInt64(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetDateTime(3),
                            reader.GetString(4));
                    }
                    return customer;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int UpdateCustomerEntry(Customer customer)
        {
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Customer SET PersonID = '"
                        + customer.PersonID + "', RegisteredDate = '"
                        + customer.RegisteredDate + "', PhotoPath = '"
                        + customer.PhotoPath + "' WHERE CustomerID ='" + customer.CustomerID + "'");
                    cmd.Connection = conx;

                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int DeleteCustomerEntry(string customerID)
        {
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Customer WHERE CustomerID = '"
                        + customerID + "'", conx);

                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion

        #region "Company"
        public static int SaveclsCompanyEntry(Company clsCompanyobj)
        {
            clsCompanyobj.CompanyID = CustomIDBuilder("Company");

            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Company VALUES('" +
                clsCompanyobj.CompanyID + "','" +
                clsCompanyobj.CompanyName + "','" +
                clsCompanyobj.CompanyTypeID + "','" +
                clsCompanyobj.Address + "','" +
                clsCompanyobj.TownCity + "','" +
                clsCompanyobj.State + "','" +
                clsCompanyobj.POBox + "','" +
                clsCompanyobj.Wereda + "','" +
                clsCompanyobj.KefleKetema + "','" +
                clsCompanyobj.Kebele + "','" +
                clsCompanyobj.HouseNo + "','" +
                clsCompanyobj.CountryID + "','" +
                clsCompanyobj.Telephone + "','" +
                clsCompanyobj.ContactName + "','" +
                clsCompanyobj.ContactMobile + "','" +
                clsCompanyobj.Email + "','" +
                clsCompanyobj.Website + "','" +
                clsCompanyobj.TIN + "','" +
                clsCompanyobj.LicenseNumber + "','" +
                clsCompanyobj.LicenseUnder + "','" +
                clsCompanyobj.LastRenewedDate + "','" +
                clsCompanyobj.LicenseIssuedDate + "','" +
                clsCompanyobj.IsActive + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static List<Company> getAllclsCompanyObj()
        {
            List<Company> companies = new List<Company>();
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Company", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Company clsCompanyObj = new Company(reader.GetInt64(0),
                        reader.GetString(1), reader.GetString(2), reader.GetString(3),
                        reader.GetString(4), reader.GetString(5), reader.GetString(6),
                        reader.GetString(7), reader.GetString(8), reader.GetString(9),
                        reader.GetString(10), reader.GetString(11), reader.GetString(12),
                        reader.GetString(13), reader.GetString(14), reader.GetString(15),
                        reader.GetString(16), reader.GetString(17), reader.GetString(18),
                        reader.GetString(19), reader.GetString(20), reader.GetDateTime(21),
                        reader.GetDateTime(22), reader.GetBoolean(23));

                        companies.Add(clsCompanyObj);
                    }
                }
            }
            catch
            {
                return companies;
            }
            return companies;
        }

        public static Company getclsCompanyObj(string CompanyID)
        {
            Company clsCompanyObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Company WHERE companyid = '" + CompanyID +
                    "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsCompanyObj = new Company(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetString(7),
                    reader.GetString(8),
                    reader.GetString(9),
                    reader.GetString(10),
                    reader.GetString(11),
                    reader.GetString(12),
                    reader.GetString(13),
                    reader.GetString(14),
                    reader.GetString(15),
                    reader.GetString(16),
                    reader.GetString(17),
                    reader.GetString(18),
                    reader.GetString(19),
                    reader.GetString(20),
                    reader.GetDateTime(21),
                    reader.GetDateTime(22),
                    reader.GetBoolean(23));
                }

                return clsCompanyObj;
            }
        }

        public static int updateclsCompanyEntry(Company clsCompanyObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE Company SET CompanyName = '" + clsCompanyObj.CompanyName
                + "', CompanyTypeID = '" + clsCompanyObj.CompanyTypeID
                + "', Address = '" + clsCompanyObj.Address
                + "', TownCity = '" + clsCompanyObj.TownCity
                + "', State = '" + clsCompanyObj.State
                + "', POBox = '" + clsCompanyObj.POBox
                + "', Wereda = '" + clsCompanyObj.Wereda
                + "', KefleKetema = '" + clsCompanyObj.KefleKetema
                + "', Kebele = '" + clsCompanyObj.Kebele
                + "', HouseNo = '" + clsCompanyObj.HouseNo
                + "', CountryID = '" + clsCompanyObj.CountryID
                + "', Telephone = '" + clsCompanyObj.Telephone
                + "', ContactName = '" + clsCompanyObj.ContactName
                + "', ContactMobile = '" + clsCompanyObj.ContactMobile
                + "', Email = '" + clsCompanyObj.Email
                + "', Website = '" + clsCompanyObj.Website
                + "', TIN = '" + clsCompanyObj.TIN
                + "', LicenseNumber = '" + clsCompanyObj.LicenseNumber
                + "', LicenseUnder = '" + clsCompanyObj.LicenseUnder
                + "', LastRenewedDate = '" + clsCompanyObj.LastRenewedDate
                + "', LicenseIssuedDate = '" + clsCompanyObj.LicenseIssuedDate
                + "', IsActive = '" + clsCompanyObj.IsActive + "' WHERE companyid ='" + clsCompanyObj.CompanyID + "'");
                cmd.Connection = conx;
                try
                {
                    int val = cmd.ExecuteNonQuery();

                    return val;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); return 0; }
            }
        }

        public static int deleteclsCompanyEntry(string companyid)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Company WHERE companyid = '" + companyid + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public long getHighestCompanyRecordID()
        {
            return getMaxID("Company");
        }
        #endregion
        #region "Consignee"
        public static int SaveclsConsigneeEntry(Consignee clsConsigneeobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Consignee VALUES('" +
                clsConsigneeobj.ConsigneeID + "','" +
                clsConsigneeobj.PersonID + "','" +
                clsConsigneeobj.CompanyID + "','" +
                clsConsigneeobj.IsAgent + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static List<Consignee> getAllclsConsigneeObj()
        {
            List<Consignee> consignees = new List<Consignee>();
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Consignee", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Consignee clsConsigneeObj = new Consignee(reader.GetInt64(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4));

                        consignees.Add(clsConsigneeObj);
                    }
                }
            }
            catch { }
            return consignees;
        }

        public static Consignee getclsConsigneeObj(string ConsigneeID)
        {
            Consignee clsConsigneeObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Consignee WHERE id = '" + ConsigneeID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsConsigneeObj = new Consignee(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4));
                }

                return clsConsigneeObj;
            }
        }

        public static int updateclsConsigneeEntry(Consignee clsConsigneeObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE Consignee SET PersonID = '" + clsConsigneeObj.PersonID
                + "', CompanyID = '" + clsConsigneeObj.CompanyID
                + "', IsAgent = '" + clsConsigneeObj.IsAgent + "' WHERE ConsigneeID ='"
                + clsConsigneeObj.ConsigneeID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsConsigneeEntry(string ConsigneeID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Consignee WHERE ConsigneeID = '" + ConsigneeID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion

        #region "Cost"

        public static int SaveclsCostEntry(Cost clsCostobj)
        {
            if (clsCostobj == null) return 1;

            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Cost VALUES('" +
                clsCostobj.ImportExportID + "'," +
                clsCostobj.Freight + "," +
                clsCostobj.Insurance + "," +
                clsCostobj.InlandTransport + "," +
                clsCostobj.PortStorageDemurrage + "," +
                clsCostobj.InlandStorage + "," +
                clsCostobj.PortHandling + "," +
                clsCostobj.TruckDetention + "," +
                clsCostobj.DryPortCharge + "," +
                clsCostobj.MaterialCost + "," +
                clsCostobj.OtherChargesBirr + "," +
                clsCostobj.OtherChargesDollar + "," +
                clsCostobj.TotalBirr + "," +
                clsCostobj.TotalDollar + ")");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();
                return val;
            }
        }

        public static Cost getclsCostObj(string ImportExportID)
        {
            Cost clsCostObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Cost WHERE ImportExportID = '" + ImportExportID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsCostObj = new Cost(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetDecimal(2),
                    reader.GetDecimal(3),
                    reader.GetDecimal(4),
                    reader.GetDecimal(5),
                    reader.GetDecimal(6),
                    reader.GetDecimal(7),
                    reader.GetDecimal(8),
                    reader.GetDecimal(9),
                    reader.GetDecimal(10),
                    reader.GetDecimal(11),
                    reader.GetDecimal(12),
                    reader.GetDecimal(13),
                    reader.GetDecimal(14));
                }

                return clsCostObj;
            }
        }

        public static int updateclsCostEntry(Cost clsCostObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE Cost SET Freight = " + clsCostObj.Freight
                + ", Insurance = " + clsCostObj.Insurance
                + ", InlandTransport = " + clsCostObj.InlandTransport
                + ", PortStorageDemurrage = " + clsCostObj.PortStorageDemurrage
                + ", InlandStorage = " + clsCostObj.InlandStorage
                + ", PortHandling = " + clsCostObj.PortHandling
                + ", TruckDetention = " + clsCostObj.TruckDetention
                + ", DryPortCharge = " + clsCostObj.DryPortCharge
                + ", MaterialCost = " + clsCostObj.MaterialCost
                + ", OtherChargesBirr = " + clsCostObj.OtherChargesBirr
                + ", OtherChargesDollar = " + clsCostObj.OtherChargesDollar
                + ", TotalBirr = " + clsCostObj.TotalBirr
                + ", TotalDollar = " + clsCostObj.TotalDollar + " WHERE ImportExportID ='" +
                clsCostObj.ImportExportID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsCostEntry(string ImportExportID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Cost WHERE ImportExportID = '" + ImportExportID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "Country"
        public static int SaveclsCountryEntry(Country clsCountryobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Country VALUES('" +
                clsCountryobj.Name + "','" +
                clsCountryobj.Region + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static List<Country> getAllclsCountryObj()
        {
            List<Country> countries = new List<Country>();

            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Country", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Country country = new Country(reader.GetInt64(0),
                        reader.GetString(1),
                        reader.GetString(2));

                        countries.Add(country);
                    }
                }
            }
            catch { }
            return countries;
        }

        public static Country getclsCountryObj(string CountryID)
        {
            Country clsCountryObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Country WHERE id = '" + CountryID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsCountryObj = new Country(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2));
                }

                return clsCountryObj;
            }
        }

        public static int updateclsCountryEntry(Country clsCountryObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE Country SET ID = '" + clsCountryObj.ID
                + "', Name = '" + clsCountryObj.Name
                + "', Region = '" + clsCountryObj.Region + "' WHERE ID ='" + clsCountryObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsCountryEntry(string id)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Country WHERE id = '" + id + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "EMA ....... ....."
        public static int SaveclsEMAEntry(EMA clsEMAobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO EMA VALUES('" +
                clsEMAobj.DateFrom + "','" +
                clsEMAobj.DateTo + "','" +
                clsEMAobj.Agent + "','" +
                clsEMAobj.CostPerContainer + "," +
                clsEMAobj.CostPerKM + "," +
                clsEMAobj.CostPerKG + "," +
                clsEMAobj.SeaFreight + "," +
                clsEMAobj.PortAndClearance + "," +
                clsEMAobj.InlandTransport + "," +
                clsEMAobj.CustomStorage + "," +
                clsEMAobj.Demurrage + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static EMA getclsEMAObj(string id)
        {
            EMA clsEMAObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM EMA WHERE id = '" + id + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsEMAObj = new EMA(reader.GetInt64(0),
                    reader.GetDateTime(1),
                    reader.GetDateTime(2),
                    reader.GetString(3),
                    reader.GetDecimal(4),
                    reader.GetDecimal(5),
                    reader.GetDecimal(6),
                    reader.GetDecimal(7),
                    reader.GetDecimal(8),
                    reader.GetDecimal(9),
                    reader.GetDecimal(10),
                    reader.GetString(11));
                }

                return clsEMAObj;
            }
        }

        public static int updateclsEMAEntry(EMA clsEMAObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE EMA SET DateFrom = '" + clsEMAObj.DateFrom
                + "', DateTo = '" + clsEMAObj.DateTo
                + "', Agent = '" + clsEMAObj.Agent
                + "', CostPerContainer = " + clsEMAObj.CostPerContainer
                + ", CostPerKM = " + clsEMAObj.CostPerKM
                + ", CostPerKG = " + clsEMAObj.CostPerKG
                + ", SeaFreight = " + clsEMAObj.SeaFreight
                + ", PortAndClearance = " + clsEMAObj.PortAndClearance
                + ", InlandTransport = " + clsEMAObj.InlandTransport
                + ", CustomStorage = " + clsEMAObj.CustomStorage
                + "', Demurrage = '" + clsEMAObj.Demurrage + "' WHERE id ='" + clsEMAObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsEMAEntry(string id)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM EMA WHERE id = '" + id + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "ImpExpType"

        public static int SaveclsImpExpTypeEntry(ImpExpType clsImpExpTypeobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO ImpExpType VALUES('" +
                clsImpExpTypeobj.TypeName + "','" +
                clsImpExpTypeobj.Description + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static List<ImpExpType> getAllclsImpExpTypeObj()
        {
            List<ImpExpType> impExpTypes = new List<ImpExpType>();
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM ImpExpType", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ImpExpType impExpType = new ImpExpType(reader.GetInt64(0),
                        reader.GetString(1),
                        reader.GetString(2));

                        impExpTypes.Add(impExpType);
                    }
                }
            }
            catch { }
            return impExpTypes;
        }

        public static ImpExpType getclsImpExpTypeObj(string id)
        {
            ImpExpType clsImpExpTypeObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ImpExpType WHERE id = '" + id + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsImpExpTypeObj = new ImpExpType(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2));
                }

                return clsImpExpTypeObj;
            }
        }

        public static int updateclsImpExpTypeEntry(ImpExpType clsImpExpTypeObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE ImpExpType SET Name = '" + clsImpExpTypeObj.TypeName
                + "', Description = '" + clsImpExpTypeObj.Description + "' WHERE ID ='" + clsImpExpTypeObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsImpExpTypeEntry(string id)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM ImpExpType WHERE ID = '" + id + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "Import"

        private static void updateImportExportItemsCostStatuses(ImportExport importExport)
        {
            updateclsItemEntry(importExport.Items);
            updateclsCostEntry(importExport.Cost);
            updateclsImportExportStatusUpdateEntry(importExport.ImportExportStatuses);
        }

        private int updateImportExport(ImportExport importExport)
        {
            int val = 0;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE ImportExport SET"
                    + " ReferenceNo = '" + importExport.ReferenceNo
                    + "', DateInserted = '" + importExport.DateInserted
                    + "', ImpExpTypeID = '" + importExport.ImpExpTypeID
                    + "', ImportExportReasonID = '" + importExport.ImportExportReasonID
                    + "', PortOfLoadingID = '" + importExport.PortOfLoadingID
                    + "', PortOfDischargeID = '" + importExport.PortOfDischargeID
                    + "', DateDischarge = '" + importExport.DateDischarge
                    + "', ModeOfTransportID = '" + importExport.ModeOfTransportID
                    + "', DateInitiated = '" + importExport.DateInitiated
                    + "', ConsigneeID = '" + importExport.ConsigneeID
                    + "', ReceiverID = '" + importExport.ReceiverID
                    + "', CarrierID = '" + importExport.CarrierID
                    + "', VesselID = '" + importExport.VesselID
                    + "', VoyageNumber = '" + importExport.VoyageNumber
                    + "', IncoTermID = '" + importExport.IncoTermID
                    + "', Remark = '" + importExport.Remark
                    + "', RemarkDate = '" + importExport.RemarkDate
                    + "', ReImportExport = '" + importExport.ReImportExport
                    + "', Unimodal = '" + importExport.Unimodal
                    + "', BillTerminated = '" + importExport.BillTerminated
                    + "', Completed = '" + importExport.Completed + "' WHERE ImportExportID ='" +
                    importExport.ImportExportID + "'");
                cmd.Connection = conx;
                val = cmd.ExecuteNonQuery();
            }
            return val;
        }

        private int saveImportExport(ImportExport importExport)
        {
            try
            {
                importExport.DateInserted = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (!string.IsNullOrEmpty(importExport.Remark) && importExport.Remark.Trim() != string.Empty)
                {
                    importExport.RemarkDate = Convert.ToDateTime(importExport.DateInserted);
                }
                else importExport.RemarkDate = DateTime.Now;

                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO ImportExport VALUES('" +
                    importExport.ImportExportID + "','" +
                    importExport.ReferenceNo + "','" +
                    importExport.DateInserted + "','" +
                    importExport.ImpExpTypeID + "','" +
                    importExport.ImportExportReasonID + "','" +
                    importExport.PortOfLoadingID + "','" +
                    importExport.PortOfDischargeID + "','" +
                    importExport.DateDischarge + "','" +
                    importExport.ModeOfTransportID + "','" +
                    importExport.DateInitiated + "','" +
                    importExport.ConsigneeID + "','" +
                    importExport.ReceiverID + "','" +
                    importExport.CarrierID + "','" +
                    importExport.VesselID + "','" +
                    importExport.VoyageNumber + "','" +
                    importExport.IncoTermID + "','" +
                    importExport.Remark + "','" +
                    importExport.RemarkDate + "','" +
                    importExport.ReImportExport + "','" +
                    importExport.Unimodal + "','" +
                    importExport.BillTerminated + "','" +
                    importExport.Completed + "')");
                    cmd.Connection = conx;

                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return 0; }
        }

        public static bool GetImportExport(ImportExport importExport, string importExportID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                string commandText = @"SELECT x.[ID], x.[ImportExportID],x.[ReferenceNo] ,x.[DateInserted]
                        ,x.[ImpExpTypeID], x.[ImportExportReasonID], x.[PortOfLoadingID], x.[PortOfDischargeID]
                        ,x.[DateDischarge], x.[ModeOfTransportID], x.[DateInitiated], x.[ConsigneeID]
                        ,x.[ReceiverID], x.[CarrierID], x.[VesselID], x.[VoyageNumber], x.[IncoTermID], x.[Remark]
                        ,x.[RemarkDate], x.[ReImportExport], x.[Unimodal], x.[BillTerminated], x.[Completed]
                        ,p.[FirstName] + ' ' + p.[LastName] ConsigneeName, p.[Email], p.[Phone], c.CompanyName
                        FROM[ImportExport] x
                            JOIN Person p ON x.ConsigneeID = p.PersonID
                            JOIN Company c on x.ReceiverID = c.CompanyID
                        WHERE x.ImportExportID = '" + importExportID + "'";

                SqlCommand cmd = new SqlCommand(commandText, conx);

                SqlDataReader reader = cmd.ExecuteReader();

                bool recordExists = reader.HasRows;

                while (reader.Read())
                {
                    importExport.ID = reader.GetInt64(0);
                    importExport.ImportExportID = reader.GetString(1);
                    importExport.ReferenceNo = reader.GetString(2);
                    importExport.DateInserted = reader.GetDateTime(3).ToString();
                    importExport.ImpExpTypeID = reader.GetString(4);
                    importExport.ImportExportReasonID = reader.GetString(5);
                    importExport.PortOfLoadingID = reader.GetString(6);
                    importExport.PortOfDischargeID = reader.GetString(7);
                    importExport.DateDischarge = reader.GetDateTime(8);
                    importExport.ModeOfTransportID = reader.GetString(9);
                    importExport.DateInitiated = reader.GetDateTime(10);
                    importExport.ConsigneeID = reader.GetString(11);
                    importExport.ReceiverID = reader.GetString(12);
                    importExport.CarrierID = reader.GetString(13);
                    importExport.VesselID = reader.GetString(14);
                    importExport.VoyageNumber = reader.GetString(15);
                    importExport.IncoTermID = reader.GetString(16);
                    importExport.Remark = reader.GetString(17);
                    importExport.RemarkDate = reader.GetDateTime(18);
                    importExport.ReImportExport = reader.GetBoolean(19);
                    importExport.Unimodal = reader.GetBoolean(20);
                    importExport.BillTerminated = reader.GetBoolean(21);
                    importExport.Completed = reader.GetBoolean(22);
                    importExport.ConsigneeName = reader.GetString(23);
                    importExport.Email = reader.GetString(24);
                    importExport.Phone = reader.GetString(25);
                    importExport.CompanyName = reader.GetString(26);

                    importExport.Items = getclsItemObjByImportExportID(importExportID);
                    importExport.Cost = getclsCostObj(importExportID);
                    importExport.ImportExportStatuses = getclsImportExportStatusUpdateObj(importExportID);
                    importExport.ProblemUpdates = GetclsProblemUpdateObj(importExportID);
                }

                return recordExists;
            }
        }

        public Tracking createTracking(string prefix, string importExportID, string agentID, string customerEmail)
        {
            var tracking = new Tracking();

            tracking.TrackingID = CustomIDBuilder("Tracking");

            var p = prefix.Replace("-", string.Empty);
            if (p.Length < 5) p = p.PadRight(5, '0');

            tracking.TrackingNumber = p.Substring(0, 5).ToUpper() + tracking.TrackingID;
            tracking.ImportExportID = importExportID;
            tracking.AgentID = agentID;
            tracking.CustomerEmail = customerEmail ?? string.Empty;
            tracking.DateInserted = DateTime.Now;

            return tracking;
        }

        public static int SaveclsImportEntry(Import import)
        {
            import.ImportExportID = CustomIDBuilder("ImportExport");

            var dal = new DataAccessLayer();
            int val = dal.saveImportExport(import as ImportExport);

            if (val > 0)
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    import.LoadingDate = import.BillOfLadingDate;
                    SqlCommand cmd = new SqlCommand("INSERT INTO Import VALUES('" +
                        import.ImportExportID + "','" +
                        import.LoadingDate + "','" +
                        import.BillOfLading + "','" +
                        import.BillOfLadingDate + "')", conx);

                    return cmd.ExecuteNonQuery();
                }
            }

            return 0;
        }

        public static List<Import> GetAllclsImportObj(long lastID)
        {
            List<Import> imports = new List<Import>();

            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    string commandText = @"SELECT TOP 5 x.[ID], x.[ImportExportID], x.[ReferenceNo] ,x.[DateInserted]
                        ,x.[ImpExpTypeID], x.[ImportExportReasonID], x.[PortOfLoadingID], x.[PortOfDischargeID]
                        ,x.[DateDischarge], x.[ModeOfTransportID], x.[DateInitiated], x.[ConsigneeID]
                        ,x.[ReceiverID], x.[CarrierID], x.[VesselID], x.[VoyageNumber], x.[IncoTermID], x.[Remark]
                        ,x.[RemarkDate], x.[ReImportExport], x.[Unimodal], x.[BillTerminated], x.[Completed]
                        ,p.[FirstName] + ' ' + p.[LastName] ConsigneeName, p.[Email], p.[Phone], c.CompanyName
                        FROM [ImportExport] x
                            JOIN Person p ON x.ConsigneeID = p.PersonID
                            JOIN Company c on x.ReceiverID = c.CompanyID
                        WHERE x.ImpExpTypeID = '1'";
                    
                    // consider last ID
                    if (lastID > 0) commandText += " AND x.[ID] < " + lastID;

                    commandText += " ORDER BY c.CompanyName, ConsigneeName, Completed, BillTerminated, ID DESC";

                    SqlCommand cmd = new SqlCommand(commandText, conx);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Import import = new Import();

                        import.ID = reader.GetInt64(0);
                        import.ImportExportID = reader.GetString(1);
                        import.ReferenceNo = reader.GetString(2);
                        import.DateInserted = reader.GetDateTime(3).ToString();
                        import.ImpExpTypeID = reader.GetString(4);
                        import.ImportExportReasonID = reader.GetString(5);
                        import.PortOfLoadingID = reader.GetString(6);
                        import.PortOfDischargeID = reader.GetString(7);
                        import.DateDischarge = reader.GetDateTime(8);
                        import.ModeOfTransportID = reader.GetString(9);
                        import.DateInitiated = reader.GetDateTime(10);
                        import.ConsigneeID = reader.GetString(11);
                        import.ReceiverID = reader.GetString(12);
                        import.CarrierID = reader.GetString(13);
                        import.VesselID = reader.GetString(14);
                        import.VoyageNumber = reader.GetString(15);
                        import.IncoTermID = reader.GetString(16);
                        import.Remark = reader.GetString(17);
                        import.RemarkDate = reader.GetDateTime(18);
                        import.ReImportExport = reader.GetBoolean(19);
                        import.Unimodal = reader.GetBoolean(20);
                        import.BillTerminated = reader.GetBoolean(21);
                        import.Completed = reader.GetBoolean(22);
                        import.ConsigneeName = reader.GetString(23);
                        import.Email = reader.GetString(24);
                        import.Phone = reader.GetString(25);
                        import.CompanyName = reader.GetString(26);

                        SqlCommand cmdImport = new SqlCommand("SELECT * FROM Import WHERE ImportID=" + import.ImportExportID, conx);
                        SqlDataReader readerImport = cmdImport.ExecuteReader();
                        if (readerImport.HasRows)
                        {
                            while (readerImport.Read())
                            {
                                import.LoadingDate = readerImport.GetDateTime(2);
                                import.BillOfLading = readerImport.GetString(3);
                                import.BillOfLadingDate = readerImport.GetDateTime(4);
                            }
                        }

                        string ImportExportID = import.ImportExportID;

                        import.Items = getclsItemObjByImportExportID(ImportExportID);
                        import.Cost = getclsCostObj(ImportExportID);
                        import.ImportExportStatuses = getclsImportExportStatusUpdateObj(ImportExportID);
                        import.ProblemUpdates = GetclsProblemUpdateObj(ImportExportID);

                        imports.Add(import);
                    }
                }
            }
            catch (Exception e) { Console.Write(e.Message); }
            return imports;
        }

        public static Import GetclsImportObj(string ImportExportID)
        {
            Import clsImportObj = new Import();

            bool recordExists = GetImportExport(clsImportObj, ImportExportID);

            if (recordExists)
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmdImport = new SqlCommand("SELECT * FROM Import WHERE ImportID=" + clsImportObj.ImportExportID, conx);
                    SqlDataReader readerImport = cmdImport.ExecuteReader();

                    while (readerImport.Read())
                    {
                        clsImportObj.LoadingDate = readerImport.GetDateTime(2);
                        clsImportObj.BillOfLading = readerImport.GetString(3);
                        clsImportObj.BillOfLadingDate = readerImport.GetDateTime(4);
                    }

                    return clsImportObj;
                }
            }

            return null;
        }

        public static int UpdateclsImportEntry(Import clsImportObj)
        {
            int val = 0;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                var update = new DataAccessLayer();
                val = update.updateImportExport(clsImportObj as ImportExport);

                if (val == 1)
                {
                    //update sub table
                    var cmd = new SqlCommand("UPDATE Import SET LoadingDate = '" + clsImportObj.LoadingDate
                        + "' BillOfLading = '" + clsImportObj.BillOfLading
                        + "' BillOfLadingDate = '" + clsImportObj.BillOfLadingDate + "'", conx);

                    val = cmd.ExecuteNonQuery();
                }
            }

            if (val > 0) updateImportExportItemsCostStatuses(clsImportObj as ImportExport);

            return val;
        }

        public static int UpdateclsImportExportCompleted(string importExportID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE ImportExport SET Completed = '1'"
                    + " WHERE ImportExportID ='" + importExportID + "'");
                cmd.Connection = conx;
                return cmd.ExecuteNonQuery();
            }
        }

        public static int UpdateclsImportExportTerminated(string importExportID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE ImportExport SET BillTerminated = '1'"
                    + " WHERE ImportExportID ='" + importExportID + "'");
                cmd.Connection = conx;
                return cmd.ExecuteNonQuery();
            }
        }

        public static async Task<int> DeleteclsImportExportEntry(string ImportExportID)
        {
            int val = 0;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM ImportExport WHERE ImportExportID = '" + ImportExportID
                    + "' AND Completed != '1'");
                cmd.Connection = conx;
                val = await cmd.ExecuteNonQueryAsync();
            }

            if (val > 0)
            {
                DeleteImportExportDelegate deleteManager = null;
                deleteManager += deleteclsCostEntry; // delete cost
                deleteManager += deleteclsItemEntryByImportExportID; // delete items by importExportID
                deleteManager += deleteclsImportExportStatusUpdateEntryByImportExportID; // delete statuses by importExportID
                deleteManager += deleteclsProblemUpdateEntry; // delete problem updates by importExportID

                deleteManager(ImportExportID);
            }

            return val;
        }
        #endregion

        #region "Export"
        public static int SaveclsExportEntry(Export export)
        {
            export.ImportExportID = CustomIDBuilder("ImportExport");

            var dal = new DataAccessLayer();
            int val = dal.saveImportExport(export as ImportExport);



            if (val > 0)
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Export VALUES('"
                        + export.ImportExportID + "','"
                        + export.ETA + "','"
                        + export.ATA + "','"
                        + export.CPORecievedDate + "','"
                        + export.OriginalDocumentDate + "','"
                        + export.WayBill + "','"
                        + export.WayBillDate + "','"
                        + export.DeclarationNo + "','"
                        + export.DeclarationDate + "','"
                        + export.StuffingLocationID + "','"
                        + export.OriginID + "','"
                        + export.ShippingInstructionNo + "','"
                        + export.ShippingInstructionDate + "')", conx);

                    return cmd.ExecuteNonQuery();
                }
            }

            return 0;
        }

        public static List<Export> GetAllclsExportObj(long lastID)
        {
            List<Export> exports = new List<Export>();

            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    string commandText = @"SELECT TOP 5 x.[ID], x.[ImportExportID],x.[ReferenceNo] ,x.[DateInserted]
                        ,x.[ImpExpTypeID], x.[ImportExportReasonID], x.[PortOfLoadingID], x.[PortOfDischargeID]
                        ,x.[DateDischarge], x.[ModeOfTransportID], x.[DateInitiated], x.[ConsigneeID]
                        ,x.[ReceiverID], x.[CarrierID], x.[VesselID], x.[VoyageNumber], x.[IncoTermID], x.[Remark]
                        ,x.[RemarkDate], x.[ReImportExport], x.[Unimodal], x.[BillTerminated], x.[Completed]
                        ,p.[FirstName] + ' ' + p.[LastName] ConsigneeName, p.[Email], p.[Phone], c.CompanyName
                        FROM [ImportExport] x
                            JOIN Person p ON x.ConsigneeID = p.PersonID
                            JOIN Company c on x.ReceiverID = c.CompanyID
                        WHERE x.ImpExpTypeID = '2'";

                    // consider last ID
                    if (lastID > 0) commandText += " AND x.[ID] < " + lastID;

                    commandText += " ORDER BY c.CompanyName, ConsigneeName, Completed, BillTerminated, ID DESC";

                    SqlCommand cmd = new SqlCommand(commandText, conx);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Export export = new Export();

                        export.ID = reader.GetInt64(0);
                        export.ImportExportID = reader.GetString(1);
                        export.ReferenceNo = reader.GetString(2);
                        export.DateInserted = reader.GetDateTime(3).ToString();
                        export.ImpExpTypeID = reader.GetString(4);
                        export.ImportExportReasonID = reader.GetString(5);
                        export.PortOfLoadingID = reader.GetString(6);
                        export.PortOfDischargeID = reader.GetString(7);
                        export.DateDischarge = reader.GetDateTime(8);
                        export.ModeOfTransportID = reader.GetString(9);
                        export.DateInitiated = reader.GetDateTime(10);
                        export.ConsigneeID = reader.GetString(11);
                        export.ReceiverID = reader.GetString(12);
                        export.CarrierID = reader.GetString(13);
                        export.VesselID = reader.GetString(14);
                        export.VoyageNumber = reader.GetString(15);
                        export.IncoTermID = reader.GetString(16);
                        export.Remark = reader.GetString(17);
                        export.RemarkDate = reader.GetDateTime(18);
                        export.ReImportExport = reader.GetBoolean(19);
                        export.Unimodal = reader.GetBoolean(20);
                        export.BillTerminated = reader.GetBoolean(21);
                        export.Completed = reader.GetBoolean(22);
                        export.ConsigneeName = reader.GetString(23);
                        export.Email = reader.GetString(24);
                        export.Phone = reader.GetString(25);
                        export.CompanyName = reader.GetString(26);

                        using (SqlCommand cmdImport = new SqlCommand("SELECT * FROM Export WHERE ExportID=" + export.ImportExportID, conx))
                        {
                            SqlDataReader readerExport = cmdImport.ExecuteReader();
                            if (readerExport.HasRows)
                            {
                                while (readerExport.Read())
                                {
                                    export.ETA = readerExport.GetDateTime(2);
                                    export.ATA = readerExport.GetDateTime(3);
                                    export.CPORecievedDate = readerExport.GetDateTime(4);
                                    export.OriginalDocumentDate = readerExport.GetDateTime(5);
                                    export.WayBill = readerExport.GetString(6);
                                    export.WayBillDate = readerExport.GetDateTime(7);
                                    export.DeclarationNo = readerExport.GetString(8);
                                    export.DeclarationDate = readerExport.GetDateTime(9);
                                    export.StuffingLocationID = readerExport.GetString(10);
                                    export.OriginID = readerExport.GetString(11);
                                    export.ShippingInstructionNo = readerExport.GetString(12);
                                    export.ShippingInstructionDate = readerExport.GetDateTime(13);
                                }
                            }
                        }

                        string ImportExportID = export.ImportExportID;

                        export.Items = getclsItemObjByImportExportID(ImportExportID);
                        export.Cost = getclsCostObj(ImportExportID);
                        export.ImportExportStatuses = getclsImportExportStatusUpdateObj(ImportExportID);
                        export.ProblemUpdates = GetclsProblemUpdateObj(ImportExportID);

                        exports.Add(export);
                    }
                }
            }
            catch (Exception e) { Console.Write(e.Message); }
            return exports;
        }

        public static Export GetclsExportObj(string ImportExportID)
        {
            Export clsExportObj = new Export();

            bool recordExists = GetImportExport(clsExportObj, ImportExportID);

            if (recordExists)
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmdExport = new SqlCommand("SELECT * FROM Export WHERE ExportID=" + clsExportObj.ImportExportID, conx);
                    SqlDataReader reader = cmdExport.ExecuteReader();

                    while (reader.Read())
                    {
                        clsExportObj.ETA = reader.GetDateTime(2);
                        clsExportObj.ATA = reader.GetDateTime(3);
                        clsExportObj.CPORecievedDate = reader.GetDateTime(4);
                        clsExportObj.OriginalDocumentDate = reader.GetDateTime(5);
                        clsExportObj.WayBill = reader.GetString(6);
                        clsExportObj.WayBillDate = reader.GetDateTime(7);
                        clsExportObj.DeclarationNo = reader.GetString(8);
                        clsExportObj.DeclarationDate = reader.GetDateTime(9);
                        clsExportObj.StuffingLocationID = reader.GetString(10);
                        clsExportObj.OriginID = reader.GetString(11);
                        clsExportObj.ShippingInstructionNo = reader.GetString(12);
                        clsExportObj.ShippingInstructionDate = reader.GetDateTime(13);
                    }

                    return clsExportObj;
                }
            }

            return null;
        }

        public static int UpdateclsExportEntry(Export clsExportObj)
        {
            int val = 0;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                var update = new DataAccessLayer();
                val = update.updateImportExport(clsExportObj as ImportExport);

                if (val == 1)
                {
                    //update sub table
                    var cmd = new SqlCommand("UPDATE Export SET ETA = '" + clsExportObj.ETA
                        + "' ATA = '" + clsExportObj.ATA
                        + "' OriginalDocumentDate = '" + clsExportObj.OriginalDocumentDate
                        + "' WayBill = '" + clsExportObj.WayBill
                        + "' WayBillDate = '" + clsExportObj.WayBillDate
                        + "' DeclarationNo = '" + clsExportObj.DeclarationNo
                        + "' DeclarationDate = '" + clsExportObj.DeclarationDate
                        + "' StuffingLocationID = '" + clsExportObj.StuffingLocationID
                        + "' OriginID = '" + clsExportObj.OriginID
                        + "' ShippingInstructionNo = '" + clsExportObj.ShippingInstructionNo
                        + "' ShippingInstructionDate = '" + clsExportObj.ShippingInstructionDate + "'", conx);

                    val = cmd.ExecuteNonQuery();
                }
            }

            if (val > 0) updateImportExportItemsCostStatuses(clsExportObj as ImportExport);

            return val;
        }
        #endregion

        #region "IncoTerms"

        public static int SaveclsIncoTermsEntry(IncoTerm clsIncoTermsobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO IncoTerms VALUES('" +
                clsIncoTermsobj.IncoName + "','" +
                clsIncoTermsobj.IncoDescription + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static List<IncoTerm> getAllclsIncoTermsObj()
        {
            List<IncoTerm> incoTerms = new List<IncoTerm>();

            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM IncoTerms", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        IncoTerm incoTerm = new IncoTerms(reader.GetInt64(0),
                        reader.GetString(1),
                        reader.GetString(2));

                        incoTerms.Add(incoTerm);
                    }
                }
            }
            catch { }
            return incoTerms;
        }

        public static IncoTerm getclsIncoTermsObj(string ID)
        {
            IncoTerm clsIncoTermsObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM IncoTerms WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsIncoTermsObj = new IncoTerms(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2));
                }

                return clsIncoTermsObj;
            }
        }

        public static int updateclsIncoTermsEntry(IncoTerm clsIncoTermsObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE IncoTerms SET ID = '" + clsIncoTermsObj.ID
                + "', IncoName = '" + clsIncoTermsObj.IncoName
                + "', IncoDescription = '" + clsIncoTermsObj.IncoDescription + "' WHERE ID ='" + clsIncoTermsObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsIncoTermsEntry(string ID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM IncoTerms WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "Item"

        public static int SaveclsItemEntry(List<Item> items)
        {
            try
            {
                int val = 0;
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    foreach (var item in items)
                    {
                        item.ItemID = CustomIDBuilder("Item");
                        item.DateInserted = DateTime.Now;
                        if (string.IsNullOrEmpty(item.VolumeUnitID)) item.VolumeUnitID = "1";

                        SqlCommand cmd = new SqlCommand("INSERT INTO Item VALUES('" +
                        item.ItemID + "','" +
                        item.DateInserted + "','" +
                        item.ImportExportID + "','" +
                        item.ImpExpTypeID + "','" +
                        item.ItemOrderNo + "','" +
                        item.CargoID + "','" +
                        item.SubCargoID + "'," +
                        item.GrossWeight + "," +
                        item.NetWeight + ",'" +
                        item.WeightUnitID + "'," +
                        item.Volume + ",'" +
                        item.VolumeUnitID + "'," +
                        item.Quantity + ",'" +
                        item.Dangerous + "',N'" +
                        item.Description + "')");
                        cmd.Connection = conx;

                        val = cmd.ExecuteNonQuery();
                    }

                }

                if (val > 0)
                {
                    val = SaveclsItemDetailEntry(items);
                }

                return val;
            }
            catch { return 0; }
        }

        public static List<Item> getclsItemObjByImportExportID(string ImportExportID)
        {
            List<Item> items = new List<Item>();
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Item WHERE ImportExportID = '" + ImportExportID + "'", conx);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var clsItemObj = new Item();
                    clsItemObj.ID = reader.GetInt64(0);
                    clsItemObj.ItemID = reader.GetString(1);
                    clsItemObj.DateInserted = reader.GetDateTime(2);
                    clsItemObj.ImportExportID = reader.GetString(3);
                    clsItemObj.ImpExpTypeID = reader.GetString(4);
                    clsItemObj.ItemOrderNo = reader.GetString(5);
                    clsItemObj.CargoID = reader.GetString(6);
                    clsItemObj.SubCargoID = reader.GetString(7);
                    clsItemObj.GrossWeight = reader.GetDecimal(8);
                    clsItemObj.NetWeight = reader.GetDecimal(9);
                    clsItemObj.WeightUnitID = reader.GetString(10);
                    clsItemObj.Volume = reader.GetDecimal(11);
                    clsItemObj.VolumeUnitID = reader.GetString(12);
                    clsItemObj.Quantity = reader.GetInt32(13);
                    clsItemObj.Dangerous = reader.GetBoolean(14);
                    clsItemObj.Description = reader.GetString(15);

                    clsItemObj.ItemDetail = getclsItemDetailObjByItemID(clsItemObj.ItemID);
                    items.Add(clsItemObj);
                }
            }
            return items;
        }

        public static Item getclsItemObj(string ItemID)
        {
            Item clsItemObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Item WHERE ItemID = '" + ItemID + "'", conx);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsItemObj = new Item();
                    clsItemObj.ID = reader.GetInt64(0);
                    clsItemObj.ItemID = reader.GetString(1);
                    clsItemObj.DateInserted = reader.GetDateTime(2);
                    clsItemObj.ImportExportID = reader.GetString(3);
                    clsItemObj.ImpExpTypeID = reader.GetString(4);
                    clsItemObj.ItemOrderNo = reader.GetString(5);
                    clsItemObj.CargoID = reader.GetString(6);
                    clsItemObj.SubCargoID = reader.GetString(7);
                    clsItemObj.GrossWeight = reader.GetDecimal(8);
                    clsItemObj.NetWeight = reader.GetDecimal(9);
                    clsItemObj.WeightUnitID = reader.GetString(10);
                    clsItemObj.Volume = reader.GetDecimal(11);
                    clsItemObj.VolumeUnitID = reader.GetString(12);
                    clsItemObj.Quantity = reader.GetInt32(13);
                    clsItemObj.Dangerous = reader.GetBoolean(14);
                    clsItemObj.Description = reader.GetString(15);

                    clsItemObj.ItemDetail = getclsItemDetailObjByItemID(clsItemObj.ItemID);
                }
            }
            return clsItemObj;
        }

        public static int updateclsItemEntry(List<Item> items)
        {
            int val = 0;
            try
            {
                foreach (var item in items)
                {
                    using (SqlConnection conx = getConnectionToMarilogDB())
                    {
                        SqlCommand cmd = new SqlCommand("UPDATE Item SET DateInserted = '" + item.DateInserted
                        + "', ImportExportID = '" + item.ImportExportID
                        + "', ImpExpTypeID = '" + item.ImpExpTypeID
                        + "', ItemOrderNo = '" + item.ItemOrderNo
                        + "', CargoID = '" + item.CargoID
                        + "', SubCargoID = '" + item.SubCargoID
                        + "', GrossWeight = " + item.GrossWeight
                        + ", NetWeight = " + item.NetWeight
                        + ", WeightUnitID = '" + item.WeightUnitID
                        + "', Volume = " + item.Volume
                        + ", VolumeUnitID = '" + item.VolumeUnitID
                        + "', Quantity = " + item.Quantity
                        + ", Dangerous = '" + item.Dangerous
                        + "', Description = N'" + item.Description + "' WHERE ItemID ='" + item.ItemID + "'");
                        cmd.Connection = conx;
                        val = cmd.ExecuteNonQuery();
                    }

                    if (val > 0)
                    {
                        val = updateclsItemDetailEntry(item.ItemDetail);
                    }
                }
                return val;
            }
            catch { return 0; }
        }

        public static int deleteclsItemEntryByImportExportID(string importExportID)
        {
            int val = 0;
            List<string> itemIDs = new List<string>();

            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT ItemID FROM Item WHERE ImportExportID = '" + importExportID + "'", conx);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    itemIDs.Add(reader.GetString(0));
                    val++;
                }
            }

            if (val > 0)
            {
                val = 0;
                foreach (string itemID in itemIDs)
                {
                    deleteclsItemDetailEntry(itemID); // delete associated item details
                }

                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Item WHERE ImportExportID = '" + importExportID + "'", conx);
                    val = cmd.ExecuteNonQuery();
                }
            }

            return val;
        }

        public static int deleteclsItemEntry(string ItemID)
        {
            int val = 0;
            if (deleteclsItemDetailEntry(ItemID) > 0)
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Item WHERE ItemID = '" + ItemID + "'", conx);
                    val = cmd.ExecuteNonQuery();
                }
            }
            return val;
        }
        #endregion
        #region "ItemDetail"

        public static int SaveclsItemDetailEntry(List<Item> items)
        {
            try
            {
                int val = 0;
                foreach (var item in items)
                {
                    using (SqlConnection conx = getConnectionToMarilogDB())
                    {
                        item.ItemDetail.ItemDetailID = CustomIDBuilder("ItemDetail");
                        item.ItemDetail.DateInserted = DateTime.Now;
                        item.ItemDetail.ItemNumber = item.ItemOrderNo;
                        item.ItemDetail.ItemID = item.ItemID;

                        SqlCommand cmd = new SqlCommand("INSERT INTO ItemDetail VALUES('" +
                        item.ItemDetail.ItemDetailID + "','" +
                        item.ItemDetail.ItemID + "','" +
                        item.ItemDetail.DateInserted + "','" +
                        item.ItemDetail.StuffModeID + "','" +
                        item.ItemDetail.ItemNumber + "','" +
                        item.ItemDetail.DestinationID + "','" +
                        item.ItemDetail.StatusID + "','" +
                        item.ItemDetail.StatusDate + "')");
                        cmd.Connection = conx;

                        val = cmd.ExecuteNonQuery();
                    }
                }

                return val;
            }
            catch { return 0; }
        }

        public static ItemDetail getclsItemDetailObjByItemID(string ItemID)
        {
            ItemDetail clsItemDetailObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ItemDetail WHERE ItemID = '" + ItemID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsItemDetailObj = new ItemDetail(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetDateTime(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetString(7),
                    reader.GetDateTime(8));
                }
                return clsItemDetailObj;
            }
        }

        public static ItemDetail getclsItemDetailObj(string ItemDetailID)
        {
            ItemDetail clsItemDetailObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ItemDetail WHERE ItemDetailID = '" + ItemDetailID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsItemDetailObj = new ItemDetail(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetDateTime(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetString(7),
                    reader.GetDateTime(8));
                }

                return clsItemDetailObj;
            }
        }

        public static int updateclsItemDetailEntry(ItemDetail clsItemDetailObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE ItemDetail SET ItemID = '" + clsItemDetailObj.ItemID
                + "', DateInserted = '" + clsItemDetailObj.DateInserted
                + "', StuffModeID = '" + clsItemDetailObj.StuffModeID
                + "', ItemNumber = '" + clsItemDetailObj.ItemNumber
                + "', DestinationID = '" + clsItemDetailObj.DestinationID
                + "', StatusID = '" + clsItemDetailObj.StatusID
                + "', StatusDate = '" + clsItemDetailObj.StatusDate + "' WHERE ItemDetailID ='"
                + clsItemDetailObj.ItemDetailID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsItemDetailEntry(string ItemID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM ItemDetail WHERE ItemID = '" + ItemID + "'", conx);
                int val = cmd.ExecuteNonQuery();
                return val;
            }
        }
        #endregion
        #region "Location"

        public static int SaveclsLocationEntry(Location clsLocationobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Location VALUES('" +
                clsLocationobj.Location + "','" +
                clsLocationobj.Latitude + "','" +
                clsLocationobj.Altitude + "','" +
                clsLocationobj.ImpOrExp + "','" +
                clsLocationobj.Country + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static List<Location> getAllclsLocationObj()
        {
            List<Location> locations = new List<Location>();
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Location ORDER BY Location", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Location location = new Location(reader.GetInt64(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4),
                        reader.GetString(5));

                        locations.Add(location);
                    }
                }
            }
            catch { }
            return locations;
        }

        public static Location getclsLocationObj(string ID)
        {
            Location clsLocationObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Location WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsLocationObj = new Location(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5));
                }

                return clsLocationObj;
            }
        }

        public static int updateclsLocationEntry(Location clsLocationObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE Location SET Location = '" + clsLocationObj.Location
                + "', Latitude = '" + clsLocationObj.Latitude
                + "', Altitude = '" + clsLocationObj.Altitude
                + "', ImpOrExp = '" + clsLocationObj.ImpOrExp
                + "', Country = '" + clsLocationObj.Country + "' WHERE ID ='" + clsLocationObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsLocationEntry(string ID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Location WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "Message"

        public static int SaveclsMessageEntry(Message clsMessageobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Message VALUES('" +
                clsMessageobj.DateEntered + "','" +
                clsMessageobj.Title + "','" +
                clsMessageobj.Message + "','" +
                clsMessageobj.FromPersonID + "','" +
                clsMessageobj.ToPersonID + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static Message getclsMessageObj(string ID)
        {
            Message clsMessageObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Message WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsMessageObj = new Message(reader.GetInt64(0),
                    reader.GetDateTime(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5));
                }

                return clsMessageObj;
            }
        }

        public static int updateclsMessageEntry(Message clsMessageObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE Message SET DateEntered = '" + clsMessageObj.DateEntered
                + "', Title = '" + clsMessageObj.Title
                + "', Message = '" + clsMessageObj.Message
                + "', FromPersonID = '" + clsMessageObj.FromPersonID
                + "', ToPersonID = '" + clsMessageObj.ToPersonID + "' WHERE ID ='" + clsMessageObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsMessageEntry(string ID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Message WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "ModeOfTransport"

        public static int SaveclsModeOfTransportEntry(ModeOfTransport clsModeOfTransportobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO ModeOfTransport VALUES('" +
                clsModeOfTransportobj.Mode + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static List<ModeOfTransport> getAllclsModeOfTransportObj()
        {
            List<ModeOfTransport> modeOfTransports = new List<ModeOfTransport>();

            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM ModeOfTransport", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ModeOfTransport modeOfTransport = new ModeOfTransport(reader.GetInt64(0),
                        reader.GetString(1));

                        modeOfTransports.Add(modeOfTransport);
                    }
                }
            }
            catch { }

            return modeOfTransports;
        }

        public static ModeOfTransport getclsModeOfTransportObj(string ID)
        {
            ModeOfTransport clsModeOfTransportObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ModeOfTransport WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsModeOfTransportObj = new ModeOfTransport(reader.GetInt64(0),
                    reader.GetString(1));
                }

                return clsModeOfTransportObj;
            }
        }

        public static int updateclsModeOfTransportEntry(ModeOfTransport clsModeOfTransportObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE ModeOfTransport SET Mode = '" + clsModeOfTransportObj.Mode + "' WHERE ID ='" + clsModeOfTransportObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsModeOfTransportEntry(string ID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM ModeOfTransport WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "Ports"

        public static int SaveclsPortsEntry(Port clsPortsobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Ports VALUES('" +
                clsPortsobj.PortName + "','" +
                clsPortsobj.PortCode + "','" +
                clsPortsobj.CountryID + "','" +
                clsPortsobj.ModeOfTransportID + "','" +
                clsPortsobj.IsDryPort + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static List<Port> getAllclsPortsObj()
        {
            List<Port> ports = new List<Port>();

            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Ports ORDER BY [PortName]", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Port port = new Port(reader.GetInt64(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4),
                        reader.GetBoolean(5));

                        ports.Add(port);
                    }
                }
            }
            catch { }
            return ports;
        }

        public static List<Port> getAllclsPortsObj(string countryID)
        {
            List<Port> ports = new List<Port>();

            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand($"SELECT * FROM Ports WHERE CountryID = {countryID} ORDER BY [PortName]", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Port port = new Port(reader.GetInt64(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4),
                        reader.GetBoolean(5));

                        ports.Add(port);
                    }
                }
            }
            catch { }
            return ports;
        }

        public static Port getclsPortsObj(string ID)
        {
            Port clsPortsObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Ports WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsPortsObj = new Port(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetBoolean(5));
                }

                return clsPortsObj;
            }
        }

        public static int updateclsPortsEntry(Port clsPortsObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE Ports SET PortName = '" + clsPortsObj.PortName
                + "', PortCode = '" + clsPortsObj.PortCode
                + "', CountryID = '" + clsPortsObj.CountryID
                + "', ModeOfTransportID = '" + clsPortsObj.ModeOfTransportID
                + "', IsDryPort = '" + clsPortsObj.IsDryPort + "' WHERE ID ='" + clsPortsObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsPortsEntry(string ID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Ports WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "Problem"

        public static int SaveclsProblemEntry(Problem clsProblemobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Problem VALUES('" +
                clsProblemobj.Problem + "','" +
                clsProblemobj.AlertLevel + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static List<Problem> getAllclsProblemObj()
        {
            List<Problem> problems = new List<Problem>();

            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Problem ORDER BY Problem", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Problem problem = new Problem(reader.GetInt64(0),
                        reader.GetString(1),
                        reader.GetString(2));

                        problems.Add(problem);
                    }
                }
            }
            catch { }
            return problems;
        }

        public static Problem getclsProblemObj(string ID)
        {
            Problem clsProblemObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Problem WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsProblemObj = new Problem(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2));
                }

                return clsProblemObj;
            }
        }

        public static int updateclsProblemEntry(Problem clsProblemObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE Problem SET Problem = '" + clsProblemObj.Problem
                + "', AlertLevel = '" + clsProblemObj.AlertLevel + "' WHERE ID ='" + clsProblemObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();


                return val;
            }
        }

        public static int deleteclsProblemEntry(string ID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Problem WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "ProblemUpdate"

        public static int SaveclsProblemUpdateEntry(ProblemUpdate clsProblemUpdateobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO ProblemUpdate VALUES('" +
                DateTime.Now + "','" +
                clsProblemUpdateobj.ImportExportID + "','" +
                clsProblemUpdateobj.ProblemID + "','" +
                clsProblemUpdateobj.ProblemDate + "','','" +
                clsProblemUpdateobj.IsResolved + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static List<ProblemUpdate> GetAllclsProblemUpdateObj()
        {
            List<ProblemUpdate> problemUpdates = new List<ProblemUpdate>();
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM ProblemUpdate", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ProblemUpdate problemUpdate = new ProblemUpdate(reader.GetInt64(0),
                            reader.GetDateTime(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetDateTime(4),
                            reader.GetDateTime(5),
                            reader.GetBoolean(6));

                        problemUpdates.Add(problemUpdate);
                    }
                }
            }
            catch { }
            return problemUpdates;
        }

        public static List<ProblemUpdate> GetclsProblemUpdateObj(string ImportExportID)
        {
            List<ProblemUpdate> problemUpdates = new List<ProblemUpdate>();
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ProblemUpdate WHERE ImportExportID = '"
                    + ImportExportID + "'", conx);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var problemUpdate = new ProblemUpdate(reader.GetInt64(0),
                        reader.GetDateTime(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetDateTime(4),
                        reader.GetDateTime(5),
                        reader.GetBoolean(6));

                    problemUpdates.Add(problemUpdate);
                }

                return problemUpdates;
            }
        }

        public static ProblemUpdate GetProblemUpdateById(string id)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ProblemUpdate WHERE ID = '"
                    + id + "'", conx);
                SqlDataReader reader = cmd.ExecuteReader();

                ProblemUpdate problemUpdate = null;
                while (reader.Read())
                {
                    problemUpdate = new ProblemUpdate(reader.GetInt64(0),
                        reader.GetDateTime(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetDateTime(4),
                        reader.GetDateTime(5),
                        reader.GetBoolean(6));
                }

                return problemUpdate;
            }
        }

        public static int updateclsProblemUpdateEntry(ProblemUpdate clsProblemUpdateObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE ProblemUpdate SET DateInserted = '"
                    + clsProblemUpdateObj.DateInserted
                    + "', ImportExportID = '" + clsProblemUpdateObj.ImportExportID
                    + "', ProblemID = '" + clsProblemUpdateObj.ProblemID
                    + "', ProblemDate = '" + clsProblemUpdateObj.ProblemDate
                    + "', ResolvedDate = '" + clsProblemUpdateObj.ResolvedDate
                    + "', IsResolved = '" + clsProblemUpdateObj.IsResolved
                    + "' WHERE ID ='"
                    + clsProblemUpdateObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int CloseProblemUpdateEntry(string problemID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE ProblemUpdate SET "
                    + "ResolvedDate = '" + DateTime.Now + "', IsResolved = '" + true
                    + "' WHERE ID ='" + problemID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsProblemUpdateEntry(string ImportExportID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM ProblemUpdate WHERE ImportExportID = '"
                    + ImportExportID + "'", conx);
                return cmd.ExecuteNonQuery();
            }
        }
        #endregion
        #region "Status"

        public static int SaveclsStatusEntry(Status clsStatusobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Status VALUES('" +
                clsStatusobj.Description + "','" +
                clsStatusobj.ImpExpTypeID + "','" +
                clsStatusobj.Sea + "','" +
                clsStatusobj.Air + "','" +
                clsStatusobj.Truck + "','" +
                clsStatusobj.PipeLine + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static List<Status> getAllclsStatusObj()
        {
            List<Status> statuses = new List<Status>();

            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Status ORDER BY [Description]", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Status status = new Status(reader.GetInt64(0),
                        reader.GetString(1),
                        reader.GetInt32(2).ToString(),
                        Convert.ToInt16(reader.GetBoolean(3)).ToString(),
                        Convert.ToInt16(reader.GetBoolean(4)).ToString(),
                        Convert.ToInt16(reader.GetBoolean(5)).ToString(),
                        Convert.ToInt16(reader.GetBoolean(6)).ToString());

                        statuses.Add(status);
                    }
                }
            }
            catch { }
            return statuses;
        }

        public static Status getclsStatusObj(string ID)
        {
            Status clsStatusObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Status WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsStatusObj = new Status(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6));
                }

                return clsStatusObj;
            }
        }

        public static int updateclsStatusEntry(Status clsStatusObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE Status SET Description = '" + clsStatusObj.Description
                + "', ImpExpTypeID = '" + clsStatusObj.ImpExpTypeID
                + "', Sea = '" + clsStatusObj.Sea
                + "', Air = '" + clsStatusObj.Air
                + "', Truck = '" + clsStatusObj.Truck
                + "', PipeLine = '" + clsStatusObj.PipeLine + "' WHERE ID ='" + clsStatusObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsStatusEntry(string ID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Status WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "ImportExportStatusUpdate"

        public static int SaveclsImportExportStatusUpdateEntry(List<StatusUpdate> importExportStatuses, bool updateExisting)
        {
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    int val = 0;
                    foreach (var importExportStatus in importExportStatuses)
                    {
                        if (importExportStatus.DateInserted.Year == 1) importExportStatus.DateInserted = DateTime.Now;

                        // check for unique StatusID and ImportExportID
                        SqlCommand cmd = new SqlCommand("SELECT ID FROM ImportExportStatusUpdate WHERE StatusID = '"
                            + importExportStatus.StatusID + "' AND ImportExportID = '"
                            + importExportStatus.ImportExportID + "'");
                        cmd.Connection = conx;

                        var statusIDReturned = cmd.ExecuteScalar();
                        if (statusIDReturned == null)
                        {
                            // insert if no such status exists
                            importExportStatus.ImportExportStatusUpdateID = CustomIDBuilder("ImportExportStatusUpdate");

                            cmd = new SqlCommand("INSERT INTO ImportExportStatusUpdate VALUES('" +
                            importExportStatus.ImportExportStatusUpdateID + "','" +
                            importExportStatus.DateInserted + "','" +
                            importExportStatus.ImportExportID + "','" +
                            importExportStatus.StatusID + "','" +
                            importExportStatus.StatusDate + "')");
                            cmd.Connection = conx;

                            val = cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            // update existing status
                            if (updateExisting)
                            {
                                importExportStatus.ID = Convert.ToInt64(statusIDReturned);
                                var importExportUpdate1 = new List<StatusUpdate>();
                                importExportUpdate1.Add(importExportStatus);
                                val = updateclsImportExportStatusUpdateEntry(importExportUpdate1);
                            }
                        }
                    }

                    return val;
                }
            }
            catch { return 0; }
        }

        public static List<StatusUpdate> getclsImportExportStatusUpdateObj(string ImportExportID)
        {
            List<StatusUpdate> importExportStatuses = new List<StatusUpdate>();
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ImportExportStatusUpdate WHERE ImportExportID = '"
                    + ImportExportID + "'", conx);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var importExportStatus = new StatusUpdate(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetDateTime(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetDateTime(5));

                    importExportStatuses.Add(importExportStatus);
                }
            }
            return importExportStatuses;
        }

        public static StatusUpdate getclsImportExportStatusUpdateObjByIESUID(string ImportExportStatusUpdateID)
        {
            StatusUpdate clsImportExportStatusUpdateObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ImportExportStatusUpdate WHERE ImportExportStatusUpdateID = '"
                    + ImportExportStatusUpdateID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsImportExportStatusUpdateObj = new StatusUpdate(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetDateTime(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetDateTime(5));
                }

                return clsImportExportStatusUpdateObj;
            }
        }

        public static int updateclsImportExportStatusUpdateEntry(List<StatusUpdate> importExportStatuses)
        {
            int recordsAffected = 0;
            foreach (var importExportStatus in importExportStatuses)
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("UPDATE ImportExportStatusUpdate SET DateInserted = '"
                        + importExportStatus.DateInserted
                        + "', StatusID = '" + importExportStatus.StatusID
                        + "', StatusDate = '" + importExportStatus.StatusDate
                        + "' WHERE ID ='" + importExportStatus.ID + "'");
                    cmd.Connection = conx;
                    recordsAffected += cmd.ExecuteNonQuery();
                }
            }

            if (recordsAffected == importExportStatuses.Count) return 1;

            return 0;
        }

        public static int deleteclsImportExportStatusUpdateEntryByImportExportID(string importExportID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM ImportExportStatusUpdate WHERE ImportExportID = '"
                    + importExportID + "'", conx);
                int val = cmd.ExecuteNonQuery();
                return val;
            }
        }

        public static int deleteclsImportExportStatusUpdateEntry(string ID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM ImportExportStatusUpdate WHERE ID = '" + ID + "'", conx);
                int val = cmd.ExecuteNonQuery();
                return val;
            }
        }
        #endregion
        #region "StuffMode"

        public static int SaveclsStuffModeEntry(StuffMode clsStuffModeobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO StuffMode VALUES(" +
                clsStuffModeobj.Description + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static List<StuffMode> getAllclsStuffModeObj()
        {
            List<StuffMode> stuffModes = new List<StuffMode>();
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM StuffMode", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        StuffMode stuffMode = new StuffMode(reader.GetInt64(0),
                        reader.GetString(1));

                        stuffModes.Add(stuffMode);
                    }
                }
            }
            catch { }
            return stuffModes;
        }

        public static StuffMode getclsStuffModeObj(string ID)
        {
            StuffMode clsStuffModeObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM StuffMode WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsStuffModeObj = new StuffMode(reader.GetInt64(0),
                    reader.GetString(1));
                }

                return clsStuffModeObj;
            }
        }

        public static int updateclsStuffModeEntry(StuffMode clsStuffModeObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE StuffMode SET Description = '" + clsStuffModeObj.Description + "' WHERE ID ='" + clsStuffModeObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsStuffModeEntry(string ID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM StuffMode WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "SubCargo"

        public static int SaveclsSubCargoEntry(SubCargo clsSubCargoobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO SubCargo VALUES('" +
                clsSubCargoobj.CargoID + "','" +
                clsSubCargoobj.Description + "','" +
                clsSubCargoobj.ShowIt + "','" +
                clsSubCargoobj.ImpExpTypeID + "','" +
                clsSubCargoobj.UnitID + "','" +
                clsSubCargoobj.HasDim + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static List<SubCargo> getAllclsSubCargoObj()
        {
            List<SubCargo> subCargos = new List<SubCargo>();
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM SubCargo ORDER BY [Description]", conx);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SubCargo subCargo = new SubCargo(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    Convert.ToInt32(reader.GetBoolean(6)).ToString());

                    subCargos.Add(subCargo);
                }
            }
            return subCargos;
        }

        public static SubCargo getclsSubCargoObj(string ID)
        {
            SubCargo clsSubCargoObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM SubCargo WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsSubCargoObj = new SubCargo(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6));
                }

                return clsSubCargoObj;
            }
        }

        public static int updateclsSubCargoEntry(SubCargo clsSubCargoObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE SubCargo SET CargoID = '" + clsSubCargoObj.CargoID
                + "', Description = '" + clsSubCargoObj.Description
                + "', ShowIt = '" + clsSubCargoObj.ShowIt
                + "', ImpExpTypeID = '" + clsSubCargoObj.ImpExpTypeID
                + "', UnitID = '" + clsSubCargoObj.UnitID
                + "', HasDim = '" + clsSubCargoObj.HasDim + "' WHERE ID ='" + clsSubCargoObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsSubCargoEntry(string ID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM SubCargo WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "Tarrif"

        public static int SaveclsTariffEntry(clsTariff clsTariffobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Tariff VALUES('" +
                clsTariffobj.HScode + "','" +
                clsTariffobj.Description + "','" +
                clsTariffobj.Unit + "'," +
                clsTariffobj.Duty + "," +
                clsTariffobj.Excise + "," +
                clsTariffobj.Vat + "," +
                clsTariffobj.Withholding + "," +
                clsTariffobj.Sur + ",'" +
                clsTariffobj.SecondSch1 + "','" +
                clsTariffobj.SecondSch2 + "','" +
                clsTariffobj.ExportTax + "','" +
                clsTariffobj.SpecialPermission + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static List<clsTariff> getAllclsTariffObj()
        {
            List<clsTariff> tarrifs = new List<clsTariff>();

            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Tariff", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        clsTariff tariff = new clsTariff(reader.GetInt64(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetFloat(4),
                        reader.GetFloat(5),
                        reader.GetFloat(6),
                        reader.GetFloat(7),
                        reader.GetFloat(8),
                        reader.GetString(9),
                        reader.GetString(10),
                        reader.GetString(11),
                        reader.GetString(12));

                        tarrifs.Add(tariff);
                    }
                }
            }
            catch { }
            return tarrifs;
        }

        public static clsTariff getclsTariffObj(string ID)
        {
            clsTariff clsTariffObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Tariff WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsTariffObj = new clsTariff(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetFloat(4),
                    reader.GetFloat(5),
                    reader.GetFloat(6),
                    reader.GetFloat(7),
                    reader.GetFloat(8),
                    reader.GetString(9),
                    reader.GetString(10),
                    reader.GetString(11),
                    reader.GetString(12));
                }

                return clsTariffObj;
            }
        }

        public static int updateclsTariffEntry(clsTariff clsTariffObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE Tariff SET HScode = '" + clsTariffObj.HScode
                + "', Description = '" + clsTariffObj.Description
                + "', Unit = '" + clsTariffObj.Unit
                + ", Duty = " + clsTariffObj.Duty
                + ", Excise = " + clsTariffObj.Excise
                + ", Vat = " + clsTariffObj.Vat
                + ", Withholding = " + clsTariffObj.Withholding
                + ", Sur = " + clsTariffObj.Sur
                + ", SecondSch1 = '" + clsTariffObj.SecondSch1
                + "', SecondSch2 = '" + clsTariffObj.SecondSch2
                + "', ExportTax = '" + clsTariffObj.ExportTax
                + "', SpecialPermission = '" + clsTariffObj.SpecialPermission + "' WHERE ID ='"
                + clsTariffObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsTariffEntry(string ID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Tariff WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "ImportExportReason"

        public static int SaveclsImportExportReasonEntry(ImportExportReason clsImportExportReasonobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO ImportExportReason VALUES('" +
                clsImportExportReasonobj.Reason + "','" +
                clsImportExportReasonobj.ImpExpTypeID + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static List<ImportExportReason> getAllclsImportExportReasonObj()
        {
            List<ImportExportReason> importExportReasons = new List<ImportExportReason>();

            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM ImportExportReason", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ImportExportReason importExportReason = new ImportExportReason(reader.GetInt64(0),
                        reader.GetString(1),
                        reader.GetString(2));

                        importExportReasons.Add(importExportReason);
                    }
                }
            }
            catch { }
            return importExportReasons;
        }

        public static ImportExportReason getclsImportExportReasonObj(string ID)
        {
            ImportExportReason clsImportExportReasonObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ImportExportReason WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsImportExportReasonObj = new ImportExportReason(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2));
                }

                return clsImportExportReasonObj;
            }
        }

        public static int updateclsImportExportReasonEntry(ImportExportReason clsImportExportReasonObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE ImportExportReason SET Reason = '" + clsImportExportReasonObj.Reason
                + "', ImpExpTypeID = '" + clsImportExportReasonObj.ImpExpTypeID + "' WHERE ID ='" + clsImportExportReasonObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();


                return val;
            }
        }

        public static int deleteclsImportExportReasonEntry(string ID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM ImportExportReason WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "Vessel"

        public static int SaveclsVesselEntry(Vessel clsVesselobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Vessel VALUES('" +
                clsVesselobj.Name + "','" +
                clsVesselobj.CarrierID + "','" +
                clsVesselobj.ModeOfTransportID + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();


                return val;
            }
        }

        public static List<Vessel> getAllclsVesselObj()
        {
            List<Vessel> vessels = new List<Vessel>();
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Vessel", conx);
                    cmd.Connection = conx;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Vessel vessel = new Vessel(reader.GetInt64(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3));

                        vessels.Add(vessel);
                    }
                }
            }
            catch { }

            if (vessels != null) vessels.Sort();
            return vessels;
        }

        public static Vessel getclsVesselObj(string ID)
        {
            Vessel clsVesselObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Vessel WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsVesselObj = new Vessel(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3));
                }

                return clsVesselObj;
            }
        }

        public static int updateclsVesselEntry(Vessel clsVesselObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE Vessel SET ID = '" + clsVesselObj.ID
                + "', Name = '" + clsVesselObj.Name
                + "', CarrierID = '" + clsVesselObj.CarrierID
                + "', ModeOfTransportID = '" + clsVesselObj.ModeOfTransportID + "' WHERE ID ='" + clsVesselObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsVesselEntry(string ID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Vessel WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "Units"


        public static int SaveclsUnitsEntry(Unit clsUnitsobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Units VALUES('" +
                clsUnitsobj.UnitName + "','" +
                clsUnitsobj.IsVolume + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static List<Unit> getAllclsUnitsObj()
        {
            List<Unit> units = new List<Unit>();

            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Units", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Unit unit = new Units(reader.GetInt64(0),
                        reader.GetString(1),
                        reader.GetBoolean(2));

                        units.Add(unit);
                    }
                }
            }
            catch { }
            return units;
        }

        public static Unit getclsUnitsObj(string ID)
        {
            Unit clsUnitsObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Units WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsUnitsObj = new Units(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetBoolean(2));
                }


                return clsUnitsObj;
            }
        }

        public static int updateclsUnitsEntry(Unit clsUnitsObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE Units SET ID = '" + clsUnitsObj.ID
                + "', UnitName = '" + clsUnitsObj.UnitName
                + "', IsVolume = '" + clsUnitsObj.IsVolume + "' WHERE ID ='" + clsUnitsObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsUnitsEntry(string ID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Units WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "Role"

        public static int SaveclsRoleEntry(Role clsRoleobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Role VALUES('" +
                clsRoleobj.RoleName + "','" +
                clsRoleobj.CompanyID + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();


                return val;
            }
        }

        public static Role getclsRoleObj(string RoleID)
        {
            Role clsRoleObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Role WHERE RoleID = '" + RoleID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsRoleObj = new Role(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2));
                }


                return clsRoleObj;
            }
        }

        public static int updateclsRoleEntry(Role clsRoleObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE Role SET RoleID = '" + clsRoleObj.RoleID
                + "', RoleName = '" + clsRoleObj.RoleName
                + "', CompanyID = '" + clsRoleObj.CompanyID + "' WHERE RoleID ='" + clsRoleObj.RoleID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();


                return val;
            }
        }

        public static int deleteclsRoleEntry(string RoleID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Role WHERE RoleID = '" + RoleID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "RolePermissions"

        public static int SaveclsRolePermissionsEntry(RolePermissions clsRolePermissionsobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO RolePermissions VALUES(" +
                clsRolePermissionsobj.RoleID + "','" +
                clsRolePermissionsobj.Import + "','" +
                clsRolePermissionsobj.Export + "','" +
                clsRolePermissionsobj.Save + "','" +
                clsRolePermissionsobj.Post + "','" +
                clsRolePermissionsobj.Print + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static RolePermissions getclsRolePermissionsObj(string RoleID)
        {
            RolePermissions clsRolePermissionsObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM RolePermissions WHERE RoleID = '" + RoleID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsRolePermissionsObj = new RolePermissions(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6));
                }

                return clsRolePermissionsObj;
            }
        }

        public static int updateclsRolePermissionsEntry(RolePermissions clsRolePermissionsObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE RolePermissions SET Import = '" + clsRolePermissionsObj.Import
                + "', Export = '" + clsRolePermissionsObj.Export
                + "', Save = '" + clsRolePermissionsObj.Save
                + "', Post = '" + clsRolePermissionsObj.Post
                + "', Print = '" + clsRolePermissionsObj.Print + "' WHERE RoleID ='"
                + clsRolePermissionsObj.RoleID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsRolePermissionsEntry(string RoleID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM RolePermissions WHERE RoleID = '" + RoleID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "Cargo"

        public static int SaveclsCargoEntry(Cargo clsCargoObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Cargo VALUES(" +
                clsCargoObj.Cargo + "','" +
                clsCargoObj.ShowIt + "','" +
                clsCargoObj.ImpExpTypeID + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static List<Cargo> getAllclsCargoObj()
        {
            List<Cargo> cargos = new List<Cargo>();

            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Cargo ORDER BY [Cargo]", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Cargo cargo = new Cargo(reader.GetInt64(0),
                        reader.GetString(1),
                        reader.GetBoolean(2),
                        reader.GetString(3));

                        cargos.Add(cargo);
                    }
                }
            }
            catch { }
            return cargos;
        }

        public static Cargo getclsCargoObj(string ID)
        {
            Cargo clsCargoObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Cargo WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsCargoObj = new Cargo(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetBoolean(2),
                    reader.GetString(3));
                }


                return clsCargoObj;
            }
        }

        public static int updateclsCargoEntry(Cargo clsCargoObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE Cargo SET Cargo = '" + clsCargoObj.Cargo
                + "', ShowIt = '" + clsCargoObj.ShowIt
                + "', ImpExpTypeID = '" + clsCargoObj.ImpExpTypeID + "' WHERE ID ='" + clsCargoObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsCargoEntry(string ID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Cargo WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "Carrier"

        public static int SaveclsCarrierEntry(Carrier clsCarrierobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Carrier VALUES('" +
                clsCarrierobj.Name + "','" +
                clsCarrierobj.CountryID + "','" +
                clsCarrierobj.ModeOfTransportID + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();


                return val;
            }
        }

        public static List<Carrier> getAllclsCarrierObj()
        {
            List<Carrier> carriers = new List<Carrier>();

            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Carrier", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Carrier carrier = new Carrier(reader.GetInt64(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3));

                        carriers.Add(carrier);
                    }
                }
            }
            catch (SqlException ex) { Console.Write(ex.Message); }
            return carriers;
        }

        public static Carrier getclsCarrierObj(string ID)
        {
            Carrier clsCarrierObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Carrier WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsCarrierObj = new Carrier(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3));
                }


                return clsCarrierObj;
            }
        }

        public static int updateclsCarrierEntry(Carrier clsCarrierObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE Carrier SET Name = '" + clsCarrierObj.Name
                + "', CountryID = '" + clsCarrierObj.CountryID
                + "', ModeOfTransportID = '" + clsCarrierObj.ModeOfTransportID + "' WHERE ID ='" + clsCarrierObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();


                return val;
            }
        }

        public static int deleteclsCarrierEntry(string ID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Carrier WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion
        #region "CompanyType"

        public static int SaveclsCompanyTypeEntry(CompanyType clsCompanyTypeobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO CompanyType VALUES('" +
                clsCompanyTypeobj.CompanyTypeName + "','" +
                clsCompanyTypeobj.Description + "')");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static List<CompanyType> getAllclsCompanyTypeObj()
        {
            List<CompanyType> companyTypes = new List<CompanyType>();
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM CompanyType WHERE ID NOT IN (99)", conx);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CompanyType companyType = new CompanyType(reader.GetInt64(0),
                        reader.GetString(1),
                        reader.GetString(2));

                        companyTypes.Add(companyType);
                    }
                }
            }
            catch { }
            return companyTypes;
        }

        public static CompanyType getclsCompanyTypeObj(string ID)
        {
            CompanyType clsCompanyTypeObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM CompanyType WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsCompanyTypeObj = new CompanyType(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2));
                }

                return clsCompanyTypeObj;
            }
        }

        public static int updateclsCompanyTypeEntry(CompanyType clsCompanyTypeObj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("UPDATE CompanyType Name = '" + clsCompanyTypeObj.CompanyTypeName
                + "', Description = '" + clsCompanyTypeObj.Description + "' WHERE ID ='" + clsCompanyTypeObj.ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public static int deleteclsCompanyTypeEntry(string ID)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM CompanyType WHERE ID = '" + ID + "'");
                cmd.Connection = conx;
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion

        #region"clsmappingmodel - MappingModel"
        /// <summary>
        /// Please call readyColumnMappings() method of the clsmappingmodel instance by using its SaveRecord instead of this method.
        /// </summary>
        /// <param name="clsmappingmodelobj"></param>
        /// <returns></returns>
        public int SaveclsmappingmodelEntry(MappingModel clsmappingmodelobj)
        {

            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("INSERT INTO mappingmodel VALUES('" +
            clsmappingmodelobj.mappingmodelid + "','" +
            clsmappingmodelobj.name + "','" +
            clsmappingmodelobj.columnmappings + "')");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            conx = null;
            return val;
        }

        /// <summary>
        /// returns maxid + 1 of mappingmodel table
        /// </summary>
        /// <returns></returns>
        public string getclsmappingmodelNewID()
        {
            return getMaxID("mappingmodel").ToString();
        }

        public MappingModel getclsmappingmodelObj(string mappingmodelid)
        {
            MappingModel clsmappingmodelObj = null;
            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("SELECT * FROM mappingmodel WHERE mappingmodelid = '" + mappingmodelid + "'");
            cmd.Connection = conx;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                clsmappingmodelObj = new MappingModel(reader.GetInt64(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3));
            }
            conx.Close();
            conx = null;
            return clsmappingmodelObj;
        }
        public List<MappingModel> getclsmappingmodels()
        {
            List<MappingModel> list = new List<MappingModel>();
            MappingModel clsmappingmodelObj = null;
            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("SELECT * FROM mappingmodel order by id desc");
            cmd.Connection = conx;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                clsmappingmodelObj = new MappingModel(reader.GetInt64(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3));
                list.Add(clsmappingmodelObj);
            }
            conx.Close();
            conx = null;
            return list;
        }

        public int updateclsmappingmodelEntry(MappingModel clsmappingmodelObj)
        {
            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("UPDATE mappingmodel SET name = '" + clsmappingmodelObj.name
            + "', columnmappings = '" + clsmappingmodelObj.columnmappings + "' WHERE mappingmodelid ='" +
            clsmappingmodelObj.mappingmodelid + "'");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            conx = null;
            return val;
        }

        public int deleteclsmappingmodelEntry(string mappingmodelid)
        {
            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("DELETE FROM mappingmodel WHERE mappingmodelid = '" + mappingmodelid + "'");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            return val;
        }
        #endregion

        #region "clsmappingmodelparams - MappingModelParams"
        public int SaveclsmappingmodelparamsEntry(MappingModelParams clsmappingmodelparamsobj)
        {
            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("INSERT INTO mappingmodelparams VALUES('" +
            clsmappingmodelparamsobj.tablename + "','" +
            clsmappingmodelparamsobj.columnname + "','" +
            clsmappingmodelparamsobj.op + "','" +
            clsmappingmodelparamsobj.val + "','" +
            clsmappingmodelparamsobj.lop + "','" +
            clsmappingmodelparamsobj.indexposition + "','" +
            clsmappingmodelparamsobj.mappingmodelid + "')");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            conx = null;
            return val;
        }

        public MappingModelParams getclsmappingmodelparamsObj(string id)
        {
            MappingModelParams clsmappingmodelparamsObj = null;
            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("SELECT * FROM mappingmodelparams WHERE id = '" + id + "'");
            cmd.Connection = conx;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                clsmappingmodelparamsObj = new MappingModelParams(reader.GetInt64(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4),
                reader.GetString(5),
                reader.GetString(6),
                reader.GetString(7));
            }
            conx.Close();
            conx = null;
            return clsmappingmodelparamsObj;
        }
        public List<MappingModelParams> getclsmappingmodelparams(string mappingmodelid)
        {
            List<MappingModelParams> list = new List<MappingModelParams>();
            MappingModelParams clsmappingmodelparamsObj = null;
            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("SELECT * FROM mappingmodelparams WHERE mappingmodelid = '" + mappingmodelid + "'");
            cmd.Connection = conx;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                clsmappingmodelparamsObj = new MappingModelParams(reader.GetInt64(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4),
                reader.GetString(5),
                reader.GetString(6),
                reader.GetString(7));
                list.Add(clsmappingmodelparamsObj);
            }
            conx.Close();
            conx = null;
            return list;
        }
        public int updateclsmappingmodelparamsEntry(MappingModelParams clsmappingmodelparamsObj)
        {
            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("UPDATE mappingmodelparams SET tablename = '" + clsmappingmodelparamsObj.tablename
            + "', columname = '" + clsmappingmodelparamsObj.columnname
            + "', op = '" + clsmappingmodelparamsObj.op
            + "', val = '" + clsmappingmodelparamsObj.val
            + "', lop = '" + clsmappingmodelparamsObj.lop
            + "', indexposition = '" + clsmappingmodelparamsObj.indexposition
            + "', mappingmodelid = '" + clsmappingmodelparamsObj.mappingmodelid + "' WHERE id ='" + clsmappingmodelparamsObj.id + "'");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            conx = null;
            return val;
        }

        public int deleteclsmappingmodelparamsEntry(string id)
        {
            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("DELETE FROM mappingmodelparams WHERE id = '" + id + "'");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            return val;
        }
        #endregion

        #region "User Types"
        public int SaveclsusertypesEntry(UserTypes clsusertypesobj)
        {
            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("INSERT INTO usertypes VALUES('" +
            clsusertypesobj.username + "','" +
            clsusertypesobj.usertype + "')");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            conx = null;
            return val;
        }

        public UserTypes getclsusertypesObj(string username)
        {
            UserTypes clsusertypesObj = null;
            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("SELECT * FROM usertypes WHERE username = '" + username + "'");
            cmd.Connection = conx;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                clsusertypesObj = new UserTypes(reader.GetInt64(0),
                reader.GetString(1),
                reader.GetString(2));
            }
            conx.Close();
            conx = null;
            return clsusertypesObj;
        }

        public int updateclsusertypesEntry(UserTypes clsusertypesObj)
        {
            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("UPDATE usertypes SET username = '" + clsusertypesObj.username
            + "', usertype = '" + clsusertypesObj.usertype + "' WHERE username ='" + clsusertypesObj.username + "'");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            conx = null;
            return val;
        }

        public int deleteclsusertypesEntry(string username)
        {
            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("DELETE FROM usertypes WHERE username = '" + username + "'");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            return val;
        }
        #endregion

        #region "Default Broker"
        public int SaveclsdefaultbrokerEntry(DefaultBroker clsdefaultbrokerobj)
        {
            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("INSERT INTO defaultbroker VALUES('" +
            clsdefaultbrokerobj.ownerusername + "','" +
            clsdefaultbrokerobj.defaultusername + "')");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            conx = null;
            return val;
        }

        public DefaultBroker getclsdefaultbrokerObj(string ownerUsername)
        {
            DefaultBroker clsdefaultbrokerObj = new DefaultBroker();

            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM defaultbroker WHERE ownerusername = '" + ownerUsername + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsdefaultbrokerObj = new DefaultBroker(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2));
                }

                return clsdefaultbrokerObj;
            }
        }

        public int updateclsdefaultbrokerEntry(DefaultBroker clsdefaultbrokerObj)
        {
            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("UPDATE defaultbroker SET defaultusername = '" + clsdefaultbrokerObj.defaultusername + "' WHERE ownerusername ='"
            + clsdefaultbrokerObj.ownerusername + "'");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            conx = null;
            return val;
        }

        public int deleteclsdefaultbrokerEntry(string ownerusername)
        {
            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("DELETE FROM defaultbroker WHERE ownerusername = '" + ownerusername + "'");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            return val;
        }
        #endregion

        #region "Clearables"
        public int SaveclsClearablesEntry(Clearables clsclearablesobj)
        {
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Clearables VALUES('" +
                clsclearablesobj.importexportid + "','" +
                clsclearablesobj.ownerusername + "','" +
                clsclearablesobj.customsbrokerusername + "','" +
                clsclearablesobj.cleared + "')");
                cmd.Connection = conx;

                return cmd.ExecuteNonQuery();
            }
        }

        public Clearables getclsClearablesObj(string importexportid)
        {
            Clearables clsclearablesObj = null;
            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM clearables WHERE importexportid = '" + importexportid + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsclearablesObj = new Clearables(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4));
                }

                return clsclearablesObj;
            }
        }

        public List<Clearables> getclsClearablesObjForCustomsBroker(string customsbrokerusername)
        {
            List<Clearables> list = new List<Clearables>();
            Clearables clsclearablesObj = null;

            using (SqlConnection conx = getConnectionToMarilogDB())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM clearables WHERE customsbrokerusername = '" + customsbrokerusername + "'");
                cmd.Connection = conx;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clsclearablesObj = new Clearables(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4));
                    list.Add(clsclearablesObj);
                }

                return list;
            }
        }

        public int updateclsClearablesEntry(Clearables clsclearablesObj)
        {
            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("UPDATE clearables SET customsbrokerusername = '" + clsclearablesObj.customsbrokerusername
            + "', cleared = '" + clsclearablesObj.cleared + "' WHERE importexportid ='"
            + clsclearablesObj.importexportid + "'");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            conx = null;
            return val;
        }

        public int deleteclsClearablesEntry(string importexportid)
        {
            SqlConnection conx = new SqlConnection();
            conx = getConnectionToMarilogDB();
            SqlCommand cmd = new SqlCommand("DELETE FROM clearables WHERE importexportid = '" + importexportid + "'");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            return val;
        }
        #endregion

        #region On Voyage Imports
        public DataTable OnVoyageImports()
        {
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append("SELECT X.ImportExportID");
                    sb.Append(", I.BillOfLading[BoL], I.BillOfLadingDate[BoLDate]");
                    sb.Append(", X.DateDischarge[Expected], S.StatusDate[OnVoyage]");
                    sb.Append(", T.Description, C.Cargo, SC.Description[SubCargo]");
                    sb.Append(", P.FirstName + ' ' + P.LastName[Agent]");
                    sb.Append(", P.Phone, P.Email, M.CompanyName [Company]");
                    sb.Append(" FROM ImportExport X");
                    sb.Append(" JOIN Import I ON X.ImportExportID = I.ImportID");
                    sb.Append(" JOIN Item T ON I.ImportID = T.ImportExportID");
                    sb.Append(" JOIN Cargo C ON T.CargoID = C.ID");
                    sb.Append(" JOIN SubCargo SC ON T.SubCargoID = SC.ID");
                    sb.Append(" JOIN ImportExportStatusUpdate S ON I.ImportID = S.ImportExportID");
                    sb.Append(" JOIN Status ST ON S.StatusID = ST.ID");
                    sb.Append(" JOIN Person P ON X.ConsigneeID = P.PersonID");
                    sb.Append(" JOIN Company M ON P.CompanyID = M.CompanyID");
                    sb.Append(" JOIN Clearables CL ON X.ImportExportID = CL.ImportExportID");
                    sb.Append(" WHERE S.StatusID = '14'");
                    sb.Append(" AND (SELECT ImportExportID FROM ImportExportStatusUpdate II WHERE II.StatusID = '57' AND ImportExportID = X.ImportExportID) IS NULL");

                    SqlCommand command = new SqlCommand(sb.ToString(), conx);

                    DataTable table = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(table);

                    return table;
                }
            }
            catch { return null; }
        }
        #endregion

        #region Discharged Imports
        public DataTable DischargedImports()
        {
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append("SELECT X.ImportExportID");
                    sb.Append(", I.BillOfLading[BoL], I.BillOfLadingDate[BoLDate]");
                    sb.Append(", X.DateDischarge[Expected], S.StatusDate[Discharged]");
                    sb.Append(", T.Description, C.Cargo, SC.Description[SubCargo]");
                    sb.Append(", P.FirstName + ' ' + P.LastName[Agent]");
                    sb.Append(", P.Phone, P.Email, M.CompanyName [Company]");
                    sb.Append(" FROM ImportExport X");
                    sb.Append(" JOIN Import I ON X.ImportExportID = I.ImportID");
                    sb.Append(" JOIN Item T ON I.ImportID = T.ImportExportID");
                    sb.Append(" JOIN Cargo C ON T.CargoID = C.ID");
                    sb.Append(" JOIN SubCargo SC ON T.SubCargoID = SC.ID");
                    sb.Append(" JOIN ImportExportStatusUpdate S ON I.ImportID = S.ImportExportID");
                    sb.Append(" JOIN Status ST ON S.StatusID = ST.ID");
                    sb.Append(" JOIN Person P ON X.ConsigneeID = P.PersonID");
                    sb.Append(" JOIN Company M ON P.CompanyID = M.CompanyID");
                    sb.Append(" JOIN Clearables CL ON X.ImportExportID = CL.ImportExportID");
                    sb.Append(" WHERE S.StatusID = '57'");
                    sb.Append(" AND (SELECT ImportExportID FROM ImportExportStatusUpdate II WHERE II.StatusID = '8' AND ImportExportID = X.ImportExportID) IS NULL");

                    SqlCommand command = new SqlCommand(sb.ToString(), conx);

                    DataTable table = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(table);

                    return table;
                }
            }
            catch { return null; }
        }
        #endregion

        #region Dispatched Imports
        public DataTable DispatchedImports()
        {
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append("SELECT X.ImportExportID");
                    sb.Append(", I.BillOfLading[BoL], I.BillOfLadingDate[BoLDate]");
                    sb.Append(", X.DateDischarge[Expected], S.StatusDate[Dispatched]");
                    sb.Append(", T.Description, C.Cargo, SC.Description[SubCargo]");
                    sb.Append(", P.FirstName + ' ' + P.LastName[Agent]");
                    sb.Append(", P.Phone, P.Email, M.CompanyName [Company]");
                    sb.Append(" FROM ImportExport X");
                    sb.Append(" JOIN Import I ON X.ImportExportID = I.ImportID");
                    sb.Append(" JOIN Item T ON I.ImportID = T.ImportExportID");
                    sb.Append(" JOIN Cargo C ON T.CargoID = C.ID");
                    sb.Append(" JOIN SubCargo SC ON T.SubCargoID = SC.ID");
                    sb.Append(" JOIN ImportExportStatusUpdate S ON I.ImportID = S.ImportExportID");
                    sb.Append(" JOIN Status ST ON S.StatusID = ST.ID");
                    sb.Append(" JOIN Person P ON X.ConsigneeID = P.PersonID");
                    sb.Append(" JOIN Company M ON P.CompanyID = M.CompanyID");
                    sb.Append(" JOIN Clearables CL ON X.ImportExportID = CL.ImportExportID");
                    sb.Append(" WHERE S.StatusID = '11'");
                    sb.Append(" AND (SELECT ImportExportID FROM ImportExportStatusUpdate II WHERE II.StatusID = '12' AND ImportExportID = X.ImportExportID) IS NULL");

                    SqlCommand command = new SqlCommand(sb.ToString(), conx);

                    DataTable table = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(table);

                    return table;
                }
            }
            catch { return null; }
        }
        #endregion

        #region Customs Cleared Imports
        public DataTable CustomsClearedImports()
        {
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append("SELECT X.ImportExportID");
                    sb.Append(", I.BillOfLading[BoL], I.BillOfLadingDate[BoLDate]");
                    sb.Append(", X.DateDischarge[Expected], S.StatusDate[Dispatched]");
                    sb.Append(", T.Description, C.Cargo, SC.Description[SubCargo]");
                    sb.Append(", P.FirstName + ' ' + P.LastName[Agent]");
                    sb.Append(", P.Phone, P.Email, M.CompanyName [Company]");
                    sb.Append(" FROM ImportExport X");
                    sb.Append(" JOIN Import I ON X.ImportExportID = I.ImportID");
                    sb.Append(" JOIN Item T ON I.ImportID = T.ImportExportID");
                    sb.Append(" JOIN Cargo C ON T.CargoID = C.ID");
                    sb.Append(" JOIN SubCargo SC ON T.SubCargoID = SC.ID");
                    sb.Append(" JOIN ImportExportStatusUpdate S ON I.ImportID = S.ImportExportID");
                    sb.Append(" JOIN Status ST ON S.StatusID = ST.ID");
                    sb.Append(" JOIN Person P ON X.ConsigneeID = P.PersonID");
                    sb.Append(" JOIN Company M ON P.CompanyID = M.CompanyID");
                    sb.Append(" JOIN Clearables CL ON X.ImportExportID = CL.ImportExportID");
                    sb.Append(" WHERE S.StatusID = '10'");
                    sb.Append(" AND (SELECT ImportExportID FROM ImportExportStatusUpdate II WHERE II.StatusID = '11' AND ImportExportID = X.ImportExportID) IS NULL");

                    SqlCommand command = new SqlCommand(sb.ToString(), conx);

                    DataTable table = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(table);

                    return table;
                }
            }
            catch { return null; }
        }
        #endregion

        #region ConfigStatsTime
        public DataTable GetConfigStatsTime()
        {
            try
            {
                using (SqlConnection conx = getConnectionToMarilogDB())
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM [ConfigStatsTime]", conx);

                    DataTable table = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(table);

                    return table;
                }
            }
            catch { return null; }
        }

        public int UpdateConfigStatsTime(List<ConfigStatsTime> configStatsTimes)
        {
            try
            {
                int val = 0;

                foreach (ConfigStatsTime configStatsTime in configStatsTimes)
                {
                    using (SqlConnection conx = getConnectionToMarilogDB())
                    {
                        SqlCommand command = new SqlCommand("UPDATE [ConfigStatsTime] SET "
                            + "Days = " + configStatsTime.Days
                            + "WHERE Code = '" + configStatsTime.Code + "'", conx);

                        val = command.ExecuteNonQuery();
                    }
                }

                return val;
            }
            catch { return 0; }
        }
        #endregion
    }
}

