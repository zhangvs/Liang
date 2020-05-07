using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Util;
using Newtonsoft.Json;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.TenPayLibV3;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HZSoft.Application.Web.Areas.WeChatManage.Controllers
{
    public class user_orderController : Controller
    {
        private OrdersBLL ordersbll = new OrdersBLL();
        //
        // GET: /WeChatManage/user_order/
        private static TenPayV3Info tenPayV3Info = new TenPayV3Info(WeixinConfig.AppID2, WeixinConfig.AppSecret2, WeixinConfig.MchId
            , WeixinConfig.Key, WeixinConfig.TenPayV3Notify);

        public ActionResult index()
        {
            return View();
        }
        public ActionResult payment(int? id,string Tel,string Price)
        {
            ViewBag.id = id;
            ViewBag.Tel = Tel;
            ViewBag.Price = Price;
            return View();
        }
        public ActionResult paymentProcess(int? id)
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
                if (result.return_code== "SUCCESS")
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

        public ActionResult queryWx(int? id)
        {
            OrdersEntity ordersEntity = ordersbll.GetEntity(id);
            if(ordersEntity.PayStatus == 1)
            {
                return Json(new { status = true });
            }
            return Json(new { status = false}); 
        }


        public ActionResult paymentFinish(int? id)
        {
            ViewBag.Id = id;
            OrdersEntity ordersEntity = ordersbll.GetEntity(id);
            if (ordersEntity!=null)
            {
                if (ordersEntity.PayStatus == 1)
                {
                    ViewBag.Result = "支付成功";
                    ViewBag.icon = "success";
                    ViewBag.display = "none";
                    ViewBag.Tel = ordersEntity.Tel;
                }
                else
                {
                    ViewBag.Result = "未支付";
                    ViewBag.icon = "warn";
                    ViewBag.display = "block";
                    ViewBag.Tel = ordersEntity.Tel;

                }
            }

            return View();
        }
        
    }
}


//http://localhost:4066/WeChatManage/user_order/payment/11?Tel=17179107520&Price=99

//<xml>
//  <return_code><![CDATA[SUCCESS]]></return_code>
//  <return_msg><![CDATA[OK]]></return_msg>
//  <appid><![CDATA[wx288f944166a4bdc6]]></appid>
//  <mch_id><![CDATA[1582948931]]></mch_id>
//  <nonce_str><![CDATA[gelx5Eej34TWkYjL]]></nonce_str>
//  <sign><![CDATA[4D19F96F050056C904DBD7371D974905]]></sign>
//  <result_code><![CDATA[SUCCESS]]></result_code>
//  <prepay_id><![CDATA[wx18152655644502b82539bf421260374600]]></prepay_id>
//  <trade_type><![CDATA[NATIVE]]></trade_type>
//  <code_url><![CDATA[weixin://wxpay/bizpayurl?pr=K9tQFgw]]></code_url>
//</xml>