using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Application.Busines.WeChatManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Application.Web.Utility;
using HZSoft.Util;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HZSoft.Application.Web.Areas.WeChatManage.Controllers
{
    public class WeiXinHomeController : Controller
    {
        WeChat_UsersBLL userBll = new WeChat_UsersBLL();
        private OrdersBLL ordersbll = new OrdersBLL();
        private TelphoneLiangBLL tlbll = new TelphoneLiangBLL();

        //跳转
        public ActionResult Redirect(string code, string state)
        {
            //若用户禁止授权，则重定向后不会带上code参数
            if (string.IsNullOrEmpty(code))
            {
                return Redirect(state);
            }
            else
            {
                WeixinToken token = new WeixinToken();

                //判断是否保存微信token
                if (Session[WebSiteConfig.WXTOKEN_SESSION_NAME] != null)
                {
                    token = Session[WebSiteConfig.WXTOKEN_SESSION_NAME] as WeixinToken;
                }
                else
                {
                    string tokenUrl = string.Format(WeixinConfig.GetTokenUrl, code);
                    token = AnalyzeHelper.Get<WeixinToken>(tokenUrl);
                    if (token.errcode != null)
                    {
                        return Content(token.errcode + ":" + token.errmsg);
                    }
                    Session[WebSiteConfig.WXTOKEN_SESSION_NAME] = token;
                }
                //查询用户是否存在
                var userEntity = userBll.GetEntity(token.openid);
                if (userEntity == null)
                {
                    string userInfoUrl = string.Format(WeixinConfig.GetUserInfoUrl, token.access_token, token.openid);
                    var userInfo = AnalyzeHelper.Get<WeixinUserInfo>(userInfoUrl);
                    if (userInfo.errcode != null)
                    {
                        Response.Write(userInfo.errcode + ":" + userInfo.errmsg);
                        Response.End();
                    }
                    else
                    {
                        userEntity = new WeChat_UsersEntity()
                        {
                            City = userInfo.city,
                            Country = userInfo.country,
                            HeadimgUrl = userInfo.headimgurl,
                            NickName = userInfo.nickname,
                            OpenId = userInfo.openid,
                            Province = userInfo.province,
                            Sex = userInfo.sex,
                            AppName=WeixinConfig.AppName
                        };
                        userBll.SaveForm("", userEntity);
                    }
                }
                Session[WebSiteConfig.WXUSER_SESSION_NAME] = userEntity;
                return Redirect(state);
            }
        }



        //微信支付回调地址
        public ActionResult Notify()
        {
            LogHelper.AddLog("支付回调地址");//记录日志
            ResponseHandler rspHandler = new ResponseHandler(null);
            rspHandler.SetKey(WeixinConfig.Key);
            LogHelper.AddLog(rspHandler.ParseXML());//记录日志


            //SUCCESS/FAIL此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断 SUCCESS
            string return_code = rspHandler.GetParameter("return_code");
            string return_msg = rspHandler.GetParameter("return_msg");
            if (rspHandler.IsTenpaySign())
            {
                if (return_code == "SUCCESS")
                {
                    //订单号
                    string orderSn = rspHandler.GetParameter("out_trade_no");
                    OrdersEntity order = ordersbll.GetEntityByOrderSn(orderSn);
                    
                    order.PayDate = DateTime.Now;
                    order.PayStatus = (int)PayStatus.已支付;
                    order.Status = (int)OrderStatus.未发货;
                    ordersbll.SaveForm(order.Id, order);

                    TelphoneLiangEntity tel = tlbll.GetEntityByOrgTel(order.Tel);
                    if (tel != null)
                    {
                        tel.SellMark = 1;
                        tel.SellerName = "头条出售";
                    }
                    tlbll.SaveForm(tel.TelphoneID,tel);
                }
            }

            string xml = string.Format(@"<xml>
<return_code><![CDATA[{0}]]></return_code>
<return_msg><![CDATA[{1}]]></return_msg>
</xml>", return_code, return_msg);
            return Content(xml, "text/xml");

        }
    }
}
