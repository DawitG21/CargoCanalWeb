using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace com.AppliedLine.CargoCanal.Models
{
    public class DataAccessLayerReports
    {
        #region "Connection"
        private SqlConnection SqlConnection
        {
            get { return DataAccessLayer.getConnectionToMarilogDB(); }
        }
        #endregion
        private void formatReportFilterDates(ReportFilter reportFilter)
        {
            if (string.IsNullOrEmpty(reportFilter.DocumentType)) reportFilter.DocumentType = "all";
            if (reportFilter.FromDate.Year == 1) reportFilter.FromDate = new DateTime(1900, 1, 1);
            if (reportFilter.ToDate.Year == 1) reportFilter.ToDate = DateTime.Now;

            reportFilter.ToDate = reportFilter.ToDate.Date.Add(new TimeSpan(23, 59, 59)); // i.e. 2016-04-29 23:59:59
            reportFilter.FromDate = reportFilter.FromDate.Date;
        }

        private StatusSummary statusSummaryReport(ReportFilter reportFilter)
        {
            using (SqlConnection connection = this.SqlConnection)
            {
                // TODO: retrieve imports in the specific date
                string commandText = "SELECT ImportExportID FROM ImportExport"
                    + " WHERE [ImpExpTypeID] = 1" 
                    + " AND [DateInserted] >='" + reportFilter.FromDate + "'"
                    + " AND [DateInserted] <='" + reportFilter.ToDate + "'";

                // check CompanyID not CC or GOV
                if (!reportFilter.CompanyID.Equals("1".PadLeft(10, '0')) && !reportFilter.CompanyID.Equals("0".PadLeft(10, '0')))
                {
                    commandText += " AND [ImportExport].[ReceiverID] ='" + reportFilter.CompanyID + "'";
                }

                SqlCommand command = new SqlCommand(commandText, connection);
                SqlDataReader dr = command.ExecuteReader();

                string query = " [ImportExport].[ImportExportID] IN(";

                while (dr.Read())
                {
                    query += dr.GetString(0) + ",";
                }

                query = query.Substring(0, query.Length - 1) + ")";

                // add query to reportFilter
                if (reportFilter.Queries.Count > 0) query = " AND " + query;
                reportFilter.Queries.Add(query);

                // get Statuses(14, 57, 8, 10, 11, 12, 13) of the importExportIDs
                StatusSummary statusSummary = new StatusSummary();

                reportFilter.ReportName = "14";
                statusSummary.OnVoyage.ImportExport = GetByStatusReport(reportFilter, false);
                
                reportFilter.ReportName = "57";
                statusSummary.Discharged.ImportExport = GetByStatusReport(reportFilter, false);

                reportFilter.ReportName = "8";
                statusSummary.UCC.ImportExport = GetByStatusReport(reportFilter, false);

                reportFilter.ReportName = "10";
                statusSummary.CRL.ImportExport = GetByStatusReport(reportFilter, false);

                reportFilter.ReportName = "11";
                statusSummary.Dispatched.ImportExport = GetByStatusReport(reportFilter, false);

                reportFilter.ReportName = "12";
                statusSummary.UC1.ImportExport = GetByStatusReport(reportFilter, false);

                reportFilter.ReportName = "13";
                statusSummary.Delivered.ImportExport = GetByStatusReport(reportFilter, false);

                // TODO: function(importExportIDs, stats1, stats2) returns day diffs
                // build a JObject containing each ID e.g. OnVoyage.Days = { "0000001234" : 6, "0000009855" : 3 }
                // OnVoyage.ImportExport = DataTable

                // Build on voyage days
                statusSummary.OnVoyage.Days = statusSummaryDays(statusSummary.OnVoyage.ImportExport, statusSummary.Discharged.ImportExport);

                // Build on Discharged days
                statusSummary.Discharged.Days = statusSummaryDays(statusSummary.Discharged.ImportExport, statusSummary.UCC.ImportExport);
                
                // Build on UCC days
                statusSummary.UCC.Days = statusSummaryDays(statusSummary.UCC.ImportExport, statusSummary.CRL.ImportExport);
                
                // Build on CRL days
                statusSummary.CRL.Days = statusSummaryDays(statusSummary.CRL.ImportExport, statusSummary.Dispatched.ImportExport);
                
                // Build on Dispatched days
                statusSummary.Dispatched.Days = statusSummaryDays(statusSummary.Dispatched.ImportExport, statusSummary.UC1.ImportExport);
                
                // Build on UC1 days
                statusSummary.UC1.Days = statusSummaryDays(statusSummary.UC1.ImportExport, statusSummary.Delivered.ImportExport);
                
                // TODO : NOT REQUIRED --- Build on Delivered days
                //statusSummary.Delivered.Days = statusSummaryDays(statusSummary.Delivered.ImportExport, statusSummary.Discharged.ImportExport);
                
                return statusSummary;
            }
        }

        private JObject statusSummaryDays(DataTable firstStats, DataTable secondStats)
        {
            JObject jDays = new JObject();

            foreach (var firstRow in firstStats.AsEnumerable())
            {
                string importExportID = firstRow["ImportExportID"].ToString();

                var secondRow = secondStats.AsEnumerable().FirstOrDefault(x => x.Field<string>("ImportExportID") == importExportID);

                DateTime date1 = Convert.ToDateTime(firstRow["StatusDate"].ToString());
                DateTime date2 = DateTime.Now;

                if (secondRow != null) date2 = Convert.ToDateTime(secondRow["StatusDate"].ToString());

                // build the JObject
                jDays.Add(new JProperty(importExportID, date2.Subtract(date1).Days));
            }

            return jDays;
        }

        private DataTable pendingReport(ReportFilter reportFilter)
        {
            using (SqlConnection connection = this.SqlConnection)
            {
                SqlCommand command = new SqlCommand("sp_PendingDocumentsReport", connection);
                command.CommandType = CommandType.StoredProcedure;

                var pDocumentType = command.Parameters.Add("@DocumentType", SqlDbType.NVarChar);
                var pCompanyID = command.Parameters.Add("@CompanyID", SqlDbType.NVarChar);
                var pFromDate = command.Parameters.Add("@FromDate", SqlDbType.DateTime);
                var pToDate = command.Parameters.Add("@ToDate", SqlDbType.DateTime);

                pDocumentType.SqlValue = reportFilter.DocumentType;
                pCompanyID.SqlValue = reportFilter.CompanyID;
                pFromDate.SqlValue = reportFilter.FromDate;
                pToDate.SqlValue = reportFilter.ToDate;

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet ds = new DataSet("dsPendingDocuments");
                da.Fill(ds);

                return ds.Tables[0];
            }
        }

        private DataTable problemsReport(ReportFilter reportFilter)
        {
            using (SqlConnection connection = this.SqlConnection)
            {
                SqlCommand command = new SqlCommand("sp_ProblemsDocumentsReport", connection);
                command.CommandType = CommandType.StoredProcedure;

                var pDocumentType = command.Parameters.Add("@DocumentType", SqlDbType.NVarChar);
                var pCompanyID = command.Parameters.Add("@CompanyID", SqlDbType.NVarChar);
                var pFromDate = command.Parameters.Add("@FromDate", SqlDbType.DateTime);
                var pToDate = command.Parameters.Add("@ToDate", SqlDbType.DateTime);

                pDocumentType.SqlValue = reportFilter.DocumentType;
                pCompanyID.SqlValue = reportFilter.CompanyID;
                pFromDate.SqlValue = reportFilter.FromDate;
                pToDate.SqlValue = reportFilter.ToDate;

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet ds = new DataSet("dsProblemsDocuments");
                da.Fill(ds);

                return ds.Tables[0];
            }
        }

        private Demurrages demurrageReport(ReportFilter reportFilter)
        {
            ConfigStatsTime daysBeforeDemurrage = reportFilter.ConfigStatsTimes.Find(x => x.Code.ToUpper().Equals("DCH"));

            // set the default to 8 days if demurrage days is not defined
            if (daysBeforeDemurrage == null) daysBeforeDemurrage = new ConfigStatsTime() { Days = 8 };

            string commandText = "SELECT CONCAT([Person].FirstName, ' ', [Person].LastName) [Name],"
                + " [Company].CompanyName [Company], [Person].Phone, [Person].Email,"
                + " [ImportExport].ReferenceNo,"
                + " [Item].Description,"
                + " [Import].ImportID [RecordNo], [Import].BillOfLading [DocumentNo], [Import].BillOfLadingDate [DocumentDate],"
                + " t.Name [DocType],"
                + " [ImportExportStatusUpdate].StatusDate [DischargedDate],"
                + " DATEDIFF(DAY, DATEADD(DAY, " + daysBeforeDemurrage.Days 
                + ", [ImportExportStatusUpdate].StatusDate), dbo.fn_StatusDateAfterDischarged([Import].ImportID)) [DemurrageDays]"
                + " FROM ImportExport"

                + " JOIN [Import] ON [ImportExport].ImportExportID = [Import].ImportID"
                + " JOIN [ImpExpType] AS t ON [ImportExport].ImpExpTypeID =  t.ID"
                + " JOIN [Item]  ON [ImportExport].ImportExportID = [Item].ImportExportID"
                + " JOIN [ItemDetail] ON [Item].ItemID = [ItemDetail].ItemID"
                + " JOIN [Units] [WUNIT] ON Item.WeightUnitID = [WUNIT].ID"
                + " JOIN [Units] [VUNIT] ON Item.VolumeUnitID = [VUNIT].ID"
                + " JOIN [StuffMode] ON [ItemDetail].StuffModeID = [StuffMode].ID"
                + " JOIN [Cargo] ON [Item].CargoID = [Cargo].ID"
                + " JOIN [SubCargo] ON [Item].SubCargoID = [SubCargo].ID"
                + " JOIN [Carrier] ON [ImportExport].CarrierID = [Carrier].ID"
                + " JOIN [Vessel] ON [ImportExport].VesselID = [Vessel].ID"
                + " JOIN [Ports] ON [ImportExport].PortOfLoadingID = [Ports].ID"
                + " JOIN [Country] ON [Ports].CountryID = [Country].ID"
                + " JOIN [ModeOfTransport] ON [ImportExport].ModeOfTransportID = [ModeOfTransport].ID"
                + " JOIN [Cost] ON [ImportExport].ImportExportID = [Cost].ImportExportID"
                + " JOIN [ImpExpType] ON [ImportExport].ImpExpTypeID = [ImpExpType].ID"
                + " JOIN [IncoTerms] ON [ImportExport].IncoTermID = [IncoTerms].ID"
                + " JOIN [ImportExportReason] ON [ImportExport].ImportExportReasonID = [ImportExportReason].ID"
                + " JOIN [Person] ON [ImportExport].ConsigneeID = [Person].PersonID"
                + " JOIN [Company] ON [ImportExport].ReceiverID = [Company].CompanyID"
                + " JOIN [CompanyType] ON [Company].CompanyTypeID = [CompanyType].ID"
                + " JOIN [ImportExportStatusUpdate] ON [ImportExport].ImportExportID = [ImportExportStatusUpdate].ImportExportID"
                + " JOIN [Status] ON [ImportExportStatusUpdate].StatusID = [Status].ID"
                + " WHERE ([ImportExport].DateInserted BETWEEN '"
                + reportFilter.FromDate + "' AND '" + reportFilter.ToDate + "')"
                + " AND [ImportExportStatusUpdate].StatusID = '57'";

            // check CompanyID not CC or GOV
            if (!reportFilter.CompanyID.Equals("1".PadLeft(10, '0')) && !reportFilter.CompanyID.Equals("0".PadLeft(10, '0')))
            {
                commandText += " AND [ImportExport].[ReceiverID] ='" + reportFilter.CompanyID + "'";
            }

            if (reportFilter.Queries != null && reportFilter.Queries.Count > 0)
            {
                /// <summary>
                /// Build the WHERE clause for the select statement
                /// </summary>

                commandText += " AND ";

                foreach (string query in reportFilter.Queries)
                {
                    commandText += query;
                }
            }

            using (SqlConnection connection = this.SqlConnection)
            {
                //SqlCommand command = new SqlCommand("sp_DemurrageDocumentsReport", connection);
                //command.CommandType = CommandType.StoredProcedure;

                //var pDocumentType = command.Parameters.Add("@DocumentType", SqlDbType.NVarChar);
                //var pCompanyID = command.Parameters.Add("@CompanyID", SqlDbType.NVarChar);
                //var pFromDate = command.Parameters.Add("@FromDate", SqlDbType.DateTime);
                //var pToDate = command.Parameters.Add("@ToDate", SqlDbType.DateTime);
                //var pDaysBeforeDemurrage = command.Parameters.Add("@DaysBeforeDemurrage", SqlDbType.Int);

                //pDocumentType.SqlValue = reportFilter.DocumentType;
                //pCompanyID.SqlValue = reportFilter.CompanyID;
                //pFromDate.SqlValue = reportFilter.FromDate;
                //pToDate.SqlValue = reportFilter.ToDate;
                //pDaysBeforeDemurrage.SqlValue = daysBeforeDemurrage.Days;

                SqlCommand command = new SqlCommand(commandText, connection);
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet ds = new DataSet("dsDemurrageDocuments");
                da.Fill(ds);

                // Build the query for GetCustomReport
                string query = "[ImportExport].[ImportExportID] IN(";
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string importExportID = row["RecordNo"].ToString();
                    query += "'" + importExportID + "',";
                }

                // get rid of the last comma and append a closing bracket to the query
                query = query.Substring(0, query.Length - 1) + ")";

                Demurrages demurrages = new Demurrages()
                {
                    DemurrageTable = ds.Tables[0],
                    DemmurageImportsWithStatuses = GetCustomImportsReport(
                        new List<string>() { query }) // Gets the Imports With Statuses for 
                                                      // the selected demurrages based on query
                };

                return demurrages;
            }
        }

        private JObject cargoWeightReport(ReportFilter reportFilter)
        {
            using (SqlConnection connection = this.SqlConnection)
            {
                string commandText = "SELECT [Import].[BillOfLading] [BoL]"
                    + ", [Import].[BillOfLadingDate] [BoL Date]"
                    + ", [Person].[FirstName] + ' ' + [Person].[LastName] [Agent]"
                    + ", [Person].[Email]"
                    + ", [Person].[Phone]"
                    + ", [Company].[CompanyName] [Company]"
                    + ", [Item].[DateInserted] [Date Inserted]"
                    + ", [Ports].[PortName] + ', ' + [Country].[Name] [Origin]"
                    + ", [Item].[ItemOrderNo] [Order Number]"
                    + ", [Item].[Quantity] [QTY]"
                    + ", [Item].[NetWeight] [Net]"
                    + ", [Item].[GrossWeight] [Gross]"
                    + ", [WUNIT].[UnitName] [W. Unit]"
                    + ", [Item].[Quantity] / [WUNIT].[BaseUnitRate] [TON]"
                    + ", [Item].[Quantity] / [WUNIT].[BaseUnitRate] * 907.185 [KG]"
                    + ", [Item].[Volume]"
                    + ", [VUNIT].[UnitName] [V. Unit]"
                    + ", [Item].[Dangerous]"
                    + ", [Item].[Description] FROM [ImportExport]"
                    + " JOIN [Import] ON [ImportExport].[ImportExportID] = [Import].[ImportID]"
                    + " JOIN [Item] ON [ImportExport].[ImportExportID] = [Item].[ImportExportID]"
                    + " JOIN [Units] [WUNIT] ON [Item].WeightUnitID = [WUNIT].[ID]"
                    + " JOIN [Units] [VUNIT] ON [Item].VolumeUnitID = [VUNIT].[ID]"
                    + " JOIN [Person] ON [ImportExport].[ConsigneeID] = [Person].[PersonID]"
                    + " JOIN [Company] ON [ImportExport].[ReceiverID] = [Company].[CompanyID]"
                    + " JOIN [Ports] ON [ImportExport].[PortOfLoadingID] = [Ports].[ID]"
                    + " JOIN [Country] ON [Ports].[CountryID] = [Country].[ID]"
                    + " WHERE [Item].[DateInserted] >= '" + reportFilter.FromDate + "'"
                    + " AND [Item].[DateInserted] <= '" + reportFilter.ToDate + "'";

                // check CompanyID not CC or GOV
                if (!reportFilter.CompanyID.Equals("1".PadLeft(10, '0')) && !reportFilter.CompanyID.Equals("0".PadLeft(10, '0')))
                {
                    commandText += " AND [ImportExport].[ReceiverID] ='" + reportFilter.CompanyID + "'";
                }

                commandText += " ORDER BY [Company]";

                SqlCommand command = new SqlCommand(commandText, connection);

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet ds = new DataSet("dsCargoWeight");
                da.Fill(ds);

                JArray data = new JArray();
                decimal kg = 0;
                decimal ton = 0;
                long recordCount = ds.Tables[0].Rows.Count;

                string company = string.Empty;

                for (var i = 0; i < recordCount; i++)
                {
                    company = ds.Tables[0].Rows[i]["Company"].ToString();
                    var companyRecords = ds.Tables[0].AsEnumerable().Where(x => x.Field<string>("Company") == company);

                    JArray records = new JArray();
                    foreach (DataRow record in companyRecords)
                    {
                        // record.Table.Columns[0].ColumnName
                        JObject recordJObject = new JObject();
                        foreach (DataColumn col in record.Table.Columns)
                        {
                            recordJObject.Add(new JProperty(col.ColumnName, record[col.ColumnName]));
                        }

                        kg += Convert.ToDecimal(record["KG"]);
                        ton += Convert.ToDecimal(record["TON"]);

                        records.Add(recordJObject);
                    }

                    JObject jsonObject = new JObject(
                        new JProperty("name", company),
                        new JProperty("data", records));

                    data.Add(jsonObject);

                    i += companyRecords.Count() - 1;

                }

                JObject result = new JObject(
                    new JProperty("data", data),
                    new JProperty("totals", new JObject(
                        new JProperty("kg", kg),
                        new JProperty("ton", ton),
                        new JProperty("count", recordCount))));

                return result;
            }
        }

        private JObject cargoInTransitWeightReport(ReportFilter reportFilter)
        {
            using (SqlConnection connection = this.SqlConnection)
            {
                string commandText = "SELECT [Import].[BillOfLading] [BoL]"
                    + ", [Import].[BillOfLadingDate] [BoL Date]"
                    + ", [ImportExportStatusUpdate].[StatusDate] [Dispatch Date]"
                    + ", [Person].[FirstName] + ' ' + [Person].[LastName] [Agent]"
                    + ", [Person].[Email]"
                    + ", [Person].[Phone]"
                    + ", [Company].[CompanyName] [Company]"
                    + ", [Item].[DateInserted] [Date Inserted]"
                    + ", [Ports].[PortName] + ', ' + [Country].[Name] [Origin]"
                    + ", [Item].[ItemOrderNo] [Order Number]"
                    + ", [Item].[Quantity] [QTY]"
                    + ", [Item].[NetWeight] [Net]"
                    + ", [Item].[GrossWeight] [Gross]"
                    + ", [WeightUnit].[UnitName] [W. Unit]"
                    + ", [Item].[Quantity] / [WeightUnit].[BaseUnitRate] [TON]"
                    + ", [Item].[Quantity] / [WeightUnit].[BaseUnitRate] * 907.185 [KG]"
                    + ", [Item].[Volume]"
                    + ", [VolumeUnit].[UnitName] [V. Unit]"
                    + ", [Item].[Dangerous]"
                    + ", [Item].[Description] FROM[ImportExport]"
                    + " JOIN[Import] ON[ImportExport].[ImportExportID] = [Import].[ImportID]"
                    + " JOIN[Item] ON[ImportExport].[ImportExportID] = [Item].[ImportExportID]"
                    + " JOIN[Units] [WeightUnit] ON [Item].WeightUnitID = [WeightUnit].[ID]"
                    + " JOIN[Units] [VolumeUnit] ON [Item].VolumeUnitID = [VolumeUnit].[ID]"
                    + " JOIN[Person] ON[ImportExport].[ConsigneeID] = [Person].[PersonID]"
                    + " JOIN[Company] ON[ImportExport].[ReceiverID] = [Company].[CompanyID]"
                    + " JOIN [ImportExportStatusUpdate] ON [ImportExport].[ImportExportID] = [ImportExportStatusUpdate].[ImportExportID]"
                    + " JOIN[Ports] ON[ImportExport].[PortOfLoadingID] = [Ports].[ID]"
                    + " JOIN[Country] ON[Ports].[CountryID] = [Country].[ID]"
                    + " WHERE [ImportExportStatusUpdate].[StatusDate] >= '" + reportFilter.FromDate + "'"
                    + " AND [ImportExportStatusUpdate].[StatusDate] <= '" + reportFilter.ToDate + "'"
                    + " AND [ImportExportStatusUpdate].[StatusID] = 11";

                // check CompanyID not CC or GOV
                if (!reportFilter.CompanyID.Equals("1".PadLeft(10, '0')) && !reportFilter.CompanyID.Equals("0".PadLeft(10, '0')))
                {
                    commandText += " AND [ImportExport].[ReceiverID] ='" + reportFilter.CompanyID + "'";
                }

                commandText += " ORDER BY [Company]";

                SqlCommand command = new SqlCommand(commandText, connection);

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet ds = new DataSet("dsCargoInTransitWeight");
                da.Fill(ds);

                JArray data = new JArray();
                decimal kg = 0;
                decimal ton = 0;
                long recordCount = ds.Tables[0].Rows.Count;

                string company = string.Empty;

                for (var i = 0; i < recordCount; i++)
                {
                    company = ds.Tables[0].Rows[i]["Company"].ToString();
                    var companyRecords = ds.Tables[0].AsEnumerable().Where(x => x.Field<string>("Company") == company);

                    JArray records = new JArray();
                    foreach (DataRow record in companyRecords)
                    {
                        // record.Table.Columns[0].ColumnName
                        JObject recordJObject = new JObject();
                        foreach (DataColumn col in record.Table.Columns)
                        {
                            recordJObject.Add(new JProperty(col.ColumnName, record[col.ColumnName]));
                        }

                        kg += Convert.ToDecimal(record["KG"]);
                        ton += Convert.ToDecimal(record["TON"]);

                        records.Add(recordJObject);
                    }

                    JObject jsonObject = new JObject(
                        new JProperty("name", company),
                        new JProperty("data", records));

                    data.Add(jsonObject);

                    i += companyRecords.Count() - 1;

                }

                JObject result = new JObject(
                    new JProperty("data", data),
                    new JProperty("totals", new JObject(
                        new JProperty("kg", kg),
                        new JProperty("ton", ton),
                        new JProperty("count", recordCount))));

                return result;
            }
        }

        public object GetReport(ReportFilter reportFilter)
        {
            try
            {
                formatReportFilterDates(reportFilter);

                switch (reportFilter.ReportName.ToLower())
                {
                    case "statussummaryreport":
                        return statusSummaryReport(reportFilter);
                    case "pendingreport":
                        return pendingReport(reportFilter);
                    case "problemsreport":
                        return problemsReport(reportFilter);
                    case "demurragereport":
                        return demurrageReport(reportFilter);
                    case "cargoweightreport":
                        return cargoWeightReport(reportFilter);
                    case "cargointransitweightreport":
                        return cargoInTransitWeightReport(reportFilter);
                    default:
                        // Fetch Status Reports
                        return GetByStatusReport(reportFilter, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public DataTable GetByStatusReport(ReportFilter reportFilter, bool useStatusDate)
        {
            try
            {
                /// <summary>
                /// Build the query to select all fields from required tables
                /// </summary>

                StringBuilder sb = new StringBuilder("SELECT ");
                sb.Append(" [Import].[BillOfLading], [Import].[LoadingDate], [Import].[BillOfLadingDate], [Status].[Description] [Status], [ImportExportStatusUpdate].[StatusDate]");
                sb.Append(", [ImportExport].[ImportExportID], [ImportExport].[ReferenceNo],[ImportExport].[DateInserted], [ImportExport].[DateDischarge], [ImportExport].[DateInitiated], [ImportExport].[Remark], [ImportExport].[RemarkDate],[ImportExport].[ReImportExport], [ImportExport].[Unimodal], [ImportExport].[BillTerminated], [ImportExport].[Completed]");
                sb.Append(", [Item].[DateInserted], [Item].[ImportExportID], [Item].[ItemOrderNo], [Item].[GrossWeight], [Item].[NetWeight], [WUNIT].UnitName [W.Unit], [Item].[Volume], [VUNIT].UnitName [V.Unit], [Item].[Quantity], [Item].[Dangerous], [Item].[Description] [ItemDesc]");
                sb.Append(", [ItemDetail].[ItemNumber], [ItemDetail].[StatusDate] [ItemDetailStatusDate]");
                sb.Append(", [StuffMode].[Description] [StuffMode]");
                sb.Append(", [Cargo].[Cargo]");
                sb.Append(", [SubCargo].[Description] [SubCargo]");
                sb.Append(", [Carrier].[Name] [Carrier]");
                sb.Append(", [Vessel].[Name] [Vessel], [ImportExport].[VoyageNumber]");
                sb.Append(", [Ports].[PortName]");
                sb.Append(", [Country].[Name] [Country], [Country].[Region]");
                sb.Append(", [ModeOfTransport].[Mode]");
                sb.Append(", [Cost].[Freight], [Cost].[Insurance], [Cost].[InlandTransport], [Cost].[PortStorageDemurrage], [Cost].[InlandStorage], [Cost].[PortHandling], [Cost].[TruckDetention], [Cost].[DryPortCharge], [Cost].[MaterialCost], [Cost].[OtherChargesBirr], [Cost].[OtherChargesDollar], [Cost].[TotalBirr], [Cost].[TotalDollar]");
                sb.Append(", [ImpExpType].[Name] [ShipmentType]");
                sb.Append(", [IncoTerms].[IncoName] [DeliveryTerms]");
                sb.Append(", [ImportExportReason].[Reason]");
                sb.Append(", [Person].[FirstName] + ' ' + [Person].[LastName] [AgentName], [Person].[Phone], [Person].[Email]");
                sb.Append(", [Company].[CompanyName], [Company].[Address], [Company].[TownCity], [Company].[State], [Company].[POBox], [Company].[Wereda], [Company].[KefleKetema], [Company].[Kebele], [Company].[HouseNo], [Company].[CountryID], [Company].[Telephone], [Company].[ContactName], [Company].[ContactMobile], [Company].[Email] [CompanyEmail], [Company].[Website], [Company].[TIN], [Company].[LicenseNumber], [Company].[LicenseUnder], [Company].[LastRenewedDate], [Company].[LicenseIssuedDate]");
                sb.Append(", [CompanyType].[Name] [CompanyType]");
                sb.Append(" FROM [ImportExport]");

                sb.Append(" JOIN [Import]  ON [ImportExport].ImportExportID = [Import].ImportID");
                sb.Append(" JOIN [Item]  ON [ImportExport].ImportExportID = [Item].ImportExportID");
                sb.Append(" JOIN [ItemDetail] ON [Item].ItemID = [ItemDetail].ItemID");
                sb.Append(" JOIN [Units] [WUNIT] ON Item.WeightUnitID = [WUNIT].ID");
                sb.Append(" JOIN [Units] [VUNIT] ON Item.VolumeUnitID = [VUNIT].ID");
                sb.Append(" JOIN [StuffMode] ON [ItemDetail].StuffModeID = [StuffMode].ID");
                sb.Append(" JOIN [Cargo] ON [Item].CargoID = [Cargo].ID");
                sb.Append(" JOIN [SubCargo] ON [Item].SubCargoID = [SubCargo].ID");
                sb.Append(" JOIN [Carrier] ON [ImportExport].CarrierID = [Carrier].ID");
                sb.Append(" JOIN [Vessel] ON [ImportExport].VesselID = [Vessel].ID");
                sb.Append(" JOIN [Ports] ON [ImportExport].PortOfLoadingID = [Ports].ID");
                sb.Append(" JOIN [Country] ON [Ports].CountryID = [Country].ID");
                sb.Append(" JOIN [ModeOfTransport] ON [ImportExport].ModeOfTransportID = [ModeOfTransport].ID");
                sb.Append(" JOIN [Cost] ON [ImportExport].ImportExportID = [Cost].ImportExportID");
                sb.Append(" JOIN [ImpExpType] ON [ImportExport].ImpExpTypeID = [ImpExpType].ID");
                sb.Append(" JOIN [IncoTerms] ON [ImportExport].IncoTermID = [IncoTerms].ID");
                sb.Append(" JOIN [ImportExportReason] ON [ImportExport].ImportExportReasonID = [ImportExportReason].ID");
                sb.Append(" JOIN [Person] ON [ImportExport].ConsigneeID = [Person].PersonID");
                sb.Append(" JOIN [Company] ON [ImportExport].ReceiverID = [Company].CompanyID");
                sb.Append(" JOIN [CompanyType] ON [Company].CompanyTypeID = [CompanyType].ID");
                sb.Append(" JOIN [ImportExportStatusUpdate] ON [ImportExport].ImportExportID = [ImportExportStatusUpdate].ImportExportID");
                sb.Append(" JOIN [Status] ON [ImportExportStatusUpdate].StatusID = [Status].ID");
                sb.Append(" WHERE ");

                // check CompanyID not CC or GOV
                if (!reportFilter.CompanyID.Equals("1".PadLeft(10, '0')) && !reportFilter.CompanyID.Equals("0".PadLeft(10, '0')))
                {
                    sb.Append(" [ImportExport].[ReceiverID] ='").Append(reportFilter.CompanyID).Append("' AND");
                }

                if (reportFilter.Queries.Count > 0)
                {
                    /// <summary>
                    /// Build the WHERE clause for the select statement
                    /// </summary>

                    foreach (string query in reportFilter.Queries)
                    {
                        sb.Append(query);
                    }

                    sb.Append(" AND");
                }

                sb.Append(" [ImportExportStatusUpdate].StatusID ='").Append(reportFilter.ReportName).Append("'");

                if (useStatusDate)
                {
                    sb.Append(" AND [ImportExportStatusUpdate].[StatusDate] >= '").Append(reportFilter.FromDate).Append("'");
                    sb.Append(" AND [ImportExportStatusUpdate].[StatusDate] <= '").Append(reportFilter.ToDate).Append("'");
                }
                
                sb.Append(" ORDER BY [Company].[CompanyName]");

                /// <remarks>
                /// Use the built selectCommand to retrieve
                /// Status records from the database
                /// </remarks>
                using (SqlConnection conx = DataAccessLayer.getConnectionToMarilogDB())
                {
                    SqlCommand command = new SqlCommand(sb.ToString(), conx);

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    return ds.Tables[0];
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

        public JArray GetCustomImportsReport(List<string> queries)
        {
            try
            {
                /// <summary>
                /// Build the query to select all fields from required tables
                /// </summary>
                StringBuilder sb = new StringBuilder("SELECT ");
                sb.Append("[ImportExport].[ImportExportID], [ImportExport].[ReferenceNo],[ImportExport].[DateInserted], [ImportExport].[DateDischarge], [ImportExport].[DateInitiated], [ImportExport].[VoyageNumber], [ImportExport].[Remark], [ImportExport].[RemarkDate], [ImportExport].[ReImportExport], [ImportExport].[Unimodal], [ImportExport].[BillTerminated], [ImportExport].[Completed]");
                sb.Append(", [Import].[LoadingDate], [Import].[BillOfLading], [Import].[BillOfLadingDate]");
                sb.Append(", [Item].[DateInserted], [Item].[ItemOrderNo], [Item].[GrossWeight], [Item].[NetWeight], [WUNIT].UnitName [W.Unit], [Item].[Volume], [VUNIT].UnitName [V.Unit], [Item].[Quantity], [Item].[Dangerous], [Item].[Description]");
                sb.Append(", [ItemDetail].[DateInserted], [ItemDetail].[ItemNumber]");
                sb.Append(", [StuffMode].[Description] [StuffMode]");
                sb.Append(", [Cargo].[Cargo]");
                sb.Append(", [SubCargo].[Description] [SubCargo], [SubCargo].[UnitID] [SubCargoUnit], [SubCargo].[HasDim]");
                sb.Append(", [Carrier].[Name] [Carrier]");
                sb.Append(", [Vessel].[Name] [Vessel]");
                sb.Append(", [Ports].[PortName] [Origin], [Ports].[IsDryPort]");
                sb.Append(", [Country].[Name] [Country], [Country].[Region]");
                sb.Append(", [ModeOfTransport].[Mode] [Transport]");
                sb.Append(", [Cost].[Freight] [FreightCost], [Cost].[Insurance] [InsuranceCost], [Cost].[InlandTransport] [InlandTransportCost], [Cost].[PortStorageDemurrage] [PortStorageDemurrageCost], [Cost].[InlandStorage] [InlandStorageCost], [Cost].[PortHandling] [PortHandlingCost], [Cost].[TruckDetention] [TruckDetentionCost], [Cost].[DryPortCharge] [DryPortChargeCost], [Cost].[MaterialCost], [Cost].[OtherChargesBirr], [Cost].[OtherChargesDollar], [Cost].[TotalBirr], [Cost].[TotalDollar]");
                sb.Append(", [ImpExpType].[Name] [ShipmentType], [ImpExpType].[Description] [ShipmentTypeDesc]");
                sb.Append(", [IncoTerms].[IncoName] [DeliveryTerms]");
                sb.Append(", [ImportExportReason].[Reason] [ShipmentReason]");
                sb.Append(", [Person].[LastName] [AgentLastName], [Person].[FirstName] [AgentFirstName], [Person].[MiddleName] [AgentMiddleName], [Person].[Phone] [AgentPhone], [Person].[Email] [AgentEmail]");
                sb.Append(", [Company].[CompanyName], [Company].[Address], [Company].[TownCity], [Company].[State], [Company].[POBox], [Company].[Wereda], [Company].[KefleKetema], [Company].[Kebele], [Company].[HouseNo], [Company].[CountryID], [Company].[Telephone], [Company].[ContactName], [Company].[ContactMobile], [Company].[Email], [Company].[Website], [Company].[TIN], [Company].[LicenseNumber], [Company].[LicenseUnder], [Company].[LastRenewedDate], [Company].[LicenseIssuedDate], [Company].[IsActive]");
                sb.Append(", [CompanyType].[Name] [CompanyType], [CompanyType].[Description] [CompanyTypeDesc]");
                sb.Append(", [Status].[Description] [Status], [ImportExportStatusUpdate].[StatusDate]");
                sb.Append(" FROM [ImportExport]");

                sb.Append(" JOIN [Import]  ON [ImportExport].ImportExportID = [Import].ImportID");
                sb.Append(" JOIN [Item]  ON [ImportExport].ImportExportID = [Item].ImportExportID");
                sb.Append(" JOIN [ItemDetail] ON [Item].ItemID = [ItemDetail].ItemID");
                sb.Append(" JOIN [Units] [WUNIT] ON Item.WeightUnitID = [WUNIT].ID");
                sb.Append(" JOIN [Units] [VUNIT] ON Item.VolumeUnitID = [VUNIT].ID");
                sb.Append(" JOIN [StuffMode] ON [ItemDetail].StuffModeID = [StuffMode].ID");
                sb.Append(" JOIN [Cargo] ON [Item].CargoID = [Cargo].ID");
                sb.Append(" JOIN [SubCargo] ON [Item].SubCargoID = [SubCargo].ID");
                sb.Append(" JOIN [Carrier] ON [ImportExport].CarrierID = [Carrier].ID");
                sb.Append(" JOIN [Vessel] ON [ImportExport].VesselID = [Vessel].ID");
                sb.Append(" JOIN [Ports] ON [ImportExport].PortOfLoadingID = [Ports].ID");
                sb.Append(" JOIN [Country] ON [Ports].CountryID = [Country].ID");
                sb.Append(" JOIN [ModeOfTransport] ON [ImportExport].ModeOfTransportID = [ModeOfTransport].ID");
                sb.Append(" JOIN [Cost] ON [ImportExport].ImportExportID = [Cost].ImportExportID");
                sb.Append(" JOIN [ImpExpType] ON [ImportExport].ImpExpTypeID = [ImpExpType].ID");
                sb.Append(" JOIN [IncoTerms] ON [ImportExport].IncoTermID = [IncoTerms].ID");
                sb.Append(" JOIN [ImportExportReason] ON [ImportExport].ImportExportReasonID = [ImportExportReason].ID");
                sb.Append(" JOIN [Person] ON [ImportExport].ConsigneeID = [Person].PersonID");
                sb.Append(" JOIN [Company] ON [ImportExport].ReceiverID = [Company].CompanyID");
                sb.Append(" JOIN [CompanyType] ON [Company].CompanyTypeID = [CompanyType].ID");
                sb.Append(" JOIN [ImportExportStatusUpdate] ON [ImportExport].ImportExportID = [ImportExportStatusUpdate].ImportExportID");
                sb.Append(" JOIN [Status] ON [ImportExportStatusUpdate].StatusID = [Status].ID");
                if (queries != null)
                {
                    /// <summary>
                    /// Build the WHERE clause for the select statement
                    /// </summary>
                    sb.Append(" WHERE ");
                    foreach (string query in queries)
                    {
                        sb.Append(query);
                    }

                    //// check CompanyID not CC or GOV
                    //if (!reportFilter.CompanyID.Equals("1".PadLeft(10, '0')) && !reportFilter.CompanyID.Equals("0".PadLeft(10, '0')))
                    //{
                    //    sb.Append(" AND [ImportExport].[ReceiverID] ='").Append(reportFilter.CompanyID).Append("'");
                    //}
                }

                sb.Append(" ORDER BY [ImportExport].[ImportExportID]");

                /// <remarks>
                /// Use the built selectCommand to retrieve
                /// the records from the database
                /// </remarks>
                using (SqlConnection conx = DataAccessLayer.getConnectionToMarilogDB())
                {
                    SqlCommand command = new SqlCommand(sb.ToString(), conx);

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);

                    /// <remarks>
                    /// Construct a Json Array of imports with statuses embeded to each.
                    /// </remarks>
                    JArray result = new JArray();
                    string importExportID = string.Empty;

                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = ds.Tables[0].Rows[i];
                        JObject jImport = new JObject();

                        importExportID = row["ImportExportID"].ToString();

                        // get the statuses for the current ImportExportID
                        var importStatuses = ds.Tables[0].AsEnumerable().Where(x => x.Field<string>("ImportExportID") == importExportID);

                        foreach (DataColumn column in row.Table.Columns)
                        {
                            // if column name is status, break outta the loop
                            if (column.ColumnName.ToLower().Equals("status")) break;
                            jImport.Add(new JProperty(column.ColumnName, row[column.ColumnName]));
                        }

                        JArray statuses = new JArray();
                        foreach (DataRow rowStats in importStatuses)
                        {
                            statuses.Add(new JObject(
                                new JProperty("Status", rowStats["Status"]),
                                new JProperty("StatusDate", rowStats["StatusDate"])
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
                "Country", "Customer", "Export", "ImpExpType", "Import",
                "ImportExport", "ImportExportReason", "ImportExportStatusUpdate",
                "IncoTerms", "Item", "ItemDetail", "Location", "ModeOfTransport",
                "Person", "Ports", "Status", "SubCargo", "Vessel"
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
            using (SqlConnection conx = DataAccessLayer.getConnectionToMarilogDB())
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
                conx.Close();
                return columns;
            }

        }

        public List<Import> GetImportExportReport(ReportFilter reportFilter)
        {
            formatReportFilterDates(reportFilter);

            using (SqlConnection conx = DataAccessLayer.getConnectionToMarilogDB())
            {
                string commandText = @"SELECT x.[ID], x.[ImportExportID],x.[ReferenceNo] ,x.[DateInserted]
                        ,x.[ImpExpTypeID], x.[ImportExportReasonID], x.[PortOfLoadingID], x.[PortOfDischargeID]
                        ,x.[DateDischarge], x.[ModeOfTransportID], x.[DateInitiated], x.[ConsigneeID]
                        ,x.[ReceiverID], x.[CarrierID], x.[VesselID], x.[VoyageNumber], x.[IncoTermID], x.[Remark]
                        ,x.[RemarkDate], x.[ReImportExport], x.[Unimodal], x.[BillTerminated], x.[Completed]
                        ,p.[FirstName] + ' ' + p.[LastName] ConsigneeName, p.[Email], p.[Phone], c.CompanyName
                        ,i.[BillOfLading]
                        FROM[ImportExport] x
                            JOIN Import i ON x.ImportExportID = i.ImportID
                            JOIN Person p ON x.ConsigneeID = p.PersonID
                            JOIN Company c on x.ReceiverID = c.CompanyID
                        WHERE x.DateInitiated >= '" + reportFilter.FromDate + "'"
                        + " AND x.DateInitiated <='" + reportFilter.ToDate + "'";

                // check CompanyID not CC or GOV
                if (!reportFilter.CompanyID.Equals("1".PadLeft(10, '0')) && !reportFilter.CompanyID.Equals("0".PadLeft(10, '0')))
                {
                    commandText += " AND x.[ReceiverID] ='" + reportFilter.CompanyID + "'";
                }

                commandText += " ORDER BY x.ImportExportID";


                SqlCommand cmd = new SqlCommand(commandText, conx);
                SqlDataReader reader = cmd.ExecuteReader();

                List<Import> importList = new List<Import>();

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
                    import.BillOfLading = reader.GetString(27);

                    import.ImportExportStatuses = DataAccessLayer.getclsImportExportStatusUpdateObj(import.ImportExportID);
                    importList.Add(import);
                }

                return importList;
            }
        }

        public List<TransitTimeModel> TransitTimeReport(List<Import> imports)
        {

            List<TransitTimeModel> transitTimeList = new List<TransitTimeModel>();
            foreach (var import in imports)
            {
                TransitTimeModel ttObj = new TransitTimeModel();
                ttObj.Import = import;

                string commandText = "SELECT P.PortName + ', ' + C.Name [Origin] FROM ImportExport X"
                    + " JOIN PORTS P ON P.ID = X.PortOfLoadingID"
                    + " JOIN Country C ON P.CountryID = C.ID"
                    + " WHERE X.ImportExportID = '" + import.ImportExportID + "'";

                using (SqlConnection connection = this.SqlConnection)
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ttObj.Origin = reader["Origin"].ToString();
                    }
                }


                transitTimeList.Add(BuildTT(ttObj));
            }

            return transitTimeList;
        }

        public TransitTimeModel BuildTT(TransitTimeModel transitTime)
        {
            StatusUpdate loadedStatus = new StatusUpdate();
            StatusUpdate voyageStatus = new StatusUpdate();
            StatusUpdate dischargedStatus = new StatusUpdate();
            StatusUpdate customClearanceStatus = new StatusUpdate();
            StatusUpdate readyLoadingStatus = new StatusUpdate();
            StatusUpdate dispatchedStatus = new StatusUpdate();
            StatusUpdate customInspectStatus = new StatusUpdate();
            StatusUpdate deliveredStatus = new StatusUpdate();

            foreach (var status in transitTime.Import.ImportExportStatuses)
            {
                switch (status.StatusID)
                {
                    case "18":
                        loadedStatus = status;
                        break;
                    case "14":
                        voyageStatus = status;
                        break;
                    case "57":
                        dischargedStatus = status;
                        break;
                    case "8":
                        customClearanceStatus = status;
                        break;
                    case "10":
                        readyLoadingStatus = status;
                        break;
                    case "11":
                        dispatchedStatus = status;
                        break;
                    case "12":
                        customInspectStatus = status;
                        break;
                    case "13":
                        deliveredStatus = status;
                        break;
                }
            }

            transitTime.LoadedToVoyage = getTimePassed(loadedStatus.StatusDate, voyageStatus.StatusDate).Days;
            transitTime.VoyageToDischarged = getTimePassed(voyageStatus.StatusDate, dischargedStatus.StatusDate).Days;
            transitTime.DischargedToUCC = getTimePassed(dischargedStatus.StatusDate, customClearanceStatus.StatusDate).Days;
            transitTime.UCCToReadyLoading = getTimePassed(customClearanceStatus.StatusDate, readyLoadingStatus.StatusDate).Days;
            transitTime.ReadyToDispatched = getTimePassed(readyLoadingStatus.StatusDate, dispatchedStatus.StatusDate).Days;
            transitTime.DispatchedToUCI = getTimePassed(dispatchedStatus.StatusDate, customInspectStatus.StatusDate).Days;
            transitTime.UCIToDelivered = getTimePassed(customInspectStatus.StatusDate, deliveredStatus.StatusDate).Days;

            transitTime.TotalTransitTime = transitTime.LoadedToVoyage + transitTime.VoyageToDischarged + transitTime.DischargedToUCC
                + transitTime.UCCToReadyLoading + transitTime.ReadyToDispatched + transitTime.DispatchedToUCI + transitTime.UCIToDelivered;

            return transitTime;
        }

        private TimeSpan getTimePassed(DateTime d1, DateTime d2)
        {
            if (d2.Year == 1)
            {
                return new TimeSpan(0, 0, 0);
            }

            return d2.Subtract(d1);
        }
    }
}
