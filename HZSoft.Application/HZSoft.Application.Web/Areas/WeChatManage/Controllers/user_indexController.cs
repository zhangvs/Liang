using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZXing;
using ZXing.Common;

namespace HZSoft.Application.Web.Areas.WeChatManage.Controllers
{
    public class user_indexController : Controller
    {
        //
        // GET: /WeChatManage/user_index/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult login()
        {
            return View();
        }
        public ActionResult login2()
        {
            return View();
        }
        public ActionResult reg()
        {
            return View();
        }

        public ActionResult loginDo()
        {
            return View();
        }
        public ActionResult regDo()
        {
            return View();
        }

        public ActionResult logout()
        {
            return View();
        }

        public ActionResult getPageqr(string pageurl)
        {
            BitMatrix bitMatrix;
            bitMatrix = new MultiFormatWriter().encode(pageurl, BarcodeFormat.QR_CODE, 600, 600);
            BarcodeWriter bw = new BarcodeWriter();

            var ms = new MemoryStream();
            var bitmap = bw.Write(bitMatrix);
            bitmap.Save(ms, ImageFormat.Png);
            //return File(ms, "image/png");
            ms.WriteTo(Response.OutputStream);
            Response.ContentType = "image/png";
            return null;
        }


    }
}
