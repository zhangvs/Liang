using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using HZSoft.Application.Busines.BaseManage;
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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HZSoft.Application.Web.Areas.webapp.Controllers
{
    public class ShopController : Controller
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
        public ActionResult ListData(string keyword,string city, int? page, string orderType, string price, string except, string yuyi, string features)
        {
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
                        { "ExistMark",1 }
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
                    var entityList = tlbll.GetPageListH5(pagination, queryJson.ToString());

                    string styleStr = "";
                    foreach (var item in entityList)
                    {
                        string qian = item.Telphone.Substring(0, 3);
                        string zhong = item.Telphone.Substring(3, 4);
                        string hou = item.Telphone.Substring(7, 4);
                        string telphone = "<font color='#E33F23'>" + qian + "</font><font color='#3A78F3'>" + zhong + "</font><font color='#E33F23'>" + hou + "</font>";
                        //利新价格调整规则，这是需要单独写代码的价格调整：
                        decimal? jg = GetJG(item.Price, item.Grade);

                        styleStr +=
                        $" <li> " +
                        $"    <a href='/webapp/shop/mobileinfo/{item.TelphoneID}'>" +
                        $"        <div class='mobile'>{telphone}</div>" +
                        $"        <div class='city'>{item.City}·{item.Operator}</div>" +
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


        public ActionResult mobileinfo(int? id)
        {
            TelphoneLiangEntity entity = tlbll.GetEntity(id);
            if (entity!=null)
            {
                //利新价格调整规则，这是需要单独写代码的价格调整：
                entity.Price = GetJG(entity.Price, entity.Grade);
                entity.MaxPrice = entity.Price * 2;
            }

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


        [HttpPost]
        public ActionResult ajaxorder(OrdersEntity ordersEntity)
        {
            //return Content("{\"code\":true,\"status\":true,\"msg\":\"提交成功！\",\"data\":{\"appid\":\"wx288f944166a4bdc6\",\"code_url\":\"weixin://wxpay/bizpayurl?pr=K9tQFgw\",\"mch_id\":\"1582948931\",\"nonce_str\":\"gelx5Eej34TWkYjL\",\"prepay_id\":\"wx18152655644502b82539bf421260374600\",\"result_code\":\"SUCCESS\",\"return_code\":\"SUCCESS\",\"return_msg\":null,\"sign\":\"4D19F96F050056C904DBD7371D974905\",\"trade_type\":\"NATIVE\",\"trade_no\":\"LX-20200418151928103008\",\"payid\":\"11\",\"wx_query_href\":\"http://localhost:4066/WeChatManage/user_order/queryWx/11\",\"wx_query_over\":\"http://localhost:4066/WeChatManage/user_order/paymentFinish/11\"}}");
            try
            {
                ordersEntity.Host = "Shop";
                ordersEntity = ordersbll.SaveForm(ordersEntity);

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
                        payid = ordersEntity.Id.ToString(),
                        wx_query_href = Config.GetValue("Domain2") + "/WeChatManage/user_order/queryWx/" + ordersEntity.Id,
                        wx_query_over = Config.GetValue("Domain2") + "/WeChatManage/user_order/paymentFinish/" + ordersEntity.Id
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


        public ActionResult zfb()
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "app_id", "merchant_private_key", "json", "1.0", "RSA2", "alipay_public_key", "GBK", false);
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
                    //0 待付款 1 待审核 2 待发货 3 待开卡 4 已完成
                    switch (ordersEntity.Status)
                    {
                        case 1:
                            msg = "待审核";
                            break;
                        case 2:
                            msg = "待发货";
                            break;
                        case 3:
                            msg = "已发货待开卡，" + ordersEntity.ExpressCompany + "：" + ordersEntity.ExpressSn;
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
            return View();
        }




        public decimal? GetJG(decimal? price, string grade)
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
    }
}
