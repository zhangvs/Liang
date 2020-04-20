using HZSoft.Application.Busines.BaseManage;
using HZSoft.Application.Busines.WeChatManage;
using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
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
    /// <summary>
    /// 实名认证
    /// </summary>
    public class Certification0Controller : Controller
    {

        #region 视图功能

        /// <summary>
        /// 实名认证页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 添加水印
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddWater()
        {
            string Message = "";
            if (Request.Files.Count > 0)
            {
                HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];
                if (file.ContentLength > 0)
                {
                    string fileExt = file.FileName.Substring(file.FileName.LastIndexOf('.'));//后缀
                    try
                    {
                        string fileGuid = Guid.NewGuid().ToString();
                        string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                        string dir = string.Format("/Resource/DocumentFile/Certification/{0}/", uploadDate);
                        if (Directory.Exists(Server.MapPath(dir)) == false)//如果不存在就创建file文件夹
                        {
                            Directory.CreateDirectory(Server.MapPath(dir));
                        }
                        //Directory.CreateDirectory(Path.GetDirectoryName(dir));
                        string newfileName = Guid.NewGuid().ToString();
                        //原图
                        string fullDir1 = dir + newfileName + fileExt;
                        string imgFilePath = Request.MapPath(fullDir1);
                        file.SaveAs(imgFilePath);
                        //裁剪
                        string imgCai = dir + newfileName + "_Cai" + fileExt;
                        string imgCaiPath = Request.MapPath(imgCai);
                        ImgWater.PercentImage(imgFilePath, imgCaiPath);
                        //ImgWater.MakeThumbnail(imgFilePath, imgCaiPath, 1130, 1130, "W");
                        //压缩
                        string imgYa = dir + newfileName + "_YaSuo" + fileExt;
                        string imgYaPath = Request.MapPath(imgYa);
                        ImgWater.ReduceImage(imgFilePath, imgYaPath, 50);
                        //水印
                        string imgShui = dir + newfileName + "_Water" + fileExt;
                        string imgWaterPath = Request.MapPath(imgShui);

                        ImgWater.zzsTextWater(imgYaPath, "仅用于开卡使用", imgWaterPath, 0.3f, 0.9f, 150);

                        return Content(new JsonMessage { Success = true, Code = "0", Message = imgShui }.ToString());

                    }
                    catch (Exception ex)
                    {
                        Message = HttpUtility.HtmlEncode(ex.Message);
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = Message }.ToString());
                    }
                }
                Message = "请选择要上传的文件！";
                return Content(new JsonMessage { Success = false, Code = "-1", Message = Message }.ToString());
            }
            Message = "请选择要上传的文件！";
            return Content(new JsonMessage { Success = false, Code = "-1", Message = Message }.ToString());
        }
        /// <summary>
        /// 文字识别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Distinguish(string smrz_zheng)
        {
            string idcardStr = Idcard(Request.MapPath(smrz_zheng));
            return Content(idcardStr);
            
        }

        // 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
        private static string clientId = "MrwmwTA0ySdsuSUHcLqYg9Ti";
        // 百度云中开通对应服务应用的 Secret Key
        private static string clientSecret = "EeDVlPwBeOPxeGnrYyGrVhdNpbI7SUOp";
        /// <summary>
        /// 百度证件识别接口
        /// </summary>
        /// <param name="imgPath"></param>
        /// <returns></returns>
        public static string Idcard(string imgPath)
        {
            var client = new Baidu.Aip.Ocr.Ocr(clientId, clientSecret);
            var image = System.IO.File.ReadAllBytes(imgPath);

            // 身份证正面识别
            Dictionary<string, object> myDictionary = new Dictionary<string, object>();
            myDictionary.Add("detect_direction", "true");//是否检测图像朝向
            myDictionary.Add("accuracy", "high");//精准度，精度越高，速度越慢
            var result = client.IdCardFront(image, myDictionary);
            // 身份证背面识别
            //result = client.IdCardBack(image);
            return result.ToString();
        }

        
        /// <summary>
        /// 提交实名认证
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Audit(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            TelphoneCertificationEntity entity = new TelphoneCertificationEntity();
            TelphoneCertificationBLL telphonecertificationbll = new TelphoneCertificationBLL();

            entity.mobileNumber = queryParam["mobileNumber"].ToString();
            entity.custName = queryParam["custName"].ToString();
            entity.custCertCode = queryParam["custCertCode"].ToString();
            entity.custCertAddress = queryParam["custCertAddress"].ToString();
            entity.photo_z = queryParam["photo_z"].ToString();
            entity.photo_b = queryParam["photo_b"].ToString();
            entity.photo_s = queryParam["photo_s"].ToString();
            entity.loadMark = 0;
            entity.createTime = DateTime.Now;
            //插入实名认证表
            telphonecertificationbll.SaveForm("", entity);
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

            //string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + token;
            
            //string touser = ConfigurationManager.AppSettings["touser"];
            //string message = "{\"touser\":\"" + touser + "\"," +
            //    "\"msgtype\":\"text\"," +
            //    "\"text\": " +
            //    "{\"content\":\"有客户提交了实名认证资料" +
            //    "\n手机号：" + entity.mobileNumber +
            //    "\n客户名称：" + entity.custName +
            //    "\n身份证：" + entity.custCertCode +
            //    "\n地址：" + entity.custCertAddress +
            //    " \"} }";
            //string str = GetResponseData(message, @url);
            return Content("true");

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
        /// 
        /// </summary>
        public class WXApi
        {
            /// <summary>
            /// 
            /// </summary>
            public string access_token { set; get; }
        }
        
        #endregion

    }
}
