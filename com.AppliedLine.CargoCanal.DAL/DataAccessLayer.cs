using com.AppliedLine.CargoCanal.Models;
using System;
using System.Collections.Generic;
using static System.Configuration.ConfigurationManager;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web;

namespace com.AppliedLine.CargoCanal.DAL
{
    public partial class DataAccessLayer
    {
        const int _DEMURRAGE_GRACE = 7;

        private readonly DateTime _utcNow;
        private readonly string _utcNowString;
        private readonly Encrypter encrypter;
        private readonly ByteProcessor byteHelper;
        public DataAccessLayer()
        {
            var utcNow = DateTime.UtcNow;
            _utcNowString = $"{utcNow.ToString("yyyy-MM-dd")} {GetTimeOffset(utcNow)}";
            _utcNow = System.DateTimeOffset.Parse(_utcNowString).UtcDateTime;
            encrypter = new Encrypter();
            byteHelper = new ByteProcessor();
        }

        public SqlConnection Connection
        {
            get
            {
                var con = new SqlConnection(ConnectionString);
                con.Open();

                return con;
            }
        }

        public string ConnectionString
        {
            get
            {
                return ConnectionStrings["CargoCanalConnection"].ConnectionString;
            }
        }

        /// <summary>
        /// Converts a string to capitalized text e.g. "Abel Martin"
        /// </summary>
        /// <param name="text"></param>
        /// <returns>string</returns>
        public string ToCapitalizeCase(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            text = text.Trim().ToLower();
            bool firstLetter = true;
            string output = string.Empty;

            foreach (char character in text)
            {
                if (char.IsWhiteSpace(character))
                {
                    firstLetter = true;
                    output += character;
                    continue;
                }

                if (firstLetter)
                {
                    output += character.ToString().ToUpper();
                    firstLetter = false;
                    continue;
                }

                output += character;
            }

            return output;
        }

        public string DateTimeOffsetString(DateTime date)
        {
            try
            {
                return $"{date.ToLocalTime().ToString("yyyy-MM-dd")} {GetTimeOffset(date)}";
            }
            catch
            {
                return null;
            }
        }

        public string GetTimeOffset(DateTime date)
        {
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime localTime = localZone.ToLocalTime(date);
            TimeSpan localOffset = localZone.GetUtcOffset(localTime);

            string time = localTime.ToString("HH:mm:ss.fff") + " "
                + (localOffset.Hours >= 0 && localOffset.Minutes >= 0 ? "+" : "")
                + localOffset.Hours.ToString().PadLeft(2, '0') + ":" + localOffset.Minutes.ToString().PadLeft(2, '0');

            return time;
        }

        #region Cargo
        public List<Cargo> SelectCargos()
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Cargo_Select_All", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    List<Cargo> cargos = new List<Cargo>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var cargo = new Cargo()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            CargoName = reader["CargoName"].ToString(),
                            ImpExpTypeID = Convert.ToInt64(reader["ImpExpTypeID"]),
                            IsEnabled = Convert.ToBoolean(reader["IsEnabled"])
                        };

                        cargos.Add(cargo);
                    }

                    return cargos;
                }
            }
            catch { return null; }
        }

        public Cargo SelectCargo(long id)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Cargo_Select", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    Cargo cargo = null;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cargo = new Cargo()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            CargoName = reader["CargoName"].ToString(),
                            ImpExpTypeID = Convert.ToInt64(reader["ImpExpTypeID"]),
                            IsEnabled = Convert.ToBoolean(reader["IsEnabled"])
                        };
                    }

                    return cargo;
                }
            }
            catch { return null; }
        }
        #endregion

        #region Carrier
        public List<Carrier> SelectCarriers()
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Carrier_Select_All", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    List<Carrier> carriers = new List<Carrier>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var carrier = new Carrier()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            CarrierName = reader["CarrierName"].ToString(),
                            CountryID = Convert.ToInt64(reader["CountryID"]),
                            ModeOfTransportID = Convert.ToInt64(reader["ModeOfTransportID"])
                        };

                        carriers.Add(carrier);
                    }

                    return carriers;
                }
            }
            catch { return null; }
        }

        public Carrier SelectCarrier(long id)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Carrier_Select", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    Carrier carrier = null;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        carrier = new Carrier()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            CarrierName = reader["CarrierName"].ToString(),
                            CountryID = Convert.ToInt64(reader["CountryID"]),
                            ModeOfTransportID = Convert.ToInt64(reader["ModeOfTransportID"])
                        };
                    }

                    return carrier;
                }
            }
            catch { return null; }
        }

        public bool InsertCarrier(Carrier carrier)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand()
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_Carrier_Insert",
                    Connection = con
                };

                command.Parameters.AddWithValue("@CarrierName", carrier.CarrierName);
                command.Parameters.AddWithValue("@CountryID", carrier.CountryID);
                command.Parameters.AddWithValue("@ModeOfTransportID", carrier.ModeOfTransportID);

                long value;
                if (long.TryParse(command.ExecuteScalar().ToString(), out value))
                {
                    carrier.ID = value;
                    return true;
                }

                return false;
            }
        }
        #endregion

        #region Company

        public long ValidateInsertCompany(Company company, Guid token)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Company_Transaction_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                var pID = new SqlParameter("@ID", SqlDbType.BigInt);
                var pErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 150);
                var pCompanyName = new SqlParameter("@CompanyName", company.CompanyName.Trim().ToUpper());
                var pCompanyTypeID = new SqlParameter("@CompanyTypeID", company.CompanyTypeID);
                var pContactName = new SqlParameter("@ContactName", company.ContactName);
                var pCountryID = new SqlParameter("@CountryID", company.CountryID);
                var pTIN = new SqlParameter("@TIN", company.TIN);
                var pEmailRecipient = new SqlParameter("@email_recipient", company.Email);
                var pToken = new SqlParameter("@Token", (object)token ?? DBNull.Value);

                pID.Direction = pErrorMessage.Direction = ParameterDirection.Output;
                cmd.Parameters.AddRange(new[] { pID, pErrorMessage, pCompanyName, pCompanyTypeID, pContactName, pCountryID, pTIN, pToken, pEmailRecipient });

                cmd.ExecuteNonQuery();
                if (pErrorMessage.Value.ToString() != string.Empty)
                    throw new ApplicationException(pErrorMessage.Value.ToString());

                return company.ID = Convert.ToInt64(pID.Value);
            }
        }

        public long InsertCompany(Company company)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Company_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                var pID = new SqlParameter("@ID", SqlDbType.BigInt);
                var pCompanyName = new SqlParameter("@CompanyName", company.CompanyName.ToUpper());
                var pCompanyTypeID = new SqlParameter("@CompanyTypeID", company.CompanyTypeID);
                var pContactName = new SqlParameter("@ContactName", company.ContactName);
                var pCountryID = new SqlParameter("@CountryID", company.CountryID);
                var pTIN = new SqlParameter("@TIN", company.TIN);
                var pEmailRecipient = new SqlParameter("@email_recipient", company.Email);

                pID.Direction = ParameterDirection.Output;
                cmd.Parameters.AddRange(new[] { pID, pCompanyName, pCompanyTypeID, pContactName, pCountryID, pTIN, pEmailRecipient });

                cmd.ExecuteNonQuery();

                return company.ID = Convert.ToInt64(pID.Value);
            }
        }

        private Company GetCompanyFromReader(SqlDataReader reader)
        {
            var lastRenewedDate = reader["LastRenewedDate"].ToString();
            var licIssuedDate = reader["LicenseIssuedDate"].ToString();

            return new Company()
            {
                ID = Convert.ToInt64(reader["ID"]),
                Address = reader["Address"].ToString(),
                CompanyName = reader["CompanyName"].ToString(),
                CompanyTypeID = Convert.ToInt64(reader["CompanyTypeID"]),
                ContactMobile = reader["ContactMobile"].ToString(),
                ContactName = reader["ContactName"].ToString(),
                CountryID = Convert.ToInt64(reader["CountryID"]),
                Email = reader["Email"].ToString(),
                HouseNo = reader["HouseNo"].ToString(),
                IsActive = Convert.ToBoolean(reader["IsActive"]),
                Kebele = reader["Kebele"].ToString(),
                KefleKetema = reader["KefleKetema"].ToString(),
                LastRenewedDate = string.IsNullOrEmpty(lastRenewedDate) ? null : (DateTime?)Convert.ToDateTime(reader["LastRenewedDate"]),
                LicenseIssuedDate = string.IsNullOrEmpty(licIssuedDate) ? null : (DateTime?)Convert.ToDateTime(reader["LicenseIssuedDate"]),
                LicenseNumber = reader["LicenseNumber"].ToString(),
                LicenseUnder = reader["LicenseUnder"].ToString(),
                POBox = reader["POBox"].ToString(),
                State = reader["State"].ToString(),
                Telephone = reader["Telephone"].ToString(),
                TIN = reader["TIN"].ToString(),
                TownCity = reader["TownCity"].ToString(),
                Website = reader["Website"].ToString(),
                Wereda = reader["Wereda"].ToString(),
                Photo = reader["Photo"].ToString(),
                PhotoFilename = reader["PhotoFilename"].ToString()
            };
        }

        public Company SelectCompanyById(long id)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Company_Select", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ID", id));

                Company company = null;

                var reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                while (reader.Read())
                {
                    company = GetCompanyFromReader(reader);
                }

                return company;
            }
        }

        public async Task<Company> SelectCompanyByTin(string tin)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Company_Select_ByTIN", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@TIN", tin));

                Company company = null;

                var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow);
                while (await reader.ReadAsync())
                {
                    company = GetCompanyFromReader(reader);
                }

                return company;
            }
        }

        public async Task<Company> SelectCompanyByName(string companyName)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Company_Select_ByName", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@companyName", companyName));

                Company company = null;

                var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow);
                while (await reader.ReadAsync())
                {
                    company = GetCompanyFromReader(reader);
                }

                return company;
            }
        }

        public async Task<List<Company>> SelectCompanyPreviousConsignee(string token)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Company_Select_PreviousConsignee", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@TOKEN", token));

                List<Company> companies = new List<Company>();

                var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var company = GetCompanyFromReader(reader);
                    companies.Add(company);
                }

                return companies;
            }
        }

        public async Task<int> ValidateCompanyTin(string tin)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Company_Select_ByTIN", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@tin", tin));

                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public List<Company> SelectCompaniesLikeTinOrName(string searchString)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Company_Select_LikeTinOrName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Criteria", searchString));

                List<Company> companies = new List<Company>();

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var company = GetCompanyFromReader(reader);
                    companies.Add(company);
                }

                return companies;
            }
        }

        public async Task<long> DeleteCompanyPhoto(Guid token)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Company_RemovePhoto", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                var tokenParam = new SqlParameter("@token", token);

                cmd.Parameters.AddRange(new[] { tokenParam });

                var result = await cmd.ExecuteNonQueryAsync();
                return result;
            }
        }

        public async Task<string> UpdateCompanyPhoto(Company company)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Company_UpdatePhoto", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                var pID = new SqlParameter("@companyId", company.ID);
                var pPhotoFilename = new SqlParameter("@PhotoFilename", company.PhotoFilename);
                var pPhoto = new SqlParameter("@Photo", company.Photo);//@oldfilename
                var oldfilename = new SqlParameter()
                {
                    ParameterName = "@oldfilename",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 50,
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.AddRange(new[] { pID, pPhotoFilename, pPhoto, oldfilename });

                await cmd.ExecuteNonQueryAsync();
                return oldfilename.Value.ToString();
            }
        }

        public int UpdateCompany(Company company)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Company_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddRange(new SqlParameter[] {
                    new SqlParameter("@ID", company.ID),
                    new SqlParameter("@Address", company.Address),
                    new SqlParameter("@CompanyName", company.CompanyName.ToUpper()),
                    new SqlParameter("@ContactMobile", company.ContactMobile),
                    new SqlParameter("@ContactName", company.ContactName),
                    new SqlParameter("@Email", company.Email),
                    new SqlParameter("@HouseNo", company.HouseNo),
                    new SqlParameter("@Kebele", company.Kebele),
                    new SqlParameter("@KefleKetema", company.KefleKetema),
                    new SqlParameter("@POBox", company.POBox),
                    new SqlParameter("@State", company.State),
                    new SqlParameter("@Telephone", company.Telephone),
                    new SqlParameter("@TIN", company.TIN),
                    new SqlParameter("@TownCity", company.TownCity),
                    new SqlParameter("@Website", company.Website),
                    new SqlParameter("@Wereda", company.Wereda)
                });

                return cmd.ExecuteNonQuery();
            }
        }

        public int UpdateCompanyIsActiveState(long id, bool isActiveState)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Company_Update_IsActive", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddRange(new SqlParameter[] {
                    new SqlParameter("@ID", id),
                    new SqlParameter("@IsActive", isActiveState)
                });

                return cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region CompanyType
        public List<CompanyType> SelectCompanyTypes()
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_CompanyType_Select_All", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    List<CompanyType> companyTypes = new List<CompanyType>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var companyType = new CompanyType()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString()
                        };

                        companyTypes.Add(companyType);
                    }

                    return companyTypes;
                }
            }
            catch { return null; }
        }

        public CompanyType SelectCompanyType(long id)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_CompanyType_Select", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    CompanyType companyType = null;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        companyType = new CompanyType()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString()
                        };
                    }

                    return companyType;
                }
            }
            catch { return null; }
        }
        #endregion

        #region Cost
        public long InsertCost(Cost cost)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Cost_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddRange(new[] {
                    new SqlParameter("@ID", SqlDbType.BigInt) { Direction = ParameterDirection.Output},
                    new SqlParameter("@ImportExportID", cost.ImportExportID),
                    new SqlParameter("@BaseCurrency", cost.BaseCurrency),
                    new SqlParameter("@Freight", cost.Freight),
                    new SqlParameter("@FreightCurrency", (object) cost.FreightExRate.Currency ?? DBNull.Value),
                    new SqlParameter("@FreightBaseRate", cost.FreightExRate.Rate),
                    new SqlParameter("@InlandStorage", cost.InlandStorage),
                    new SqlParameter("@InlandStorageCurrency", (object)cost.InlandStorageExRate.Currency ?? DBNull.Value),
                    new SqlParameter("@InlandStorageBaseRate", cost.InlandStorageExRate.Rate),
                    new SqlParameter("@InlandTransport", cost.InlandTransport),
                    new SqlParameter("@InlandTransportCurrency", (object)cost.InlandTransportExRate.Currency ?? DBNull.Value),
                    new SqlParameter("@InlandTransportBaseRate", cost.InlandTransportExRate.Rate),
                    new SqlParameter("@Insurance", cost.Insurance),
                    new SqlParameter("@InsuranceCurrency", (object)cost.InsuranceExRate.Currency ?? DBNull.Value),
                    new SqlParameter("@InsuranceBaseRate", cost.InsuranceExRate.Rate),
                    new SqlParameter("@AgentCommision", cost.AgentCommision),
                    new SqlParameter("@AgentCommisionCurrency", (object)cost.AgentCommisionExRate.Currency ?? DBNull.Value),
                    new SqlParameter("@AgentCommisionBaseRate", cost.AgentCommisionExRate.Rate),
                    new SqlParameter("@PortHandling", cost.PortHandling),
                    new SqlParameter("@PortHandlingCurrency", (object)cost.PortHandlingExRate.Currency ?? DBNull.Value),
                    new SqlParameter("@PortHandlingBaseRate", cost.PortHandlingExRate.Rate),
                    new SqlParameter("@DischargePortStorage", cost.DischargePortStorage),
                    new SqlParameter("@DischargePortStorageCurrency", (object)cost.DischargePortStorageExRate.Currency ?? DBNull.Value),
                    new SqlParameter("@DischargePortStorageBaseRate", cost.DischargePortStorageExRate.Rate),
                    new SqlParameter("@TruckDetention", cost.TruckDetention),
                    new SqlParameter("@TruckDetentionCurrency", (object)cost.TruckDetentionExRate.Currency ?? DBNull.Value),
                    new SqlParameter("@TruckDetentionBaseRate", cost.TruckDetentionExRate.Rate),
                    new SqlParameter("@OtherChargesDollar", cost.OtherChargesDollar),
                    new SqlParameter("@OtherChargesLocal", cost.OtherChargesLocal) });

                cmd.ExecuteNonQuery();
                return cost.ID = Convert.ToInt64(cmd.Parameters["@ID"].Value);
            }
        }

        public Cost SelectCost(long importExportID)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Cost_Select", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ImportExportID", importExportID));

                Cost cost = null;

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cost = new Cost()
                    {
                        ID = Convert.ToInt64(reader["ID"]),
                        ImportExportID = Convert.ToInt64(reader["ImportExportID"]),
                        BaseCurrency = reader["BaseCurrency"].ToString(),
                        Freight = Convert.ToDecimal(reader["Freight"]),
                        FreightExRate = new CostExRate
                        {
                            Currency = reader["FreightCurrency"].ToString(),
                            Rate = Convert.ToDecimal(reader["FreightBaseRate"])
                        },
                        InlandStorage = Convert.ToDecimal(reader["InlandStorage"]),
                        InlandStorageExRate = new CostExRate
                        {
                            Currency = reader["InlandStorageCurrency"].ToString(),
                            Rate = Convert.ToDecimal(reader["InlandStorageBaseRate"])
                        },
                        InlandTransport = Convert.ToDecimal(reader["InlandTransport"]),
                        InlandTransportExRate = new CostExRate
                        {
                            Currency = reader["InlandTransportCurrency"].ToString(),
                            Rate = Convert.ToDecimal(reader["InlandTransportBaseRate"])
                        },
                        Insurance = Convert.ToDecimal(reader["Insurance"]),
                        InsuranceExRate = new CostExRate
                        {
                            Currency = reader["InsuranceCurrency"].ToString(),
                            Rate = Convert.ToDecimal(reader["InsuranceBaseRate"])
                        },
                        AgentCommision = Convert.ToDecimal(reader["AgentCommision"]),
                        AgentCommisionExRate = new CostExRate
                        {
                            Currency = reader["AgentCommisionCurrency"].ToString(),
                            Rate = Convert.ToDecimal(reader["AgentCommisionBaseRate"])
                        },
                        PortHandling = Convert.ToDecimal(reader["PortHandling"]),
                        PortHandlingExRate = new CostExRate
                        {
                            Currency = reader["PortHandlingCurrency"].ToString(),
                            Rate = Convert.ToDecimal(reader["PortHandlingBaseRate"])
                        },
                        DischargePortStorage = Convert.ToDecimal(reader["DischargePortStorage"]),
                        DischargePortStorageExRate = new CostExRate
                        {
                            Currency = reader["DischargePortStorageCurrency"].ToString(),
                            Rate = Convert.ToDecimal(reader["DischargePortStorageBaseRate"])
                        },
                        TruckDetention = Convert.ToDecimal(reader["TruckDetention"]),
                        TruckDetentionExRate = new CostExRate
                        {
                            Currency = reader["TruckDetentionCurrency"].ToString(),
                            Rate = Convert.ToDecimal(reader["TruckDetentionBaseRate"])
                        },
                        OtherChargesDollar = Convert.ToDecimal(reader["OtherChargesDollar"]),
                        OtherChargesLocal = Convert.ToDecimal(reader["OtherChargesLocal"])
                    };
                }

                return cost;
            }
        }


        public int UpdateCost(Cost cost)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Cost_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddRange(new[] {
                    new SqlParameter("@ID", cost.ID),
                    new SqlParameter("@Freight", cost.Freight),
                    new SqlParameter("@FreightCurrency", (object) cost.FreightExRate.Currency ?? DBNull.Value),
                    new SqlParameter("@FreightBaseRate", cost.FreightExRate.Rate),
                    new SqlParameter("@InlandStorage", cost.InlandStorage),
                    new SqlParameter("@InlandStorageCurrency", (object)cost.InlandStorageExRate.Currency ?? DBNull.Value),
                    new SqlParameter("@InlandStorageBaseRate", cost.InlandStorageExRate.Rate),
                    new SqlParameter("@InlandTransport", cost.InlandTransport),
                    new SqlParameter("@InlandTransportCurrency", (object)cost.InlandTransportExRate.Currency ?? DBNull.Value),
                    new SqlParameter("@InlandTransportBaseRate", cost.InlandTransportExRate.Rate),
                    new SqlParameter("@Insurance", cost.Insurance),
                    new SqlParameter("@InsuranceCurrency", (object)cost.InsuranceExRate.Currency ?? DBNull.Value),
                    new SqlParameter("@InsuranceBaseRate", cost.InsuranceExRate.Rate),
                    new SqlParameter("@AgentCommision", cost.AgentCommision),
                    new SqlParameter("@AgentCommisionCurrency", (object)cost.AgentCommisionExRate.Currency ?? DBNull.Value),
                    new SqlParameter("@AgentCommisionBaseRate", cost.AgentCommisionExRate.Rate),
                    new SqlParameter("@PortHandling", cost.PortHandling),
                    new SqlParameter("@PortHandlingCurrency", (object)cost.PortHandlingExRate.Currency ?? DBNull.Value),
                    new SqlParameter("@PortHandlingBaseRate", cost.PortHandlingExRate.Rate),
                    new SqlParameter("@DischargePortStorage", cost.DischargePortStorage),
                    new SqlParameter("@DischargePortStorageCurrency", (object)cost.DischargePortStorageExRate.Currency ?? DBNull.Value),
                    new SqlParameter("@DischargePortStorageBaseRate", cost.DischargePortStorageExRate.Rate),
                    new SqlParameter("@TruckDetention", cost.TruckDetention),
                    new SqlParameter("@TruckDetentionCurrency", (object)cost.TruckDetentionExRate.Currency ?? DBNull.Value),
                    new SqlParameter("@TruckDetentionBaseRate", cost.TruckDetentionExRate.Rate),
                    new SqlParameter("@OtherChargesDollar", cost.OtherChargesDollar),
                    new SqlParameter("@OtherChargesLocal", cost.OtherChargesLocal) });

                return cmd.ExecuteNonQuery();
            }
        }


        #endregion

        #region Country
        public List<Country> SelectCountries()
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Country_Select_All", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    List<Country> countries = new List<Country>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var country = new Country()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            Name = reader["Name"].ToString(),
                            Region = reader["Region"].ToString()
                        };

                        countries.Add(country);
                    }

                    return countries;
                }
            }
            catch { return null; }
        }

        public Country SelectCountry(long id)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Country_Select", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    Country country = null;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        country = new Country()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            Name = reader["Name"].ToString(),
                            Region = reader["Region"].ToString()
                        };
                    }

                    return country;
                }
            }
            catch { return null; }
        }
        #endregion

        #region Document
        public async Task<long> InsertDocument(Document document)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand
                {
                    CommandText = "sp_Document_Insert",
                    CommandType = CommandType.StoredProcedure,
                    Connection = con
                };

                SqlParameter pID = new SqlParameter { ParameterName = "@ID", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Output };
                SqlParameter pDocumentName = new SqlParameter("@DocumentName", document.DocumentName);
                SqlParameter pFileData = new SqlParameter("@FileData", document.FileData);
                SqlParameter pFileExtension = new SqlParameter("@FileExtension", document.FileExtension);
                SqlParameter pFilename = new SqlParameter("@Filename", document.Filename);

                command.Parameters.AddRange(new[] { pID, pDocumentName, pFileData, pFileExtension, pFilename });

                try
                {
                    await command.ExecuteNonQueryAsync();
                    return document.ID = Convert.ToInt64(pID.Value);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public async Task<Document> SelectDocument(long id)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand
                {
                    CommandText = "sp_Document_Select",
                    CommandType = CommandType.StoredProcedure,
                    Connection = con
                };

                command.Parameters.AddWithValue("@ID", id);

                Document doc = null;
                var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow);
                while (reader.Read())
                {
                    doc = new Document
                    {
                        DocumentName = reader["DocumentName"].ToString(),
                        FileData = reader["FileData"].ToString(),
                        FileExtension = reader["FileExtension"].ToString(),
                        Filename = reader["Filename"].ToString(),
                        ID = Convert.ToInt64(reader["ID"])
                    };
                }

                return doc;
            }
        }

        public async Task<long> DeleteDocument(long id)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand
                {
                    CommandText = "sp_Document_Delete",
                    CommandType = CommandType.StoredProcedure,
                    Connection = con
                };

                command.Parameters.AddWithValue("@ID", id);
                return await command.ExecuteNonQueryAsync();
            }
        }
        #endregion

        #region Export
        public long InsertExport(Export export)
        {
            string sinDate = string.Empty;
            string eta = string.Empty;
            string cpoDt = string.Empty;
            string origDt = string.Empty;
            string wayBillDt = string.Empty;
            string decDt = string.Empty;

            if (export.ShippingInstructionDate != null)
            {
                sinDate = DateTimeOffsetString(export.ShippingInstructionDate.Value);
            }

            if (export.ETA != null)
            {
                eta = DateTimeOffsetString(export.ETA.Value);
            }

            if (export.CPORecievedDate != null)
            {
                cpoDt = DateTimeOffsetString(export.CPORecievedDate.Value);
            }

            if (export.OriginalDocumentDate != null)
            {
                origDt = DateTimeOffsetString(export.OriginalDocumentDate.Value);
            }

            if (export.DeclarationDate != null)
            {
                decDt = DateTimeOffsetString(export.DeclarationDate.Value);
            }

            if (export.WayBillDate != null)
            {
                wayBillDt = DateTimeOffsetString(export.WayBillDate.Value);
            }

            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Export_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                var pID = new SqlParameter("@ID", SqlDbType.BigInt);
                var pImportExportID = new SqlParameter("@ImportExportID", export.ImportExportID);
                var pCPORecievedDate = new SqlParameter("@CPORecievedDate", string.IsNullOrEmpty(cpoDt) ? DBNull.Value : (object)cpoDt);
                var pOriginalDocumentDate = new SqlParameter("@OriginalDocumentDate", string.IsNullOrEmpty(origDt) ? DBNull.Value : (object)origDt);
                var pWayBill = new SqlParameter("@WayBill", export.WayBill ?? string.Empty);
                var pWayBillDate = new SqlParameter("@WayBillDate", string.IsNullOrEmpty(wayBillDt) ? DBNull.Value : (object)wayBillDt);
                var pDeclarationNo = new SqlParameter("@DeclarationNo", export.DeclarationNo.ToUpper() ?? string.Empty);
                var pDeclarationDate = new SqlParameter("@DeclarationDate", string.IsNullOrEmpty(decDt) ? DBNull.Value : (object)decDt);
                var pStuffingLocationID = new SqlParameter("@StuffingLocationID", export.StuffingLocationID);
                var pOriginID = new SqlParameter("@OriginID", export.OriginID);
                var pShippingInstructionNo = new SqlParameter("@ShippingInstructionNo", export.ShippingInstructionNo ?? string.Empty);
                var pShippingInstructionDate = new SqlParameter("@ShippingInstructionDate", (string.IsNullOrEmpty(export.ShippingInstructionNo) || string.IsNullOrEmpty(sinDate)) ? DBNull.Value : (object)sinDate);
                var pETA = new SqlParameter("@ETA", string.IsNullOrEmpty(eta) ? DBNull.Value : (object)eta);
                var pATA = new SqlParameter("@ATA", (object)export.ATA ?? DBNull.Value);


                pID.Direction = ParameterDirection.Output;
                cmd.Parameters.AddRange(new[] { pID, pImportExportID, pCPORecievedDate,
                    pOriginalDocumentDate, pWayBill, pWayBillDate, pDeclarationNo,
                    pDeclarationDate, pStuffingLocationID, pOriginID,
                    pShippingInstructionNo, pShippingInstructionDate, pETA, pATA });

                cmd.ExecuteNonQuery();

                return export.ID = Convert.ToInt64(pID.Value);
            }
        }

        public Export SelectExport(long importExportID)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Export_Select", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ImportExportID", importExportID));

                Export export = null;

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    export = new Export()
                    {
                        ATA = (reader["ATA"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(reader["ATA"]),
                        ETA = (reader["ETA"] == DBNull.Value) ? null : (DateTime?)DateTimeOffset.Parse(reader["ETA"].ToString()).UtcDateTime,
                        ID = Convert.ToInt64(reader["ID"]),
                        ImportExportID = Convert.ToInt64(reader["ImportExportID"]),
                        CPORecievedDate = (reader["CPORecievedDate"] == DBNull.Value) ? null : (DateTime?)DateTimeOffset.Parse(reader["CPORecievedDate"].ToString()).UtcDateTime,
                        DeclarationDate = (reader["DeclarationDate"] == DBNull.Value) ? null : (DateTime?)DateTimeOffset.Parse(reader["DeclarationDate"].ToString()).UtcDateTime,
                        DeclarationNo = reader["DeclarationNo"].ToString(),
                        OriginalDocumentDate = (reader["OriginalDocumentDate"] == DBNull.Value) ? null : (DateTime?)DateTimeOffset.Parse(reader["OriginalDocumentDate"].ToString()).UtcDateTime,
                        OriginID = Convert.ToInt64(reader["OriginID"]),
                        ShippingInstructionDate = (reader["ShippingInstructionDate"] == DBNull.Value) ? null : (DateTime?)DateTimeOffset.Parse(reader["ShippingInstructionDate"].ToString()).UtcDateTime,
                        ShippingInstructionNo = reader["ShippingInstructionNo"].ToString(),
                        StuffingLocationID = Convert.ToInt64(reader["StuffingLocationID"]),
                        WayBill = reader["WayBill"].ToString(),
                        WayBillDate = (reader["WayBillDate"] == DBNull.Value) ? null : (DateTime?)DateTimeOffset.Parse(reader["WayBillDate"].ToString()).UtcDateTime
                    };
                }

                return export;
            }
        }

        public int UpdateExport(Export export)
        {
            string sinDate = string.Empty;
            string eta = string.Empty;
            string cpoDt = string.Empty;
            string origDt = string.Empty;
            string wayBillDt = string.Empty;
            string decDt = string.Empty;

            if (export.ShippingInstructionDate != null)
            {
                sinDate = DateTimeOffsetString(export.ShippingInstructionDate.Value);
            }

            if (export.ETA != null)
            {
                eta = DateTimeOffsetString(export.ETA.Value);
            }

            if (export.CPORecievedDate != null)
            {
                cpoDt = DateTimeOffsetString(export.CPORecievedDate.Value);
            }

            if (export.OriginalDocumentDate != null)
            {
                origDt = DateTimeOffsetString(export.OriginalDocumentDate.Value);
            }

            if (export.DeclarationDate != null)
            {
                decDt = DateTimeOffsetString(export.DeclarationDate.Value);
            }

            if (export.WayBillDate != null)
            {
                wayBillDt = DateTimeOffsetString(export.WayBillDate.Value);
            }

            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Export_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;

                var pID = new SqlParameter("@ID", export.ID);
                var pCPORecievedDate = new SqlParameter("@CPORecievedDate", string.IsNullOrEmpty(cpoDt) ? DBNull.Value : (object)cpoDt);
                var pOriginalDocumentDate = new SqlParameter("@OriginalDocumentDate", string.IsNullOrEmpty(origDt) ? DBNull.Value : (object)origDt);
                var pWayBill = new SqlParameter("@WayBill", export.WayBill ?? string.Empty);
                var pWayBillDate = new SqlParameter("@WayBillDate", string.IsNullOrEmpty(wayBillDt) ? DBNull.Value : (object)wayBillDt);
                var pDeclarationNo = new SqlParameter("@DeclarationNo", export.DeclarationNo ?? string.Empty);
                var pDeclarationDate = new SqlParameter("@DeclarationDate", string.IsNullOrEmpty(decDt) ? DBNull.Value : (object)decDt);
                var pStuffingLocationID = new SqlParameter("@StuffingLocationID", export.StuffingLocationID);
                var pOriginID = new SqlParameter("@OriginID", export.OriginID);
                var pShippingInstructionNo = new SqlParameter("@ShippingInstructionNo", export.ShippingInstructionNo ?? string.Empty);
                var pShippingInstructionDate = new SqlParameter("@ShippingInstructionDate", string.IsNullOrEmpty(export.ShippingInstructionNo) ? DBNull.Value : (object)sinDate);
                var pETA = new SqlParameter("@ETA", string.IsNullOrEmpty(eta) ? DBNull.Value : (object)eta);
                var pATA = new SqlParameter("@ATA", (object)export.ATA ?? DBNull.Value);


                cmd.Parameters.AddRange(new[] { pID, pCPORecievedDate,
                    pOriginalDocumentDate, pWayBill, pWayBillDate, pDeclarationNo,
                    pDeclarationDate, pStuffingLocationID, pOriginID,
                    pShippingInstructionNo, pShippingInstructionDate, pETA, pATA });

                return cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region IncoTerm
        public List<IncoTerm> SelectIncoTerms()
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_IncoTerms_Select_All", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    List<IncoTerm> incoTerms = new List<IncoTerm>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var incoTerm = new IncoTerm()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            IncoDescription = reader["IncoDescription"].ToString(),
                            IncoName = reader["IncoName"].ToString()
                        };

                        incoTerms.Add(incoTerm);
                    }

                    return incoTerms;
                }
            }
            catch { return null; }
        }

        public IncoTerm SelectIncoTerm(long id)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_IncoTerms_Select", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    IncoTerm incoTerm = null;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        incoTerm = new IncoTerm()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            IncoDescription = reader["IncoDescription"].ToString(),
                            IncoName = reader["IncoName"].ToString()
                        };
                    }

                    return incoTerm;
                }
            }
            catch { return null; }
        }
        #endregion

        #region ImpExpType
        public List<ImpExpType> SelectImpExpTypes()
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_ImpExpType_Select_All", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    List<ImpExpType> impExpTypes = new List<ImpExpType>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var impExpType = new ImpExpType()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            Description = reader["Description"].ToString(),
                            TypeName = reader["TypeName"].ToString()
                        };

                        impExpTypes.Add(impExpType);
                    }

                    return impExpTypes;
                }
            }
            catch { return null; }
        }
        #endregion

        #region Import
        public long InsertImport(Import import)
        {
            string billDate = DateTimeOffsetString(import.BillDate);

            string eta = string.Empty;
            if (import.ETA != null)
            {
                eta = DateTimeOffsetString(import.ETA.Value);
            }


            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Import_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                var pID = new SqlParameter("@ID", SqlDbType.BigInt);
                var pImportExportID = new SqlParameter("@ImportExportID", import.ImportExportID);
                var pBillNumber = new SqlParameter("@BillNumber", import.BillNumber);
                var pBillDate = new SqlParameter("@BillDate", billDate);
                var pETA = new SqlParameter("@ETA", string.IsNullOrEmpty(eta) ? DBNull.Value : (object)eta);
                var pATA = new SqlParameter("@ATA", (object)import.ATA ?? DBNull.Value);

                pETA.IsNullable = pATA.IsNullable = true;
                pID.Direction = ParameterDirection.Output;
                cmd.Parameters.AddRange(new[] { pID, pImportExportID, pBillNumber, pBillDate, pETA, pATA });

                cmd.ExecuteNonQuery();
                return import.ID = Convert.ToInt64(pID.Value);
            }
        }

        public Import SelectImport(long importExportID)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Import_Select", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ImportExportID", importExportID));

                Import import = null;

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    import = new Import()
                    {
                        ATA = (reader["ATA"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(reader["ATA"]),
                        BillDate = DateTimeOffset.Parse(reader["BillDate"].ToString()).UtcDateTime,
                        BillNumber = reader["BillNumber"].ToString(),
                        ETA = (reader["ETA"] == DBNull.Value) ? null : (DateTime?)DateTimeOffset.Parse(reader["ETA"].ToString()).UtcDateTime,
                        ID = Convert.ToInt64(reader["ID"]),
                        ImportExportID = Convert.ToInt64(reader["ImportExportID"])
                    };
                }

                return import;
            }
        }

        public int UpdateImport(Import import)
        {
            string billDate = DateTimeOffsetString(import.BillDate);

            string eta = string.Empty;
            if (import.ETA != null)
            {
                eta = DateTimeOffsetString(import.ETA.Value);
            }

            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Import_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;

                var pID = new SqlParameter("@ID", import.ID);
                var pBillNumber = new SqlParameter("@BillNumber", import.BillNumber);
                var pBillDate = new SqlParameter("@BillDate", billDate);
                var pETA = new SqlParameter("@ETA", string.IsNullOrEmpty(eta) ? DBNull.Value : (object)eta);
                var pATA = new SqlParameter("@ATA", import.ATA);

                cmd.Parameters.AddRange(new[] { pID, pBillNumber, pBillDate, pETA, pATA });

                return cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region ImportExport
        public DashboardImportExportSummary DashboardImportExport(Guid token, bool isImport)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand("Dashboard_Company_Import", con);

                // change the command procedure if export
                if (!isImport) command = new SqlCommand("Dashboard_Company_Export", con);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TOKEN", token);

                try
                {
                    // this would always return a row
                    var reader = command.ExecuteReader(CommandBehavior.SingleRow);
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return new DashboardImportExportSummary()
                        {
                            Completed = Convert.ToInt64(reader["Completed"]),
                            Pending = Convert.ToInt64(reader["Pending"]),
                            PercentCompleted = Convert.ToDouble(reader["PercentCompleted"]),
                            PercentPending = Convert.ToDouble(reader["PercentPending"]),
                            Total = Convert.ToInt64(reader["Total"])
                        };
                    }
                }
                catch (Exception)
                {
                    return null;
                }
                return null;
            }
        }

        public JObject DashboardShipmentsAnalytics(Guid token, int days)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand("Dashboard_ShipmentsAnalytics", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TOKEN", token);
                command.Parameters.AddWithValue("@days", days);
                command.Parameters["@days"].Direction = ParameterDirection.InputOutput;

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet ds = new DataSet("ShipmentsAnalytics");
                da.Fill(ds);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    return new JObject(
                        new JProperty("labels", new JArray(_utcNow)),
                        new JProperty("series", new JArray("Import", "Export")),
                        new JProperty("data", new JArray(new JArray(0), new JArray(0))));
                }

                // build json object
                var table = ds.Tables[0].AsEnumerable();
                //var dates = table.AsEnumerable().Select(s => s.Field<string>("DateInserted")).Distinct();

                JArray series = new JArray("Import", "Export");
                JArray labels = new JArray();
                JArray imports = new JArray();
                JArray exports = new JArray();

                foreach (var r in table)
                {
                    labels.Add(DateTimeOffset.Parse(r["DateInserted"].ToString()).UtcDateTime);
                    imports.Add(r["Import"]);
                    exports.Add(r["Export"]);
                }

                JArray data = new JArray(imports, exports);
                JObject shipmentAnalysis = new JObject(
                    new JProperty("labels", labels),
                    new JProperty("series", series),
                    new JProperty("data", data));

                return shipmentAnalysis;
            }
        }

        public JObject DashboardDemurrageAnalytics(Guid token, int days)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand("Dashboard_DemurrageAnalytics", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TOKEN", token);
                command.Parameters.AddWithValue("@DemurrageGrace", _DEMURRAGE_GRACE);
                command.Parameters.AddWithValue("@days", days);
                command.Parameters["@days"].Direction = ParameterDirection.InputOutput;

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet ds = new DataSet("DemurrageAnalytics");
                da.Fill(ds);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    return new JObject(
                        new JProperty("labels", new JArray(_utcNow)),
                        new JProperty("datasets", new JArray(new JObject(
                            new JProperty("data", 0), 
                            new JProperty("label", ""), 
                            new JProperty("status", "")))
                        ));
                }

                // build json object
                var table = ds.Tables[0].AsEnumerable();

                JArray labels = new JArray();
                foreach (var r in table)
                {
                    var cdt = DateTimeOffset.Parse(r["StatusDate"].ToString()).Date;
                    var cday = Convert.ToInt64(r["DemurrageDays"]);
                    labels.Add(cdt);
                    labels.Add(cdt.AddDays(_DEMURRAGE_GRACE + cday));
                }

                // add today to labels array
                labels.Add(DateTimeOffset.UtcNow.Date);

                var labelsDistinct = labels.OrderBy(d => d).Distinct();
                labels = new JArray();

                foreach (var dtLabel in labelsDistinct)
                {
                    labels.Add(dtLabel);
                }

                JArray datasets = new JArray();

                foreach (var r in table)
                {
                    JArray data = new JArray();
                    DateTime cdt = DateTimeOffset.Parse(r["StatusDate"].ToString()).Date;
                    long cday = Convert.ToInt64(r["DemurrageDays"]);

                    foreach (var dtLabel in labels)
                    {
                        if (cdt.AddDays(_DEMURRAGE_GRACE + cday).ToString() == dtLabel.ToString())
                        {
                            data.Add(cday);
                        }
                        else data.Add(null);
                    }

                    datasets.Add(new JObject(new JProperty("data", data),
                        new JProperty("status", r["DemurrageStatus"].ToString()),
                        new JProperty("company", r["CompanyName"].ToString()),
                        new JProperty("currentStatus", r["CurrentStatus"].ToString()),
                        new JProperty("discharged", r["StatusDate"]),
                        new JProperty("label", r["BillNumber"].ToString())
                    ));
                }


                JObject dataObj = new JObject(
                    new JProperty("labels", labels),
                    new JProperty("datasets", datasets));

                return dataObj;
            }
        }


        public DataTable DashboardTopImportCountries(Guid token)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand("DashboardTopImportCountries", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TOKEN", token);

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet ds = new DataSet("TopImportCountries");
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }

                return null;
            }
        }

        public DataTable DashboardTopExportCountries(Guid token)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand("DashboardTopExportCountries", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TOKEN", token);

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet ds = new DataSet("TopExportCountries");
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }

                return null;
            }
        }


        public async Task<long> InsertImportExport(ImportExport importExport)
        {
            string dateInitiated = DateTimeOffsetString(importExport.DateInitiated);
            importExport.DateChanged = importExport.DateInserted = _utcNow;

            // get the consignee's company id using the tin number provided
            if (!string.IsNullOrEmpty(importExport.Consignee?.TIN))
            {
                importExport.ConsigneeID = (await SelectCompanyByTin(importExport.Consignee.TIN)).ID;
            }

            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_ImportExport_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                var pID = new SqlParameter("@ID", SqlDbType.BigInt);
                var pCountryID = new SqlParameter("@CountryID", importExport.CountryID);
                var pCreatedBy = new SqlParameter("@CreatedBy", importExport.CreatedBy);
                var pReferenceNo = new SqlParameter("@ReferenceNo", importExport.ReferenceNo.ToUpper());
                var pImpExpTypeID = new SqlParameter("@ImpExpTypeID", importExport.ImpExpTypeID);
                var pImportExportReasonID = new SqlParameter("@ImportExportReasonID", importExport.ImportExportReasonID);
                var pPortOfLoadingID = new SqlParameter("@PortOfLoadingID", importExport.PortOfLoadingID);
                var pPortOfDischargeID = new SqlParameter("@PortOfDischargeID", importExport.PortOfDischargeID);
                var pModeOfTransportID = new SqlParameter("@ModeOfTransportID", importExport.ModeOfTransportID);
                var pDateInitiated = new SqlParameter("@DateInitiated", dateInitiated);
                var pConsigneeID = new SqlParameter("@ConsigneeID", importExport.ConsigneeID == 0 ? DBNull.Value : (object)importExport.ConsigneeID);
                var pCompanyID = new SqlParameter("@CompanyID", importExport.CompanyID);
                var pCarrierID = new SqlParameter("@CarrierID", importExport.CarrierID == 0 ? DBNull.Value : (object)importExport.CarrierID);
                var pVessel = new SqlParameter("@Vessel", importExport.Vessel);
                var pVoyageNumber = new SqlParameter("@VoyageNumber", importExport.VoyageNumber.ToUpper());
                var pIncoTermID = new SqlParameter("@IncoTermID", importExport.IncoTermID);
                var pRemark = new SqlParameter("@Remark", importExport.Remark.ToUpper());
                var pReImportExport = new SqlParameter("@ReImportExport", importExport.ReImportExport);
                var pUnimodal = new SqlParameter("@Unimodal", importExport.Unimodal);
                var pTerminated = new SqlParameter("@Terminated", importExport.Terminated);
                var pCompleted = new SqlParameter("@Completed", importExport.Completed);

                pID.Direction = ParameterDirection.Output;

                cmd.Parameters.AddRange(new[] {
                    pID, pCountryID, pCreatedBy, pReferenceNo, pImpExpTypeID,
                    pImportExportReasonID, pPortOfLoadingID, pPortOfDischargeID,
                    pModeOfTransportID, pDateInitiated, pConsigneeID, pCompanyID,
                    pCarrierID, pVessel, pVoyageNumber, pIncoTermID, pRemark,
                    pReImportExport, pUnimodal, pTerminated, pCompleted
                });

                cmd.ExecuteNonQuery();
                return importExport.ID = Convert.ToInt64(pID.Value);
            }
        }

        // TODO: Used during testing to load sample importExport data with Statuses
        public List<long> GetImportExportIDs()
        {
            List<long> listImportExport = new List<long>();

            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("select ID from ImportExport Where ID > 10066", con);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listImportExport.Add(reader.GetInt64(0));
                }

                return listImportExport;
            }
        }

        private ImportExport GetImportExportFromReader(SqlDataReader reader)
        {
            var importExport = new ImportExport()
            {
                ID = Convert.ToInt64(reader["ID"]),
                CarrierID = Convert.ToInt64(reader["CarrierID"] == DBNull.Value ? 0 : reader["CarrierID"]),
                ChangedBy = Convert.ToInt64(reader["ChangedBy"]),
                CreatedBy = Convert.ToInt64(reader["CreatedBy"]),
                DateChanged = DateTimeOffset.Parse(reader["DateChanged"].ToString()).UtcDateTime,
                Completed = Convert.ToBoolean(reader["Completed"]),
                ConsigneeID = Convert.ToInt64(reader["ConsigneeID"] == DBNull.Value ? 0 : reader["ConsigneeID"]),
                CountryID = Convert.ToInt64(reader["CountryID"]),
                DateInitiated = DateTimeOffset.Parse(reader["DateInitiated"].ToString()).UtcDateTime,
                DateInserted = DateTimeOffset.Parse(reader["DateInserted"].ToString()).UtcDateTime,
                CompanyID = Convert.ToInt64(reader["CompanyID"]),
                ImpExpTypeID = Convert.ToInt64(reader["ImpExpTypeID"]),
                ImportExportReasonID = Convert.ToInt64(reader["ImportExportReasonID"]),
                IncoTermID = Convert.ToInt64(reader["IncoTermID"]),
                ModeOfTransportID = Convert.ToInt64(reader["ModeOfTransportID"]),
                PortOfDischargeID = Convert.ToInt64(reader["PortOfDischargeID"]),
                PortOfLoadingID = Convert.ToInt64(reader["PortOfLoadingID"]),
                ReferenceNo = reader["ReferenceNo"].ToString(),
                ReImportExport = Convert.ToBoolean(reader["ReImportExport"]),
                Remark = reader["Remark"].ToString(),
                Terminated = Convert.ToBoolean(reader["Terminated"]),
                Unimodal = Convert.ToBoolean(reader["Unimodal"]),
                Vessel = reader["Vessel"].ToString(),
                VoyageNumber = reader["VoyageNumber"].ToString()
            };
            importExport.LC = SelectLC(importExport.ID);
            importExport.Consignee = SelectCompanyById(importExport.ConsigneeID);

            return importExport;
        }

        public void DeleteImportExport(long id)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_ImportExport_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ID", id));

                cmd.ExecuteNonQuery();
            }
        }

        public ImportExport SelectImportExport(long id)
        {
            ImportExport importExport = null;
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_ImportExport_Select", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ID", id));

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    importExport = GetImportExportFromReader(reader);
                }

                return importExport;
            }
        }

        public List<ImportExport> SelectImportExports(long companyID)
        {
            List<ImportExport> listImportExport = new List<ImportExport>();

            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_ImportExport_Select_All", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CompanyID", companyID));

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listImportExport.Add(GetImportExportFromReader(reader));
                }

                return listImportExport;
            }
        }

        public List<ImportExport> SelectImportExports(int ieTypeId, long companyID, long skip, long pageSize)
        {
            List<ImportExport> listImportExport = new List<ImportExport>();

            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_ImportExport_Select_All_PAGED", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ieTypeID", ieTypeId));
                cmd.Parameters.Add(new SqlParameter("@CompanyID", companyID));
                cmd.Parameters.Add(new SqlParameter("@skip", skip));
                cmd.Parameters.Add(new SqlParameter("@pageSize", pageSize));

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listImportExport.Add(GetImportExportFromReader(reader));
                }

                return listImportExport;
            }
        }


        public List<ImportExport> SearchImportExport(int ieTypeId, long companyID, long skip, long pageSize, SearchBill bill, bool isImport)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_ImportExportSearchByBill", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddRange(new[] {
                    new SqlParameter("@searchText", bill.SearchText ?? string.Empty),
                    new SqlParameter("@token", bill.Token),
                    new SqlParameter("@isImport", isImport),
                    new SqlParameter("@ieTypeID", ieTypeId),
                    new SqlParameter("@CompanyID", companyID),
                    new SqlParameter("@skip", skip),
                    new SqlParameter("@pageSize", pageSize)
                });

                List<ImportExport> listImportExport = new List<ImportExport>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listImportExport.Add(GetImportExportFromReader(reader));
                }

                return listImportExport;
            }
        }

        public int UpdateImportExport(ImportExport importExport)
        {
            importExport.DateChanged = _utcNow;
            string dateInitiated = DateTimeOffsetString(importExport.DateInitiated);

            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_ImportExport_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;

                var pID = new SqlParameter("@ID", importExport.ID);
                var pCountryID = new SqlParameter("@CountryID", importExport.CountryID);
                var pChangedBy = new SqlParameter("@ChangedBy", importExport.ChangedBy);
                var pReferenceNo = new SqlParameter("@ReferenceNo", importExport.ReferenceNo);
                var pImpExpTypeID = new SqlParameter("@ImpExpTypeID", importExport.ImpExpTypeID);
                var pImportExportReasonID = new SqlParameter("@ImportExportReasonID", importExport.ImportExportReasonID);
                var pPortOfLoadingID = new SqlParameter("@PortOfLoadingID", importExport.PortOfLoadingID);
                var pPortOfDischargeID = new SqlParameter("@PortOfDischargeID", importExport.PortOfDischargeID);
                var pModeOfTransportID = new SqlParameter("@ModeOfTransportID", importExport.ModeOfTransportID);
                var pDateInitiated = new SqlParameter("@DateInitiated", dateInitiated);
                var pConsigneeID = new SqlParameter("@ConsigneeID", importExport.ConsigneeID == 0 ? DBNull.Value : (object)importExport.ConsigneeID);
                var pCompanyID = new SqlParameter("@CompanyID", importExport.CompanyID);
                var pCarrierID = new SqlParameter("@CarrierID", importExport.CarrierID);
                //var pVesselID = new SqlParameter("@VesselID", importExport.Vessel);
                // auto-complete implementataion for Vessel
                var pVessel = new SqlParameter("@Vessel", importExport.Vessel);
                var pVoyageNumber = new SqlParameter("@VoyageNumber", importExport.VoyageNumber);
                var pIncoTermID = new SqlParameter("@IncoTermID", importExport.IncoTermID);
                var pRemark = new SqlParameter("@Remark", importExport.Remark);
                var pReImportExport = new SqlParameter("@ReImportExport", importExport.ReImportExport);
                var pUnimodal = new SqlParameter("@Unimodal", importExport.Unimodal);
                var pTerminated = new SqlParameter("@Terminated", importExport.Terminated);
                var pCompleted = new SqlParameter("@Completed", importExport.Completed);

                cmd.Parameters.AddRange(new[] {
                    pID, pCountryID, pReferenceNo, pChangedBy, pImpExpTypeID,
                    pImportExportReasonID, pPortOfLoadingID, pPortOfDischargeID,
                    pModeOfTransportID, pDateInitiated, pConsigneeID, pCompanyID,
                    pCarrierID, pVessel, pVoyageNumber, pIncoTermID, pRemark,
                    pReImportExport, pUnimodal, pTerminated, pCompleted
                });

                return cmd.ExecuteNonQuery();
            }
        }

        public async Task<string> UpdateImportExportCompleted(long importExportId)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_ImportExport_Completed", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                var pID = new SqlParameter("@importExportId", importExportId);
                var pError = new SqlParameter { ParameterName = "@error", SqlDbType = SqlDbType.NVarChar, Size = 50, Direction = ParameterDirection.Output };

                cmd.Parameters.AddRange(new[] { pID, pError });
                await cmd.ExecuteNonQueryAsync();

                var result = pError.Value.ToString();
                return result;
            }
        }

        public async Task<string> UpdateImportExportTerminated(CallParams args)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_ImportExport_Terminated", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                var pID = new SqlParameter("@importExportId", args.ID);
                var pToken = new SqlParameter("@token", args.Token);
                var pError = new SqlParameter { ParameterName = "@error", SqlDbType = SqlDbType.NVarChar, Size = 50, Direction = ParameterDirection.Output };

                cmd.Parameters.AddRange(new[] { pID, pToken, pError });
                await cmd.ExecuteNonQueryAsync();

                var result = pError.Value.ToString();
                return result;
            }
        }

        public long UpdateConsigneeTIN(ConsigneeImportExportWithTin consigneeImportExport)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand("sp_ImportExport_Update_Consignee", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@ImportExportID", consigneeImportExport.ImportExportID));
                command.Parameters.Add(new SqlParameter("@TIN", consigneeImportExport.TIN));

                var r = command.ExecuteScalar();
                long consigneeId;
                long.TryParse(r.ToString(), out consigneeId);
                return consigneeId;
            }
        }

        // this method is used by national bank and consignee
        public List<ImportExport> SelectImportExportsByTinOrLc(long companyID, long skip, long pageSize, SearchBill search)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand("sp_ImportExport_Select_TIN_LC", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@searchText", search.SearchText ?? string.Empty));
                command.Parameters.Add(new SqlParameter("@token", search.Token));
                command.Parameters.Add(new SqlParameter("@companyID", companyID));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@pageSize", pageSize));

                List<ImportExport> importExports = new List<ImportExport>();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    importExports.Add(GetImportExportFromReader(reader));
                }

                return importExports;
            }
        }

        #endregion

        #region ImportExportDoc

        public async Task<long> InsertImportExportDoc(ImportExportDoc doc)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand
                {
                    CommandText = "sp_ImportExportDoc_Insert",
                    CommandType = CommandType.StoredProcedure,
                    Connection = con
                };

                var pID = new SqlParameter { ParameterName = "@ID", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.BigInt };
                var pDocumentID = new SqlParameter { ParameterName = "@DocumentID", Value = doc.DocumentID };
                var pImportExportID = new SqlParameter { ParameterName = "@ImportExportID", Value = doc.ImportExportID };
                var pRequired = new SqlParameter { ParameterName = "@Required", Value = doc.Required };
                var pIsPublic = new SqlParameter { ParameterName = "@IsPublic", Value = doc.IsPublic };


                command.Parameters.AddRange(new[] { pID, pDocumentID, pImportExportID, pRequired, pIsPublic });
                await command.ExecuteNonQueryAsync();

                return doc.ID = Convert.ToInt64(pID.Value);
            }
        }


        public async Task<List<Document>> SelectImportExportDocs(long importExportId)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand
                {
                    CommandText = "sp_ImportExportDoc_Select",
                    CommandType = CommandType.StoredProcedure,
                    Connection = con
                };

                command.Parameters.AddWithValue("@importExportId", importExportId);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                List<Document> docs = new List<Document>();

                while (reader.Read())
                {
                    docs.Add(new Document
                    {
                        DocumentName = reader["DocumentName"].ToString(),
                        FileData = reader["FileData"].ToString(),
                        FileExtension = reader["FileExtension"].ToString(),
                        Filename = reader["Filename"].ToString(),
                        ID = Convert.ToInt64(reader["ID"])
                    });
                }

                return docs;
            }
        }
        #endregion

        #region ImportExportReason
        public List<ImportExportReason> SelectImportExportReasons()
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_ImportExportReason_Select_All", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    List<ImportExportReason> importExportReasons = new List<ImportExportReason>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var reason = new ImportExportReason()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            Reason = reader["Reason"].ToString()
                        };

                        importExportReasons.Add(reason);
                    }

                    return importExportReasons;
                }
            }
            catch { return null; }
        }
        #endregion

        #region Item

        public long InsertItem(Item item)
        {
            item.DateInserted = _utcNow;

            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Item_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    var pID = new SqlParameter("@ID", SqlDbType.BigInt);
                    var pCargoID = new SqlParameter("@CargoID", item.CargoID);
                    var pDangerous = new SqlParameter("@Dangerous", item.Dangerous);
                    var pDescription = new SqlParameter("@Description", item.Description.ToUpper());
                    var pGrossWeight = new SqlParameter("@GrossWeight", item.GrossWeight);
                    var pImportExportID = new SqlParameter("@ImportExportID", item.ImportExportID);
                    var pItemOrderNo = new SqlParameter("@ItemOrderNo", item.ItemOrderNo ?? string.Empty);
                    var pNetWeight = new SqlParameter("@NetWeight", item.NetWeight);
                    var pQuantity = new SqlParameter("@Quantity", item.Quantity);
                    var pSubCargoID = new SqlParameter("@SubCargoID", item.SubCargoID);
                    var pVolume = new SqlParameter("@Volume", item.Volume);
                    var pVolumeUnitID = new SqlParameter("@VolumeUnitID", item.VolumeUnitID);
                    var pWeightUnitID = new SqlParameter("@WeightUnitID", item.WeightUnitID);

                    pID.Direction = ParameterDirection.Output;

                    cmd.Parameters.AddRange(new[] { pID, pCargoID, pDangerous,
                        pDescription, pGrossWeight, pImportExportID, pItemOrderNo, pNetWeight,
                        pQuantity, pSubCargoID, pVolume, pVolumeUnitID, pWeightUnitID });

                    cmd.ExecuteNonQuery();

                    return item.ID = Convert.ToInt64(pID.Value);
                }
            }
            catch { return 0; }
        }

        public List<Item> SelectItems(long importExportID)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Item_Select", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ImportExportID", importExportID));

                    List<Item> items = new List<Item>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var item = new Item()
                        {
                            CargoID = Convert.ToInt64(reader["CargoID"]),
                            Dangerous = Convert.ToBoolean(reader["Dangerous"]),
                            DateInserted = System.DateTimeOffset.Parse(reader["DateInserted"].ToString()).UtcDateTime,
                            Description = reader["Description"].ToString(),
                            GrossWeight = Convert.ToDecimal(reader["GrossWeight"]),
                            ID = Convert.ToInt64(reader["ID"]),
                            ImportExportID = Convert.ToInt64(reader["ImportExportID"]),
                            ItemOrderNo = reader["ItemOrderNo"].ToString(),
                            NetWeight = Convert.ToDecimal(reader["NetWeight"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            SubCargoID = Convert.ToInt64(reader["SubCargoID"]),
                            Volume = Convert.ToDecimal(reader["Volume"]),
                            VolumeUnitID = Convert.ToInt64(reader["VolumeUnitID"] == DBNull.Value ? "0" : reader["VolumeUnitID"].ToString()),
                            WeightUnitID = Convert.ToInt64(reader["WeightUnitID"] == DBNull.Value ? "0" : reader["WeightUnitID"].ToString())
                        };

                        items.Add(item);
                    }

                    return items;
                }
            }
            catch { return null; }
        }

        public int UpdateItem(Item item)
        {
            item.DateInserted = _utcNow;

            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Item_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    var pID = new SqlParameter("@ID", item.ID);
                    var pCargoID = new SqlParameter("@CargoID", item.CargoID);
                    var pDangerous = new SqlParameter("@Dangerous", item.Dangerous);
                    var pDescription = new SqlParameter("@Description", item.Description);
                    var pGrossWeight = new SqlParameter("@GrossWeight", item.GrossWeight);
                    var pItemOrderNo = new SqlParameter("@ItemOrderNo", item.ItemOrderNo);
                    var pNetWeight = new SqlParameter("@NetWeight", item.NetWeight);
                    var pQuantity = new SqlParameter("@Quantity", item.Quantity);
                    var pSubCargoID = new SqlParameter("@SubCargoID", item.SubCargoID);
                    var pVolume = new SqlParameter("@Volume", item.Volume);
                    var pVolumeUnitID = new SqlParameter("@VolumeUnitID", item.VolumeUnitID);
                    var pWeightUnitID = new SqlParameter("@WeightUnitID", item.WeightUnitID);

                    cmd.Parameters.AddRange(new[] { pID, pCargoID, pDangerous,
                        pDescription, pGrossWeight, pItemOrderNo, pNetWeight,
                        pQuantity, pSubCargoID, pVolume, pVolumeUnitID, pWeightUnitID });

                    return cmd.ExecuteNonQuery();
                }
            }
            catch { return 0; }
        }

        #endregion

        #region ItemDetail

        public long InsertItemDetail(ItemDetail itemDetail)
        {
            itemDetail.DateInserted = _utcNow;

            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_ItemDetail_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                var pID = new SqlParameter("@ID", SqlDbType.BigInt);
                var pDestinationID = new SqlParameter("@DestinationID", (object)itemDetail.DestinationID ?? DBNull.Value);
                var pItemID = new SqlParameter("@ItemID", itemDetail.ItemID);
                var pItemNumber = new SqlParameter("@ItemNumber", itemDetail.ItemNumber.ToUpper() ?? string.Empty);
                var pStuffModeID = new SqlParameter("@StuffModeID", itemDetail.StuffModeID);

                pID.Direction = ParameterDirection.Output;
                cmd.Parameters.AddRange(new[] { pID, pDestinationID, pItemID, pItemNumber, pStuffModeID });

                cmd.ExecuteNonQuery();
                return itemDetail.ID = Convert.ToInt64(pID.Value);
            }
        }

        public List<ItemDetail> SelectItemDetail(long itemID)
        {
            try
            {
                List<ItemDetail> itemDetails = new List<ItemDetail>();

                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_ItemDetail_Select", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ItemID", itemID));

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        itemDetails.Add(new ItemDetail()
                        {
                            DateInserted = DateTimeOffset.Parse(reader["DateInserted"].ToString()).UtcDateTime,
                            DestinationID = (reader["DestinationID"] == DBNull.Value) ? null : (long?)Convert.ToInt64(reader["DestinationID"]),
                            ID = Convert.ToInt64(reader["ID"]),
                            ItemID = Convert.ToInt64(reader["ItemID"]),
                            ItemNumber = reader["ItemNumber"].ToString(),
                            StuffModeID = Convert.ToInt64(reader["StuffModeID"])
                        });
                    }

                    return itemDetails;
                }
            }
            catch { return null; }
        }

        public int UpdateItemDetail(ItemDetail itemDetail)
        {
            itemDetail.DateInserted = _utcNow;

            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_ItemDetail_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    var pID = new SqlParameter("@ID", itemDetail.ID);
                    var pDestinationID = new SqlParameter("@DestinationID", (object)itemDetail.DestinationID ?? DBNull.Value);
                    var pItemNumber = new SqlParameter("@ItemNumber", itemDetail.ItemNumber);
                    var pStuffModeID = new SqlParameter("@StuffModeID", itemDetail.StuffModeID);

                    cmd.Parameters.AddRange(new[] { pID, pDestinationID, pItemNumber, pStuffModeID });

                    return cmd.ExecuteNonQuery();
                }
            }
            catch { return 0; }
        }

        #endregion

        #region LC
        public long InsertLC(LC lc)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_LC_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                var pID = new SqlParameter("@ID", SqlDbType.BigInt);
                var pImportExportID = new SqlParameter("@ImportExportID", lc.ImportExportID);
                var pLCNumber = new SqlParameter("@LCNumber", lc.LCNumber.ToUpper());

                pID.Direction = ParameterDirection.Output;

                cmd.Parameters.AddRange(new[] { pID, pImportExportID, pLCNumber });
                cmd.ExecuteNonQuery();

                return lc.ID = Convert.ToInt64(pID.Value);
            }
        }

        public LC SelectLC(long importExportID)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_LC_Select_By_ImportExportID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ImportExportID", importExportID));

                    LC lc = null;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lc = new LC()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            ImportExportID = Convert.ToInt64(reader["ImportExportID"]),
                            LCNumber = reader["LCNumber"].ToString()
                        };
                    }

                    return lc;
                }
            }
            catch { return null; }
        }

        public List<LC> SelectLCsByLCNumber(string lcNumber)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_LC_Select_By_LCNumber", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@LCNumber", lcNumber));

                    List<LC> lcs = new List<LC>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var lc = new LC()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            ImportExportID = Convert.ToInt64(reader["ImportExportID"]),
                            LCNumber = reader["LCNumber"].ToString()
                        };

                        lcs.Add(lc);
                    }

                    return lcs;
                }
            }
            catch { return null; }
        }

        public int UpdateLC(LC lc)
        {
            if (lc == null) return 0;
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_LC_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ID", lc.ID));
                cmd.Parameters.Add(new SqlParameter("@LCNumber", lc.LCNumber.ToUpper()));

                return cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region Location
        public List<Location> SelectLocations(long countryID)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Location_Select_ByCountry", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CountryID", countryID));

                    List<Location> locations = new List<Location>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var location = new Location()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            CountryID = Convert.ToInt64(reader["CountryID"]),
                            LocationName = reader["LocationName"].ToString()
                        };

                        locations.Add(location);
                    }

                    return locations;
                }
            }
            catch { return null; }
        }

        public Location SelectLocation(long id)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Location_Select", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    Location location = null;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        location = new Location()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            CountryID = Convert.ToInt64(reader["CountryID"]),
                            LocationName = reader["LocationName"].ToString()
                        };
                    }

                    return location;
                }
            }
            catch { return null; }
        }

        public bool InsertLocation(Location location)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand()
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_Location_Insert",
                    Connection = con
                };

                command.Parameters.AddWithValue("@LocationName", location.LocationName);
                command.Parameters.AddWithValue("@CountryID", location.CountryID);

                long value;
                if (long.TryParse(command.ExecuteScalar().ToString(), out value))
                {
                    location.ID = value;
                    return true;
                }

                return false;
            }
        }
        #endregion

        #region Login
        public string SelectMobileApiKey(long id)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_LoginMobile_Select", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LoginId", id);

                return cmd.ExecuteScalar().ToString();
            }
        }

        public bool ValidateMobileApiKey(long loginId, Guid appKey)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand()
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_LoginMobile_Validate",
                    Connection = con
                };
                cmd.Parameters.AddWithValue("@LoginId", loginId);
                cmd.Parameters.AddWithValue("@AppKey", appKey);

                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        public int InsertLogin(Login login)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Login_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                var pPersonID = new SqlParameter("@PersonID", login.PersonID);
                var pUsername = new SqlParameter("@Username", login.Username.ToLower());
                var pPassword = new SqlParameter("@Password", encrypter.ComputeMD5Hash(login.Password));
                var pRoleID = new SqlParameter("@RoleID", login.RoleID);
                var pIsActive = new SqlParameter("@IsActive", login.IsActive);

                cmd.Parameters.AddRange(new SqlParameter[] { pPersonID, pUsername, pPassword, pRoleID, pIsActive });
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public Login SelectLogin(string username, string password)
        {
            Login login = null;

            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Login_Select", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Username", username));
                cmd.Parameters.Add(new SqlParameter("@PasswordHash", encrypter.ComputeMD5Hash(password)));

                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                while (reader.Read())
                {
                    login = new Login()
                    {
                        ID = Convert.ToInt64(reader["ID"]),
                        IsActive = Convert.ToBoolean(reader["IsActive"]),
                        PersonID = Convert.ToInt64(reader["PersonID"]),
                        RoleID = Convert.ToInt64(reader["RoleID"]),
                        Username = reader["Username"].ToString(),
                        Token = Guid.Parse(reader["Token"].ToString()),
                        LastSeen = null,
                        LastPasswordChange = null
                    };

                    DateTimeOffset lastSeen;
                    if (System.DateTimeOffset.TryParse(reader["LastSeen"].ToString(), out lastSeen))
                        login.LastSeen = lastSeen.UtcDateTime;

                    DateTimeOffset lastPasswordChange;
                    if (System.DateTimeOffset.TryParse(reader["LastPasswordChange"].ToString(), out lastPasswordChange))
                        login.LastPasswordChange = lastPasswordChange.UtcDateTime;
                }

                return login;
            }
        }

        public async Task<bool> SelectLoginEmailExists(string username)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Login_EmailExist", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Username", username));
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Result",
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Output
                });

                await cmd.ExecuteNonQueryAsync();
                return Convert.ToBoolean(cmd.Parameters["@Result"].Value);
            }
        }

        public int DeleteLogin(long id)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Login_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ID", id));
                return cmd.ExecuteNonQuery();
            }
        }

        public int ChangePassword(ChangePasswordView changePassword)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_ChangePassword", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddRange(new SqlParameter[] {
                    new SqlParameter("@ID", changePassword.ID),
                    new SqlParameter("@Token", changePassword.Token),
                    new SqlParameter("@OldPasswordHash", encrypter.ComputeMD5Hash(changePassword.OldPassword)),
                    new SqlParameter("@PasswordHash", encrypter.ComputeMD5Hash(changePassword.Password))
                });

                return cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region ModeOfTransport
        public List<ModeOfTransport> SelectModeOfTransports()
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_ModeOfTransport_Select_All", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    List<ModeOfTransport> mots = new List<ModeOfTransport>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var mot = new ModeOfTransport()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            Mode = reader["Mode"].ToString()
                        };

                        mots.Add(mot);
                    }

                    return mots;
                }
            }
            catch { return null; }
        }

        public ModeOfTransport SelectModeOfTransport(long id)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_ModeOfTransport_Select", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    ModeOfTransport mot = null;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        mot = new ModeOfTransport()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            Mode = reader["Mode"].ToString()
                        };
                    }

                    return mot;
                }
            }
            catch { return null; }
        }
        #endregion

        #region PasswordReset
        public bool PasswordResetCheck(Guid uid, string usalt)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_PasswordResetCheck", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddRange(new SqlParameter[] {
                    new SqlParameter("@id", uid),
                    new SqlParameter("@usernameSalt", byteHelper.TransformVarbinary(usalt))
                });

                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        public bool PasswordResetChange(PasswordResetChange passwordResetChange)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_PasswordResetChange", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddRange(new SqlParameter[] {
                    new SqlParameter("@id", passwordResetChange.Uid),
                    new SqlParameter("@usernameSalt", byteHelper.TransformVarbinary(passwordResetChange.Usalt)),
                    new SqlParameter("@passwordHash", encrypter.ComputeMD5Hash(passwordResetChange.Password))
                });

                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        public void ResetPassword(PasswordResetRequest resetRequest)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_PasswordResetInsert", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddRange(new SqlParameter[] {
                    new SqlParameter("@email", resetRequest.Username),
                    new SqlParameter("@referrer", resetRequest.Referrer),
                    new SqlParameter("@uri", resetRequest.Url)
                });

                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Person
        public long InsertPerson(Person person)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Person_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                var pID = new SqlParameter("@ID", SqlDbType.BigInt);
                var pFirstName = new SqlParameter("@Firstname", ToCapitalizeCase(person.FirstName));
                var pMiddleName = new SqlParameter("@Middlename", ToCapitalizeCase(person.MiddleName) ?? string.Empty);
                var pLastName = new SqlParameter("@Lastname", ToCapitalizeCase(person.LastName));
                var pCompanyID = new SqlParameter("@CompanyID", person.CompanyID);
                var pPhone = new SqlParameter("@Phone", person.Phone ?? string.Empty);
                var pEmail = new SqlParameter("@Email", person.Email.ToLower());
                var pPhotoFilename = new SqlParameter("@PhotoFilename", person.PhotoFilename ?? string.Empty);
                var pPhoto = new SqlParameter("@Photo", person.Photo ?? string.Empty);

                pID.Direction = ParameterDirection.Output;
                cmd.Parameters.AddRange(new[] { pID, pFirstName, pMiddleName, pLastName, pCompanyID, pPhone, pEmail, pPhotoFilename, pPhoto });


                cmd.ExecuteNonQuery();
                return person.ID = Convert.ToInt64(pID.Value);
            }
        }

        public Person SelectPerson(long id)
        {
            Person person = null;
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Person_Select", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ID", id));

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    person = new Person()
                    {
                        CompanyID = Convert.ToInt64(reader["CompanyID"]),
                        Email = reader["Email"].ToString(),
                        FirstName = reader["FirstName"].ToString(),
                        ID = Convert.ToInt64(reader["ID"]),
                        LastName = reader["LastName"].ToString(),
                        MiddleName = reader["MiddleName"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Photo = reader["Photo"].ToString(),
                        PhotoFilename = reader["PhotoFilename"].ToString()
                    };
                }

                return person;
            }
        }

        public int UpdatePerson(Person person)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Person_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;

                var pID = new SqlParameter("@ID", person.ID);
                var pFirstName = new SqlParameter("@Firstname", ToCapitalizeCase(person.FirstName));
                var pMiddleName = new SqlParameter("@Middlename", ToCapitalizeCase(person.MiddleName));
                var pLastName = new SqlParameter("@Lastname", ToCapitalizeCase(person.LastName));
                var pPhone = new SqlParameter("@Phone", person.Phone);
                var pEmail = new SqlParameter("@Email", person.Email.ToLower());

                cmd.Parameters.AddRange(new[] { pID, pFirstName, pMiddleName, pLastName, pPhone, pEmail });

                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        public async Task<string> UpdatePersonPhoto(Person person)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Person_UpdatePhoto", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                var pID = new SqlParameter("@personId", person.ID);
                var pPhotoFilename = new SqlParameter("@PhotoFilename", person.PhotoFilename);
                var pPhoto = new SqlParameter("@Photo", person.Photo);
                var oldfilename = new SqlParameter()
                {
                    ParameterName = "@oldfilename",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 50,
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.AddRange(new[] { pID, pPhotoFilename, pPhoto, oldfilename });

                await cmd.ExecuteNonQueryAsync();
                return oldfilename.Value.ToString();
            }
        }

        public async Task<long> DeletePersonPhoto(Guid token)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Person_RemovePhoto", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                var tokenParam = new SqlParameter("@token", token);

                cmd.Parameters.AddRange(new[] { tokenParam });

                var result = await cmd.ExecuteNonQueryAsync();
                return result;
            }
        }

        public int DeletePerson(long id)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Person_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ID", id));
                return cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Port
        public List<Port> SelectPorts(long countryID)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Ports_Select_ByCountry", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CountryID", countryID));

                    List<Port> ports = new List<Port>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Port port = new Port()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            CountryID = Convert.ToInt64(reader["CountryID"]),
                            IsDryPort = Convert.ToBoolean(reader["IsDryPort"]),
                            ModeOfTransportID = Convert.ToInt64(reader["ModeOfTransportID"]),
                            PortCode = reader["ID"].ToString(),
                            PortName = reader["PortName"].ToString()
                        };

                        ports.Add(port);
                    }

                    return ports;
                }
            }
            catch { return null; }
        }

        public Port SelectPort(long id)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Ports_Select", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    Port port = null;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        port = new Port()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            CountryID = Convert.ToInt64(reader["CountryID"]),
                            IsDryPort = Convert.ToBoolean(reader["IsDryPort"]),
                            ModeOfTransportID = Convert.ToInt64(reader["ModeOfTransportID"]),
                            PortCode = reader["ID"].ToString(),
                            PortName = reader["PortName"].ToString()
                        };
                    }

                    return port;
                }
            }
            catch { return null; }
        }

        public bool InsertPort(Port port)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand()
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_Ports_Insert",
                    Connection = con
                };

                command.Parameters.AddWithValue("@PortName", port.PortName);
                command.Parameters.AddWithValue("@PortCode", port.PortCode ?? string.Empty);
                command.Parameters.AddWithValue("@CountryID", port.CountryID);
                command.Parameters.AddWithValue("@ModeOfTransportID", port.ModeOfTransportID);
                command.Parameters.AddWithValue("@IsDryPort", port.IsDryPort);

                long value;
                if (long.TryParse(command.ExecuteScalar().ToString(), out value))
                {
                    port.ID = value;
                    return true;
                }

                return false;
            }
        }
        #endregion

        #region Problem
        public List<Problem> SelectProblems()
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Problem_Select_All", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    List<Problem> problems = new List<Problem>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Problem problem = new Problem()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            AlertLevel = Convert.ToInt32(reader["AlertLevel"]),
                            ProblemName = reader["ProblemName"].ToString()
                        };

                        problems.Add(problem);
                    }

                    return problems;
                }
            }
            catch { return null; }
        }

        public Problem SelectProblem(long id)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Problem_Select", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    Problem problem = null;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        problem = new Problem()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            AlertLevel = Convert.ToInt32(reader["AlertLevel"]),
                            ProblemName = reader["ProblemName"].ToString()
                        };
                    }

                    return problem;
                }
            }
            catch { return null; }
        }
        #endregion

        #region ProblemUpdate
        public long InsertProblemUpdate(ProblemUpdate problemUpdate)
        {
            problemUpdate.DateInserted = _utcNow;
            string problemDate = DateTimeOffsetString(problemUpdate.ProblemDate);

            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_ProblemUpdate_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                var pID = new SqlParameter("@ID", SqlDbType.BigInt);
                var pImportExportID = new SqlParameter("@ImportExportID", problemUpdate.ImportExportID);
                var pProblemDate = new SqlParameter("@ProblemDate", problemDate);
                var pProblemID = new SqlParameter("@ProblemID", problemUpdate.ProblemID);

                pID.Direction = ParameterDirection.Output;

                cmd.Parameters.AddRange(new[] { pID, pImportExportID, pProblemDate, pProblemID });
                cmd.ExecuteNonQuery();

                return problemUpdate.ID = Convert.ToInt64(pID.Value);
            }
        }

        public ProblemUpdate SelectProblemUpdate(long id)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_ProblemUpdate_Select", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    ProblemUpdate problemUpdate = null;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        problemUpdate = new ProblemUpdate()
                        {
                            DateInserted = DateTimeOffset.Parse(reader["DateInserted"].ToString()).UtcDateTime,
                            ID = Convert.ToInt64(reader["ID"]),
                            ImportExportID = Convert.ToInt64(reader["ImportExportID"]),
                            IsResolved = Convert.ToBoolean(reader["IsResolved"]),
                            ProblemDate = DateTimeOffset.Parse(reader["ProblemDate"].ToString()).UtcDateTime,
                            ProblemID = Convert.ToInt64(reader["ProblemID"]),
                            ResolvedDate = (reader["ResolvedDate"] == DBNull.Value) ? null : (DateTime?)DateTimeOffset.Parse(reader["ResolvedDate"].ToString()).UtcDateTime
                        };

                        problemUpdate.Messages = SelectProblemUpdateMessages(problemUpdate.ID);
                    }

                    return problemUpdate;
                }
            }
            catch { return null; }
        }

        public List<ProblemUpdate> SelectProblemUpdates(long importExportID)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_ProblemUpdate_Select_By_ImportExportID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ImportExportID", importExportID));

                    List<ProblemUpdate> problemUpdates = new List<ProblemUpdate>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var problemUpdate = new ProblemUpdate()
                        {
                            DateInserted = DateTimeOffset.Parse(reader["DateInserted"].ToString()).UtcDateTime,
                            ID = Convert.ToInt64(reader["ID"]),
                            ImportExportID = Convert.ToInt64(reader["ImportExportID"]),
                            IsResolved = Convert.ToBoolean(reader["IsResolved"]),
                            ProblemDate = DateTimeOffset.Parse(reader["ProblemDate"].ToString()).UtcDateTime,
                            ProblemID = Convert.ToInt64(reader["ProblemID"]),
                            ResolvedDate = (reader["ResolvedDate"] == DBNull.Value) ? null : (DateTime?)DateTimeOffset.Parse(reader["ResolvedDate"].ToString()).UtcDateTime
                        };

                        problemUpdate.Messages = SelectProblemUpdateMessages(problemUpdate.ID);
                        problemUpdates.Add(problemUpdate);
                    }

                    return problemUpdates;
                }
            }
            catch { return null; }
        }

        public int ResolveProblemUpdate(ProblemUpdate problemUpdate)
        {
            problemUpdate.ResolvedDate = _utcNow;
            problemUpdate.IsResolved = true;

            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_ProblemUpdate_Resolve", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ID", problemUpdate.ID));
                cmd.Parameters.Add(new SqlParameter("@IsResolved", problemUpdate.IsResolved));
                cmd.Parameters.Add(new SqlParameter("@ResolvedDate", _utcNowString));

                return cmd.ExecuteNonQuery();
            }
        }


        public int UpdateProblemUpdate(ProblemUpdate problemUpdate)
        {
            problemUpdate.DateInserted = _utcNow;
            string problemDate = DateTimeOffsetString(problemUpdate.ProblemDate);

            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_ProblemUpdate_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ID", problemUpdate.ID));
                cmd.Parameters.Add(new SqlParameter("@IsResolved", problemUpdate.IsResolved));
                cmd.Parameters.Add(new SqlParameter("@ProblemID", problemUpdate.ProblemID));
                cmd.Parameters.Add(new SqlParameter("@ProblemDate", problemDate));

                return cmd.ExecuteNonQuery();
            }
        }

        public async Task<long> DeleteProblemUpdate(CallParams args)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_ProblemUpdate_Delete", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@ID", args.ID));
                cmd.Parameters.Add(new SqlParameter("@token", args.Token));

                return await cmd.ExecuteNonQueryAsync();
            }
        }

        #endregion

        #region ProblemUpdateMessages
        public List<ProblemUpdateMessage> SelectProblemUpdateMessages(long problemUpdateId)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_ProblemUpdateMessages_Select_By_ProblemUpdateID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ProblemUpdateID", problemUpdateId));

                    List<ProblemUpdateMessage> messages = new List<ProblemUpdateMessage>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var message = new ProblemUpdateMessage()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            ProblemUpdateID = Convert.ToInt64(reader["ProblemUpdateID"]),
                            Message = reader["Message"].ToString(),
                            DateInserted = DateTimeOffset.Parse(reader["DateInserted"].ToString()).UtcDateTime
                        };

                        messages.Add(message);
                    }

                    return messages;
                }
            }
            catch { return null; }
        }


        public long InsertProblemUpdateMessage(ProblemUpdateMessage message)
        {
            message.DateInserted = _utcNow;

            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_ProblemUpdateMessages_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    var pID = new SqlParameter("@ID", SqlDbType.BigInt);
                    var pProblemUpdateID = new SqlParameter("@ProblemUpdateID", message.ProblemUpdateID);
                    var pMessage = new SqlParameter("@Message", message.Message);

                    pID.Direction = ParameterDirection.Output;

                    cmd.Parameters.AddRange(new[] { pID, pProblemUpdateID, pMessage });

                    cmd.ExecuteNonQuery();
                    return message.ID = Convert.ToInt64(pID.Value);
                }
            }
            catch { return 0; }
        }

        #endregion

        #region Reports
        public DataSet GenerateReport(ReportFilters filter)
        {
            SqlCommand command;
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            string fromDateString = null, toDateString = null;

            if (filter.FromDate != null)
            {
                fromDateString = DateTimeOffsetString(filter.FromDate.Value);
            }

            if (filter.ToDate != null)
            {
                toDateString = DateTimeOffsetString(filter.ToDate.Value);
            }

            using (SqlConnection con = Connection)
            {
                SqlParameter fromDate = new SqlParameter("@fromdate", (object)fromDateString ?? DBNull.Value);
                SqlParameter toDate = new SqlParameter("@todate", (object)toDateString ?? DBNull.Value);
                SqlParameter userToken = new SqlParameter("@TOKEN", filter.Token ?? string.Empty);
                SqlParameter bill = new SqlParameter("@BILL", filter.Bill ?? string.Empty);
                SqlParameter tin = new SqlParameter("@TIN", filter.TIN ?? string.Empty);
                SqlParameter cargo = new SqlParameter("@CARGO", filter.Cargo ?? string.Empty);
                SqlParameter country = new SqlParameter("@COUNTRY", filter.Country ?? string.Empty);
                SqlParameter problem = new SqlParameter("@PROBLEM", filter.Problem ?? string.Empty);


                SqlParameter demurrageGrace = new SqlParameter("@DemurrageGrace", SqlDbType.Int)
                {
                    Value = _DEMURRAGE_GRACE
                };
                switch (filter.ReportName)
                {
                    case "cargo_dispatched_weight_grouped_by_month_report":
                        command = new SqlCommand("Report_CargoDispatchedWeightGroupedByMonth", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new SqlParameter[] { fromDate, toDate, tin, cargo, userToken });

                        da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        return ds;
                    case "cargo_import_weight_grouped_by_tin_report":
                        command = new SqlCommand("Report_CargoImportWeightGroupedByTIN", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new[] { fromDate, toDate, tin, cargo, userToken });

                        da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        return ds;
                    case "cargo_on_voyage_only_grouped_by_country_report":
                        command = new SqlCommand("Report_CargoOnVoyageOnlyGroupedByCountry", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new SqlParameter[] { tin, cargo, country, userToken });

                        da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        return ds;
                    case "problem_grouped_by_tin_report":
                        command = new SqlCommand("Report_ProblemGroupedByTin", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new SqlParameter[] { fromDate, toDate, tin, problem, userToken });

                        da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        return ds;
                    case "problem_grouped_by_tin_unresolved_report":
                        command = new SqlCommand("Report_ProblemUnresolvedGroupedByTin", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new SqlParameter[] { tin, problem, userToken });

                        da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        return ds;
                    case "demurrage_grouped_by_tin_report":
                        command = new SqlCommand("Report_Demurrage", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new SqlParameter[] { fromDate, toDate, tin, demurrageGrace, userToken });

                        da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        return ds;
                    case "demurrage_grouped_by_tin_active_report":
                        command = new SqlCommand("Report_Demurrage_Active", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new SqlParameter[] { fromDate, toDate, tin, demurrageGrace, userToken });

                        da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        return ds;
                    case "transit_time_grouped_by_import_report":
                        command = new SqlCommand("Report_TransitTimeGroupedByImport", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new SqlParameter[] { fromDate, toDate, bill, userToken });

                        da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        return ds;
                    case "transit_time_grouped_by_country_report":
                        command = new SqlCommand("Report_TransitTimeGroupedByCountry", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new SqlParameter[] { fromDate, toDate, country, userToken });

                        da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        return ds;
                    case "transit_time_grouped_by_country_summary_report":
                        command = new SqlCommand("Report_TransitTimeSummaryGroupedByCountry", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new SqlParameter[] { fromDate, toDate, country, userToken });

                        da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        return ds;
                    case "transit_time_grouped_by_tin_report":
                        command = new SqlCommand("Report_TransitTimeGroupedByTIN", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new SqlParameter[] { fromDate, toDate, tin, userToken });

                        da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        return ds;
                    case "bill_statuses_grouped_by_tin_report":
                        command = new SqlCommand("Report_BillStatusesByTransitor", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new SqlParameter[] { fromDate, toDate, tin, bill, userToken });

                        da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        return ds;
                    default:
                        return null;
                }

            }
        }
        #endregion

        #region Role
        public long InsertRole(Role role)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Role_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                var pID = new SqlParameter("@ID", SqlDbType.BigInt);
                var pCompanyID = new SqlParameter("@CompanyID", role.CompanyID);
                var pDescription = new SqlParameter("@Description", role.Description ?? string.Empty);
                var pRoleCode = new SqlParameter("@RoleCode", role.RoleCode ?? string.Empty);
                var pRoleName = new SqlParameter("@RoleName", ToCapitalizeCase(role.RoleName));

                pID.Direction = ParameterDirection.Output;
                cmd.Parameters.AddRange(new[] { pID, pCompanyID, pDescription, pRoleCode, pRoleName });

                cmd.ExecuteNonQuery();

                return role.ID = Convert.ToInt64(pID.Value);
            }
        }

        #endregion

        #region RolePermission
        public long InsertRolePermissionsForNewCompany(long roleId, long companyTypeId)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_RolePermisions_Insert_ForNewCompany", con);
                cmd.CommandType = CommandType.StoredProcedure;

                var pID = new SqlParameter("@ID", SqlDbType.BigInt);
                var pRoleID = new SqlParameter("@RoleID", roleId);
                var pCompanyTypeID = new SqlParameter("@CompanyTypeID", companyTypeId);

                pID.Direction = ParameterDirection.Output;

                cmd.Parameters.AddRange(new[] { pID, pRoleID, pCompanyTypeID });
                cmd.ExecuteNonQuery();

                return Convert.ToInt64(pID.Value);
            }
        }

        public long InsertRolePermission(RolePermission permission)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_RolePermissions_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                var pID = new SqlParameter("@ID", SqlDbType.BigInt);
                var pRoleID = new SqlParameter("@RoleID", permission.RoleID);
                var pEditUser = new SqlParameter("@EditUser", permission.EditUser);
                var pEditAdmin = new SqlParameter("@EditAdmin", permission.EditAdmin);
                var pEditSA = new SqlParameter("@EditSA", permission.EditSA);
                var pEditCC = new SqlParameter("@EditCC", permission.EditCC);
                var pEditFF = new SqlParameter("@EditFF", permission.EditFF);

                pID.Direction = ParameterDirection.Output;

                cmd.Parameters.AddRange(new[] { pID, pRoleID, pEditUser, pEditAdmin, pEditSA, pEditCC, pEditFF });
                cmd.ExecuteNonQuery();

                return permission.ID = Convert.ToInt64(pID.Value);
            }
        }

        public RolePermission SelectRolePermissionsByToken(Guid token)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_RolePermissions_Select_ByToken", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Token", token));

                RolePermission perm = null;

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    do
                    {
                        reader.Read();
                        perm = new RolePermission()
                        {
                            EditAdmin = Convert.ToBoolean(reader["EditAdmin"]),
                            EditAll = Convert.ToBoolean(reader["EditAll"]),
                            EditCC = Convert.ToBoolean(reader["EditCC"]),
                            EditFF = Convert.ToBoolean(reader["EditFF"]),
                            EditSA = Convert.ToBoolean(reader["EditSA"]),
                            EditUser = Convert.ToBoolean(reader["EditUser"]),
                            ID = Convert.ToInt64(reader["ID"]),
                            RoleID = Convert.ToInt64(reader["RoleID"])
                        };
                    } while (reader.Read());
                }

                return perm;
            }
        }
        #endregion

        #region Status
        public List<Status> SelectStatuses()
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Status_Select_All", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    List<Status> statuses = new List<Status>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Status status = new Status()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            Description = reader["Description"].ToString(),
                            Abbr = reader["Abbr"].ToString(),
                            Air = Convert.ToBoolean(reader["Air"]),
                            ImpExpTypeID = Convert.ToInt64(reader["ImpExpTypeID"]),
                            PipeLine = Convert.ToBoolean(reader["PipeLine"]),
                            Sea = Convert.ToBoolean(reader["Sea"]),
                            Truck = Convert.ToBoolean(reader["Truck"])
                        };

                        statuses.Add(status);
                    }

                    return statuses;
                }
            }
            catch { return null; }
        }

        public Status SelectStatus(long id)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Status_Select", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    Status status = null;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        status = new Status()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            Description = reader["Description"].ToString(),
                            Abbr = reader["Abbr"].ToString(),
                            Air = Convert.ToBoolean(reader["Air"]),
                            ImpExpTypeID = Convert.ToInt64(reader["ImpExpTypeID"]),
                            PipeLine = Convert.ToBoolean(reader["PipeLine"]),
                            Sea = Convert.ToBoolean(reader["Sea"]),
                            Truck = Convert.ToBoolean(reader["Truck"])
                        };
                    }

                    return status;
                }
            }
            catch { return null; }
        }
        #endregion

        #region StatusUpdate
        private StatusUpdate GetStatusUpdateFromReader(SqlDataReader reader)
        {
            var statusUpdate = new StatusUpdate();
            var locationId = reader["LocationID"].ToString();

            statusUpdate.ID = Convert.ToInt64(reader["ID"]);
            statusUpdate.ImportExportID = Convert.ToInt64(reader["ImportExportID"]);
            statusUpdate.LocationID = Convert.ToInt64(locationId == "" ? "0" : locationId);
            statusUpdate.DateInserted = DateTimeOffset.Parse(reader["DateInserted"].ToString()).UtcDateTime;
            statusUpdate.StatusDate = DateTimeOffset.Parse(reader["StatusDate"].ToString()).UtcDateTime;
            statusUpdate.StatusID = Convert.ToInt64(reader["StatusID"]);


            return statusUpdate;
        }

        public long InsertStatusUpdate(StatusUpdate statusUpdate)
        {
            statusUpdate.DateInserted = _utcNow;
            string statusDate = DateTimeOffsetString(statusUpdate.StatusDate);

            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_StatusUpdate_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    var pID = new SqlParameter("@ID", SqlDbType.BigInt);
                    var pImportExportID = new SqlParameter("@ImportExportID", statusUpdate.ImportExportID);
                    var pStatusDate = new SqlParameter("@StatusDate", statusDate);
                    var pStatusID = new SqlParameter("@StatusID", statusUpdate.StatusID);
                    var pLocationID = new SqlParameter("@LocationID", statusUpdate.LocationID);

                    pID.Direction = ParameterDirection.Output;
                    cmd.Parameters.AddRange(new[] { pID, pImportExportID, pStatusDate, pStatusID, pLocationID });

                    cmd.ExecuteNonQuery();

                    return statusUpdate.ID = Convert.ToInt64(pID.Value);
                }
            }
            catch { return 0; }
        }

        public StatusUpdate SelectStatusUpdate(long id)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_StatusUpdate_Select", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ID", id));

                StatusUpdate statusUpdate = null;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    statusUpdate = GetStatusUpdateFromReader(reader);
                }

                return statusUpdate;
            }
        }

        public List<StatusUpdate> SelectStatusUpdates(long importExportID)
        {
            List<StatusUpdate> statusUpdates = new List<StatusUpdate>();
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_StatusUpdate_Select_By_ImportExportID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ImportExportID", importExportID));

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var statusUpdate = GetStatusUpdateFromReader(reader);
                    statusUpdates.Add(statusUpdate);
                }
            }

            return statusUpdates;
        }

        public int UpdateStatusUpdate(StatusUpdate statusUpdate)
        {
            statusUpdate.DateInserted = _utcNow;
            string statusDate = DateTimeOffsetString(statusUpdate.StatusDate);

            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_StatusUpdate_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ID", statusUpdate.ID));
                cmd.Parameters.Add(new SqlParameter("@StatusDate", statusDate));
                cmd.Parameters.Add(new SqlParameter("@StatusID", statusUpdate.StatusID));
                cmd.Parameters.Add(new SqlParameter("@LocationID", statusUpdate.LocationID));

                return cmd.ExecuteNonQuery();
            }
        }

        public async Task<long> DeleteStatusUpdate(CallParams args)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_StatusUpdate_Delete", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@ID", args.ID));
                cmd.Parameters.Add(new SqlParameter("@token", args.Token));

                return await cmd.ExecuteNonQueryAsync();
            }
        }

        #endregion

        #region StuffMode
        public List<StuffMode> SelectStuffModes()
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_StuffMode_Select_All", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    List<StuffMode> stuffModes = new List<StuffMode>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        StuffMode stuffMode = new StuffMode()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            Description = reader["Description"].ToString()
                        };

                        stuffModes.Add(stuffMode);
                    }

                    return stuffModes;
                }
            }
            catch { return null; }
        }

        public StuffMode SelectStuffMode(long id)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_StuffMode_Select", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    StuffMode stuffMode = null;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        stuffMode = new StuffMode()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            Description = reader["Description"].ToString()
                        };
                    }

                    return stuffMode;
                }
            }
            catch { return null; }
        }
        #endregion

        #region SubCargo
        public List<SubCargo> SelectSubCargos(long cargoID)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_SubCargo_Select_ByCargo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CargoID", cargoID));

                    List<SubCargo> subCargos = new List<SubCargo>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SubCargo subCargo = new SubCargo()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            Description = reader["Description"].ToString(),
                            CargoID = Convert.ToInt64(reader["CargoID"]),
                            HasDim = Convert.ToBoolean(reader["HasDim"]),
                            ImpExpTypeID = Convert.ToInt64(reader["ImpExpTypeID"]),
                            IsEnabled = Convert.ToBoolean(reader["IsEnabled"]),
                            UnitID = Convert.ToInt64(reader["UnitID"])
                        };

                        subCargos.Add(subCargo);
                    }

                    return subCargos;
                }
            }
            catch { return null; }
        }

        public SubCargo SelectSubCargo(long id)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_SubCargo_Select", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    SubCargo subCargo = null;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        subCargo = new SubCargo()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            Description = reader["Description"].ToString(),
                            CargoID = Convert.ToInt64(reader["CargoID"]),
                            HasDim = Convert.ToBoolean(reader["HasDim"]),
                            ImpExpTypeID = Convert.ToInt64(reader["ImpExpTypeID"]),
                            IsEnabled = Convert.ToBoolean(reader["IsEnabled"]),
                            UnitID = Convert.ToInt64(reader["UnitID"])
                        };
                    }

                    return subCargo;
                }
            }
            catch { return null; }
        }
        #endregion

        #region Subscriptions
        public List<SubscriptionType> SubscriptionTypes()
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand("sp_SubscriptionTypes_Select", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                List<SubscriptionType> subscriptionTypes = new List<SubscriptionType>();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    subscriptionTypes.Add(new SubscriptionType
                    {
                        ID = Convert.ToInt64(reader["ID"]),
                        SubscriptionName = reader["SubscriptionName"].ToString(),
                        Category = reader["Category"].ToString(),
                        Description = reader["Description"].ToString(),
                        RenewalCycle = reader["RenewalCycle"].ToString(),
                        Cost = Convert.ToDecimal(reader["Cost"]),
                        Currency = reader["Currency"].ToString()
                    });
                }

                return subscriptionTypes;
            }
        }

        // Inserts a new subscription for a company
        public long InsertSubscriptionHistory(Subscribe subscribe)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand("sp_SubscriptionsHistory_Insert", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                var pID = new SqlParameter("@ID", SqlDbType.BigInt);
                var pActivatedDate = new SqlParameter("@ActivatedDate", SqlDbType.DateTimeOffset);
                var pExpiryDate = new SqlParameter("@ExpiryDate", SqlDbType.DateTimeOffset);
                var pCompanyID = new SqlParameter("@CompanyID", subscribe.CompanyID);
                var pSubcriptionTypeID = new SqlParameter("@SubcriptionTypeID", subscribe.SubcriptionTypeID);

                pID.Direction = pActivatedDate.Direction = pExpiryDate.Direction = ParameterDirection.Output;
                command.Parameters.AddRange(new[] { pID, pActivatedDate, pExpiryDate, pCompanyID, pSubcriptionTypeID });

                var result = command.ExecuteNonQuery();
                return result;
            }
        }

        // Gets user subscriptions
        public List<SubscriptionHistoryView> SubscriptionHistoriesCurrent(long companyId, Guid token = new Guid())
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand("sp_SubscriptionsHistory_Current", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@companyId", companyId);
                command.Parameters.AddWithValue("@token", token);

                List<SubscriptionHistoryView> currentSubscriptions = new List<SubscriptionHistoryView>();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    currentSubscriptions.Add(new SubscriptionHistoryView
                    {
                        ID = Convert.ToInt64(reader["ID"]),
                        SubscriptionName = reader["SubscriptionName"].ToString(),
                        ServiceType = reader["ServiceType"].ToString(),
                        Category = reader["Category"].ToString(),
                        Currency = reader["Currency"].ToString(),
                        Cost = Convert.ToDecimal(reader["Cost"]),
                        RenewalCycle = reader["RenewalCycle"].ToString(),
                        ActivatedDate = DateTimeOffset.Parse(reader["ActivatedDate"].ToString()).UtcDateTime,
                        ExpiryDate = DateTimeOffset.Parse(reader["ExpiryDate"].ToString()).UtcDateTime,
                        MaximumUsers = Convert.ToInt32(reader["MaximumUsers"]),
                        UsersCount = Convert.ToInt32(reader["UsersCount"]),
                        IsActive = Convert.ToBoolean(reader["IsActive"])
                    });
                }

                return currentSubscriptions;
            }
        }

        // Renew a subscription
        public long RenewSubscriptionHistory(long subsHistoryId)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand("sp_SubscriptionsHistory_Renew", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@subsHistoryId", subsHistoryId);

                return command.ExecuteNonQuery();
            }
        }

        // Validate user subscription is active for app
        public async Task<SubscriptionHistoryView> GetAppSubscription(Guid token)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand()
                {
                    Connection = con,
                    CommandText = "sp_SubscriptionsHistory_App",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@token", token);
                SubscriptionHistoryView appSubsHistory = null;

                var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow);
                while (await reader.ReadAsync())
                {
                    appSubsHistory = new SubscriptionHistoryView()
                    {
                        SubscriptionName = reader["SubscriptionName"].ToString(),
                        ServiceType = reader["ServiceType"].ToString(),
                        Category = reader["Category"].ToString(),
                        Currency = reader["Currency"].ToString(),
                        Cost = Convert.ToDecimal(reader["Cost"]),
                        RenewalCycle = reader["RenewalCycle"].ToString(),
                        ActivatedDate = DateTimeOffset.Parse(reader["ActivatedDate"].ToString()).UtcDateTime,
                        ExpiryDate = DateTimeOffset.Parse(reader["ExpiryDate"].ToString()).UtcDateTime,
                        MaximumUsers = Convert.ToInt32(reader["MaximumUsers"]),
                        UsersCount = Convert.ToInt32(reader["UsersCount"]),
                        IsActive = Convert.ToBoolean(reader["IsActive"])
                    };
                }

                return appSubsHistory;
            }

        }

        /// <summary>
        /// Select SubscriptionHistories for a company
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<SubscriptionHistoryView>> SelectSubscriptionHistories(string token)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand()
                {
                    Connection = con,
                    CommandText = "sp_SubscriptionsHistory_All",
                    CommandType = CommandType.StoredProcedure
                };

                List<SubscriptionHistoryView> subscriptionHistoriesView = new List<SubscriptionHistoryView>();

                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    subscriptionHistoriesView.Add(new SubscriptionHistoryView()
                    {
                        SubscriptionName = reader["SubscriptionName"].ToString(),
                        ServiceType = reader["ServiceType"].ToString(),
                        Category = reader["Category"].ToString(),
                        AmountPaidCurrency = reader["AmountPaidCurrency"].ToString(),
                        AmountPaid = Convert.ToDecimal(reader["AmountPaid"]),
                        RenewalCycle = reader["RenewalCycle"].ToString(),
                        ActivatedDate = DateTimeOffset.Parse(reader["ActivatedDate"].ToString()).UtcDateTime,
                        ExpiryDate = DateTimeOffset.Parse(reader["ExpiryDate"].ToString()).UtcDateTime,
                        MaximumUsers = Convert.ToInt32(reader["MaximumUsers"]),
                        UsersCount = Convert.ToInt32(reader["UsersCount"]),
                        IsActive = Convert.ToBoolean(reader["IsActive"])
                    });
                }

                return subscriptionHistoriesView;
            }
        }
        #endregion

        #region Tarrif
        public List<Tariff> SelectTarrifs()
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Vessel_Select_All", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    List<Tariff> tarrifs = new List<Tariff>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Tariff tarrif = new Tariff()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            Description = reader["Description"].ToString(),
                            Duty = Convert.ToDecimal(reader["Duty"]),
                            Excise = Convert.ToDecimal(reader["Excise"]),
                            ExportTax = Convert.ToDecimal(reader["ExportTax"]),
                            HScode = reader["HScode"].ToString(),
                            SecondSch1 = reader["SecondSch1"].ToString(),
                            SecondSch2 = reader["SecondSch2"].ToString(),
                            SpecialPermission = reader["SpecialPermission"].ToString(),
                            Sur = Convert.ToDecimal(reader["Sur"]),
                            Unit = reader["Unit"].ToString(),
                            Vat = Convert.ToDecimal(reader["Vat"]),
                            Withholding = Convert.ToDecimal(reader["Withholding"])
                        };

                        tarrifs.Add(tarrif);
                    }

                    return tarrifs;
                }
            }
            catch { return null; }

        }

        public Tariff SelectTarrif(long id)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Tarrif_Select", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    Tariff tariff = null;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        tariff = new Tariff()
                        {
                            ID = Convert.ToInt64(reader["ID"]),
                            Description = reader["Description"].ToString(),
                            Duty = Convert.ToDecimal(reader["Duty"]),
                            Excise = Convert.ToDecimal(reader["Excise"]),
                            ExportTax = Convert.ToDecimal(reader["ExportTax"]),
                            HScode = reader["HScode"].ToString(),
                            SecondSch1 = reader["SecondSch1"].ToString(),
                            SecondSch2 = reader["SecondSch2"].ToString(),
                            SpecialPermission = reader["SpecialPermission"].ToString(),
                            Sur = Convert.ToDecimal(reader["Sur"]),
                            Unit = reader["Unit"].ToString(),
                            Vat = Convert.ToDecimal(reader["Vat"]),
                            Withholding = Convert.ToDecimal(reader["Withholding"])
                        };
                    }

                    return tariff;
                }
            }
            catch { return null; }
        }
        #endregion

        #region Unit
        public List<Unit> SelectUnits()
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Unit_Select_All", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    List<Unit> units = new List<Unit>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string baseUnitString = reader["BaseUnitRate"].ToString();
                        decimal baseUnitRate = 0;

                        baseUnitRate = decimal.Parse(baseUnitString, NumberStyles.Float, CultureInfo.InvariantCulture);

                        Unit unit = new Unit()
                        {
                            BaseUnitRate = baseUnitRate,
                            ID = Convert.ToInt64(reader["ID"]),
                            IsVolume = Convert.ToBoolean(reader["IsVolume"]),
                            UnitName = reader["UnitName"].ToString()
                        };

                        units.Add(unit);
                    }

                    return units;
                }
            }
            catch (Exception) { return null; }
        }

        public Unit SelectUnit(long id)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand cmd = new SqlCommand("sp_Unit_Select", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ID", id));

                Unit unit = null;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    unit = new Unit()
                    {
                        BaseUnitRate = Convert.ToDecimal(reader["BaseUnitRate"]),
                        ID = Convert.ToInt64(reader["ID"]),
                        IsVolume = Convert.ToBoolean(reader["IsVolume"]),
                        UnitName = reader["UnitName"].ToString()
                    };
                }

                return unit;
            }
        }

        #endregion

        #region Users & Roles w/ Permissions View for Company
        public UsersInCompanyView LockOrUnlockUserLogin(UsersInCompanyView userView)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand("sp_LockOrUnlockLogin", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", userView.ID));

                int value = command.ExecuteNonQuery();

                if (value == 1)
                {
                    userView.IsActive = !userView.IsActive;
                    return userView;
                }

                return userView;
            }
        }
        public List<UsersInCompanyView> SelectUsersInCompany(Guid token)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand("sp_UsersInCompany", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Token", token));

                List<UsersInCompanyView> usersView = new List<UsersInCompanyView>();

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var users = new UsersInCompanyView()
                    {
                        ID = Convert.ToInt64(reader["ID"]),
                        RoleID = Convert.ToInt64(reader["RoleID"]),
                        Description = reader["Description"].ToString(),
                        Email = reader["Email"].ToString(),
                        Fullname = reader["Fullname"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Photo = reader["Photo"].ToString(),
                        RoleCode = reader["RoleCode"].ToString(),
                        RoleName = reader["RoleName"].ToString(),
                        IsActive = Convert.ToBoolean(reader["IsActive"])
                    };

                    usersView.Add(users);
                }

                return usersView;
            }
        }

        public List<Role> SelectRolesInCompanyByPrivilege(Guid token)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand("sp_RolesInCompanyByPrivilege", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Token", token));

                List<Role> roles = new List<Role>();

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var role = new Role()
                    {
                        ID = Convert.ToInt64(reader["ID"]),
                        Description = reader["Description"].ToString(),
                        RoleCode = reader["RoleCode"].ToString(),
                        RoleName = reader["RoleName"].ToString()
                    };

                    roles.Add(role);
                }

                return roles;
            }
        }
        #endregion

        #region Vessel
        public async Task<List<Vessel>> SelectVessels(string term)
        {
            List<Vessel> vessels = new List<Vessel>();

            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Vessel_Select_Match")
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add(new SqlParameter("@vesselName", term));

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (reader.ReadAsync().IsCompleted)
                    {
                        vessels.Add(new Vessel()
                        {
                            CarrierID = Convert.ToInt64(reader["CarrierID"]),
                            ID = Convert.ToInt64(reader["ID"]),
                            ModeOfTransportID = Convert.ToInt64(reader["ModeOfTransportID"]),
                            VesselName = reader["VesselName"].ToString()
                        });
                    }

                    return vessels;
                }
            }
            catch (Exception) { return vessels; }

        }

        public List<Vessel> SelectVessels(long carrierID)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Vessel_Select_ByCarrier", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CarrierID", carrierID));

                    List<Vessel> vessels = new List<Vessel>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Vessel vessel = new Vessel()
                        {
                            CarrierID = Convert.ToInt64(reader["CarrierID"]),
                            ID = Convert.ToInt64(reader["ID"]),
                            ModeOfTransportID = Convert.ToInt64(reader["ModeOfTransportID"]),
                            VesselName = reader["VesselName"].ToString()
                        };

                        vessels.Add(vessel);
                    }

                    return vessels;
                }
            }
            catch { return null; }

        }

        public Vessel SelectVessel(long id)
        {
            try
            {
                using (SqlConnection con = Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_Vessel_Select", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    Vessel vessel = null;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        vessel = new Vessel()
                        {
                            CarrierID = Convert.ToInt64(reader["CarrierID"]),
                            ID = Convert.ToInt64(reader["ID"]),
                            ModeOfTransportID = Convert.ToInt64(reader["ModeOfTransportID"]),
                            VesselName = reader["VesselName"].ToString()
                        };
                    }

                    return vessel;
                }
            }
            catch { return null; }
        }

        public bool InsertVessel(Vessel vessel)
        {
            using (SqlConnection con = Connection)
            {
                SqlCommand command = new SqlCommand()
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "sp_Vessel_Insert",
                    Connection = con
                };

                command.Parameters.AddWithValue("@vesselName", vessel.VesselName);
                command.Parameters.AddWithValue("@carrierID", vessel.CarrierID);
                command.Parameters.AddWithValue("@modeOfTransportID", vessel.ModeOfTransportID);

                long value;
                if (long.TryParse(command.ExecuteScalar().ToString(), out value))
                {
                    vessel.ID = value;
                    return true;
                }

                return false;
            }
        }
        #endregion





    }
}
