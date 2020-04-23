using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Application.Web.Utility;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HZSoft.Application.Web.Utility;
using HZSoft.Application.Entity.CustomerManage;
using Senparc.Weixin.MP.Containers;

namespace HZSoft.Application.Web.Areas.WeChatManage.Controllers
{
    public class BaseWxUserController : Controller
    {
        #region [Private Method]
        /// <summary>
        /// 微信授权登录
        /// </summary>
        protected string WeixinAuth(string returnUrl)
        {
            //判断是否登录
            if (CurrentWxUser.Users == null)
            {
                //string returnUrl = Url.Action("Index", new { id = id });
                string url = string.Format(WeixinConfig.GetCodeUrl, returnUrl);
                return url;
            }
            return "";
        }
        #endregion

        //获得微信js sdk config
        protected BaseWxModel GetWxModel()
        {
            BaseWxModel model = new BaseWxModel();
            
            model.appid = WeixinConfig.AppID;
            model.timestamp = JSSDKHelper.GetTimestamp();
            model.nonce = JSSDKHelper.GetNoncestr();

            model.thisUrl = Request.Url.ToString();//MyCommFun.getTotalUrl();

            string ticket = JsApiTicketContainer.TryGetJsApiTicket(WeixinConfig.AppID, WeixinConfig.AppSecret);

            JSSDKHelper jsHelper = new JSSDKHelper();
            //最后一个参数url，必须为当前的网址
            var signature = JSSDKHelper.GetSignature(ticket, model.nonce, model.timestamp, model.thisUrl);
            model.signature = signature;
            return model;
        }

        //获得微信js sdk config
        protected BaseWxModel GetWxModel2()
        {
            BaseWxModel model = new BaseWxModel();

            model.appid = WeixinConfig.AppID;
            model.timestamp = JSSDKHelper.GetTimestamp();
            model.nonce = JSSDKHelper.GetNoncestr();

            model.thisUrl = Request.Url.ToString();//MyCommFun.getTotalUrl();

            string ticket = JsApiTicketContainer.TryGetJsApiTicket(WeixinConfig.AppID, WeixinConfig.AppSecret);

            JSSDKHelper jsHelper = new JSSDKHelper();
            //最后一个参数url，必须为当前的网址
            var signature = JSSDKHelper.GetSignature(ticket, model.nonce, model.timestamp, model.thisUrl);
            model.signature = signature;
            return model;
        }

        //判断验证码是否正确
        protected void ValidateSmsCode(SmsInfoEntity model, string smsCode)
        {
            if (model == null)
                throw new Exception("验证码不正确");
            if (smsCode != model.Captcha)
                throw new Exception("验证码不正确");

            //判断是否过期
            if (model.CreateDate.AddMonths(WebSiteConfig.SMS_EXPIRE_MIN) < DateTime.Now)
                throw new Exception("验证码已过期，请重新发送");
        }

    }
}
