using com.AppliedLine.CargoCanal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.DAL
{
    public interface IDocumentable
    {
        Task<bool> SaveImportExportDocs(List<ImportExportDoc> ieDocs, long ieId);
    }
}
