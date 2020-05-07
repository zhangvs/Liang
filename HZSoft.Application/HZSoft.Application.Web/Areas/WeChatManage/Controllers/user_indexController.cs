using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.TenPayLibV3;
using ServiceStack.Text;
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
        private OrdersBLL ordersbll = new OrdersBLL();
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
            string tel = Request["tel"];
            //string pass = Request["pass"];

            //JObject queryJson = new JObject { { "Tel", account }};//queryJson.ToString()

            var ordersEntity = ordersbll.GetEntityByTel(tel);
            if (ordersEntity!=null)
            {
                string msg = "";
                //0 待付款 1 待审核 2 待发货 3 待开卡 4 已完成
                switch (ordersEntity.Status)
                {
                    case 0:
                        msg = "待付款";
                        break;
                    case 1:
                        msg = "待审核";
                        break;
                    case 2:
                        msg = "待发货";
                        break;
                    case 3:
                        msg = "已发货待开卡，" + ordersEntity.ExpressCompany + "："+ordersEntity.ExpressSn;
                        break;
                    case 4:
                        msg = "已完成";
                        break;
                    default:
                        break;
                }
                H5Response root = new H5Response { code = true, status = true, msg = msg, data = new H5ResponseData() };
                root.data.code = "3";
                return Content(JsonConvert.SerializeObject(root));
                //return Redirect("/WeChatManage/user_index/index?id=" + account);
            }
            else
            {
                H5Response root = new H5Response { code = false, status = false, msg = "该靓号订单不存在!" };
                return Content(JsonConvert.SerializeObject(root));
            }
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
