using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Application.Busines.WeChatManage;
using HZSoft.Application.Code;
using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Application.Web.Utility;
using HZSoft.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HZSoft.Application.Web.Areas.WeChatManage.Controllers
{
    /// <summary>
    /// 个人中心页面
    /// </summary>
    [HandlerWXAuthorizeAttribute(LoginMode.Enforce)]
    public class UserCenterController : Controller
    {
        WeChat_UsersBLL wechatUserBll = new WeChat_UsersBLL();

        /// <summary>
        /// 个人中心
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            WeChat_UsersEntity entity = wechatUserBll.GetEntity("o7HEd1LjnupfP0BBBMz5f69MFYVE");//CurrentWxUser.OpenId
            return View(entity);
        }
    }
}
