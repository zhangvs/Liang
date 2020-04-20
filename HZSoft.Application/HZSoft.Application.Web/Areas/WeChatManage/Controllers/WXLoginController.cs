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
    public class WXLoginController : BaseWxUserController
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
            //LogHelper.AddLog($"微信认证请求地址：{System.Web.HttpContext.Current.Request.Url.ToString()}  参数code： {code}，参数state： {state}");
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
                    string tokenUrl = string.Format(WeixinConfig.GetTokenUrl, code);
                    //LogHelper.AddLog($"请求tokenUrl地址： {tokenUrl}");
                    token = AnalyzeHelper.Get<WeixinToken>(tokenUrl);
                    if (token.errcode != null)
                    {
                        return Content("网页授权Error：" + token.errcode + "：" + token.errmsg);
                    }
                    Session[WebSiteConfig.WXTOKEN_SESSION_NAME] = token;
                }

                //WeixinTokenBase tokenBase = new WeixinTokenBase();
                //判断是否保存微信token基础，基础接口，2000次
                //if (Session[WebSiteConfig.WXTOKEN_SESSION_NAME_BASE] != null)
                //{
                //    tokenBase = Session[WebSiteConfig.WXTOKEN_SESSION_NAME_BASE] as WeixinTokenBase;
                //}
                //else
                //{
                //    string tokenBaseUrl = string.Format(WeixinConfig.GetTokenBaseUrl, code);
                //    tokenBase = AnalyzeHelper.Get<WeixinTokenBase>(tokenBaseUrl);
                //    if (tokenBase.errcode != null)
                //    {
                //        return Content(tokenBase.errcode + ":" + tokenBase.errmsg);
                //    }
                //    Session[WebSiteConfig.WXTOKEN_SESSION_NAME_BASE] = tokenBase;
                //}


                //查询用户是否存在
                var userEntity = wechatUserBll.GetEntity(token.openid);
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
                            AppName = Config.GetValue("AppName")
                        };
                        wechatUserBll.SaveForm("", userEntity);
                    }
                }
                Session[WebSiteConfig.WXUSER_SESSION_NAME] = userEntity;
                return Redirect(state);
            }
        }



        /// <summary>
        /// 微信登录：1.登录过得直接跳转到地图界面
        /// 2.没登录的新用户进入登录界面，使用账号密码登录，登录后会修改微信用户的userid，username字段，下次可直接进入主界面
        /// </summary>
        /// <returns></returns>
        [HandlerWXAuthorizeAttribute(LoginMode.Enforce)]
        public ActionResult Index(string urlstr)
        {
            //1.2根据注册的微信id去用户表中匹配是否有此员工
            WeChat_UsersEntity entity = wechatUserBll.GetEntity(CurrentWxUser.OpenId);
            //WeChat_UsersEntity entity = wechatUserBll.GetEntity("o7HEd1LjnupfP0BBBMz5f69MFYVE");
            if (!string.IsNullOrEmpty(entity.UserName) && !string.IsNullOrEmpty(entity.UserId))
            {
                UserEntity userEntity = new UserBLL().GetEntity(entity.UserId);

                LogEntity logEntity = new LogEntity();
                logEntity.CategoryId = 1;
                logEntity.OperateTypeId = ((int)OperationType.Login).ToString();
                logEntity.OperateType = EnumAttribute.GetDescription(OperationType.AppLogin);
                logEntity.OperateAccount = userEntity.RealName;
                logEntity.OperateUserId = userEntity.RealName;
                logEntity.Module = Config.GetValue("SoftName");
                //写入日志
                logEntity.ExecuteResult = 1;
                logEntity.ExecuteResultJson = "登录成功";
                logEntity.WriteLog();

                AuthorizeBLL authorizeBLL = new AuthorizeBLL();
                Operator operators = new Operator();
                operators.UserId = userEntity.UserId;
                operators.Code = userEntity.EnCode;
                operators.Account = userEntity.Account;
                operators.UserName = userEntity.RealName;
                operators.Password = userEntity.Password;
                operators.Secretkey = userEntity.Secretkey;
                operators.CompanyId = userEntity.OrganizeId;
                operators.DepartmentId = userEntity.DepartmentId;
                operators.IPAddress = Net.Ip;
                operators.IPAddressName = IPLocation.GetLocation(Net.Ip);
                operators.ObjectId = new PermissionBLL().GetObjectStr(userEntity.UserId);
                operators.LogTime = DateTime.Now;
                operators.Token = DESEncrypt.Encrypt(Guid.NewGuid().ToString());

                //写入当前用户数据权限
                AuthorizeDataModel dataAuthorize = new AuthorizeDataModel();
                dataAuthorize.ReadAutorize = authorizeBLL.GetDataAuthor(operators);
                dataAuthorize.ReadAutorizeUserId = authorizeBLL.GetDataAuthorUserId(operators);
                dataAuthorize.WriteAutorize = authorizeBLL.GetDataAuthor(operators, true);
                dataAuthorize.WriteAutorizeUserId = authorizeBLL.GetDataAuthorUserId(operators, true);
                operators.DataAuthorize = dataAuthorize;
                //判断是否系统管理员
                if (userEntity.Account == "System")
                {
                    operators.IsSystem = true;
                }
                else
                {
                    operators.IsSystem = false;
                }
                OperatorProvider.Provider.AddCurrent(operators);
                //return RedirectToAction("Index", "WeiXinHome");
                return RedirectToAction("Index", "WeiXinHome", new { urlstr = urlstr });
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public ActionResult CheckLogin(string username, string password)
        {
            UserEntity userEntity = new UserBLL().CheckLogin(username, password);
            if (userEntity != null)
            {
                //登录后会修改微信用户的userid，username字段，下次可直接进入主界面
                WeChat_UsersEntity entity = wechatUserBll.GetEntity(CurrentWxUser.OpenId);
                if (entity!=null)
                {
                    entity.UserId = userEntity.UserId;
                    entity.UserName = userEntity.RealName;
                    wechatUserBll.SaveForm(CurrentWxUser.OpenId, entity);
                }
                return Content("true");
            }
            else
            {
                return Content("false");
            }
        }



    }
}
