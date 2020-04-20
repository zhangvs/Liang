using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HZSoft.Util
{
    public class WechatHelper
    {
        private static string WxToken;   
        

        public static string GetToken()
        {

            //通知指定的微信客服
            #region 获取access_token
            string appid = ConfigurationManager.AppSettings["AppID"];
            string secret = ConfigurationManager.AppSettings["AppSecret"];
            string apiurl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret;

            string shortUrl = HttpClientHelper.Get(apiurl);


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


        #region 微信提醒
        public static void SendToOpenOK(string touser, string businessName,string des)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + GetToken();
            string template = "{\"touser\":\"" + touser + "\"," +
           "\"template_id\":\"P6XBY2wO9P1_YUOhIl8Zk-vdbheth0dyHZbuwBafTBM\"," +
           "\"url\":\"" + des + "\"," +
           "\"data\":{" +
                "\"first\": {" +
                    "\"value\":\"您好，您提交的靓号店铺申请已经通过。\"" +
                  " }," +
                   "\"keyword1\":{" +
                    "\"value\":\"" + businessName + "\"" +
                   "}," +
                   "\"keyword2\": {" +
                    "\"value\":\"" + DateTime.Now.ToString() + "\"" +
                   "}," +
                   "\"remark\":{" +
                    "\"value\":\"现在就开始分享您的靓号店铺到朋友圈吧！\"" +
                   "}" +
            "}" +
        "}";
            string str = GetResponseData(template, @url);
        }


        public static void SendToManager(string ordercode,string good,string price,string des)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + GetToken();
            //string touser = "ouEs61pPppa9xQr_0_5j_hVp1Nfw";// CurrentWxUser.OpenId;
            string touser = ConfigurationManager.AppSettings["touser"];
            string template = "{\"touser\":\"" + touser + "\"," +
           "\"template_id\":\"eiyX_yPknLLWUatUhDhW8jMbyZA62yLWFwWGdvsJdck\"," +
           "\"data\":{" +
                "\"first\": {" +
                    "\"value\":\"有人提交了靓号订单！\"" +
                  " }," +
                   "\"keyword1\":{" +
                    "\"value\":\""+ ordercode + "\"" +
                   "}," +
                   "\"keyword2\": {" +
                    "\"value\":\""+ good + "\"" +
                   "}," +
                   "\"keyword3\": {" +
                    "\"value\":\"" + price + "\"" +
                   "}," +
                   "\"keyword4\": {" +
                    "\"value\":\"" + des + "\"" +
                   "}," +
                   "\"remark\":{" +
                    "\"value\":\"感谢您的使用！\"" +
                   "}" +
            "}" +
        "}";
            string str = GetResponseData(template, @url);
        }

        public static void SendToTemplate(string touser)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + GetToken();
            //string touser = "ouEs61pPppa9xQr_0_5j_hVp1Nfw";// CurrentWxUser.OpenId;
            string template = "{\"touser\":\"" + touser + "\"," +
           "\"template_id\":\"n7HytrqruuthmO5Cs_Y6A31S_EQ_xTkBS67QmI2FuPI\"," +
           "\"data\":{" +
                "\"first\": {" +
                    "\"value\":\"您的认证资料已经提交，客服会1个工作日内审核，耐心等待哦！\"" +
                  " }," +
                   "\"keyword1\":{" +
                    "\"value\":\"17052254879\"" +
                   "}," +
                   "\"keyword2\": {" +
                    "\"value\":\"0539-8768321\"" +
                   "}," +
                   "\"remark\":{" +
                    "\"value\":\"感谢您的使用！\"" +
                   "}" +
            "}" +
        "}";
            string str = GetResponseData(template, @url);
        }

        public static void SendToOK(string touser)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + GetToken();
            //string touser = "ouEs61pPppa9xQr_0_5j_hVp1Nfw";// CurrentWxUser.OpenId;
            string template = "{\"touser\":\"" + touser + "\"," +
           "\"template_id\":\"CxDjjBalx3aw4vB4n5BKeyQGgCt1qiMHeFJH0WWoaKw\"," +
           "\"data\":{" +
                "\"first\": {" +
                    "\"value\":\"您好，您提交的实名审核已经完成！\"" +
                  " }," +
                   "\"keyword1\":{" +
                    "\"value\":\"实名认证审核\"" +
                   "}," +
                   "\"keyword2\": {" +
                    "\"value\":\"审核通过！\"" +
                   "}," +
                   "\"keyword3\": {" +
                    "\"value\":\"" + DateTime.Now.ToString() + "\"" +
                   "}," +
                   "\"remark\":{" +
                    "\"value\":\"感谢您的使用！\"" +
                   "}" +
            "}" +
        "}";
            string str = GetResponseData(template, @url);
        }

        public static void SendToFail(string touser, string failText)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + GetToken();
            //string touser = "ouEs61pPppa9xQr_0_5j_hVp1Nfw";// CurrentWxUser.OpenId;
            string template = "{\"touser\":\"" + touser + "\"," +
           "\"template_id\":\"iu5Y-dR0F_VMoppphwnqg8XSuP8iiRh0r18vWUHMpNM\"," +
           "\"data\":{" +
                "\"first\": {" +
                    "\"value\":\"您好，您提交的实名审核已经完成！\"" +
                  " }," +
                   "\"keyword1\":{" +
                    "\"value\":\"未通过\"" +
                   "}," +
                   "\"keyword2\": {" +
                    "\"value\":\"" + failText + "\"" +
                   "}," +
                   "\"keyword3\": {" +
                    "\"value\":\"" + DateTime.Now.ToString() + "\"" +
                   "}," +
                   "\"remark\":{" +
                    "\"value\":\"感谢您的使用！\"" +
                   "}" +
            "}" +
        "}";
            string str = GetResponseData(template, @url);
        }

        /// <summary>
        /// 实名认证
        /// </summary>
        public static void SendWX(string mobileNumber, string custName, string custCertCode, string custCertAddress)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + GetToken();
            string touser = ConfigurationManager.AppSettings["touser"];
            string message = "{\"touser\":\"" + touser + "\"," +
                "\"msgtype\":\"text\"," +
                "\"text\": " +
                "{\"content\":\"有客户提交了实名认证资料" +
                "\n手机号：" + mobileNumber +
                "\n客户名称：" + custName +
                "\n身份证：" + custCertCode +
                "\n地址：" + custCertAddress +
                " \"} }";
            string str = GetResponseData(message, @url);
        }

        /// <summary>
        /// 微信提醒
        /// </summary>
        public static void SendJoin(string companyName, string fullName, string telphone)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + GetToken();
            string touser = ConfigurationManager.AppSettings["touser"];
            string message = "{\"touser\":\"" + touser + "\"," +
                "\"msgtype\":\"text\"," +
                "\"text\": " +
                "{\"content\":\"有人提交了靓号加盟申请资料" +
                "\n公司名称：" + companyName +
                "\n客户名称：" + fullName +
                "\n手机号：" + telphone +
                " \"} }";
            string str = GetResponseData(message, @url);
        }


        /// <summary>
        /// 微信提醒
        /// </summary>
        public static void SendOrder(string Telphone, string SellerName, string Amount)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + GetToken();
            string touser = ConfigurationManager.AppSettings["touser"];
            string message = "{\"touser\":\"" + touser + "\"," +
                "\"msgtype\":\"text\"," +
                "\"text\": " +
                "{\"content\":\"有人提交了靓号订单" +
                "\n靓号：" + Telphone +
                "\n提交人：" + SellerName +
                "\n价格：" + Amount +
                " \"} }";
            string str = GetResponseData(message, @url);
        }
        #endregion

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
    }
}
