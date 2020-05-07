using HZSoft.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HZSoft.Application.Web.Utility.AliPay
{
    public class AliPayConfig
    {
        //支付宝网关地址
        public static string serviceUrl = Config.GetValue("aliServiceUrl");

        //应用ID
        public static string appId = Config.GetValue("aliAppId");

        //开发者私钥，由开发者自己生成
        public static string privateKey = Config.GetValue("aliPrivateKey");

        //支付宝的应用公钥
        public static string publicKey = Config.GetValue("aliPublicKey");

        //支付宝的支付公钥
        public static string payKey = Config.GetValue("aliPayKey");

        //服务器异步通知页面路径
        public static string notify_url = Config.GetValue("aliNotifyUrl");

        //页面跳转同步通知页面路径
        public static string return_url = Config.GetValue("aliReturnUrl");

        //参数返回格式，只支持json
        public static string format = Config.GetValue("aliFormat");

        // 调用的接口版本，固定为：1.0
        public static string version = Config.GetValue("aliVersion");

        // 商户生成签名字符串所使用的签名算法类型，目前支持RSA2和RSA，推荐使用RSA2
        public static string signType = Config.GetValue("aliSignType");

        // 字符编码格式 目前支持utf-8
        public static string charset = Config.GetValue("aliCharset");

        // false 表示不从文件加载密钥
        public static bool keyFromFile = false;

        // 日志记录
        public static string LogPath = Config.GetValue("AliLog");
    }
}
//前面讨论了微信支付，接下来聊聊支付宝的APP支付（新款支付宝支付）。其实这些支付原理都一样，只不过具体到每个支付平台，所使用的支付配置参数不同，返回至支付端的下单参数也不同。

//话不多说，直接上代码。

//在App.Pay项目中使用NuGet管理器添加引用Alipay.AopSdk，也可以不添加引用，将官方SDK源码放至项目中。



//添加完引用后，我们就可以开工了，新建文件夹AliPay，在文件夹中新建AliPayConfig类，存放支付宝APP支付所需的参数，同样，这些参数我也放在了配置文件中。

// View Code
// 支付宝支付中有个沙箱测试环境，我们可以先在沙箱环境下调通整个流程（沙箱支付宝里面的钱是虚拟的哦）。介绍一下这几个支付参数。

//　　①aliServiceUrl支付宝网关地址，固定不变的，沙箱环境下用沙箱的，正式环境下用正式的。

//　　②aliAppId支付宝APPID，aliPrivateKey支付宝应用私钥，aliPublicKey支付宝应用公钥，aliPayKey支付宝公钥

//aliPublicKey和aliPayKey是不一样的，一个是应用公钥，一个是支付宝公钥，回调接口中验签使用的是支付宝公钥

//　　③aliNotifyUrl服务器通知，aliReturnUrl网页重定向通知（暂时没有用到）。主要使用到的还是aliNotifyUrl，买家付完款后（trade_status=WAIT_SELLER_SEND_GOODS），支付宝服务端会自动向商户后台发送支付回调通知，同样，商户在支付回调通知中修改订单相关状态，反馈给支付宝success，表示成功接收到回调，这个状态下支付宝不会再继续通知商户后台。

//　　④aliFormat、aliVersion、aliSignType、aliCharset这几个参数都是固定不变的，签名的时候使用。