using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Web;
using System.Web.Mvc;


namespace HZSoft.Application.Web.Areas.H5.Controllers
{
    public class HomePageController : Controller
    {
        //
        // GET: /H5/Homepage/

        private TelphoneLiangBLL tlbll = new TelphoneLiangBLL();
        private OrdersBLL ordersbll = new OrdersBLL();

        private static TenPayV3Info tenPayV3Info = new TenPayV3Info(WeixinConfig.AppID2, WeixinConfig.AppSecret2, WeixinConfig.MchId
            , WeixinConfig.Key, WeixinConfig.TenPayV3Notify);



        public ActionResult Index(string keyword, string city, string orderType, string price, string pricef, string pricet, string features,string manager, int? page)
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
                        {"Operator",manager }
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
            if (page == null)
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
            if (page == 1)
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
                    data = new H5ResponseData()
                };
                string html = "";
                foreach (var bl in listEntity)
                {
                    html += "<li>" +
                        "<a href='/H5/home/mobileinfo/"+bl.TelphoneID+"'>" +
                            "<h1>" +
                                "<font class='f-orange'>" + bl.Telphone.Substring(0, 3) + "</font><font class='f-green'>" + bl.Telphone.Substring(3, 4) + "</font><font class='f-red'>" + bl.Telphone.Substring(7, 4) + "</font>" +
                            "</h1>" +
                            "<div>" +
                                "<span class='text-hidden'>" + bl.City + bl.Operator + "</span>" +
                                " <p>" +
                                    "<font class='color-red'>\u00a5" + bl.MaxPrice + "</font>" +
                                    "<font class='money-market'>\u00a5"+ bl.MaxPrice*2 + " </font>" +
                                "</p>" +
                            "</div>" +
                         " </a>" +
                      "</li>";
                }
                root.data.html = html;
                return Content(JsonConvert.SerializeObject(root));
            }
        }

        //
        // GET: /H5/Home/

        public ActionResult mobileinfo(int? id)
        {
            TelphoneLiangEntity entity = tlbll.GetEntity(id);
            return View(entity);
        }
        //
        // GET: /H5/Home/

        public ActionResult mobileorder(int? id, string Tel, string Price)
        {
            ViewBag.id = id;
            ViewBag.Tel = Tel;
            ViewBag.Price = Price;
            return View();
        }



        public ActionResult ajaxorder(int? id)
        {
            //return Content("{\"code\":true,\"status\":true,\"msg\":\"提交成功！\",\"data\":{\"appid\":\"wx288f944166a4bdc6\",\"code_url\":\"weixin://wxpay/bizpayurl?pr=K9tQFgw\",\"mch_id\":\"1582948931\",\"nonce_str\":\"gelx5Eej34TWkYjL\",\"prepay_id\":\"wx18152655644502b82539bf421260374600\",\"result_code\":\"SUCCESS\",\"return_code\":\"SUCCESS\",\"return_msg\":null,\"sign\":\"4D19F96F050056C904DBD7371D974905\",\"trade_type\":\"NATIVE\",\"trade_no\":\"LX-20200418151928103008\",\"payid\":\"11\",\"wx_query_href\":\"http://localhost:4066/WeChatManage/user_order/queryWx/11\",\"wx_query_over\":\"http://localhost:4066/WeChatManage/user_order/paymentFinish/11\"}}");
            try
            {
                OrdersEntity ordersEntity = ordersbll.GetEntity(id);

                var sp_billno = ordersEntity.OrderSn;
                var nonceStr = TenPayV3Util.GetNoncestr();

                //商品Id，用户自行定义
                string productId = ordersEntity.TelphoneID.ToString();

                //创建请求统一订单接口参数
                var xmlDataInfo = new TenPayV3UnifiedorderRequestData(WeixinConfig.AppID2,
                tenPayV3Info.MchId,
                "支付靓号",
                sp_billno,
                Convert.ToInt32(ordersEntity.Price * 100),
                Request.UserHostAddress,
                tenPayV3Info.TenPayV3Notify,
               TenPayV3Type.NATIVE,
                null,
                tenPayV3Info.Key,
                nonceStr,
                productId: productId);
                //调用统一订单接口
                var result = TenPayV3.Unifiedorder(xmlDataInfo);

                LogHelper.AddLog(result.ResultXml);//记录日志

                H5Response root = null;
                if (result.return_code == "SUCCESS")
                {
                    H5PayData h5PayData = new H5PayData()
                    {
                        appid = WeixinConfig.AppID2,
                        code_url = result.code_url,//weixin://wxpay/bizpayurl?pr=lixpXgt
                        mch_id = WeixinConfig.MchId,
                        nonce_str = result.nonce_str,
                        prepay_id = result.prepay_id,
                        result_code = result.result_code,
                        return_code = result.return_code,
                        return_msg = result.return_msg,
                        sign = result.sign,
                        trade_type = "NATIVE",
                        trade_no = sp_billno,
                        payid = id.ToString(),
                        wx_query_href = Config.GetValue("Domain2") + "/WeChatManage/user_order/queryWx/" + id,
                        wx_query_over = Config.GetValue("Domain2") + "/WeChatManage/user_order/paymentFinish/" + id
                    };

                    root = new H5Response { code = true, status = true, msg = "\u63d0\u4ea4\u6210\u529f\uff01", data = h5PayData };
                }
                else
                {
                    root = new H5Response { code = false, status = false, msg = result.return_msg };
                }
                LogHelper.AddLog(JsonConvert.SerializeObject(root));//记录日志

                return Content(JsonConvert.SerializeObject(root));
            }
            catch (Exception ex)
            {
                LogHelper.AddLog(ex.Message);//记录日志
                throw;
            }
        }
        //
        // GET: /H5/Home/

        public ActionResult express(string mobile)
        {
            if (!string.IsNullOrEmpty(mobile))
            {
                var ordersEntity = ordersbll.GetEntityByTel(mobile);
                if (ordersEntity != null)
                {
                    string msg = "";
                    //0 待付款 1 待发货 2 待开卡 3 已完成
                    switch (ordersEntity.Status)
                    {
                        case 1:
                            msg = "待发货";
                            break;
                        case 2:
                            msg = "已发货待开卡，" + ordersEntity.ExpressCompany + "：" + ordersEntity.ExpressSn;
                            break;
                        case 3:
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
            return View();
        }
        

    }
}
