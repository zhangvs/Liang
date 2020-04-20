using HZSoft.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace HZSoft.Application.Web
{
    public class WeixinConfig
    {
        public static string AppID { private set; get; }
        public static string AppSecret { private set; get; }
        public static string AppName { private set; get; }
        public static string RedirectUri { private set; get; }
        public static string GetCodeUrl { private set; get; }
        public static string GetTokenUrl { private set; get; }
        public static string GetTokenBaseUrl { private set; get; }
        public static string GetUserInfoUrl { private set; get; }


        public static string AppID2 { private set; get; }
        public static string AppSecret2 { private set; get; }
        public static string MchId { private set; get; }
        public static string Key { private set; get; }
        public static string TenPayV3Notify { private set; get; }
        


        public static void Register()
        {
            AppID = Config.GetValue("AppID");
            AppSecret = Config.GetValue("AppSecret");
            AppID2 = Config.GetValue("AppID2");
            AppSecret2 = Config.GetValue("AppSecret2");
            MchId = Config.GetValue("MchId");
            Key = Config.GetValue("Key");
            AppName = Config.GetValue("AppName");
            RedirectUri = Config.GetValue("Domain") + "/WeChatManage/WXLogin/Redirect";
            TenPayV3Notify = Config.GetValue("Domain2") + "/WeChatManage/WeiXinHome/Notify";
            GetCodeUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + AppID + "&redirect_uri=" + HttpUtility.UrlEncode(WeixinConfig.RedirectUri) + "&response_type=code&scope=snsapi_userinfo&state={0}#wechat_redirect";//HttpUtility.UrlEncode(WeixinConfig.ReturnUri)
            GetTokenUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + AppID + "&secret=" + AppSecret + "&code={0}&grant_type=authorization_code";
            GetTokenBaseUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppID + "&secret=" + AppSecret;
            GetUserInfoUrl = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";
        }
    }
}
