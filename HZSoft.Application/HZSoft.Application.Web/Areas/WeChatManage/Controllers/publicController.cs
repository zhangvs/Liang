using HZSoft.Application.Entity.WeChatManage;
using Newtonsoft.Json;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HZSoft.Application.Web.Areas.WeChatManage.Controllers
{
    public class publicController : Controller
    {
        //
        // GET: /WeChatManage/public/

        public ActionResult getWxShare()
        {
            H5Response root = new H5Response { code = true, status = true, msg = "成功!", data = new BaseWxModel2() };

            root.data.appId = WeixinConfig.AppID2;
            root.data.timestamp = JSSDKHelper.GetTimestamp();
            root.data.nonceStr = JSSDKHelper.GetNoncestr();

            root.data.url = Request.Url.ToString();//MyCommFun.getTotalUrl();

            string ticket = JsApiTicketContainer.TryGetJsApiTicket(WeixinConfig.AppID2, WeixinConfig.AppSecret2);

            JSSDKHelper jsHelper = new JSSDKHelper();
            //最后一个参数url，必须为当前的网址
            var signature = JSSDKHelper.GetSignature(ticket, root.data.nonceStr, root.data.timestamp, root.data.url);
            root.data.signature = signature;
            return Content(JsonConvert.SerializeObject(root));
        }

    }
}
