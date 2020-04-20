using HZSoft.Application.Entity.WeChatManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HZSoft.Application.Web.Utility
{
    /// <summary>
    /// 当前买家
    /// </summary>
    public class CurrentWxUser
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public static string NickName
        {
            get
            {
                var current = HttpContext.Current.Session[WebSiteConfig.WXUSER_SESSION_NAME];
                if (current != null)
                {
                    WeChat_UsersEntity mem = current as WeChat_UsersEntity;
                    if (mem != null)
                        return mem.NickName;
                }
                return "";
            }
        }

        /// <summary>
        /// 公共ID
        /// </summary>
        public static string OpenId
        {
            get
            {
                var current = HttpContext.Current.Session[WebSiteConfig.WXUSER_SESSION_NAME];
                if (current != null)
                {
                    WeChat_UsersEntity mem = current as WeChat_UsersEntity;
                    if (mem != null)
                        return mem.OpenId;
                }
                return "";
            }
        }

        /// <summary>
        /// 头像
        /// </summary>
        public static string HeadimgUrl
        {
            get
            {
                var current = HttpContext.Current.Session[WebSiteConfig.WXUSER_SESSION_NAME];
                if (current != null)
                {
                    WeChat_UsersEntity mem = current as WeChat_UsersEntity;
                    if (mem != null)
                        return mem.HeadimgUrl;
                }
                return "";
            }
        }

        /// <summary>
        /// 买家
        /// </summary>
        public static WeChat_UsersEntity Users
        {
            get
            {
                var current = HttpContext.Current.Session[WebSiteConfig.WXUSER_SESSION_NAME];
                if (current != null)
                {
                    WeChat_UsersEntity mem = current as WeChat_UsersEntity;
                    if (mem != null)
                        return mem;
                }
                return null;
            }
        }
    }
}