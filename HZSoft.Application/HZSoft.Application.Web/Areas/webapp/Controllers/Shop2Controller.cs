using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using HZSoft.Application.Busines.BaseManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Application.Web.Utility.AliPay;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace HZSoft.Application.Web.Areas.webapp.Controllers
{
    public class Shop2Controller : Controller
    {

        private TelphoneLiangBLL tlbll = new TelphoneLiangBLL();
        private OrdersBLL ordersbll = new OrdersBLL();
        private OrganizeBLL organizebll = new OrganizeBLL();

        private static TenPayV3Info tenPayV3Info = new TenPayV3Info(WeixinConfig.AppID2, WeixinConfig.AppSecret2, WeixinConfig.MchId
            , WeixinConfig.Key, WeixinConfig.TenPayV3Notify);
        
        //
        // GET: /webapp/Shop/
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
            string host= Request.Url.Host;
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
                        decimal? jg = GetJG(item.Price, item.Grade, item.ExistMark);

                        styleStr +=
                        $" <li> " +
                        $"    <a href='https://shop.jnlxsm.net/webapp/shop2/mobileinfo/{item.TelphoneID}?host={host}'>" +//跳转到135服务器
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


        public ActionResult mobileinfo(int? id,string host)
        {
            TelphoneLiangEntity entity = tlbll.GetEntity(id);
            if (entity != null)
            {
                //利新价格调整规则，这是需要单独写代码的价格调整：
                entity.Price = GetJG(entity.Price, entity.Grade, entity.ExistMark);
                entity.MaxPrice = entity.Price * 2;
                ViewBag.host = host;
            }

            return View(entity);
        }
        //
        // GET: /H5/Home/

        public ActionResult mobileorder(int? id, string Tel, string Price,string host)
        {
            ViewBag.id = id;
            ViewBag.Tel = Tel;
            ViewBag.Price = Price;
            ViewBag.host = host;
            return View();
        }


        [HttpPost]
        public ActionResult ajaxorder(OrdersEntity ordersEntity)
        {
            //return Content("{\"code\":true,\"status\":true,\"msg\":\"提交成功！\",\"data\":{\"appid\":\"wx288f944166a4bdc6\",\"code_url\":\"weixin://wxpay/bizpayurl?pr=K9tQFgw\",\"mch_id\":\"1582948931\",\"nonce_str\":\"gelx5Eej34TWkYjL\",\"prepay_id\":\"wx18152655644502b82539bf421260374600\",\"result_code\":\"SUCCESS\",\"return_code\":\"SUCCESS\",\"return_msg\":null,\"sign\":\"4D19F96F050056C904DBD7371D974905\",\"trade_type\":\"NATIVE\",\"trade_no\":\"LX-20200418151928103008\",\"payid\":\"11\",\"wx_query_href\":\"http://localhost:4066/webapp/shop2/queryWx/11\",\"wx_query_over\":\"http://localhost:4066/webapp/shop2/paymentFinish/11\"}}");
            try
            {
                string[] area = ordersEntity.City.Split(' ');
                if (area.Length > 0)
                {
                    ordersEntity.Province = area[0];//省
                    ordersEntity.City = area[1];//市
                }

                
                ordersEntity = ordersbll.SaveForm(ordersEntity);

                var sp_billno = ordersEntity.OrderSn;
                var nonceStr = TenPayV3Util.GetNoncestr();

                //商品Id，用户自行定义
                string productId = ordersEntity.TelphoneID.ToString();

                if (ordersEntity.PayType == "alipay")
                {
                    Redirect("alipay");
                }

                H5Response root = null;
                //pc端返回二维码，否则H5
                if (ordersEntity.PC == 1)
                {
                    //创建请求统一订单接口参数
                    var xmlDataInfo = new TenPayV3UnifiedorderRequestData(WeixinConfig.AppID2,
                    tenPayV3Info.MchId,
                    "扫码支付靓号",
                    sp_billno,
                    //Convert.ToInt32(ordersEntity.Price * 100),
                    1,
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
                            wx_query_href = Config.GetValue("Domain2") + "/webapp/shop2/queryWx/" + ordersEntity.Id,
                            wx_query_over = Config.GetValue("Domain2") + "/webapp/shop2/paymentFinish/" + ordersEntity.Id
                        };

                        root = new H5Response { code = true, status = true, msg = "\u63d0\u4ea4\u6210\u529f\uff01", data = h5PayData };
                    }
                    else
                    {
                        root = new H5Response { code = false, status = false, msg = result.return_msg };
                    }
                }
                else
                {
                    var xmlDataInfoH5 = new TenPayV3UnifiedorderRequestData(WeixinConfig.AppID2, tenPayV3Info.MchId, "H5购买靓号", sp_billno, 1,
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
                            wx_query_href = Config.GetValue("Domain2") + "/webapp/shop2/queryWx/" + ordersEntity.Id,
                            wx_query_over = Config.GetValue("Domain2") + "/webapp/shop2/paymentFinish/" + ordersEntity.Id
                        };

                        root = new H5Response { code = true, status = true, msg = "\u63d0\u4ea4\u6210\u529f\uff01", data = h5PayData };

                        //var timeStamp = TenPayV3Util.GetTimestamp();

                        //var package = string.Format("prepay_id={0}", resultH5.prepay_id);

                        //ViewData["product"] = ordersEntity.Tel;

                        //ViewData["appId"] = WeixinConfig.AppID2;
                        //ViewData["timeStamp"] = timeStamp;
                        //ViewData["nonceStr"] = nonceStr;
                        //ViewData["package"] = package;
                        //ViewData["paySign"] = TenPayV3.GetJsPaySign(WeixinConfig.AppID2, timeStamp, nonceStr, package, tenPayV3Info.Key);

                        ////设置成功页面（也可以不设置，支付成功后默认返回来源地址）
                        //var returnUrl = Config.GetValue("Domain2") + "/webapp/shop2/paymentFinish/" + ordersEntity.Id;

                        //var mwebUrl = resultH5.mweb_url;
                        //if (!string.IsNullOrEmpty(returnUrl))
                        //{
                        //    mwebUrl += string.Format("&redirect_url={0}", returnUrl.AsUrlData());
                        //}

                        //ViewData["MWebUrl"] = mwebUrl;
                        //return View();
                    }
                    else
                    {
                        root = new H5Response { code = false, status = false, msg = resultH5.return_msg };
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

        public decimal? GetJG(decimal? price, string grade, int? existMark)
        {
            //秒杀号码不同步
            //8001以上价位不变
            //10 - 100价格改成 399元
            //101 - 200加318元
            //201 - 300加308元
            //301 - 600加258
            //601 - 800加208
            //801 - 2500加199
            //2501 - 8000四连号加599 三连号以及其他号码价格不动

            decimal? jg = price;
            if (existMark == 2)
            {
                return jg;//秒杀价格不变
            }
            if (jg > 0 && jg <= 100)
            {
                jg = 399;
            }
            else if (jg > 101 && jg <= 200)
            {
                jg = jg + 318;
            }
            else if (jg > 201 && jg <= 300)
            {
                jg = jg + 308;
            }
            else if (jg > 301 && jg <= 600)
            {
                jg = jg + 258;
            }
            else if (jg > 601 && jg <= 800)
            {
                jg = jg + 208;
            }
            else if (jg > 801 && jg <= 2500)
            {
                jg = jg + 199;
            }
            else if (jg > 2501 && jg <= 8000 && grade == "3")
            {
                jg = jg + 599;
            }
            return jg;
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
            for (int i = 0; i < 5; i++)
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
                    LogHelper.AddLog(id + "支付结果获取："+i);
                }
            }
            //如果超过15秒还未支付成功，返回未支付，防止直接跳转结果页面，显示失败，微信回调还没有完成
            ViewBag.Result = "未支付";
            ViewBag.icon = "warn";
            ViewBag.display = "block";
            ViewBag.Tel = ordersEntity.Tel;
            ViewBag.TelphoneID = ordersEntity.TelphoneID;
            ViewBag.Price = ordersEntity.Price;
            return View();
        }










        public ActionResult zfb()
        {
            IAopClient client = Utility.AliPay.AliPay.GetAlipayClient();
            //IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "app_id", "merchant_private_key", "json", "1.0", "RSA2", "alipay_public_key", "GBK", false);
            AlipayTradeCreateRequest request = new AlipayTradeCreateRequest();
            request.BizContent = "{" +
            "\"out_trade_no\":\"20150320010101001\"," +
            "\"seller_id\":\"2088102146225135\"," +
            "\"total_amount\":88.88," +
            "\"discountable_amount\":8.88," +
            "\"subject\":\"Iphone6 16G\"," +
            "\"body\":\"Iphone6 16G\"," +
            "\"buyer_id\":\"2088102146225135\"," +
            "      \"goods_detail\":[{" +
            "        \"goods_id\":\"apple-01\"," +
            "\"goods_name\":\"ipad\"," +
            "\"quantity\":1," +
            "\"price\":2000," +
            "\"goods_category\":\"34543238\"," +
            "\"categories_tree\":\"124868003|126232002|126252004\"," +
            "\"body\":\"特价手机\"," +
            "\"show_url\":\"http://www.alipay.com/xxx.jpg\"" +
            "        }]," +
            "\"product_code\":\"FACE_TO_FACE_PAYMENT\"," +
            "\"operator_id\":\"Yx_001\"," +
            "\"store_id\":\"NJ_001\"," +
            "\"terminal_id\":\"NJ_T_001\"," +
            "\"extend_params\":{" +
            "\"sys_service_provider_id\":\"2088511833207846\"," +
            "\"card_type\":\"S0JP0000\"" +
            "    }," +
            "\"timeout_express\":\"90m\"," +
            "\"settle_info\":{" +
            "        \"settle_detail_infos\":[{" +
            "          \"trans_in_type\":\"cardAliasNo\"," +
            "\"trans_in\":\"A0001\"," +
            "\"summary_dimension\":\"A0001\"," +
            "\"settle_entity_id\":\"2088xxxxx;ST_0001\"," +
            "\"settle_entity_type\":\"SecondMerchant、Store\"," +
            "\"amount\":0.1" +
            "          }]," +
            "\"settle_period_time\":\"7d\"" +
            "    }," +
            "\"logistics_detail\":{" +
            "\"logistics_type\":\"EXPRESS\"" +
            "    }," +
            "\"business_params\":{" +
            "\"campus_card\":\"0000306634\"," +
            "\"card_type\":\"T0HK0000\"," +
            "\"actual_order_time\":\"2019-05-14 09:18:55\"" +
            "    }," +
            "\"receiver_address_info\":{" +
            "\"name\":\"张三\"," +
            "\"address\":\"上海市浦东新区陆家嘴银城中路501号\"," +
            "\"mobile\":\"13120180615\"," +
            "\"zip\":\"200120\"," +
            "\"division_code\":\"310115\"" +
            "    }" +
            "  }";
            AlipayTradeCreateResponse response = client.Execute(request);
            Console.WriteLine(response.Body);
            return null;
        }
        /// <summary>
        /// 订单编号
        /// </summary>
        /// <param name="oidStr"></param>
        /// <returns></returns>
        public void AliPay()
        {

            #region 统一下单
            try
            {

                DefaultAopClient client = new DefaultAopClient(WeixinConfig.serviceUrl, WeixinConfig.appId, WeixinConfig.privateKey, "json", "1.0",
                    WeixinConfig.signType, WeixinConfig.publicKey, WeixinConfig.charset, false);

                // 外部订单号，商户网站订单系统中唯一的订单号
                string out_trade_no = "20170216test01";

                // 订单名称
                string subject = "App支付测试DoNet";

                // 付款金额
                string total_amout = "0.01";

                // 商品描述
                string body = "我是测试数据";

                // 支付中途退出返回商户网站地址
                string quit_url = "https://wl012.rrzwl.com/webapp/shop2/index";

                // 组装业务参数model
                AlipayTradeWapPayModel model = new AlipayTradeWapPayModel();
                model.Body = body;
                model.Subject = subject;
                model.TotalAmount = total_amout;
                model.OutTradeNo = out_trade_no;
                model.ProductCode = "QUICK_WAP_WAY";
                model.QuitUrl = quit_url;

                AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();
                // 设置支付完成同步回调地址
                // request.SetReturnUrl("");
                // 设置支付完成异步通知接收地址
                // request.SetNotifyUrl("");
                // 将业务model载入到request
                request.SetBizModel(model);

                AlipayTradeWapPayResponse response = null;
                try
                {
                    response = client.pageExecute(request, null, "post");
                    Response.Write(response.Body);
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
            #endregion
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






//支付宝

////var notify_url = AliPayConfig.notify_url;
////var return_url = AliPayConfig.return_url;
//IAopClient client = Utility.AliPay.AliPay.GetAlipayClient();
//AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
////SDK已经封装掉了公共参数，这里只需要传入业务参数。以下方法为sdk的model入参方式(model和biz_content同时存在的情况下取biz_content)。
//AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
//model.Subject = "商品购买";
//model.TotalAmount = payPrice.ToString("F2");
//model.ProductCode = "QUICK_MSECURITY_PAY";
//Random rd = new Random();
//var payNum = DateTime.Now.ToString("yyyyMMddHHmmss") + rd.Next(0, 1000).ToString().PadLeft(3, '0');
//model.OutTradeNo = payNum;
//model.TimeoutExpress = "30m";
//request.SetBizModel(model);
////request.SetNotifyUrl(notify_url);
////request.SetReturnUrl(return_url);
////这里和普通的接口调用不同，使用的是sdkExecute
//AlipayTradeAppPayResponse response = client.SdkExecute(request);

////统一下单
////OrderBll.Value.UpdateOrderApp(oIds, payNum);
//Response.Write(HttpUtility.HtmlEncode(response.Body));
////return Json(true, new { response.Body }, "OK");
//return null;



//IAopClient client = Utility.AliPay.AliPay.GetAlipayClient();
////IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", APPID, APP_PRIVATE_KEY, "json", "1.0", "RSA2", ALIPAY_PUBLIC_KEY, CHARSET, false);
////实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
//AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
////SDK已经封装掉了公共参数，这里只需要传入业务参数。以下方法为sdk的model入参方式(model和biz_content同时存在的情况下取biz_content)。
//AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
//model.Body = "我是测试数据";
//model.Subject = "App支付测试DoNet";
//model.TotalAmount = "0.01";
//model.ProductCode = "QUICK_MSECURITY_PAY";
//model.OutTradeNo = "20170216test01";
//model.TimeoutExpress = "30m";
//request.SetBizModel(model);
//request.SetNotifyUrl("https://shop.jnlxsm.net/WeChatManage/WeiXinHome/AliPayNotifyUrl");
////这里和普通的接口调用不同，使用的是sdkExecute
//AlipayTradeAppPayResponse response = client.SdkExecute(request);
////HttpUtility.HtmlEncode是为了输出到页面时防止被浏览器将关键参数html转义，实际打印到日志以及http传输不会有这个问题
//Response.Write(HttpUtility.HtmlEncode(response.Body));
////页面输出的response.Body就是orderString 可以直接给客户端请求，无需再做处理。
//return null;