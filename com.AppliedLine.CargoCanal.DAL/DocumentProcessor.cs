using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.AppliedLine.CargoCanal.Models;

namespace com.AppliedLine.CargoCanal.DAL
{
    public class ImportExportRequiredDoc : IDocumentable
    {
        private readonly DataAccessLayer dal;
        public ImportExportRequiredDoc()
        {
            dal = new DataAccessLayer();
        }

        public async Task<bool> SaveImportExportDocs(List<ImportExportDoc> ieDocs, long ieId)
        {
            foreach (ImportExportDoc doc in ieDocs)
            {
                doc.ImportExportID = ieId;
                doc.Required = doc.IsPublic = true;
                await dal.InsertImportExportDoc(doc);
            }

            return true;
        }
    }

    public class ImportExportOptionalDoc : IDocumentable
    {
        private readonly DataAccessLayer dal;
        public ImportExportOptionalDoc()
        {
            dal = new DataAccessLayer();
        }

        public async Task<bool> SaveImportExportDocs(List<ImportExportDoc> ieDocs, long ieId)
        {
            foreach (ImportExportDoc doc in ieDocs)
            {
                doc.ImportExportID = ieId;
                await dal.InsertImportExportDoc(doc);
            }

            return true;
        }
    }
}
