using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Application.Busines.SystemManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HZSoft.Application.Web.Areas.WeChatManage.Controllers
{
    public class MobileCardController : BaseWxUserController
    {

        private TelphoneReserveSeeBLL seebll = new TelphoneReserveSeeBLL();
        private TelphoneReserverBLL bll = new TelphoneReserverBLL();
        private TelphoneSourceBLL telphonesourcebll = new TelphoneSourceBLL();

        private TelphoneOrderBLL orderBll = new TelphoneOrderBLL();


        public ActionResult jrtt_6539()
        {
            return View();
        }

        public ActionResult suning_5227()
        {
            return View();
        }

        /// <summary>
        /// 17052277707
        /// </summary>
        /// <param name="qian8"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu(string qian8)
        {
            var list = telphonesourcebll.GetListQian8(qian8);
            ViewBag.list = list;
            return View();
        }

        public ActionResult Index()
        {
            TelphoneReserveSeeEntity seeEntity = new TelphoneReserveSeeEntity()
            {
                IPAddress = Net.Ip,
                IPAddressName = IPLocation.GetLocation(Net.Ip)
            };
            seebll.SaveForm("", seeEntity);
            return View();
        }

        public ActionResult Index0539()
        {
            TelphoneReserveSeeEntity seeEntity = new TelphoneReserveSeeEntity()
            {
                IPAddress = Net.Ip,
                IPAddressName = IPLocation.GetLocation(Net.Ip)
            };
            seebll.SaveForm("", seeEntity);

            IEnumerable<TelphoneOrderEntity> orderliset = orderBll.GetListTop10();
            foreach (TelphoneOrderEntity item in orderliset)
            {
                item.Description = FormatDateTime(item.CreateDate.ToString());
            }
            ViewBag.list = orderliset;
            return View();
        }

        private CodeRuleBLL codeRuleBLL = new CodeRuleBLL();

        SmsInfoBLL smsBll = new SmsInfoBLL();
        /// <summary>
        /// 提交预约
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveForm(TelphoneOrderEntity entity,string smsCode)
        {
            //1.验证手机验证码是否有效，是否过期
            var sms = smsBll.GetList("{}").Where(t => t.Tel == entity.Contact && t.Type == (int)SmsType.号码预定).OrderByDescending(t => t.CreateDate).FirstOrDefault();
            if (sms!=null)
            {
                ValidateSmsCode(sms, smsCode);

                string[] tels = entity.Telphone.Split(',');
                string[] amounts = entity.PaidDate.ToString().Split(',');
                for (int i = 0; i < tels.Length; i++)
                {
                    entity.Telphone = tels[i];
                    entity.Amount = Convert.ToDecimal(amounts[i]);

                    if (Request["keyValue"] == null)
                    {
                        entity.OrderCode = codeRuleBLL.GetBillCode("c576c3f7-631d-4108-baaf-1495bdc0d6bb");
                        //ViewBag.OrderCode = codeRuleBLL.GetBillCode(SystemInfo.CurrentUserId, "", ((int)CodeRuleEnum.Telphone_OrderCode).ToString());
                    }

                    entity.CheckMark = 0;
                    //插入预约号码
                    orderBll.SaveForm("", entity);

                    //SendToUser(entity);//微信提醒到配置的openid
                    //SendToTemplate();
                }


                return Content("true");
            }
            else
            {
                return Content("验证码有误");
            }

        }

        public string GetToken()
        {
            //通知指定的微信客服
            #region 获取access_token
            string apiurl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wx24e47efa56c2e554&secret=1f8de99c6304d13e5a65efa418638ee4";
            WebRequest req = WebRequest.Create(@apiurl);
            req.Method = "POST";
            WebResponse response = req.GetResponse();
            Stream stream = response.GetResponseStream();
            Encoding encode = Encoding.UTF8;
            StreamReader reader = new StreamReader(stream, encode);
            string detail = reader.ReadToEnd();
            var jd = JsonConvert.DeserializeObject<WXApi>(detail);
            string token = (String)jd.access_token;
            #endregion
            return token;
        }

        public void SendToTemplate()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + GetToken();

            string template = "{\"touser\":\"o7pxvxMYg9Xu-4GAyBovu_kHZbT0\"," +
           "\"template_id\":\"n7HytrqruuthmO5Cs_Y6A31S_EQ_xTkBS67QmI2FuPI\"," +
           "\"data\":{" +
                "\"first\": {" +
                    "\"value\":\"您的认证资料已经提交，客服会1个工作日内审核资料，耐心等待哦！\"," +
                  " }," +
                   "\"keyword1\":{" +
                    "\"value\":\"17052254879\"," +
                   "}," +
                   "\"keyword2\": {" +
                    "\"value\":\"0539-8768321\"," +
                   "}," +
                   "\"remark\":{" +
                    "\"value\":\"欢迎您的支持！\"," +
                   "}" +
            "}" +
        "}";
            string str = GetResponseData(template, @url);
        }
        /// <summary>
        /// 微信提醒
        /// </summary>
        public void SendToUser(TelphoneOrderEntity entity)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + GetToken();

            string touser = ConfigurationManager.AppSettings["touser"];
            string message = "{\"touser\":\"" + touser + "\"," +
                "\"msgtype\":\"text\"," +
                "\"text\": " +
                "{\"content\":\"有客户提交了预约资料" +
                "\n预约号码：" + entity.Telphone +
                "\n客户名称：" + entity.Consignee +
                "\n联系电话：" + entity.Contact +
                "\n地址：" + entity.Pro + entity.City+entity.Area+entity.Address+
                " \"} }";
            string str = GetResponseData(message, @url);
        }

        /// <summary>
        /// 
        /// </summary>
        public class WXApi
        {
            public string access_token { set; get; }
        }


        /// <summary>
        /// 返回JSon数据
        /// </summary>
        /// <param name="JSONData">要处理的JSON数据</param>
        /// <param name="Url">要提交的URL</param>
        /// <returns>返回的JSON处理字符串</returns>
        public static string GetResponseData(string JSONData, string Url)
        {
            string strResult = "";
            if (JSONData != "")
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JSONData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentLength = bytes.Length;
                request.ContentType = "json";
                Stream reqstream = request.GetRequestStream();
                reqstream.Write(bytes, 0, bytes.Length);
                //声明一个HttpWebRequest请求
                request.Timeout = 90000;
                //设置连接超时时间
                request.Headers.Set("Pragma", "no-cache");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                Encoding encoding = Encoding.UTF8;
                StreamReader streamReader = new StreamReader(streamReceive, encoding);
                strResult = streamReader.ReadToEnd();
                streamReceive.Dispose();
                streamReader.Dispose();
            }
            return strResult;
        }

        /// <summary>
        /// 格式化输出日期
        /// </summary>
        /// <param name="pubDate"></param>
        /// <returns>XXX小时前，XXX天以前，XXX分钟以前</returns>
        public static string FormatDateTime(string pubDate)
        {
            //string sql = string.Format("select DATEDIFF(mi,'{0}',getdate()) 'min',DATEDIFF(hh,'{0}',getdate()) 'hour',DATEDIFF(dd,'{0}',getdate()) 'day'", pubDate);


            DateTime dt1 = DateTime.Parse(pubDate);//2005-11-5 5:21:25 .ToUniversalTime()
            DateTime dt2 = DateTime.Now;


            TimeSpan ts1 = new TimeSpan(dt1.Ticks);
            TimeSpan ts2 = new TimeSpan(dt2.Ticks);
            TimeSpan tsp = ts1.Subtract(ts2).Duration();


            //string dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
            //TimeSpan tsp = DateTime.Parse(DateTime.Now.ToString()) - DateTime.Parse(pubDate);
            //TimeSpan tsp = DateTime.Now.Subtract(DateTime.Parse(pubDate));


            int hr = tsp.Hours;
            int min = tsp.Minutes;
            int day = tsp.Days;
            string result = pubDate;
            if (day > 1)
            {
                return (day + "天之前");
            }


            if (hr > 1)
            {
                return (hr + "小时之前");
            }
            if (min > 1)
            {
                return (min + "分钟之前");
            }


            return result; ;
        }
    }
}
