using Aop.Api.Util;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Application.Busines.WeChatManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Application.Web.Utility;
using HZSoft.Application.Web.Utility.AliPay;
using HZSoft.Util;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HZSoft.Application.Web.Areas.WeChatManage.Controllers
{
    public class WeiXinHomeController : Controller
    {
        WeChat_UsersBLL userBll = new WeChat_UsersBLL();
        private OrdersBLL ordersbll = new OrdersBLL();
        private TelphoneLiangBLL tlbll = new TelphoneLiangBLL();

        //跳转
        public ActionResult Redirect(string code, string state)
        {
            //若用户禁止授权，则重定向后不会带上code参数
            if (string.IsNullOrEmpty(code))
            {
                return Redirect(state);
            }
            else
            {
                WeixinToken token = new WeixinToken();

                //判断是否保存微信token
                if (Session[WebSiteConfig.WXTOKEN_SESSION_NAME] != null)
                {
                    token = Session[WebSiteConfig.WXTOKEN_SESSION_NAME] as WeixinToken;
                }
                else
                {
                    string tokenUrl = string.Format(WeixinConfig.GetTokenUrl, code);
                    token = AnalyzeHelper.Get<WeixinToken>(tokenUrl);
                    if (token.errcode != null)
                    {
                        return Content(token.errcode + ":" + token.errmsg);
                    }
                    Session[WebSiteConfig.WXTOKEN_SESSION_NAME] = token;
                }
                //查询用户是否存在
                var userEntity = userBll.GetEntity(token.openid);
                if (userEntity == null)
                {
                    string userInfoUrl = string.Format(WeixinConfig.GetUserInfoUrl, token.access_token, token.openid);
                    var userInfo = AnalyzeHelper.Get<WeixinUserInfo>(userInfoUrl);
                    if (userInfo.errcode != null)
                    {
                        Response.Write(userInfo.errcode + ":" + userInfo.errmsg);
                        Response.End();
                    }
                    else
                    {
                        userEntity = new WeChat_UsersEntity()
                        {
                            City = userInfo.city,
                            Country = userInfo.country,
                            HeadimgUrl = userInfo.headimgurl,
                            NickName = userInfo.nickname,
                            OpenId = userInfo.openid,
                            Province = userInfo.province,
                            Sex = userInfo.sex,
                            AppName = WeixinConfig.AppName
                        };
                        userBll.SaveForm("", userEntity);
                    }
                }
                Session[WebSiteConfig.WXUSER_SESSION_NAME] = userEntity;
                return Redirect(state);
            }
        }



        //微信支付回调地址
        public ActionResult Notify()
        {
            LogHelper.AddLog("支付回调地址");//记录日志
            ResponseHandler rspHandler = new ResponseHandler(null);
            rspHandler.SetKey(WeixinConfig.Key);
            LogHelper.AddLog(rspHandler.ParseXML());//记录日志


            //SUCCESS/FAIL此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断 SUCCESS
            string return_code = rspHandler.GetParameter("return_code");
            string return_msg = rspHandler.GetParameter("return_msg");
            if (rspHandler.IsTenpaySign())
            {
                if (return_code == "SUCCESS")
                {
                    //订单号
                    string orderSn = rspHandler.GetParameter("out_trade_no");
                    OrdersEntity order = ordersbll.GetEntityByOrderSn(orderSn);

                    order.PayDate = DateTime.Now;
                    order.PayStatus = (int)PayStatus.已支付;
                    order.Status = (int)OrderStatus.未发货;
                    ordersbll.SaveForm(order.Id, order);

                    TelphoneLiangEntity tel = tlbll.GetEntity(order.TelphoneID);//根据靓号id获取靓号，修改售出状态
                    if (tel != null)
                    {
                        tel.SellMark = 1;
                        tel.SellerName = order.Host;
                    }
                    tlbll.SaveForm(tel.TelphoneID, tel);
                }
            }

            string xml = string.Format(@"<xml>
<return_code><![CDATA[{0}]]></return_code>
<return_msg><![CDATA[{1}]]></return_msg>
</xml>", return_code, return_msg);
            return Content(xml, "text/xml");

        }

        /// <summary>
        /// 服务器异步通知页面
        /// </summary>
        public ActionResult ReturnUrl()
        {
            /* 实际验证过程建议商户添加以下校验。
            1、商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，
            2、判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），
            3、校验通知中的seller_id（或者seller_email) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id/seller_email）
            4、验证app_id是否为该商户本身。
            */
            Dictionary<string, string> sArray = GetRequestGet();
            if (sArray.Count != 0)
            {
                bool flag = AlipaySignature.RSACheckV1(sArray, WeixinConfig.payKey, WeixinConfig.charset, WeixinConfig.signType, false);//支付宝公钥
                //订单号
                OrdersEntity order = ordersbll.GetEntityByOrderSn(sArray["out_trade_no"]);
                if (flag)
                {
                    ViewBag.Result = "支付成功";
                    ViewBag.icon = "success";
                    ViewBag.display = "none";
                    ViewBag.Tel = order.Tel;
                    LogHelper.AddLog("同步验证通过,订单号：" + order.Tel);
                }
                else
                {
                    ViewBag.Result = "未支付";
                    ViewBag.icon = "warn";
                    ViewBag.display = "block";
                    ViewBag.Tel = order.Tel;
                    ViewBag.TelphoneID = order.TelphoneID;
                    ViewBag.Price = order.Price;
                    LogHelper.AddLog("同步验证失败,订单号：" + order.Tel);
                }
            }
            return View();
        }

        /// <summary>
        /// 服务器异步通知页面
        /// </summary>
        public void AliPayNotifyUrl()
        {
            ////Pay.Log Log = new Pay.Log(AliPayConfig.LogPath);
            //IDictionary<string, string> map = GetRequestPost();

            //LogHelper.AddLog("AliPayNotifyUrl：支付页面异步回调："+ map.Count);
            //if (map.Count > 0)
            //{
            //    try
            //    {
            //        string alipayPublicKey = AliPayConfig.payKey;
            //        string signType = AliPayConfig.signType;
            //        string charset = AliPayConfig.charset;
            //        bool keyFromFile = false;

            //        bool verify_result = AlipaySignature.RSACheckV1(map, alipayPublicKey, charset, signType, keyFromFile);
            //        LogHelper.AddLog("AliPayNotifyUrl验签:" + verify_result + "");

            //        //验签成功后，按照支付结果异步通知中的描述，对支付结果中的业务内容进行二次校验，校验成功后再response中返回success并继续商户自身业务处理，校验失败返回false
            //        if (verify_result)
            //        {
            //            //商户订单号
            //            string out_trade_no = map["out_trade_no"];
            //            //支付宝交易号
            //            string trade_no = map["trade_no"];
            //            //交易创建时间
            //            string gmt_create = map["gmt_create"];
            //            //交易付款时间
            //            string gmt_payment = map["gmt_payment"];
            //            //通知时间
            //            string notify_time = map["notify_time"];
            //            //通知类型  trade_status_sync
            //            string notify_type = map["notify_type"];
            //            //通知校验ID
            //            string notify_id = map["notify_id"];
            //            //开发者的app_id
            //            string app_id = map["app_id"];
            //            //卖家支付宝用户号
            //            string seller_id = map["seller_id"];
            //            //买家支付宝用户号
            //            string buyer_id = map["buyer_id"];
            //            //实收金额
            //            string receipt_amount = map["receipt_amount"];
            //            //交易状态
            //            string return_code = map["trade_status"];

            //            //交易状态TRADE_FINISHED的通知触发条件是商户签约的产品不支持退款功能的前提下，买家付款成功；
            //            //或者，商户签约的产品支持退款功能的前提下，交易已经成功并且已经超过可退款期限
            //            //状态TRADE_SUCCESS的通知触发条件是商户签约的产品支持退款功能的前提下，买家付款成功
            //            if (return_code == "TRADE_FINISHED" || return_code == "TRADE_SUCCESS")
            //            {
            //                string msg;

            //                LogHelper.AddLog("AliPayNotifyUrl：" + receipt_amount + "==" + trade_no + "==" + return_code + "==" + out_trade_no + "==" + gmt_payment);

            //                //判断该笔订单是否在商户网站中已经做过处理
            //                ///支付回调的业务处理
            //                //bool res = OrderBll.Value.CompleteAliPay(receipt_amount, trade_no, return_code, out_trade_no, gmt_payment, out msg);


            //                //订单号
            //                OrdersEntity order = ordersbll.GetEntityByOrderSn(out_trade_no);

            //                order.PayDate = DateTime.Now;
            //                order.PayStatus = (int)PayStatus.已支付;
            //                order.Status = (int)OrderStatus.未发货;
            //                ordersbll.SaveForm(order.Id, order);

            //                TelphoneLiangEntity tel = tlbll.GetEntity(order.TelphoneID);//根据靓号id获取靓号，修改售出状态
            //                if (tel != null)
            //                {
            //                    tel.SellMark = 1;
            //                    tel.SellerName = order.Host;
            //                }
            //                tlbll.SaveForm(tel.TelphoneID, tel);

            //                bool res = true;

            //                if (res == false)
            //                {
            //                    Response.Write("添加支付信息失败!");
            //                }
            //                LogHelper.AddLog("AliPayNotifyUrl：支付成功:" + out_trade_no + "下架：" + tel.TelphoneID);
            //                Response.Write("success");  //请不要修改或删除
            //            }
            //        }
            //        else
            //        {
            //            //验证失败
            //            LogHelper.AddLog("AliPayNotifyUrl：支付验证失败");
            //            Response.Write("验证失败!");
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        Response.Write("添加支付信息失败!");
            //        LogHelper.AddLog("AliPayNotifyUrl：添加支付信息失败");
            //    }
            //}
            //else
            //{
            //    //无返回参数
            //    Response.Write("无返回参数!");
            //    LogHelper.AddLog("AliPayNotifyUrl：无返回参数");
            //}

            /* 实际验证过程建议商户添加以下校验。
            1、商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，
            2、判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），
            3、校验通知中的seller_id（或者seller_email) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id/seller_email）
            4、验证app_id是否为该商户本身。
            */
            Dictionary<string, string> sArray = GetRequestPost();
            if (sArray.Count != 0)
            {
                bool flag = AlipaySignature.RSACheckV1(sArray, WeixinConfig.payKey, WeixinConfig.charset, WeixinConfig.signType, false);//支付宝公钥
                //订单号
                OrdersEntity order = ordersbll.GetEntityByOrderSn(sArray["out_trade_no"]);
                if (flag)
                {
                    //交易状态
                    //判断该笔订单是否在商户网站中已经做过处理
                    //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                    //请务必判断请求时的total_amount与通知时获取的total_fee为一致的
                    //如果有做过处理，不执行商户的业务程序


                    order.PayDate = DateTime.Now;
                    order.PayStatus = (int)PayStatus.已支付;
                    order.Status = (int)OrderStatus.未发货;
                    ordersbll.SaveForm(order.Id, order);

                    TelphoneLiangEntity tel = tlbll.GetEntity(order.TelphoneID);//根据靓号id获取靓号，修改售出状态
                    if (tel != null)
                    {
                        tel.SellMark = 1;
                        tel.SellerName = order.Host;
                    }
                    tlbll.SaveForm(tel.TelphoneID, tel);

                    //注意：
                    //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
                    string trade_status = Request.Form["trade_status"];

                    LogHelper.AddLog("异步调用success,订单号："+ order.Id);
                }
                else
                {
                    LogHelper.AddLog("异步调用fail,订单号：" + order.Id);
                }
            }
        }


        public Dictionary<string, string> GetRequestPost()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //coll = Request.Form;
            coll = Request.Form;
            String[] requestItem = coll.AllKeys;
            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }
            return sArray;

        }

        public Dictionary<string, string> GetRequestGet()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //coll = Request.Form;
            coll = Request.QueryString;
            String[] requestItem = coll.AllKeys;
            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }
            return sArray;

        }
    }
}
