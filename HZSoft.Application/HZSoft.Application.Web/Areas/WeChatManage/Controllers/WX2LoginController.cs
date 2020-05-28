using HZSoft.Application.Busines.BaseManage;
using HZSoft.Application.Busines.WeChatManage;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Application.Web.Utility;
using HZSoft.Util;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Net;
using HZSoft.Application.Entity.SystemManage;
using HZSoft.Application.Code;
using HZSoft.Application.Busines.AuthorizeManage;
using HZSoft.Application.Busines.SystemManage;
using HZSoft.Util.Attributes;
using Newtonsoft.Json;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Busines.CustomerManage;

namespace HZSoft.Application.Web.Areas.WeChatManage.Controllers
{
    /// <summary>
    /// 微信认证和登录
    /// </summary>
    public class WX2LoginController : Controller
    {
        WeChat_UsersBLL wechatUserBll = new WeChat_UsersBLL();
        UserBLL userBLL = new UserBLL();

        /// <summary>
        /// 微信认证
        /// https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx24e47efa56c2e554&redirect_uri=http%3a%2f%2fmap.lywenkai.cn%2fWeChatManage%2fWeiXinHome%2fRedirect&response_type=code&scope=snsapi_userinfo&state=http%3a%2f%2fmap.lywenkai.cn%2fWeChatManage%2fLogin%2fIndex#wechat_redirect
        /// </summary>
        /// <param name="code">snsapi_userinfo</param>
        /// <param name="state">回调url</param>
        /// <returns></returns>
        public ActionResult Redirect(string code, string state)
        {
            LogHelper.AddLog($"微信认证请求地址：{System.Web.HttpContext.Current.Request.Url.ToString()}  参数code： {code}，参数state： {state}");
            //若用户禁止授权，则重定向后不会带上code参数
            if (string.IsNullOrEmpty(code))
            {
                return Redirect(state);
            }
            else
            {
                WeixinToken token = new WeixinToken();
                //判断是否保存微信token，用户网页授权,不限制
                if (Session[WebSiteConfig.WXTOKEN_SESSION_NAME] != null)
                {
                    token = Session[WebSiteConfig.WXTOKEN_SESSION_NAME] as WeixinToken;
                }
                else
                {
                    string tokenUrl = string.Format(WeixinConfig.GetTokenUrl2, code);
                    LogHelper.AddLog($"请求tokenUrl地址： {tokenUrl}");
                    token = AnalyzeHelper.Get<WeixinToken>(tokenUrl);
                    if (token.errcode != null)
                    {
                        return Content("网页授权Error：" + token.errcode + "：" + token.errmsg);
                    }
                    Session[WebSiteConfig.WXTOKEN_SESSION_NAME] = token;
                }
                Session["OpenId"] = token.openid;//进行登录
                LogHelper.AddLog($"token.openid： {Session["OpenId"]}");
                //查询用户是否存在
                var userEntity = wechatUserBll.GetEntity(token.openid);
                if (userEntity == null)
                {
                    string userInfoUrl = string.Format(WeixinConfig.GetUserInfoUrl2, token.access_token, token.openid);
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
                            AppName = Config.GetValue("AppName2")
                        };
                        wechatUserBll.SaveForm("", userEntity);
                    }
                }
                Session[WebSiteConfig.WXUSER_SESSION_NAME] = userEntity;
                return Redirect(state);
            }
        }
        
    }
}
