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
        public static string AppName2 { private set; get; }
        public static string RedirectUri2 { private set; get; }
        public static string GetCodeUrl2 { private set; get; }
        public static string GetTokenUrl2 { private set; get; }
        public static string GetTokenBaseUrl2 { private set; get; }
        public static string GetUserInfoUrl2 { private set; get; }
        public static string MchId { private set; get; }
        public static string Key { private set; get; }
        public static string TenPayV3Notify { private set; get; }



        //支付宝网关地址
        public static string serviceUrl { private set; get; }

        //应用ID
        public static string aliAppId { private set; get; }

        //开发者私钥，由开发者自己生成
        public static string privateKey { private set; get; }

        //支付宝的应用公钥
        public static string publicKey { private set; get; }

        //支付宝的支付公钥
        public static string payKey { private set; get; }

        //服务器异步通知页面路径
        public static string notify_url { private set; get; }

        //页面跳转同步通知页面路径
        public static string return_url { private set; get; }

        //参数返回格式，只支持json
        public static string format { private set; get; }

        // 调用的接口版本，固定为：1.0
        public static string version { private set; get; }

        // 商户生成签名字符串所使用的签名算法类型，目前支持RSA2和RSA，推荐使用RSA2
        public static string signType{ private set; get; }

        // 字符编码格式 目前支持utf-8
        public static string charset { private set; get; }

        // false 表示不从文件加载密钥
        public static bool keyFromFile { private set; get; }




        public static void Register()
        {
            AppID = Config.GetValue("AppID");
            AppSecret = Config.GetValue("AppSecret");
            AppName = Config.GetValue("AppName");
            RedirectUri = Config.GetValue("Domain") + "/WeChatManage/WXLogin/Redirect";
            GetCodeUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + AppID + "&redirect_uri=" + HttpUtility.UrlEncode(WeixinConfig.RedirectUri) + "&response_type=code&scope=snsapi_userinfo&state={0}#wechat_redirect";//HttpUtility.UrlEncode(WeixinConfig.ReturnUri)
            GetTokenUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + AppID + "&secret=" + AppSecret + "&code={0}&grant_type=authorization_code";
            GetTokenBaseUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppID + "&secret=" + AppSecret;
            GetUserInfoUrl = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";

            AppID2 = Config.GetValue("AppID2");
            AppSecret2 = Config.GetValue("AppSecret2");
            AppName2 = Config.GetValue("AppName2");
            RedirectUri2 = Config.GetValue("Domain2") + "/WeChatManage/WX2Login/Redirect";
            GetCodeUrl2 = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + AppID2 + "&redirect_uri=" + HttpUtility.UrlEncode(WeixinConfig.RedirectUri2) + "&response_type=code&scope=snsapi_userinfo&state={0}#wechat_redirect";
            GetTokenUrl2 = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + AppID2 + "&secret=" + AppSecret2 + "&code={0}&grant_type=authorization_code";
            GetTokenBaseUrl2 = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppID2 + "&secret=" + AppSecret2;
            GetUserInfoUrl2 = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";
            

            TenPayV3Notify = Config.GetValue("Domain2") + "/WeChatManage/WeiXinHome/Notify";
            MchId = Config.GetValue("MchId");
            Key = Config.GetValue("Key");

            serviceUrl = Config.GetValue("aliServiceUrl");
            aliAppId = Config.GetValue("aliAppId");
            privateKey = Config.GetValue("aliPrivateKey");
            publicKey = Config.GetValue("aliPublicKey");
            payKey = Config.GetValue("aliPayKey");
            notify_url = Config.GetValue("Domain2") + Config.GetValue("aliNotifyUrl");
            return_url = Config.GetValue("Domain2") + Config.GetValue("aliReturnUrl");
            format = Config.GetValue("aliFormat");
            version = Config.GetValue("aliVersion");
            signType = Config.GetValue("aliSignType");
            charset = Config.GetValue("aliCharset");
            keyFromFile = false;


        }
    }
}
