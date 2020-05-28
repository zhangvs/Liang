using System.Web.Mvc;
using HZSoft.Application.Code;
using HZSoft.Util;
using System.Web;
using HZSoft.Application.Web.Utility;

namespace HZSoft.Application.Web
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创建人：佘赐雄
    /// 日 期：2015.11.9 10:45
    /// 描 述：登录认证（会话验证组件）
    /// </summary>
    public class HandlerWX2AuthorizeAttribute : AuthorizeAttribute
    {
        private LoginMode _customMode;
        /// <summary>默认构造</summary>
        /// <param name="Mode">认证模式</param>
        public HandlerWX2AuthorizeAttribute(LoginMode Mode)
        {
            _customMode = Mode;
        }

        /// <summary>
        /// 响应前执行登录验证,查看当前用户是否有效 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //登录拦截是否忽略
            if (_customMode == LoginMode.Ignore)
            {
                return;
            }
            //请求地址
            HttpRequest request = HttpContext.Current.Request;
            string RequestUri = request.RawUrl;//AbsoluteUri//FilePath//!string.IsNullOrEmpty(request.Params["urlstr"]) ? request.Params["urlstr"] : 
            if (RequestUri.IndexOf("&amp;")>0)
            {
                //WeChatManage/Liang/Index?organizeId=4da3b884-ac5b-46fb-af10-150beb9e6c69&amp;amp;amp;amp;amp;from=timeline&amp;amp;amp;amp;from=timeline&amp;amp;amp;from=timeline&amp;amp;from=timeline&amp;from=timeline
                //防止提示state参数过长问题,连接转发多次之后会带着一些转发到哪里的冗余信息
                RequestUri = RequestUri.Substring(0, RequestUri.IndexOf("&amp;"));//WeChatManage/Liang/Index?organizeId=4da3b884-ac5b-46fb-af10-150beb9e6c69
            }
            //判断是否微信通过认证
            if (CurrentWxUser.Users == null)
            {
                string url = string.Format(WeixinConfig.GetCodeUrl2, HttpUtility.UrlEncode(RequestUri));
                filterContext.Result = new RedirectResult(url);
                LogHelper.AddLog("filterContext.Result:" + url);
                return;
            }
        }



    }
}