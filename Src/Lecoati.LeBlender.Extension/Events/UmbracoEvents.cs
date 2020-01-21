﻿//clear cache on publish
using System;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Publishing;
using System.Web;
using Lecoati.LeBlender.Extension.Helpers;
using System.Web.Http;

namespace Lecoati.LeBlender.Extension.Events
{
    public class UmbracoEvents : ApplicationEventHandler
    {

        protected override void ApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationInitialized(umbracoApplication, applicationContext);

            RouteTable.Routes.MapRoute(
                "leblender",
                "umbraco/backoffice/leblender/helper/{action}",
                new
                {
                    controller = "Helper",
                }
            );
            RouteTable.Routes.MapRoute(
              name: "Transfer",
              url: "umbraco/api/Transfer/TransferEditor",
              defaults: new { controller = "Transfer", action = "TransferEditor" }
            );
        }

        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // Upgrate default view path for LeBlender 1.0.0
            var gridConfig = HttpContext.Current.Server.MapPath("~/Config/grid.editors.config.js");
            if (System.IO.File.Exists(gridConfig))
            {
                try
                {
                    string readText = System.IO.File.ReadAllText(gridConfig);
                    if (readText.IndexOf("/App_Plugins/Lecoati.LeBlender/core/LeBlendereditor.html") > 0
                        || readText.IndexOf("/App_Plugins/Lecoati.LeBlender/editors/leblendereditor/LeBlendereditor.html") > 0
                        || readText.IndexOf("/App_Plugins/Lecoati.LeBlender/core/views/Base.cshtml") > 0
                        || readText.IndexOf("/App_Plugins/Lecoati.LeBlender/editors/leblendereditor/views/Base.cshtml") > 0
                        )
                    {
                        readText = readText.Replace("/App_Plugins/Lecoati.LeBlender/core/LeBlendereditor.html", "/App_Plugins/LeBlender/editors/leblendereditor/LeBlendereditor.html")
                            .Replace("/App_Plugins/Lecoati.LeBlender/editors/leblendereditor/LeBlendereditor.html", "/App_Plugins/LeBlender/editors/leblendereditor/LeBlendereditor.html")
                            .Replace("/App_Plugins/Lecoati.LeBlender/core/views/Base.cshtml", "/App_Plugins/LeBlender/editors/leblendereditor/views/Base.cshtml")
                            .Replace("/App_Plugins/Lecoati.LeBlender/editors/leblendereditor/views/Base.cshtml", "/App_Plugins/LeBlender/editors/leblendereditor/views/Base.cshtml");
                        System.IO.File.WriteAllText(gridConfig, readText);
                    }

                    var databaseHelper = new DatabaseHelper();
                    //Transfer grid.config items to database
                    databaseHelper.CreateOrUpdateTables();
                }
                catch (Exception ex)
                {
                    LogHelper.Error<Helper>("Enable to upgrate LeBlender 1.0.0", ex);
                }
            }
        }
    }
}