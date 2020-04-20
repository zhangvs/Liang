using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HZSoft.Application.Web
{
    /// <summary>
    /// 微信登录权限验证
    /// </summary>
    public class WeiXinLoginAuthorizeAttribute : AuthorizeAttribute
    {
        private string WeiXinSettingId = "D4169D8F-F85C-485F-8DDE-90248100ADE4";
        /// <summary>
        /// 响应前执行验证,查看当前用户是否有效 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var areaName = filterContext.RouteData.DataTokens["area"];
            var controllerName = filterContext.RouteData.Values["controller"];
            var action = filterContext.RouteData.Values["Action"];

            //验证是否存在会员 或 session是否存在会员信息登录
            //if (!ManageProvider.Provider.IsOverdue())
            //{
            //    filterContext.Result = new RedirectResult("~/Login/Default");
            //}
            WeiXinLogIn(filterContext, areaName, controllerName, action);
        }

        private static void WeiXinLogIn(AuthorizationContext filterContext, object areaName, object controllerName, object action)
        {
            var url = "~/" + areaName + "/" + controllerName + "/" + action;
            var param = filterContext.ActionDescriptor.GetParameters();
            if (param.Length > 0)
            {
                url += "?";
            }
            for (int i = 0; i < param.Length; i++)
            {
                var value = filterContext.Controller.ValueProvider.GetValue(param[i].ParameterName);
                url += param[i].ParameterName + "=" + (value == null ? "" : value.AttemptedValue) + "&";
            }
            url = url.TrimEnd('&');
            var member = HttpContext.Current.Session["WeiXinLoginMember"] as Entity.UserInfoModel;
            if (null != member)
            {
                //filterContext.Result = new RedirectResult(url);
            }
            else
            {
                filterContext.Result = new RedirectResult("~/WeChatManage/WeiXinHome/GetUserInfo?url=" + HttpUtility.UrlEncode(url, System.Text.Encoding.UTF8));
            }
        }
    }

}