using com.AppliedLine.CargoCanal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using System.Web.Http.OData.Routing;
using System.Web.Http.OData.Routing.Conventions;

namespace com.AppliedLine.CargoCanal.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //config.EnableCors();

            // enable cors globally
            //var allowedCors = new System.Web.Http.Cors.EnableCorsAttribute("*", "*", "*");
            //config.EnableCors(allowedCors);

            config.AddODataQueryFilter();
            var supportMIME = config.Formatters.JsonFormatter.SupportedMediaTypes;
            supportMIME.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            supportMIME.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            // enable Tracing
            config.EnableSystemDiagnosticsTracing();

            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );



            // OData Route configurations
            ODataModelBuilder builder = new ODataConventionModelBuilder();

            builder.EntitySet<sp_ImportExport_Select_Company_Pending_Result>("importexport").EntityType.HasKey(p => p.ID);
            builder.EntitySet<Controllers.Duct>("OnDuct").EntityType.HasKey(p => p.ID);
            builder.EntitySet<ImportView>("ODataImport").EntityType.HasKey(e => e.ID);
            builder.EntitySet<ExportView>("ODataExport").EntityType.HasKey(e => e.ID);
            var customers = builder.EntitySet<CustomerImportExportView>("ODataCustomer");
            customers.HasManyBinding(c => c.StatusesViews, "StatusesViews");
            //builder.EntitySet<CustomerImportExportView>("ODataCustomer").EntityType.HasKey(e => e.ID);
            builder.EntitySet<LC>("LC");
            builder.EntitySet<Document>("Documents");
            builder.EntitySet<ConsigneeImportExportWithTin>("Consignee");
            builder.EntitySet<Cost>("Cost");
            builder.EntitySet<ItemView>("Items");
            builder.EntitySet<ItemDetailView>("ItemDetails");
            builder.EntitySet<StatusUpdateView>("StatusesViews");


            // Add an action to the EDM, and define the parameter and return type.
            ActionConfiguration searchImportExport = builder.Entity<CustomerImportExportView>().Action("SearchImportExport");
            searchImportExport.Parameter<int>("skip");
            searchImportExport.Parameter<string>("token");
            searchImportExport.Parameter<string>("searchText");
            searchImportExport.ReturnsCollectionFromEntitySet<CustomerImportExportView>("ODataCustomer");

            ActionConfiguration searchImports = builder.Entity<ImportView>().Action("SearchImports");
            searchImports.Parameter<int>("skip");
            searchImports.Parameter<string>("token");
            searchImports.Parameter<string>("searchText");
            searchImports.ReturnsCollectionFromEntitySet<ImportView>("ODataImport");

            ActionConfiguration getImports = builder.Entity<ImportView>().Action("GetImports");
            getImports.Parameter<int>("skip");
            getImports.Parameter<string>("token");
            getImports.Parameter<string>("searchText");
            getImports.ReturnsCollectionFromEntitySet<ImportView>("ODataImport");

            ActionConfiguration getExports = builder.Entity<ExportView>().Action("GetExports");
            getExports.Parameter<int>("skip");
            getExports.Parameter<string>("token");
            getExports.Parameter<string>("searchText");
            getExports.ReturnsCollectionFromEntitySet<ExportView>("ODataExport");

            ActionConfiguration searchExports = builder.Entity<ExportView>().Action("SearchExports");
            searchExports.Parameter<int>("skip");
            searchExports.Parameter<string>("token");
            searchExports.Parameter<string>("searchText");
            searchExports.ReturnsCollectionFromEntitySet<ExportView>("ODataExport");



            config.Routes.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "odata",
                model: builder.GetEdmModel());

        }
    }
}
