using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using HZSoft.Application.Busines.BaseManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Application.Code;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Application.Web.Utility.AliPay;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace HZSoft.Application.Web.Areas.webapp.Controllers
{
    /// <summary>
    /// 广州头条：先跳转到www.jnlxsm.net:8064，
    /// 再跳转到 shop.jnlxsm.net
    /// </summary>
    public class jinan2Controller : Controller
    {

        private TelphoneLiangH5BLL tlbll = new TelphoneLiangH5BLL();
        private OrdersBLL ordersbll = new OrdersBLL();
        private OrganizeBLL organizebll = new OrganizeBLL();

        private static TenPayV3Info tenPayV3Info = new TenPayV3Info(WeixinConfig.AppID2, WeixinConfig.AppSecret2, WeixinConfig.MchId
            , WeixinConfig.Key, WeixinConfig.TenPayV3Notify);
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Apply()
        {
            return View();
        }
        public ActionResult info2()
        {
            return View();
        }

        /// <summary>
        /// 模糊搜索等 + '&price=' + price + '&except=' + except + '&yy=' + yuyi;
        /// </summary>
        /// <returns></returns>
        public ActionResult ListData(string keyword, string city, int? page, string orderType, string price, string except, string yuyi, string features)
        {
            string host = Request.Url.Host + Request.Url.Port;
            int ipage = page == null ? 1 : int.Parse(page.ToString());
            string organizeId = "bae859c9-3df5-4da0-bea9-3e20bbc7c353";
            if (!string.IsNullOrEmpty(organizeId))
            {
                var organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    JObject queryJson = new JObject { { "Telphone", keyword },
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city },
                        { "price", price },
                        { "except", except },
                        { "yuyi", yuyi },
                        { "Grade",features },
                        { "SellMark",0 },
                        //{ "ExistMark","1,2" }
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
                        //sidx = "right(Telphone,1)";//按照最后一位排序
                        //sord = "asc";
                        sidx = "TelphoneID";//按照最后一位排序
                        sord = "desc";
                    }
                    Pagination pagination = new Pagination()
                    {
                        rows = 40,
                        page = ipage,
                        sidx = sidx,
                        sord = sord
                    };
                    var entityList = tlbll.GetPageListH5LX(pagination, queryJson.ToString());//自身秒杀可卖，其它平台秒杀不卖

                    string styleStr = "";
                    foreach (var item in entityList)
                    {
                        string qian = item.Telphone.Substring(0, 3);
                        string zhong = item.Telphone.Substring(3, 4);
                        string hou = item.Telphone.Substring(7, 4);
                        string telphone = "<font color='#E33F23'>" + qian + "</font><font color='#3A78F3'>" + zhong + "</font><font color='#E33F23'>" + hou + "</font>";
                        //利新价格调整规则，这是需要单独写代码的价格调整：
                        decimal? jg = item.Price;

                        styleStr +=
                        $" <li> " +
                        //$"    <a href='https://shop.jnlxsm.net/webapp/jinan2/product/{item.TelphoneID}?host={host}'>" +//跳转到135服务器详情页面
                        $"    <a href=\"#\" onclick=\"toInfo('{item.TelphoneID}','{host}')\">" +//跳转到135服务器
                        $"        <div class='mobile'>{telphone}</div>" +
                        $"        <div class='city'>{item.City}·{item.Description}</div>" +//·{item.Operator}
                        $"        <div class='price'>" +
                        $"            <i>￥</i>{jg}" +
                        $"            <span class='hide oldprice'>原价<i><i>￥</i>{jg * 2}</i></span>" +
                        //$"            <span class='hide huafei'>话费0</span>" +
                        $"        </div>" +
                        $"    </a>" +
                        $"</li>";
                    }
                    return Content(styleStr);
                }
                return Content("机构暂时未生效或不存在");
            }
            else
            {
                return Content("链接不正确不或完整");
            }
        }


        public ActionResult mobileinfo(int? id, string host)
        {
            TelphoneLiangH5Entity entity = tlbll.GetEntity(id);
            if (entity != null)
            {
                entity.MaxPrice = entity.Price * 2;
                ViewBag.host = host;
            }

            return View(entity);
        }
        //
        // GET: /H5/Home/

        public ActionResult mobileorder(int? id, string Tel, string Price, string host)
        {
            ViewBag.id = id;
            ViewBag.Tel = Tel;
            ViewBag.Price = Price;
            ViewBag.host = host;
            return View();
        }


        public ActionResult product(int? id, string host)
        {
            TelphoneLiangH5Entity entity = tlbll.GetEntity(id);
            if (entity != null)
            {
                entity.MaxPrice = entity.Price * 2;
                ViewBag.host = host;
            }

            return View(entity);
        }

        [HttpPost]
        public ActionResult ajaxorder(OrdersEntity ordersEntity)
        {
            try
            {
                string city = ordersEntity.City;
                if (city.Contains("-"))
                {
                    string[] area = ordersEntity.City.Split('-');
                    if (area.Length > 0)
                    {
                        ordersEntity.Province = area[0];//省
                        ordersEntity.City = area[1];//市
                        ordersEntity.Country = area[2];//市
                    }
                }
                else
                {
                    string[] area = ordersEntity.City.Split(' ');
                    if (area.Length > 0)
                    {
                        ordersEntity.Province = area[0];//省
                        ordersEntity.City = area[1];//市
                    }
                }

                //创建订单表
                string payType = ordersEntity.PayType;
                if (payType == "alipay")
                {
                    payType = "支付宝";
                }
                else
                {
                    if (ordersEntity.PC == 1)
                    {
                        payType = "微信扫码";
                    }
                    else
                    {
                        payType = "微信H5";
                    }
                }
                ordersEntity.PayType = payType;
                ordersEntity = ordersbll.SaveForm(ordersEntity);

                var sp_billno = ordersEntity.OrderSn;
                var nonceStr = TenPayV3Util.GetNoncestr();

                //商品Id，用户自行定义
                string productId = ordersEntity.TelphoneID.ToString();

                H5Response root = null;

                if (payType == "支付宝")
                {
                    try
                    {
                        DefaultAopClient client = new DefaultAopClient(WeixinConfig.serviceUrl, WeixinConfig.aliAppId, WeixinConfig.privateKey, "json", "1.0",
                            WeixinConfig.signType, WeixinConfig.payKey, WeixinConfig.charset, false);

                        // 组装业务参数model
                        AlipayTradeWapPayModel model = new AlipayTradeWapPayModel();
                        model.Body = "支付宝购买靓号";// 商品描述
                        model.Subject = productId;// 订单名称
                        model.TotalAmount = ordersEntity.Price.ToString();// 付款金额"0.01"
                        model.OutTradeNo = sp_billno;// 外部订单号，商户网站订单系统中唯一的订单号
                        model.ProductCode = "QUICK_WAP_WAY";
                        model.QuitUrl = "https://www.jnlxsm.net:8064/webapp/jinan2/index";// 支付中途退出返回商户网站地址
                        model.TimeoutExpress = "90m";
                        AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();
                        //设置支付完成同步回调地址
                        request.SetReturnUrl(WeixinConfig.return_url);
                        //设置支付完成异步通知接收地址
                        request.SetNotifyUrl(WeixinConfig.notify_url);
                        // 将业务model载入到request
                        request.SetBizModel(model);

                        AlipayTradeWapPayResponse response = null;
                        try
                        {
                            response = client.pageExecute(request, null, "post");
                            //Response.Write(response.Body);

                            H5PayData h5PayData = new H5PayData();
                            h5PayData.form = response.Body;
                            root = new H5Response { code = true, status = true, msg = "\u652f\u4ed8\u5b9d\u63d0\u4ea4\u6210\u529f\uff01", data = h5PayData };

                        }
                        catch (Exception exp)
                        {
                            throw exp;
                        }
                    }
                    catch (Exception ex)
                    {
                        //return Json(new { Result = false, msg = "缺少参数" });
                    }
                }
                else
                {
                    //0 手机（H5支付）  1 电脑（扫码Native支付），2微信浏览器（JSAPI）
                    //pc端返回二维码，否则H5
                    if (payType == "微信扫码")
                    {
                        //创建请求统一订单接口参数
                        var xmlDataInfo = new TenPayV3UnifiedorderRequestData(WeixinConfig.AppID2,
                        tenPayV3Info.MchId,
                        "扫码支付靓号",
                        sp_billno,
                        Convert.ToInt32(ordersEntity.Price * 100),
                        //1,
                        Request.UserHostAddress,
                        tenPayV3Info.TenPayV3Notify,
                       TenPayV3Type.NATIVE,
                        null,
                        tenPayV3Info.Key,
                        nonceStr,
                        productId: productId);
                        //调用统一订单接口
                        var result = TenPayV3.Unifiedorder(xmlDataInfo);
                        if (result.return_code == "SUCCESS")
                        {
                            H5PayData h5PayData = new H5PayData()
                            {
                                appid = WeixinConfig.AppID2,
                                code_url = result.code_url,//weixin://wxpay/bizpayurl?pr=lixpXgt-----------扫码支付
                                mch_id = WeixinConfig.MchId,
                                nonce_str = result.nonce_str,
                                prepay_id = result.prepay_id,
                                result_code = result.result_code,
                                return_code = result.return_code,
                                return_msg = result.return_msg,
                                sign = result.sign,
                                trade_type = "NATIVE",
                                trade_no = sp_billno,
                                payid = ordersEntity.Id.ToString(),
                                wx_query_href = "https://shop.jnlxsm.net/webapp/jinan2/queryWx/" + ordersEntity.Id,
                                wx_query_over = "https://shop.jnlxsm.net/webapp/jinan2/paymentFinish/" + ordersEntity.Id
                            };

                            root = new H5Response { code = true, status = true, msg = "\u5fae\u4fe1\u626b\u7801\u63d0\u4ea4\u6210\u529f\uff01", data = h5PayData };
                        }
                        else
                        {
                            root = new H5Response { code = false, status = false, msg = result.return_msg };
                        }
                    }
                    else
                    {
                        var xmlDataInfoH5 = new TenPayV3UnifiedorderRequestData(WeixinConfig.AppID2, tenPayV3Info.MchId, "H5购买靓号", sp_billno,
                        // 1,
                        Convert.ToInt32(ordersEntity.Price * 100),
                        Request.UserHostAddress, tenPayV3Info.TenPayV3Notify, TenPayV3Type.MWEB/*此处无论传什么，方法内部都会强制变为MWEB*/, null, tenPayV3Info.Key, nonceStr);

                        var resultH5 = TenPayV3.Html5Order(xmlDataInfoH5);//调用统一订单接口
                        LogHelper.AddLog(resultH5.ResultXml);//记录日志
                        if (resultH5.return_code == "SUCCESS")
                        {
                            H5PayData h5PayData = new H5PayData()
                            {
                                appid = WeixinConfig.AppID2,
                                mweb_url = resultH5.mweb_url,//H5访问链接
                                mch_id = WeixinConfig.MchId,
                                nonce_str = resultH5.nonce_str,
                                prepay_id = resultH5.prepay_id,
                                result_code = resultH5.result_code,
                                return_code = resultH5.return_code,
                                return_msg = resultH5.return_msg,
                                sign = resultH5.sign,
                                trade_type = "H5",
                                trade_no = sp_billno,
                                payid = ordersEntity.Id.ToString(),
                                wx_query_href = "https://shop.jnlxsm.net/webapp/jinan2/queryWx/" + ordersEntity.Id,
                                wx_query_over = "https://shop.jnlxsm.net/webapp/jinan2/paymentFinish/" + ordersEntity.Id
                            };

                            root = new H5Response { code = true, status = true, msg = "\u5fae\u4fe1\u0048\u0035\u63d0\u4ea4\u6210\u529f\uff01", data = h5PayData };
                        }
                        else
                        {
                            root = new H5Response { code = false, status = false, msg = resultH5.return_msg };
                        }
                    }
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

        //需要OAuth登录
        [HandlerWX2AuthorizeAttribute(LoginMode.Enforce)]
        public ActionResult JsApi(int? id, string Tel, string Price, string host)
        {
            OrdersEntity ordersEntity = new OrdersEntity()
            {
                TelphoneID = id,
                Tel = Tel,
                Price = Convert.ToDecimal(Price),
                Host = host,
                PayType = "JsApi"
            };

            //创建订单表
            ordersEntity = ordersbll.SaveForm(ordersEntity);

            var openId = (string)Session["OpenId"];
            var sp_billno = ordersEntity.OrderSn;
            var nonceStr = TenPayV3Util.GetNoncestr();
            var timeStamp = TenPayV3Util.GetTimestamp();

            //商品Id，用户自行定义
            var xmlDataInfoH5 = new TenPayV3UnifiedorderRequestData(WeixinConfig.AppID2, tenPayV3Info.MchId, "JSAPI购买靓号", sp_billno,
Convert.ToInt32(ordersEntity.Price * 100),
Request.UserHostAddress, tenPayV3Info.TenPayV3Notify, TenPayV3Type.JSAPI, openId, tenPayV3Info.Key, nonceStr);
            var result = TenPayV3.Unifiedorder(xmlDataInfoH5);//调用统一订单接口
            LogHelper.AddLog(result.ResultXml);//记录日志
            var package = string.Format("prepay_id={0}", result.prepay_id);
            if (result.return_code == "SUCCESS")
            {
                WFTWxModel jsApiPayData = new WFTWxModel()
                {
                    appId = WeixinConfig.AppID2,
                    timeStamp = timeStamp,
                    nonceStr = nonceStr,
                    package = package,
                    paySign = TenPayV3.GetJsPaySign(WeixinConfig.AppID2, timeStamp, nonceStr, package, WeixinConfig.Key),
                    callback_url = "https://shop.jnlxsm.net/webapp/jinan2/paymentFinish/" + ordersEntity.Id
                };
                ViewBag.WxModel = jsApiPayData;
                LogHelper.AddLog(JsonConvert.SerializeObject(jsApiPayData));//记录日志
            }
            return View(ordersEntity);
        }
        

        //需要OAuth登录
        [HttpPost]
        public ActionResult JsApi(OrdersEntity ordersEntity)
        {
            try
            {
                string city = ordersEntity.City;
                if (city.Contains("-"))
                {
                    string[] area = ordersEntity.City.Split('-');
                    if (area.Length > 0)
                    {
                        ordersEntity.Province = area[0];//省
                        ordersEntity.City = area[1];//市
                        ordersEntity.Country = area[2];//市
                    }
                }
                else
                {
                    string[] area = ordersEntity.City.Split(' ');
                    if (area.Length > 0)
                    {
                        ordersEntity.Province = area[0];//省
                        ordersEntity.City = area[1];//市
                    }
                }

                ordersEntity.PayType = "微信JSAPI";
                ordersbll.SaveForm(ordersEntity.Id, ordersEntity);
                H5Response root = new H5Response { code = true, status = true, msg = "\u5fae\u4fe1\u004a\u0053\u0041\u0050\u0049\u63d0\u4ea4\u6210\u529f\uff01", data = { } };
                return Content(JsonConvert.SerializeObject(root));
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                msg += "<br>" + ex.StackTrace;
                msg += "<br>==Source==<br>" + ex.Source;

                if (ex.InnerException != null)
                {
                    msg += "<br>===InnerException===<br>" + ex.InnerException.Message;
                }
                return Content(msg);
            }
        }



        //需要OAuth登录
        [HandlerWX2AuthorizeAttribute(LoginMode.Enforce)]
        public ActionResult productJsApi(int? id, string host)
        {
            TelphoneLiangH5Entity entity = tlbll.GetEntity(id);
            var sp_billno = string.Format("{0}{1}", "LX-", DateTime.Now.ToString("yyyyMMddHHmmss"));
            var openId = (string)Session["OpenId"];
            var nonceStr = TenPayV3Util.GetNoncestr();
            var timeStamp = TenPayV3Util.GetTimestamp();

            //商品Id，用户自行定义
            var xmlDataInfoH5 = new TenPayV3UnifiedorderRequestData(WeixinConfig.AppID2, tenPayV3Info.MchId, "JSAPI购买靓号", sp_billno,
Convert.ToInt32(Convert.ToDecimal(entity.Price) * 100),
Request.UserHostAddress, tenPayV3Info.TenPayV3Notify, TenPayV3Type.JSAPI, openId, tenPayV3Info.Key, nonceStr);
            var result = TenPayV3.Unifiedorder(xmlDataInfoH5);//调用统一订单接口
            LogHelper.AddLog(result.ResultXml);//记录日志
            var package = string.Format("prepay_id={0}", result.prepay_id);
            if (result.return_code == "SUCCESS")
            {
                WFTWxModel jsApiPayData = new WFTWxModel()
                {
                    appId = WeixinConfig.AppID2,
                    timeStamp = timeStamp,
                    nonceStr = nonceStr,
                    package = package,
                    paySign = TenPayV3.GetJsPaySign(WeixinConfig.AppID2, timeStamp, nonceStr, package, WeixinConfig.Key),
                    callback_url = "https://shop.jnlxsm.net/webapp/jinan2/paymentFinish/" + id
                };
                ViewBag.WxModel = jsApiPayData;
                LogHelper.AddLog(JsonConvert.SerializeObject(jsApiPayData));//记录日志
            }
            ViewBag.OrderSn = sp_billno;
            ViewBag.Host = host;
            return View(entity);
        }


        //需要OAuth登录
        [HttpPost]
        public ActionResult productJsApi(OrdersEntity ordersEntity)
        {
            try
            {
                string city = ordersEntity.City;
                if (city.Contains("-"))
                {
                    string[] area = ordersEntity.City.Split('-');
                    if (area.Length > 0)
                    {
                        ordersEntity.Province = area[0];//省
                        ordersEntity.City = area[1];//市
                        ordersEntity.Country = area[2];//市
                    }
                }
                else
                {
                    string[] area = ordersEntity.City.Split(' ');
                    if (area.Length > 0)
                    {
                        ordersEntity.Province = area[0];//省
                        ordersEntity.City = area[1];//市
                    }
                }

                ordersEntity.PayType = "微信JSAPI";
                ordersbll.SaveForm(ordersEntity.Id, ordersEntity);
                H5Response root = new H5Response { code = true, status = true, msg = "\u5fae\u4fe1\u004a\u0053\u0041\u0050\u0049\u63d0\u4ea4\u6210\u529f\uff01", data = { } };
                return Content(JsonConvert.SerializeObject(root));
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                msg += "<br>" + ex.StackTrace;
                msg += "<br>==Source==<br>" + ex.Source;

                if (ex.InnerException != null)
                {
                    msg += "<br>===InnerException===<br>" + ex.InnerException.Message;
                }
                return Content(msg);
            }
        }

        public ActionResult express(string mobile)
        {
            string display = "none";
            if (!string.IsNullOrEmpty(mobile))
            {
                var ordersEntity = ordersbll.GetEntityByTel(mobile);
                if (ordersEntity != null)
                {
                    string msg = "";
                    //0 待付款 1 待发货 2 待开卡 3 已完成
                    switch (ordersEntity.Status)
                    {
                        case 0:
                            msg = "待付款";
                            break;
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
                    ViewBag.msg = msg;
                }
                else
                {
                    ViewBag.msg = "暂无信息";
                }
                display = "block";

            }
            ViewBag.display = display;
            return View();
        }
        
        public ActionResult queryWx(int? id)
        {
            OrdersEntity ordersEntity = ordersbll.GetEntity(id);
            if (ordersEntity.PayStatus == 1)
            {
                return Json(new { status = true });
            }
            return Json(new { status = false });
        }

        /// <summary>
        /// 支付结果页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult paymentFinish(int? id)
        {
            ViewBag.Id = id;
            OrdersEntity ordersEntity = ordersbll.GetEntity(id);//第一次打开，微信回调还没完成，一般是未支付状态
            for (int i = 0; i < 2; i++)
            {
                ordersEntity = ordersbll.GetEntity(id);//第二次才会成功获取到支付成功状态
                if (ordersEntity.PayStatus == 1)//如果支付成功直接返回
                {
                    ViewBag.Result = "支付成功";
                    ViewBag.icon = "success";
                    ViewBag.display = "none";
                    ViewBag.Tel = ordersEntity.Tel;
                    LogHelper.AddLog(id + "支付成功");
                    return View();
                }
                else
                {
                    Thread.Sleep(3000);//当前线程休眠3秒，等待微信回调执行完成
                    LogHelper.AddLog(id + "支付结果获取：" + i);
                }
            }
            //如果超过15秒还未支付成功，返回未支付，防止直接跳转结果页面，显示失败，微信回调还没有完成
            ViewBag.Result = "未支付";
            ViewBag.icon = "warn";
            ViewBag.display = "block";
            ViewBag.Tel = ordersEntity.Tel;
            ViewBag.TelphoneID = ordersEntity.TelphoneID;
            ViewBag.Price = ordersEntity.Price.ToString().Replace(".00", "");
            ViewBag.Host = ordersEntity.Host;
            return View();
        }
        
    }
}
//H5支付点击按钮返回报文
//<xml>
//  <return_code><![CDATA[SUCCESS]]></return_code>
//  <return_msg><![CDATA[OK]]></return_msg>
//  <appid><![CDATA[wx288f944166a4bdc6]]></appid>
//  <mch_id><![CDATA[1582948931]]></mch_id>
//  <nonce_str><![CDATA[N8M3gWuQWTFU4GB7]]></nonce_str>
//  <sign><![CDATA[9BDF874BB44D75ECBED699BCCA55ADB7]]></sign>
//  <result_code><![CDATA[SUCCESS]]></result_code>
//  <prepay_id><![CDATA[wx0821504501009699fc47ba7d1821679000]]></prepay_id>
//  <trade_type><![CDATA[MWEB]]></trade_type>
//  <mweb_url><![CDATA[https://wx.tenpay.com/cgi-bin/mmpayweb-bin/checkmweb?prepay_id=wx0821504501009699fc47ba7d1821679000&package=3205204241]]></mweb_url>
//</xml>