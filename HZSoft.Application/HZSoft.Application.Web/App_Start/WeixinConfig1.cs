using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Deepleo.Weixin.SDK.Helpers;
using HZSoft.Util;

namespace HZSoft.Application.Web
{
    public class WeixinConfig
    {
        public static string Token { private set; get; }
        public static string EncodingAESKey { private set; get; }
        public static string AppID { private set; get; }
        public static string AppSecret { private set; get; }
        public static string PartnerKey { private set; get; }
        public static string mch_id { private set; get; }
        public static string device_info { private set; get; }
        public static string spbill_create_ip { private set; get; }

        public static TokenHelper TokenHelper { private set; get; }

        public static void Register()
        {
            Token = Config.GetValue("Token");
            EncodingAESKey = Config.GetValue("EncodingAESKey");
            AppID = Config.GetValue("AppID"); ;
            AppSecret = Config.GetValue("AppSecret");
            PartnerKey = Config.GetValue("PartnerKey");
            mch_id = Config.GetValue("mch_id");
            device_info = Config.GetValue("device_info");
            spbill_create_ip = Config.GetValue("spbill_create_ip");
            var openJSSDK = int.Parse(Config.GetValue("OpenJSSDK")) > 0;
            TokenHelper = new TokenHelper(6000, AppID, AppSecret, openJSSDK);
            TokenHelper.Run();
        }
    }
}