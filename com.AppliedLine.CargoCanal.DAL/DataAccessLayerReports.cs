using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using com.AppliedLine.CargoCanal.Models;

namespace com.AppliedLine.CargoCanal.DAL
{
    public class DataAccessLayerReports
    {
        #region "Connection"

        private SqlConnection Connection
        {
            get { return new DataAccessLayer().Connection; }
        }

        #endregion

        public StringBuilder CustomReportSb()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT [Import].[BillNumber] [BoL], CAST(CONVERT(date, [Import].[BillDate]) as NVARCHAR(15)) [BoL Date]");
            sb.Append(", [ImportExport].ID [ImportExportID], [ImportExport].[ReferenceNo] [Ref #],[ImportExport].[DateInserted] [Date Inserted], [ImportExport].[DateInitiated] [Date Initiated], [ImportExport].[VoyageNumber] [Voyage #], [ImportExport].[Remark], [ImportExport].[ReImportExport] [Re-import], [ImportExport].[Unimodal], [ImportExport].[Vessel], [ImportExport].[Terminated], [ImportExport].[Completed]");
            sb.Append(", [Item].[ItemOrderNo] [Item Order #], [Item].[GrossWeight] [Gross Weight], [Item].[NetWeight] [Net Weight], (Select UnitName from Units where ID = Item.WeightUnitID) [Weight Unit], [Item].[Volume], (Select UnitName from Units where ID = Item.VolumeUnitID) [Volume Unit], [Item].[Quantity], [Item].[Dangerous], [Item].[Description] [Item Description]");
            sb.Append(", [ItemDetail].[ItemNumber] [Item #]");
            sb.Append(", [StuffMode].[Description] [Stuff Mode]");
            sb.Append(", [Cargo].[CargoName] [Cargo]");
            sb.Append(", [SubCargo].[Description] [SubCargo], [SubCargo].[UnitID] [SubCargo Unit], [SubCargo].[HasDim]");
            sb.Append(", [Carrier].[CarrierName] [Carrier]");
            //sb.Append(", [Vessel].[Name] [Vessel]");
            sb.Append(", [Ports].[PortName] [Origin], [Ports].[IsDryPort] [Is Dry Port]");
            sb.Append(", [Country].[Name] [Country], [Country].[Region]");
            sb.Append(", [ModeOfTransport].[Mode] [Transport]");
            //sb.Append(", [Cost].[Freight] [FreightCost], [Cost].[Insurance] [InsuranceCost], [Cost].[InlandTransport] [InlandTransportCost], [Cost].[PortStorageDemurrage] [PortStorageDemurrageCost], [Cost].[InlandStorage] [InlandStorageCost], [Cost].[PortHandling] [PortHandlingCost], [Cost].[TruckDetention] [TruckDetentionCost], [Cost].[DryPortCharge] [DryPortChargeCost], [Cost].[MaterialCost], [Cost].[OtherChargesBirr], [Cost].[OtherChargesDollar], [Cost].[TotalBirr], [Cost].[TotalDollar]");
            sb.Append(", [ImpExpType].[TypeName] [Shipment Type], [ImpExpType].[Description] [Shipment Type Description]");
            sb.Append(", [IncoTerms].[IncoName] [Delivery Terms]");
            sb.Append(", [ImportExportReason].[Reason] [Shipment Reason]");
            sb.Append(", [Person].[FirstName] [FF First Name], [Person].[MiddleName] [FF Middle Name], [Person].[LastName] [FF Last Name], [Person].[Phone] [FF Phone], [Person].[Email] [FF Email]");
            sb.Append(", [Company].[CompanyName] [Company], [Company].[Address], [Company].[TownCity] [Town / City], [Company].[State], [Company].[POBox] [PO Box], [Company].[Wereda], [Company].[KefleKetema] [Kefle Ketema], [Company].[Kebele], [Company].[HouseNo] [House #], [Company].[Telephone], [Company].[ContactName] [Company Contact Person], [Company].[ContactMobile] [Company Contact Mobile], [Company].[Email] [Company Email], [Company].[Website], [Company].[TIN], [Company].[LicenseNumber] [License #], [Company].[LicenseUnder] [Licensed Under], [Company].[LastRenewedDate] [License Last Renewed Date], [Company].[LicenseIssuedDate] [License Issued Date], [Company].[IsActive] [Is Company Active]");
            sb.Append(", [CompanyType].[Name] [Company Type], [CompanyType].[Description] [Company Type Description]");
            sb.Append(", [Status].[Description] [Status], [StatusUpdate].[StatusDate] [Status Date]");
            sb.Append(" FROM [ImportExport]");

            sb.Append(" JOIN [Import]  ON [ImportExport].ID = [Import].ImportExportID");
            sb.Append(" JOIN [Item]  ON [ImportExport].ID = [Item].ImportExportID");
            sb.Append(" JOIN [ItemDetail] ON [Item].ID = [ItemDetail].ItemID");
            sb.Append(" JOIN [StuffMode] ON [ItemDetail].StuffModeID = [StuffMode].ID");
            sb.Append(" JOIN [Cargo] ON [Item].CargoID = [Cargo].ID");
            sb.Append(" JOIN [SubCargo] ON [Item].SubCargoID = [SubCargo].ID");
            sb.Append(" JOIN [Carrier] ON [ImportExport].CarrierID = [Carrier].ID");
            //sb.Append(" JOIN [Vessel] ON [ImportExport].VesselID = [Vessel].ID");
            sb.Append(" JOIN [Ports] ON [ImportExport].PortOfLoadingID = [Ports].ID");
            sb.Append(" JOIN [Country] ON [Ports].CountryID = [Country].ID");
            sb.Append(" JOIN [ModeOfTransport] ON [ImportExport].ModeOfTransportID = [ModeOfTransport].ID");
            //sb.Append(" JOIN [Cost] ON [ImportExport].ID = [Cost].ImportExportID");
            sb.Append(" JOIN [ImpExpType] ON [ImportExport].ImpExpTypeID = [ImpExpType].ID");
            sb.Append(" JOIN [IncoTerms] ON [ImportExport].IncoTermID = [IncoTerms].ID");
            sb.Append(" JOIN [ImportExportReason] ON [ImportExport].ImportExportReasonID = [ImportExportReason].ID");
            sb.Append(" JOIN [Person] ON [ImportExport].CreatedBy = [Person].ID");
            sb.Append(" JOIN [Company] ON [ImportExport].CompanyID = [Company].ID");
            sb.Append(" JOIN [CompanyType] ON [Company].CompanyTypeID = [CompanyType].ID");
            sb.Append(" JOIN [StatusUpdate] ON [ImportExport].ID = [StatusUpdate].ImportExportID");
            sb.Append(" JOIN [Status] ON [StatusUpdate].StatusID = [Status].ID");

            return sb;
        }

        public JArray GetColumnHeaders()
        {
            StringBuilder sb = CustomReportSb().Append(" WHERE [ImportExport].ID=0");
            using (SqlConnection conx = Connection)
            {
                SqlCommand command = new SqlCommand(sb.ToString(), conx);

                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);

                var columns = ds.Tables[0].Columns;

                var jArray = new JArray();

                foreach (DataColumn column in columns)
                {
                    // skip ImportExportID, Status and Status Date
                    if (column.ColumnName.Contains("ImportExportID")
                        || column.ColumnName.Equals("Status Date")
                        || column.ColumnName.Equals("Status")) continue;

                    jArray.Add(column.Caption);
                }

                return jArray;
            }
        }

        public JArray GetCustomImportsReport(CustomReportQuery crQuery)
        {
            try
            {
                /// <summary>
                /// Build the query to select all fields from required tables
                /// </summary>

                bool whereClauseAdded = false;
                StringBuilder sb = CustomReportSb();
                if (crQuery != null && crQuery.Queries != null && crQuery.Queries.Count > 0)
                {
                    /// <summary>
                    /// Build the WHERE clause for the select statement
                    /// </summary>

                    sb.Append(" WHERE ");
                    whereClauseAdded = true;
                    foreach (string query in crQuery.Queries)
                    {
                        sb.Append(query);
                    }
                }

                // check if CompanyID not Maritime
                if (crQuery.CompanyTypeID != 99)
                {
                    // Where clause required since crQuery.Queries is empty
                    if (!whereClauseAdded)
                    {
                        sb.Append(" WHERE [ImportExport].CompanyID = ").Append(crQuery.CompanyID);
                    }
                    else sb.Append(" AND [ImportExport].CompanyID = ").Append(crQuery.CompanyID);
                }

                sb.Append(" ORDER BY [ImportExport].[ID]");

                /// <remarks>
                /// Use the built selectCommand to retrieve
                /// the records from the database
                /// </remarks>
                using (SqlConnection conx = Connection)
                {
                    SqlCommand command = new SqlCommand(sb.ToString(), conx);

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);

                    /// <remarks>
                    /// Construct a Json Array of imports with statuses embeded to each.
                    /// </remarks>
                    JArray result = new JArray();
                    long importExportID;

                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = ds.Tables[0].Rows[i];
                        JObject jImport = new JObject();

                        importExportID = Convert.ToInt64(row["ImportExportID"]);

                        // get the statuses for the current ImportExportID
                        var importStatuses = ds.Tables[0].AsEnumerable().Where(x => x.Field<long>("ImportExportID") == importExportID);

                        foreach (DataColumn column in row.Table.Columns)
                        {
                            // skip the ImportExportID, Status and Status Date
                            if (column.ColumnName.Contains("ImportExportID")
                                || column.ColumnName.Equals("Status Date")
                                || column.ColumnName.Equals("Status")) continue;

                            // if column name is status, break outta the loop
                            if (column.ColumnName.ToLower().Equals("status")) break;
                            jImport.Add(new JProperty(column.ColumnName, row[column.ColumnName]));
                        }

                        JArray statuses = new JArray();
                        foreach (DataRow rowStats in importStatuses)
                        {
                            statuses.Add(new JObject(
                                new JProperty("Status", rowStats["Status"]),
                                new JProperty("StatusDate", rowStats["Status Date"])
                                ));
                        }

                        // add the statues array to its corresponding jImport
                        jImport.Add(new JProperty("Statuses", statuses));

                        // add the jImport to the array that will be returned
                        result.Add(jImport);

                        i += importStatuses.Count() - 1;
                    }

                    return result;
                }
            }
            catch (SqlException e)
            {
                Console.Write(e.Message + " >> " + e.Procedure + " >> " + e.LineNumber + " >> " + e.Data);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }

            return null;
        }

        public List<string> GetTables()
        {
            /// <summary>
            /// List of tables to be returned to the view for selection.
            /// Each table's columns would be retrieved using the GetColumns method
            /// and passing in the tableName as a parameter
            /// </summary>
            var tables = new List<string>();
            tables.AddRange(new string[] {
                "Cargo", "Carrier", "Company", "CompanyType", "Cost",
                "Country", "Export", "ImpExpType", "Import",
                "ImportExport", "ImportExportReason", "StatusUpdate",
                "IncoTerms", "Item", "ItemDetail", "Location", "ModeOfTransport",
                "Person", "Ports", "Status", "SubCargo"//, "Vessel"
            });
            return tables;

            //using (SqlConnection con = DataAccessLayer.getConnectionToMarilogDB())
            //{
            //    var schema = con.GetSchema("Tables"); // get tables information
            //    foreach (DataRow field in schema.Rows)
            //    {
            //        //Console.WriteLine(field[schema.Columns[0]]);
            //        var y = schema.Columns["TABLE_NAME"]; // get tables
            //        Console.WriteLine(y.ColumnName + " = " + field[y].ToString()); // field[y] is the table name
            //        foreach (DataColumn col in schema.Columns)
            //        {
            //            Console.WriteLine(col.ColumnName + " = " + field[col]);
            //        }
            //    }
            //    //SqlCommand cmd = new SqlCommand("select * from " + tableName, con);
            //    //SqlDataReader myReader;
            //    //myReader = cmd.ExecuteReader(CommandBehavior.KeyInfo);
            //    //var schemaTable = myReader.GetSchemaTable();
            //    //foreach (DataRow myField in schemaTable.Rows)
            //    //{
            //    //    var property = schemaTable.Columns[0];
            //    //    Console.WriteLine(property.ColumnName + " = " + myField[property].ToString());
            //    //    //foreach (DataColumn property in schemaTable.Columns)
            //    //    //{
            //    //    //    Console.WriteLine(property.ColumnName + " = " + myField[property].ToString());
            //    //    //}
            //    //}
            //    //myReader.Close();
            //    con.Close();
            //}
        }

        public List<string> GetColumns(string tableName)
        {
            using (SqlConnection conx = Connection)
            {
                if (tableName == "undefined") return null;
                SqlCommand cmd = new SqlCommand("select * from " + tableName, conx);
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.KeyInfo);
                var schemaTable = myReader.GetSchemaTable();
                List<string> columns = new List<string>();

                foreach (DataRow myField in schemaTable.Rows)
                {
                    ///<summary>
                    /// Adds all the columns for the selected table
                    /// to a list and returns that to the view.
                    ///</summary>

                    var property = schemaTable.Columns["ColumnName"];
                    columns.Add(myField[property].ToString());
                }

                myReader.Close();
                return columns;
            }

        }

    }
}
