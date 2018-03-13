using com.AppliedLine.CargoCanal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.AppliedLine.CargoCanal.DAL
{
    public class BizLogic
    {
        const string profilesDir = "~/Upload/ProfileImages";
        private readonly DataAccessLayer dal;
        private readonly IDocumentable documentable;
        
        public BizLogic(DataAccessLayer dal, IDocumentable documentable)
        {
            this.dal = dal;
            this.documentable = documentable;
        }

        private ItemView GetItemView(Item item)
        {
            item.ItemDetails = dal.SelectItemDetail(item.ID);
            var weightUnit = dal.SelectUnit(item.WeightUnitID);
            var volumeUnit = dal.SelectUnit(item.VolumeUnitID);

            var itemView = new ItemView()
            {
                ID = item.ID,
                Cargo = dal.SelectCargo(item.CargoID).CargoName,
                Dangerous = item.Dangerous,
                Description = item.Description,
                GrossWeight = $"{item.GrossWeight} {weightUnit?.UnitName ?? string.Empty}".Trim(),
                ItemOrderNo = item.ItemOrderNo,
                NetWeight = $"{item.NetWeight} {weightUnit?.UnitName ?? string.Empty}".Trim(),
                Quantity = item.Quantity,
                SubCargo = dal.SelectSubCargo(item.SubCargoID).Description,
                Volume = $"{item.Volume} {volumeUnit?.UnitName ?? string.Empty}".Trim(),
                ItemDetails = new List<ItemDetailView>()
            };
            //ItemNumber = details.ItemNumber,
            //StuffMode = dal.SelectStuffMode(details.StuffModeID).Description,
            //Destination = dal.SelectLocation(details.DestinationID ?? 0)?.LocationName,

            foreach (var itemDetail in item.ItemDetails)
            {
                itemView.ItemDetails.Add(new ItemDetailView
                {
                    ID = itemDetail.ID,
                    Destination = dal.SelectLocation(itemDetail.DestinationID ?? 0)?.LocationName,
                    ItemNumber = itemDetail.ItemNumber,
                    StuffMode = dal.SelectStuffMode(itemDetail.StuffModeID).Description
                });
            }

            return itemView;
        }

        private List<ImportView> GetImportView(List<ImportExport> importExports)
        {
            DataAccessLayer dal = new DataAccessLayer();
            List<ImportView> importViews = new List<ImportView>();

            // TODO: Provide a better implementation by generating the ids as a comma separated string variable
            // and passing that to a SPROC to get records IN (comma separated IDs), then create the view
            foreach (var ie in importExports)
            {
                var import = dal.SelectImport(ie.ID);
                var pod = dal.SelectPort(ie.PortOfDischargeID);
                var pol = dal.SelectPort(ie.PortOfLoadingID);

                // create the importView model
                var importView = new ImportView()
                {
                    ID = ie.ID,
                    ImpExpTypeID = ie.ImpExpTypeID,
                    Bill = import.BillNumber,
                    BillDate = import.BillDate,
                    Carrier = dal.SelectCarrier(ie.CarrierID)?.CarrierName,
                    Completed = ie.Completed,
                    Terminated = ie.Terminated,
                    DateInitiated = ie.DateInitiated,
                    DateInserted = ie.DateInserted,
                    ETA = import.ETA,
                    IncoTerm = dal.SelectIncoTerm(ie.IncoTermID).IncoName,
                    MOT = dal.SelectModeOfTransport(ie.ModeOfTransportID).Mode,
                    PortOfDischarge = $"{pod.PortName}, {dal.SelectCountry(pod.CountryID).Name}",
                    PortOfLoading = $"{pol.PortName}, {dal.SelectCountry(pol.CountryID).Name}",
                    Reason = dal.SelectImportExportReasons().Find(r => r.ID == ie.ImportExportReasonID).Reason,
                    ReferenceNo = ie.ReferenceNo,
                    ReImportExport = ie.ReImportExport,
                    Remark = ie.Remark,
                    Unimodal = ie.Unimodal,
                    Vessel = ie.Vessel,
                    VoyageNumber = ie.VoyageNumber,
                    LC = ie.LC,
                    Consignee = new ConsigneeImportExportWithTin(),
                    Items = new List<ItemView>(),
                    Cost = dal.SelectCost(ie.ID),
                    ProblemsUnresolved = dal.SelectProblemUpdates(ie.ID).Where(p => !p.IsResolved).Count()
                };

                importView.Consignee.ImportExportID = ie.ID;

                if (ie.Consignee != null)
                {
                    importView.Consignee.ID = ie.Consignee.ID;
                    importView.Consignee.Address = ie.Consignee.Address;
                    importView.Consignee.CompanyName = ie.Consignee.CompanyName;
                    importView.Consignee.ContactMobile = ie.Consignee.ContactMobile;
                    importView.Consignee.ContactName = ie.Consignee.ContactName;
                    importView.Consignee.Email = ie.Consignee.Email;
                    importView.Consignee.TIN = ie.Consignee.TIN;
                }


                // create itemView model and assign to the importView model
                var items = dal.SelectItems(ie.ID);
                foreach (var item in items)
                {
                    importView.Items.Add(GetItemView(item));
                }

                importViews.Add(importView);
            }

            return importViews;
        }


        private List<ExportView> GetExportView(List<ImportExport> importExports)
        {
            DataAccessLayer dal = new DataAccessLayer();
            List<ExportView> exportViews = new List<ExportView>();

            foreach (var ie in importExports)
            {
                var export = dal.SelectExport(ie.ID);
                var pod = dal.SelectPort(ie.PortOfDischargeID);
                var pol = dal.SelectPort(ie.PortOfLoadingID);

                // create the exportView model which is returned to the UI
                var exportView = new ExportView()
                {
                    ID = ie.ID,
                    ImpExpTypeID = ie.ImpExpTypeID,
                    ShippingInstructionNo = export.ShippingInstructionNo,
                    ShippingInstructionDate = export.ShippingInstructionDate,
                    CPORecievedDate = export.CPORecievedDate,
                    DeclarationDate = export.DeclarationDate,
                    DeclarationNo = export.DeclarationNo,
                    OriginalDocumentDate = export.OriginalDocumentDate,
                    Origin = dal.SelectLocation(export.OriginID).LocationName,
                    StuffingLocation = dal.SelectLocation(export.StuffingLocationID).LocationName,
                    WayBill = export.WayBill,
                    WayBillDate = export.WayBillDate,
                    Carrier = dal.SelectCarrier(ie.CarrierID)?.CarrierName,
                    Completed = ie.Completed,
                    Terminated = ie.Terminated,
                    DateInitiated = ie.DateInitiated,
                    DateInserted = ie.DateInserted,
                    ETA = export.ETA,
                    IncoTerm = dal.SelectIncoTerm(ie.IncoTermID).IncoName,
                    MOT = dal.SelectModeOfTransport(ie.ModeOfTransportID).Mode,
                    PortOfDischarge = $"{pod.PortName}, {dal.SelectCountry(pod.CountryID).Name}",
                    PortOfLoading = $"{pol.PortName}, {dal.SelectCountry(pol.CountryID).Name}",
                    Reason = dal.SelectImportExportReasons().Find(r => r.ID == ie.ImportExportReasonID).Reason,
                    ReferenceNo = ie.ReferenceNo,
                    ReImportExport = ie.ReImportExport,
                    Remark = ie.Remark,
                    Unimodal = ie.Unimodal,
                    Vessel = ie.Vessel,
                    VoyageNumber = ie.VoyageNumber,
                    LC = ie.LC,
                    Consignee = new ConsigneeImportExportWithTin(),
                    Items = new List<ItemView>(),
                    Cost = dal.SelectCost(ie.ID),
                    ProblemsUnresolved = dal.SelectProblemUpdates(ie.ID).Where(p => !p.IsResolved).Count()
                };

                exportView.Consignee.ImportExportID = ie.ID;

                if (ie.Consignee != null)
                {
                    exportView.Consignee.ID = ie.Consignee.ID;
                    exportView.Consignee.Address = ie.Consignee.Address;
                    exportView.Consignee.CompanyName = ie.Consignee.CompanyName;
                    exportView.Consignee.ContactMobile = ie.Consignee.ContactMobile;
                    exportView.Consignee.ContactName = ie.Consignee.ContactName;
                    exportView.Consignee.Email = ie.Consignee.Email;
                    exportView.Consignee.TIN = ie.Consignee.TIN;
                }

                // create itemView model and assign to the exportView model
                var items = dal.SelectItems(ie.ID);
                foreach (var item in items)
                {
                    exportView.Items.Add(GetItemView(item));
                }

                exportViews.Add(exportView);
            }

            return exportViews;
        }

        public User AuthoriseUser(Login login)
        {
            DataAccessLayer dal = new DataAccessLayer();
            login = dal.SelectLogin(login.Username, login.Password);
            var person = dal.SelectPerson(login.PersonID);

            // create physical file from base64string if it does not exist on server
            if(person != null && !string.IsNullOrEmpty(person.PhotoFilename))
            {
                FileProcessor.CreateFileFromByteOnDisc(HttpContext.Current.Server.MapPath(profilesDir), person.PhotoFilename, Convert.FromBase64String(person.Photo));
            }

            var company = dal.SelectCompanyById(person.CompanyID);

            User user = new User()
            {
                Login = login,
                Person = person,
                Company = company
            };

            return user;
        }

        public int SavePersonWithRole(User user)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (dal.InsertPerson(user.Person) > 0)
            {
                user.Login.PersonID = user.Person.ID;
                user.Login.Username = user.Person.Email;
                return dal.InsertLogin(user.Login);
            }

            return 0;
        }

        private long InsertAdminForNewCompany(User user)
        {
            DataAccessLayer dal = new DataAccessLayer();
            long result;

            Role role = new Role()
            {
                CompanyID = user.Company.ID,
                Description = string.Empty,
                RoleCode = user.Company.TIN,
                RoleName = "Super Admin " + user.Company.TIN
            };

            // Create the SA Role with required permissions set 
            // by ensuring the stored procedure checks the company type
            // and exec the corresponding proc to set permissions
            result = dal.InsertRole(role);
            result = dal.InsertRolePermissionsForNewCompany(role.ID, user.Company.CompanyTypeID);

            // Create the Person with assigned company id
            user.Person.CompanyID = user.Company.ID;
            result = dal.InsertPerson(user.Person);

            // Create the Login with assigned Role id and Person id
            if (role.ID > 0 && user.Person.ID > 0)
            {
                user.Login.RoleID = role.ID;
                user.Login.PersonID = user.Person.ID;
                user.Login.Username = user.Person.Email;
                result = dal.InsertLogin(user.Login);
            }

            return result;
        }

        public List<StatusUpdateView> GetStatusUpdateView(long importExportId)
        {
            DataAccessLayer dal = new DataAccessLayer();
            // Get all the statuses
            var statusUpdates = dal.SelectStatusUpdates(importExportId);

            // get the status text for each of the statues
            List<StatusUpdateView> statusUpdateViews = new List<StatusUpdateView>();
            foreach (var update in statusUpdates)
            {
                var stat = dal.SelectStatus(update.StatusID);
                var statusUpdateView = new StatusUpdateView()
                {
                    DateInserted = update.DateInserted,
                    ID = update.ID,
                    ImportExportID = update.ImportExportID,
                    Location = dal.SelectLocation(update.LocationID)?.LocationName ?? string.Empty,
                    StatusDate = update.StatusDate,
                    StatusText = stat.Description,
                    Abbr = stat.Abbr
                };

                statusUpdateViews.Add(statusUpdateView);
            }

            return statusUpdateViews;
        }

        public List<ProblemUpdateView> GetProblemUpdateView(long id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            var problemUpdates = dal.SelectProblemUpdates(id);

            var problemUpdatesView = new List<ProblemUpdateView>();
            foreach (var p in problemUpdates)
            {
                var view = new ProblemUpdateView()
                {
                    DateInserted = p.DateInserted,
                    ID = p.ID,
                    IsResolved = p.IsResolved,
                    Messages = p.Messages,
                    ProblemDate = p.ProblemDate,
                    ProblemName = dal.SelectProblem(p.ProblemID).ProblemName,
                    ResolvedDate = p.ResolvedDate
                };

                problemUpdatesView.Add(view);
            }

            return problemUpdatesView;
        }

        public long SaveProblemUpdate(ProblemUpdate problemUpdate)
        {
            DataAccessLayer dal = new DataAccessLayer();
            var id = dal.InsertProblemUpdate(problemUpdate);

            if (id > 0)
            {
                foreach (var m in problemUpdate.Messages)
                {
                    if (string.IsNullOrEmpty(m.Message.Trim())) continue;

                    m.ProblemUpdateID = problemUpdate.ID;
                    dal.InsertProblemUpdateMessage(m);
                }
            }

            return id;
        }

        public long SaveCompany(User user)
        {
            // change this implementation to check the user's permission before saving the company
            // call TRANSACTION proc & pass user Token, ID output, company name, company type, countryid, tin, contact name
            // proc checks user's role and permissions
            // proc finds what company type name to create
            // proc switch company type name and checks if user role permission is allowed
            // proc creates the company if permission is ok else RAISERROR
            user.Company.ContactName = dal.ToCapitalizeCase($"{user.Person.FirstName} {user.Person.LastName}");
            user.Company.Email = user.Login.Username = user.Person.Email;
            var result = dal.ValidateInsertCompany(user.Company, user.Login.Token);

            if (result > 0) result = InsertAdminForNewCompany(user);

            return result;
        }

        public long SaveAllCompanies(User user)
        {
            // Create the Company, set contact name and get the ID
            user.Company.ContactName = dal.ToCapitalizeCase($"{user.Person.FirstName} {user.Person.LastName}");
            user.Company.Email = user.Login.Username = user.Person.Email;
            var result = dal.InsertCompany(user.Company);

            if (result > 0) result = InsertAdminForNewCompany(user);

            return result;
        }

        public List<CustomerImportExportView> GetImportWithStatusesByTinLc(long companyID, long skip, long pageSize, SearchBill search)
        {
            DataAccessLayer dal = new DataAccessLayer();
            var importExports = dal.SelectImportExportsByTinOrLc(companyID, skip, pageSize, search);

            List<CustomerImportExportView> customerImportExportViews = new List<CustomerImportExportView>();

            foreach (var ie in importExports)
            {
                Import import = dal.SelectImport(ie.ID);
                Export export = null;
                if (import == null) export = dal.SelectExport(ie.ID);

                var pod = dal.SelectPort(ie.PortOfDischargeID);
                var pol = dal.SelectPort(ie.PortOfLoadingID);

                // create the National bank view model
                var view = new CustomerImportExportView()
                {
                    ID = ie.ID,
                    ImpExpTypeID = ie.ImpExpTypeID,
                    Carrier = dal.SelectCarrier(ie.CarrierID)?.CarrierName,
                    Completed = ie.Completed,
                    Terminated = ie.Terminated,
                    DateInitiated = ie.DateInitiated,
                    DateInserted = ie.DateInserted,
                    IncoTerm = dal.SelectIncoTerm(ie.IncoTermID).IncoName,
                    MOT = dal.SelectModeOfTransport(ie.ModeOfTransportID).Mode,
                    PortOfDischarge = $"{pod.PortName}, {dal.SelectCountry(pod.CountryID).Name}",
                    PortOfLoading = $"{pol.PortName}, {dal.SelectCountry(pol.CountryID).Name}",
                    Reason = dal.SelectImportExportReasons().Find(r => r.ID == ie.ImportExportReasonID).Reason,
                    ReferenceNo = ie.ReferenceNo,
                    ReImportExport = ie.ReImportExport,
                    Remark = ie.Remark,
                    Unimodal = ie.Unimodal,
                    Vessel = ie.Vessel,
                    VoyageNumber = ie.VoyageNumber,
                    LC = ie.LC,
                    Consignee = new ConsigneeImportExportWithTin(),
                    Items = new List<ItemView>(),
                    StatusesViews = new List<StatusUpdateView>(),
                    ProblemsUnresolved = dal.SelectProblemUpdates(ie.ID).Where(p => !p.IsResolved).Count()
                };

                if (import != null)
                {
                    view.Bill = import.BillNumber;
                    view.BillDate = import.BillDate;
                    view.ETA = import.ETA;
                }
                else if (export != null)
                {
                    view.Origin = dal.SelectLocation(export.OriginID).LocationName;
                    view.StuffingLocation = dal.SelectLocation(export.StuffingLocationID).LocationName;
                    view.ShippingInstructionNo = export.ShippingInstructionNo;
                    view.ShippingInstructionDate = export.ShippingInstructionDate;
                    view.CPORecievedDate = export.CPORecievedDate;
                    view.OriginalDocumentDate = export.OriginalDocumentDate;
                    view.WayBill = export.WayBill;
                    view.WayBillDate = export.WayBillDate;
                    view.DeclarationNo = export.DeclarationNo;
                    view.DeclarationDate = export.DeclarationDate;
                    view.ETA = export.ETA;
                }

                view.Consignee.ImportExportID = ie.ID;

                // build the consignee information
                if (ie.Consignee != null)
                {
                    view.Consignee.ID = ie.Consignee.ID;
                    view.Consignee.Address = ie.Consignee.Address;
                    view.Consignee.CompanyName = ie.Consignee.CompanyName;
                    view.Consignee.ContactMobile = ie.Consignee.ContactMobile;
                    view.Consignee.ContactName = ie.Consignee.ContactName;
                    view.Consignee.Email = ie.Consignee.Email;
                    view.Consignee.TIN = ie.Consignee.TIN;
                }

                // get the statuses view
                view.StatusesViews = GetStatusUpdateView(ie.ID);

                // create itemView model and assign to the nationalBankView model
                var items = dal.SelectItems(ie.ID);
                foreach (var item in items)
                {
                    view.Items.Add(GetItemView(item));
                }

                customerImportExportViews.Add(view);
            }

            return customerImportExportViews;
        }


        public List<ImportView> GetImportsPendingView(long companyID)
        {
            // Select all imports for the company
            DataAccessLayer dal = new DataAccessLayer();
            var importExports = dal.SelectImportExports(companyID).FindAll(x => x.ImpExpTypeID == 1);

            return GetImportView(importExports);
        }

        public List<ImportView> GetImportsPendingView(int ieTypeId, long companyID, long skip, long pageSize)
        {
            // Select all imports for the company
            DataAccessLayer dal = new DataAccessLayer();
            List<ImportExport> importExports = dal.SelectImportExports(ieTypeId: ieTypeId, companyID: companyID, skip: skip, pageSize: pageSize);

            return GetImportView(importExports);
        }


        /// <summary>
        /// Gets ImportExport documents that match the criteria specified in the bill object
        /// then transforms each record in the returned data into an _ImportView.
        /// </summary>
        /// <param name="search"></param>
        /// <returns>
        /// List<_ImportView>
        /// </returns>

        public List<ImportView> SearchImportsView(int ieTypeId, long companyID, long skip, long pageSize, SearchBill search)
        {
            DataAccessLayer dal = new DataAccessLayer();
            List<ImportExport> importExports = dal.SearchImportExport(ieTypeId, companyID, skip, pageSize, search, true);

            return GetImportView(importExports);
        }

        public async Task<long> SaveImport(ImportExport importExport)
        {
            // Set the ImpExpTypeID
            importExport.ImpExpTypeID = 1;

            DataAccessLayer dal = new DataAccessLayer();
            long id = await dal.InsertImportExport(importExport);

            if (id > 0)
            {
                try
                {
                    importExport.Import.ImportExportID = importExport.LC.ImportExportID = id;
                    dal.InsertImport(importExport.Import);
                    dal.InsertLC(importExport.LC);
                    SaveItemWithDetail(importExport.Items, id);

                    await documentable.SaveImportExportDocs(importExport.Documents, id);
                }
                catch (Exception e)
                {
                    dal.DeleteImportExport(id);
                    return 0;
                }
            }

            return id;
        }

        
        public List<ExportView> GetExportsPendingView(long companyID)
        {
            // Select all imports for the company
            DataAccessLayer dal = new DataAccessLayer();
            var importExports = dal.SelectImportExports(companyID).FindAll(x => x.ImpExpTypeID == 2);

            return GetExportView(importExports);
        }

        public List<ExportView> GetExportsPendingView(int ieTypeId, long companyID, long skip, long pageSize)
        {
            // Select all exports for the company
            DataAccessLayer dal = new DataAccessLayer();
            List<ImportExport> importExports = dal.SelectImportExports(ieTypeId: ieTypeId, companyID: companyID, skip: skip, pageSize: pageSize);

            return GetExportView(importExports);
        }


        /// <summary>
        /// Gets ImportExport documents that match the criteria specified in the bill object
        /// then transforms each record in the returned data into an _ExportView.
        /// </summary>
        /// <param name="search"></param>
        /// <returns>
        /// List<_ExportView>
        /// </returns>
        public List<ExportView> SearchExportsView(int ieTypeId, long companyID, long skip, long pageSize, SearchBill search)
        {
            DataAccessLayer dal = new DataAccessLayer();
            var importExports = dal.SearchImportExport(ieTypeId, companyID, skip, pageSize, search, false);

            return GetExportView(importExports);
        }

        public async Task<long> SaveExport(ImportExport importExport)
        {
            // Set the ImpExpTypeID
            importExport.ImpExpTypeID = 2;

            DataAccessLayer dal = new DataAccessLayer();
            long id = await dal.InsertImportExport(importExport);

            if (id > 0)
            {
                try
                {
                    importExport.Export.ImportExportID = importExport.LC.ImportExportID = id;
                    dal.InsertExport(importExport.Export);
                    dal.InsertLC(importExport.LC);
                    SaveItemWithDetail(importExport.Items, id);

                    await documentable.SaveImportExportDocs(importExport.Documents, id);
                }
                catch
                {
                    dal.DeleteImportExport(id);
                }
            }

            return id;
        }

        public void SaveItemWithDetail(List<Item> items, long importExportID)
        {
            // save the item and item detail
            foreach (var item in items)
            {
                item.ImportExportID = importExportID;
                var itemId = dal.InsertItem(item);

                foreach (var itemDetail in item.ItemDetails)
                {
                    itemDetail.ItemID = itemId;
                    dal.InsertItemDetail(itemDetail);
                }
            }
        }
    }
}
