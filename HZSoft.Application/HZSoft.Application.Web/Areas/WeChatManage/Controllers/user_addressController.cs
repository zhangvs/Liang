using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HZSoft.Application.Web.Areas.WeChatManage.Controllers
{
    public class user_addressController : Controller
    {
        //
        // GET: /WeChatManage/user_address/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult add()
        {
            return View();
        }
        public ActionResult edit(int? id)
        {
            return View();
        }
        
    }
}
