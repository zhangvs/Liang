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
using Newtonsoft.Json.Linq;

namespace HZSoft.Application.Web.Areas.WeChatManage.Controllers
{
    /// <summary>
    /// 靓号菜单
    /// </summary>
    //[HandlerWXAuthorizeAttribute(LoginMode.Enforce)]
    public class LiangMenuController : Controller
    {
        private TelphoneLiangBLL tlbll = new TelphoneLiangBLL();
        private OrganizeBLL organizebll = new OrganizeBLL();

        /// <summary>
        /// 超级精品靓号
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu0(string organizeId, string city)
        {            
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    JObject queryJson = new JObject {
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city },
                        { "Grade", "0" }
                    };
                    ViewBag.list0 = tlbll.GetList(queryJson.ToString());
                    ViewBag.FullName = organize.FullName;
                    ViewBag.organizeId = organizeId;
                    ViewBag.city = city;
                    return View();
                }
                else
                {
                    return Content("机构暂时未生效或不存在！");
                }                
            }
            else
            {
                return Content("链接不正确不或完整！");
            }
        }
        /// <summary>
        /// 6A-5A
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu1(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    JObject queryJson = new JObject {
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city },
                        { "Grade", "1" }
                    };
                    var list6A = tlbll.GetList(queryJson.ToString());
                    ViewBag.list6A = list6A;
                    queryJson.Remove("Grade");
                    queryJson.Add("Grade", "2");
                    var list5A = tlbll.GetList(queryJson.ToString());
                    ViewBag.list5A = list5A;

                    ViewBag.FullName = organize.FullName;
                    ViewBag.organizeId = organizeId;
                    ViewBag.city = city;
                    return View();
                }
                else
                {
                    return Content("机构暂时未生效或不存在！");
                }
            }
            else
            {
                return Content("链接不正确不或完整！");
            }
        }
        /// <summary>
        /// 4A
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu2(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    JObject queryJson = new JObject {
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city },
                        { "Grade", "3" }
                    };
                    var list4A = tlbll.GetList(queryJson.ToString());
                    ViewBag.list4A = list4A;

                    ViewBag.FullName = organize.FullName;
                    ViewBag.organizeId = organizeId;
                    ViewBag.city = city;
                    return View();
                }
                else
                {
                    return Content("机构暂时未生效或不存在！");
                }
            }
            else
            {
                return Content("链接不正确不或完整！");
            }

        }
        /// <summary>
        /// 3A双豹子(0-5)
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu3(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    JObject queryJson = new JObject {
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city },
                        { "Grade", "4" }
                    };
                    var list3A3B_05 = tlbll.GetList(queryJson.ToString());
                    ViewBag.list3A3B_05 = list3A3B_05;


                    queryJson.Remove("Grade");
                    queryJson.Add("Grade", "11");
                    var list3A_05 = tlbll.GetList(queryJson.ToString());
                    ViewBag.list3A_05 = list3A_05;

                    ViewBag.FullName = organize.FullName;
                    ViewBag.organizeId = organizeId;
                    ViewBag.city = city;
                    return View();
                }
                else
                {
                    return Content("机构暂时未生效或不存在！");
                }
            }
            else
            {
                return Content("链接不正确不或完整！");
            }
            
        }

        /// <summary>
        /// 3A双豹子(6-9)
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu4(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    JObject queryJson = new JObject {
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city },
                        { "Grade", "20" }
                    };
                    var list3A3B_69 = tlbll.GetList(queryJson.ToString());
                    ViewBag.list3A3B_69 = list3A3B_69;


                    queryJson.Remove("Grade");
                    queryJson.Add("Grade", "12");
                    var list3A_69 = tlbll.GetList(queryJson.ToString());
                    ViewBag.list3A_69 = list3A_69;

                    ViewBag.FullName = organize.FullName;
                    ViewBag.organizeId = organizeId;
                    ViewBag.city = city;
                    return View();
                }
                else
                {
                    return Content("机构暂时未生效或不存在！");
                }
            }
            else
            {
                return Content("链接不正确不或完整！");
            }
        }
        /// <summary>
        /// 6顺-4顺
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu5(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {

                    JObject queryJson = new JObject {
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city },
                        { "Grade", "9" }
                    };

                    var list6shun = tlbll.GetList(queryJson.ToString());
                    ViewBag.list6shun = list6shun;


                    queryJson.Remove("Grade");
                    queryJson.Add("Grade", "5");
                    var list5shun = tlbll.GetList(queryJson.ToString());
                    ViewBag.list5shun = list5shun;


                    queryJson.Remove("Grade");
                    queryJson.Add("Grade", "6");
                    var list4shun = tlbll.GetList(queryJson.ToString());
                    ViewBag.list4shun = list4shun;

                    ViewBag.FullName = organize.FullName;
                    ViewBag.organizeId = organizeId;
                    ViewBag.city = city;
                    return View();
                }
                else
                {
                    return Content("机构暂时未生效或不存在！");
                }
            }
            else
            {
                return Content("链接不正确不或完整！");
            }
        }
        /// <summary>
        /// 四(五六)拖一
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu6(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    JObject queryJson = new JObject {
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city },
                        { "Grade", "18" }
                    };
                    var list61 = tlbll.GetList(queryJson.ToString());
                    ViewBag.list61 = list61;


                    queryJson.Remove("Grade");
                    queryJson.Add("Grade", "15");
                    var list51 = tlbll.GetList(queryJson.ToString());
                    ViewBag.list51 = list51;


                    queryJson.Remove("Grade");
                    queryJson.Add("Grade", "14");
                    var list41 = tlbll.GetList(queryJson.ToString());
                    ViewBag.list41 = list41;

                    ViewBag.FullName = organize.FullName;
                    ViewBag.organizeId = organizeId;
                    ViewBag.city = city;
                    return View();
                }
                else
                {
                    return Content("机构暂时未生效或不存在！");
                }
            }
            else
            {
                return Content("链接不正确不或完整！");
            }
        }
        /// <summary>
        /// 个性号
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu7(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    JObject queryJson = new JObject {
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city },
                        { "Grade", "17" }
                    };
                    var listABABAB = tlbll.GetList(queryJson.ToString());
                    ViewBag.listABABAB = listABABAB;


                    queryJson.Remove("Grade");
                    queryJson.Add("Grade", "13");
                    var listABCDABCD = tlbll.GetList(queryJson.ToString());
                    ViewBag.listABCDABCD = listABCDABCD;


                    queryJson.Remove("Grade");
                    queryJson.Add("Grade", "16");
                    var listABACAD = tlbll.GetList(queryJson.ToString());
                    ViewBag.listABACAD = listABACAD;

                    ViewBag.FullName = organize.FullName;
                    ViewBag.organizeId = organizeId;
                    ViewBag.city = city;
                    return View();
                }
                else
                {
                    return Content("机构暂时未生效或不存在！");
                }
            }
            else
            {
                return Content("链接不正确不或完整！");
            }
        }
        /// <summary>
        /// AABBCC(DD)
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu8(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    JObject queryJson = new JObject {
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city },
                        { "Grade", "19" }
                    };
                    var listAABBCCDD = tlbll.GetList(queryJson.ToString());
                    ViewBag.listAABBCCDD = listAABBCCDD;

                    ViewBag.FullName = organize.FullName;
                    ViewBag.organizeId = organizeId;
                    ViewBag.city = city;
                    return View();
                }
                else
                {
                    return Content("机构暂时未生效或不存在！");
                }
            }
            else
            {
                return Content("链接不正确不或完整！");
            }
        }

        /// <summary>
        /// 全国区号专区
        /// 
        /// 经典区号5(6)A
        /// 经典区号4A
        /// 经典区号3A
        /// 经典区号ABCD(EF)
        /// 经典区号四（五六）拖一
        /// 经典区号个性号
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu9(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    JObject queryJson = new JObject {
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city },
                        { "Grade", "901" }
                    };
                    for (int i = 901; i <= 917; i++)
                    {
                        queryJson.Remove("Grade");
                        queryJson.Add("Grade", i.ToString());
                        ViewData["list" + i] = tlbll.GetList(queryJson.ToString());
                    }

                    ViewBag.FullName = organize.FullName;
                    ViewBag.organizeId = organizeId;
                    ViewBag.city = city;
                    return View();
                }
                else
                {
                    return Content("机构暂时未生效或不存在！");
                }
            }
            else
            {
                return Content("链接不正确不或完整！");
            }
        }

        /// <summary>
        /// 打包专区
        /// 
        /// 打包108
        /// 打包118
        /// 打包4A
        /// 打包3A
        /// 打包ABCD
        /// 打包AABB(CCDD)
        /// 打包三（四五）拖一
        /// 打包ABC
        /// 打包ABAB(ABAB)
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu10(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    JObject queryJson = new JObject {
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city }
                    };
                    for (int i = 1001; i <= 1009; i++)
                    {
                        queryJson.Remove("Grade");
                        queryJson.Add("Grade", i.ToString());
                        ViewData["list" + i] = tlbll.GetList(queryJson.ToString());
                    }
                    ViewBag.FullName = organize.FullName;
                    ViewBag.organizeId = organizeId;
                    ViewBag.city = city;
                    return View();
                }
                else
                {
                    return Content("机构暂时未生效或不存在！");
                }
            }
            else
            {
                return Content("链接不正确不或完整！");
            }
        }
        /// <summary>
        /// ABC
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu11(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {

                    JObject queryJson = new JObject {
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city },
                        { "Grade", "7" }
                    };

                    var listABC = tlbll.GetList(queryJson.ToString());
                    ViewBag.listABC = listABC;

                    ViewBag.FullName = organize.FullName;
                    ViewBag.organizeId = organizeId;
                    ViewBag.city = city;
                    return View();
                }
                else
                {
                    return Content("机构暂时未生效或不存在！");
                }
            }
            else
            {
                return Content("链接不正确不或完整！");
            }
        }

        /// <summary>
        /// 其它靓号
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu12(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    JObject queryJson = new JObject {
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city }
                    };
                    for (int i = 1201; i <= 1211; i++)
                    {
                        queryJson.Remove("Grade");
                        queryJson.Add("Grade", i.ToString());
                        ViewData["list" + i] = tlbll.GetList(queryJson.ToString());
                    }
                    
                    ViewBag.FullName = organize.FullName;
                    ViewBag.organizeId = organizeId;
                    ViewBag.city = city;
                    return View();
                }
                else
                {
                    return Content("机构暂时未生效或不存在！");
                }
            }
            else
            {
                return Content("链接不正确不或完整！");
            }
        }



        /// <summary>
        /// 其它靓号
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu13(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    JObject queryJson = new JObject {
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city },
                        { "ExistMark",2}
                    };

                    var listMS = tlbll.GetList(queryJson.ToString());//秒杀状态
                    ViewBag.listMS = listMS;
                    queryJson.Remove("ExistMark");

                    for (int i = 1301; i <= 1311; i++)
                    {
                        queryJson.Remove("Grade");
                        queryJson.Add("Grade", i.ToString());
                        ViewData["list" + i] = tlbll.GetList(queryJson.ToString());
                    }
                    
                    ViewBag.FullName = organize.FullName;
                    ViewBag.organizeId = organizeId;
                    ViewBag.city = city;
                    return View();
                }
                else
                {
                    return Content("机构暂时未生效或不存在！");
                }
            }
            else
            {
                return Content("链接不正确不或完整！");
            }
        }
    }
}
