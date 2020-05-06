using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HZSoft.Application.Web.Areas.Shop
{
    public class webappAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "webapp";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
             this.AreaName + "_Default",
             this.AreaName + "/{controller}/{action}/{id}",
             new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
             new string[] { "HZSoft.Application.Web.Areas." + this.AreaName + ".Controllers" }
           );
        }
    }
}