using Aop.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HZSoft.Application.Web.Utility.AliPay
{
    public class AliPay
    {
        public static IAopClient GetAlipayClient()
        {
            string serviceUrl = WeixinConfig.serviceUrl;

            string appId = WeixinConfig.aliAppId;

            string privateKey = WeixinConfig.privateKey;

            string publivKey = WeixinConfig.publicKey;

            string format = WeixinConfig.format;

            string version = WeixinConfig.version;

            string signType = WeixinConfig.signType;

            string charset = WeixinConfig.charset;

            bool keyFromFile = WeixinConfig.keyFromFile;


            IAopClient client = new DefaultAopClient(serviceUrl, appId, privateKey, format, version, signType, publivKey, charset, keyFromFile);

            return client;
        }
    }
}
