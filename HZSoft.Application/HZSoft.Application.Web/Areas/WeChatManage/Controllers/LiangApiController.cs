using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Application.Entity.CustomerManage;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Xml;
using Deepleo.Weixin.SDK.Helpers;
using Deepleo.Weixin.SDK.JSSDK;
using System.Text.RegularExpressions;
using HZSoft.Application.Web;
using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Application.Busines.BaseManage;
using HZSoft.Util;
using HZSoft.Application.Web.Utility;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Cache;
using HZSoft.Application.Code;
using HZSoft.Util.WebControl;
using Newtonsoft.Json.Linq;
using HZSoft.Application.Busines.SystemManage;
using HZSoft.Application.Entity.SystemManage;

namespace HZSoft.Application.Web.Areas.WeChatManage.Controllers
{
    /// <summary>
    /// 靓号
    /// </summary>
    public class LiangApiController : BaseWxUserController
    {
        private TelphoneLiangBLL tlbll = new TelphoneLiangBLL();
        private OrganizeBLL organizebll = new OrganizeBLL();


        /// <summary>
        /// 对外api号码列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="organizeId">机构</param>
        /// <param name="city">城市</param>
        /// <param name="page">页码</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="price">价格区间</param>
        /// <param name="except">except</param>
        /// <param name="yuyi"></param>
        /// <param name="features"></param>
        /// <param name="ExistMark">状态</param>
        /// <returns></returns>
        public ActionResult ListData(string keyword, string organizeId, string city, int page, string orderType, string price, string except, string yuyi, string features, string ExistMark)
        {
            string url = Request.Url.ToString();
            if (!string.IsNullOrEmpty(organizeId))
            {
                var organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    JObject queryJson = new JObject {
                        { "Telphone", keyword },
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city },
                        { "price", price },
                        { "except", except },
                        { "yuyi", yuyi },
                        { "Grade",features },
                        { "SellMark",0 },
                        { "ExistMark",ExistMark }
                    };
                    string sidx = "";
                    string sord = "";
                    if (orderType == "1")
                    {
                        sidx = "price";
                        sord = "asc";
                    }
                    else if (orderType == "2")
                    {
                        sidx = "price";
                        sord = "desc";
                    }
                    else
                    {
                        sidx = "right(Telphone,1)";//按照最后一位排序
                        sord = "asc";
                    }
                    Pagination pagination = new Pagination()
                    {
                        rows = 40,
                        page = page,
                        sidx = sidx,
                        sord = sord
                    };
                    var entityList = tlbll.GetPageListH5(pagination, queryJson.ToString());

                    LogHelper.AddLog("LiangApi调用:"+ url);//记录日志
                    return Content(JsonConvert.SerializeObject(entityList));
                }
                return Content("机构暂时未生效或不存在");
            }
            else
            {
                return Content("链接不正确不或完整");
            }
        }

    }
}
