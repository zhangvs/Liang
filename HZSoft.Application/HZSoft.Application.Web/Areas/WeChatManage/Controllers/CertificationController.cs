using HZSoft.Application.Busines.BaseManage;
using HZSoft.Application.Busines.WeChatManage;
using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Web.Utility;
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
    public class CertificationController : BaseWxUserController
    {
        TelphoneCertificationBLL telphonecertificationbll = new TelphoneCertificationBLL();

        #region 视图功能
        /// <summary>
        /// 实名认证页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            //1.授权登录
            string returnUrl = Request.Url.ToString();
            string url = WeixinAuth(returnUrl);
            if (!string.IsNullOrEmpty(url))
                return Redirect(url);
            return View();
        }

        #endregion

        /// <summary>
        /// 提交实名认证
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Audit(TelphoneCertificationEntity entity)
        {
            entity.createId=CurrentWxUser.OpenId;
            entity.createName = CurrentWxUser.NickName;
            //插入实名认证表
            string responseText=telphonecertificationbll.SaveForm("", entity);
            if (responseText.IndexOf("OK")>=0)
            {
                //微信提醒
                WechatHelper.SendWX(entity.mobileNumber, entity.custName, entity.custCertCode, entity.custCertAddress);
                //订单提醒
                WechatHelper.SendToTemplate(entity.createId);
            }

            return Content(responseText);
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
                        //水印
                        string imgShui = dir + newfileName + "_Water" + fileExt;
                        string imgWaterPath = Request.MapPath(imgShui);

                        //ImgWater.zzsTextWater(imgYaPath, "仅用于开卡使用", imgWaterPath, 0.3f, 0.9f, 150);
                        using (FileStream fromFile = new FileStream(imgFilePath, FileMode.Open))
                        {
                            ImageHelper.ZoomAuto(fromFile, imgWaterPath, 1130, 1130, "仅用于开卡使用", "", true);
                        }
                        //删除原图节省空间
                        System.IO.File.Delete(imgFilePath);

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

        #region 文字识别
        // 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
        private static string clientId = "MrwmwTA0ySdsuSUHcLqYg9Ti";
        // 百度云中开通对应服务应用的 Secret Key
        private static string clientSecret = "EeDVlPwBeOPxeGnrYyGrVhdNpbI7SUOp";

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
        /// <summary>
        /// 文字识别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IDFront(string smrz_front)
        {
            string idcardStr = Idcard(Request.MapPath(smrz_front));

            var client = new Baidu.Aip.Ocr.Ocr(clientId, clientSecret);
            var image = System.IO.File.ReadAllBytes(Request.MapPath(smrz_front));

            // 身份证正面识别
            Dictionary<string, object> myDictionary = new Dictionary<string, object>();
            myDictionary.Add("detect_direction", "true");//是否检测图像朝向
            myDictionary.Add("accuracy", "high");//精准度，精度越高，速度越慢
            var result = client.IdCardFront(image, myDictionary);

            return Content(result.ToString());

        }

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
        #endregion


        /// <summary>
        /// 添加水印
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload()
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
                        string dir = string.Format("/Resource/DocumentFile/WxQRcode/{0}/", uploadDate);
                        if (Directory.Exists(Server.MapPath(dir)) == false)//如果不存在就创建file文件夹
                        {
                            Directory.CreateDirectory(Server.MapPath(dir));
                        }
                        string newfileName = Guid.NewGuid().ToString();
                        //原图
                        string fullDir1 = dir + newfileName + fileExt;
                        string imgFilePath = Request.MapPath(fullDir1);
                        file.SaveAs(imgFilePath);

                        return Content(new JsonMessage { Success = true, Code = "0", Message = fullDir1 }.ToString());

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
    }
}
