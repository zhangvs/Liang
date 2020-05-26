using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Util.WebControl;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HZSoft.Application.Web.Areas.WeChatManage.Controllers
{
    public class mpgController : BaseWxUserController
    {
        //
        // GET: /WeChatManage/mpg/
        private TelphoneLiangBLL tlbll = new TelphoneLiangBLL();
        private OrdersBLL ordersbll = new OrdersBLL();

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Index(string keyword, string city, string orderType, string price, string pricef, string pricet, string features, int? page)
        {
            if (!string.IsNullOrEmpty(city))
            {
                city = HttpUtility.UrlDecode(city).Replace("市", "");
            }
            if (!string.IsNullOrEmpty(pricef) || !string.IsNullOrEmpty(pricet))
            {
                price = pricef + "-" + pricet;
            }
            JObject queryJson = new JObject { { "Telphone", keyword },
                        { "OrganizeIdH5", "bae859c9-3df5-4da0-bea9-3e20bbc7c353" },//济南利新
                        { "pid", "bae859c9-3df5-4da0-bea9-3e20bbc7c353"},
                        { "top", "bae859c9-3df5-4da0-bea9-3e20bbc7c353" },
                        { "City", city },
                        { "MaxPrice", price },
                        { "Grade",features },
                        { "SellMark",0 },
                        {"max",1 }
                    };
            string sidx = "";
            string sord = "";
            if (orderType == "1")
            {
                sidx = "price";
                sord = "asc";
            }
            else if (orderType == "2")
            {
                sidx = "price";
                sord = "desc";
            }
            else
            {
                sidx = "right(Telphone,1)";//按照最后一位排序
                sord = "asc";
            }
            if (page==null)
            {
                page = 1;
            }
            Pagination pagination = new Pagination()
            {
                rows = 20,
                page = Convert.ToInt32(page),
                sidx = sidx,
                sord = sord
            };
            var listEntity = tlbll.GetPageListH5(pagination, queryJson.ToString());
            if (page==1)
            {
                ViewBag.list = listEntity;
                return View();
            }
            else
            {
                H5Response root = new H5Response
                {
                    code = true,
                    status = true,
                    msg = "\u64cd\u4f5c\u6210\u529f",
                    data=new H5ResponseData()
                };
                string html = "";
                foreach (var bl in listEntity)
                {
                    html+= "<div class=\"weui-col-50\"><a class=\"mobile-item\" href=\"/WeChatManage/mpg/detail/" + bl.TelphoneID
                        + "\">\r\n\t<p class=\"tit\">\r\n\t\t<font class=\"f-orange\">"+ bl.Telphone.Substring(0, 3) + "</font><font class=\"f-green\">"+ bl.Telphone.Substring(3, 4) 
                        + "</font><font class=\"f-red\">"+ bl.Telphone.Substring(7, 4) + "</font>\r\n\t\t<font class=\"f-orange pri\">\u00a5"+bl.MaxPrice + "</font>\r\n\t\t<font class=\"money-market\" >"+ bl.MaxPrice*2 + "</font>\r\n\t</p>\r\n\t<p class=\"txt\"><span>"+ bl.City+bl.Operator + "</span></p>\r\n</a></div>";
                }
                root.data.html = html;
                return Content(JsonConvert.SerializeObject(root));
            }
            
        }

        public ActionResult detail(int? id)
        {
            TelphoneLiangEntity entity = tlbll.GetEntity(id);
            return View(entity);
        }
        public ActionResult flow(int? id,string Tel,string Price)
        {
            ViewBag.id = id;
            ViewBag.Tel = Tel;
            ViewBag.Price = Price;

            return View();
        }
        public ActionResult flowDo(int? id)
        {
            try
            {
                H5Response root =null;

                string contact_tel = Request["contact_tel"];
                if (string.IsNullOrEmpty(contact_tel))
                {
                    root = new H5Response { code = false, status = false, msg = "联系电话为空！" };
                    return Content(JsonConvert.SerializeObject(root));
                }

                string contact_name = Request["contact_name"];
                if (string.IsNullOrEmpty(contact_tel))
                {
                    root = new H5Response { code = false, status = false, msg = "联系人为空！" };
                    return Content(JsonConvert.SerializeObject(root));
                }

                string contact_regions = Request["contact_regions"];
                if (string.IsNullOrEmpty(contact_regions))
                {
                    root = new H5Response { code = false, status = false, msg = "配货区域为空！" };
                    return Content(JsonConvert.SerializeObject(root));
                }
                string[] area = contact_regions.Split(' ');


                string contact_address = Request["contact_address"];
                if (string.IsNullOrEmpty(contact_address))
                {
                    root = new H5Response { code = false, status = false, msg = "详细地址为空！" };
                    return Content(JsonConvert.SerializeObject(root));
                }
                string tel = Request["Tel"];
                string price = Request["Price"];

                string host = Request.Url.Host + Request.Url.Port;
                string url = Request.Url.ToString();//获取 完整url （协议名+域名+站点名+文件名+参数）

                OrdersEntity orderEntity = new OrdersEntity()
                {
                    TelphoneID = id,
                    Tel = tel,
                    Price = Convert.ToDecimal(price),
                    Province = area[0],
                    City = area[1],
                    Country = area[2],
                    Address = contact_address,
                    Receiver = contact_name,
                    ContactTel = contact_tel,
                    Status = 0,
                    Host = host
                };
                orderEntity = ordersbll.SaveForm(orderEntity);



                root = new H5Response{ code = true,status = true,msg = "\u63d0\u4ea4\u6210\u529f\uff01",data = new H5ResponseData()};
                root.data.code = "1";
                root.data.gotoUrl = "/WeChatManage/user_order/payment/" + orderEntity.Id+"?Tel="+tel+"&Price="+ price;

                return Content(JsonConvert.SerializeObject(root));
                //return Redirect("/WeChatManage/user_order/payment?id=" + orderEntity.Id);
                //return Json(new { iserror = false, message = "提交订单成功" });
                //return RedirectToAction("payment", "user_order", new {id= orderEntity.Id});
            }
            catch (Exception ex)
            {
                return Json(new { code = false, status = false, msg = ex.Message });
            }
        }



        

    }
}
