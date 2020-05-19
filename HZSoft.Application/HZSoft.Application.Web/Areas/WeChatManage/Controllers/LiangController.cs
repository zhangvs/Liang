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
    [HandlerWXAuthorizeAttribute(LoginMode.Enforce)]
    public class LiangController : BaseWxUserController
    {
        private UserBLL userBLL = new UserBLL();
        private AreaBLL areaBLL = new AreaBLL();
        private TelphoneLiangBLL tlbll = new TelphoneLiangBLL();
        private OrganizeBLL organizebll = new OrganizeBLL();
        private TelphoneLiangShareBLL telphoneliangsharebll = new TelphoneLiangShareBLL();
        private TelphoneLiangSeeBLL telphoneliangseebll = new TelphoneLiangSeeBLL();
        private TelphoneLiangJoinBLL joinbll = new TelphoneLiangJoinBLL();
        private TelphoneLiangVipBLL vipbll = new TelphoneLiangVipBLL();

        private OrganizeCache organizeCache = new OrganizeCache();

        #region 页面
        /// <summary>
        /// 模糊搜索等
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchHigh()
        {
            return View();
        }
        /// <summary>
        /// 选择城市
        /// </summary>
        /// <returns></returns>
        public ActionResult City()
        {
            return View();
        }
        /// <summary>
        /// 选择城市
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectCity()
        {
            return View();
        }
        /// <summary>
        /// 选择城市
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectCityHeBei()
        {
            return View();
        }
        /// <summary>
        /// 河北老葛
        /// </summary>
        /// <returns></returns>
        public ActionResult CityHeBei()
        {
            return View();
        }
        /// <summary>
        /// 济宁
        /// </summary>
        /// <returns></returns>
        public ActionResult CityJiNing()
        {
            return View();
        }
        /// <summary>
        /// 山西
        /// </summary>
        /// <returns></returns>
        public ActionResult CityShanXi()
        {
            return View();
        }

        /// <summary>
        /// 二维码
        /// </summary>
        /// <returns></returns>
        public ActionResult QrCode(string organizeId)
        {
            ViewBag.organizeId = organizeId;
            return View();
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(int? telphoneId)
        {
            TelphoneLiangEntity entity = tlbll.GetEntity(telphoneId);
            return View(entity);
        }
        

        /// <summary>
        /// 靓号主页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string organizeId, string city)
        {
            //添加靓号浏览实体
            //organizeId = "2287f3ae-e6b6-4a91-9b0c-66fc87658eaf";
            if (!string.IsNullOrEmpty(organizeId))
            {
                var organize = organizebll.GetEntity(organizeId);
                var viplist= vipbll.GetVipOrgList(organize.OrganizeId, organize.ParentId, organize.TopOrganizeId);
                if (viplist.Count==0)
                {
                    return Content("当前机构为非Vip机构或需要续费！");
                }
                if (organize != null)
                {
                    ViewBag.FullName = organize.FullName;
                    ViewBag.Tel = organize.OuterPhone;
                    ViewBag.Img1 = organize.Img1;
                    ViewBag.Img2 = organize.Img2;
                    ViewBag.Img3 = organize.Img3;
                    ViewBag.Img4 = organize.Img4;

                    ViewBag.city = "";
                    ViewBag.cityName = "城市";
                    if (!string.IsNullOrEmpty(city))
                    {
                        AreaEntity base_Area = areaBLL.GetEntity(city);
                        if (base_Area != null)
                        {
                            ViewBag.city = base_Area.AreaId;
                            ViewBag.cityName = base_Area.AreaName.Substring(0, 2) + "..";
                        }
                    }


                    ViewBag.organizeId = organizeId;
                    ViewBag.TopOrganizeId = organize.TopOrganizeId;
                    if (organize.VipMark != null)
                    {
                        ViewBag.VipMark = organize.VipMark;
                    }
                    else
                    {
                        ViewBag.VipMark = 0;
                    }

                    if (organize.TopOrganizeId == "86c7e97d-b6fd-4a6e-9b94-1e751307be45")
                    {
                        ViewBag.SelectCity = "CityHeBei";//河北老葛
                    }
                    else if (organize.TopOrganizeId == "68490e33-851d-4b1d-99b9-d604d7a8f39d")
                    {
                        ViewBag.SelectCity = "CityJiNing";//济宁
                    }
                    else if (organize.TopOrganizeId == "ee957bd5-af60-41c5-aa01-1103cf695c71")
                    {
                        ViewBag.SelectCity = "CityShanXi";//山西
                    }
                    else
                    {
                        ViewBag.SelectCity = "City";
                    }

                    //添加浏览主页记录
                    //如果不是自己查看，访客才添加查看记录CurrentWxUser.OpenId
                    if (organize.ManagerId != CurrentWxUser.OpenId)
                    {
                        TelphoneLiangSeeEntity seeEntity = new TelphoneLiangSeeEntity()
                        {
                            OrganizeId = organizeId,
                            IPAddress = Net.Ip,
                            IPAddressName = IPLocation.GetLocation(Net.Ip),
                            OpenId = CurrentWxUser.OpenId,
                            NickName = CurrentWxUser.NickName,
                            Sex = CurrentWxUser.Users.Sex,
                            HeadimgUrl = CurrentWxUser.Users.HeadimgUrl,
                            Province = CurrentWxUser.Users.Province,
                            City = CurrentWxUser.Users.City,//微信城市
                            Country = CurrentWxUser.Users.Country
                        };
                        telphoneliangseebll.SaveForm("", seeEntity);
                    }
                    else
                    {
                        //判断协议是否签写
                        if (organize.AgreementMark != 1)
                        {
                            return RedirectToAction("Agreement", new { organizeId = organizeId });
                        }
                    }

                    JObject queryJson = new JObject {
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city },
                        { "Grade", "0" },//精品推荐
                        { "Count", 10 }
                    };
                    ViewBag.list0 = tlbll.GetList(queryJson.ToString());

                    JObject queryJsonMS = new JObject {
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city },
                        { "ExistMark",2},//秒杀状态
                        { "Count", 10 }
                    };

                    ViewBag.listMS = tlbll.GetList(queryJsonMS.ToString());

                    return View();
                }
                else
                {
                    return Content("机构暂时未生效或不存在");
                }
            }
            else
            {
                return Content("链接不正确不或完整");
            }
        }
        /// <summary>
        /// 靓号主页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index2(string organizeId, string city)
        {
            //添加靓号浏览实体
            //organizeId = "2287f3ae-e6b6-4a91-9b0c-66fc87658eaf";
            if (!string.IsNullOrEmpty(organizeId))
            {
                var organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    ViewBag.FullName = organize.FullName;
                    ViewBag.Tel = organize.OuterPhone;
                    ViewBag.Img1 = organize.Img1;
                    ViewBag.Img2 = organize.Img2;
                    ViewBag.Img3 = organize.Img3;
                    ViewBag.Img4 = organize.Img4;
                    ViewBag.city = city;
                    ViewBag.organizeId = organizeId;
                    ViewBag.TopOrganizeId = organize.TopOrganizeId;
                    if (organize.VipMark != null)
                    {
                        ViewBag.VipMark = organize.VipMark;
                    }
                    else
                    {
                        ViewBag.VipMark = 0;
                    }

                    if (organize.TopOrganizeId == "86c7e97d-b6fd-4a6e-9b94-1e751307be45")
                    {
                        ViewBag.SelectCity = "SelectCityHeBei";//河北老葛
                    }
                    else
                    {
                        ViewBag.SelectCity = "SelectCity";
                    }

                    //添加浏览主页记录
                    //如果不是自己查看，访客才添加查看记录CurrentWxUser.OpenId
                    //UserEntity userEntity = userBLL.CheckLogin(CurrentWxUser.NickName);
                    //if (userEntity == null)

                    if (organize.ManagerId != CurrentWxUser.OpenId)
                    {
                        TelphoneLiangSeeEntity seeEntity = new TelphoneLiangSeeEntity()
                        {
                            OrganizeId = organizeId,
                            IPAddress = Net.Ip,
                            IPAddressName = IPLocation.GetLocation(Net.Ip),
                            OpenId = CurrentWxUser.OpenId,
                            NickName = CurrentWxUser.NickName,
                            Sex = CurrentWxUser.Users.Sex,
                            HeadimgUrl = CurrentWxUser.Users.HeadimgUrl,
                            Province = CurrentWxUser.Users.Province,
                            City = CurrentWxUser.Users.City,//微信城市
                            Country = CurrentWxUser.Users.Country
                        };
                        telphoneliangseebll.SaveForm("", seeEntity);
                    }
                    else
                    {
                        //判断协议是否签写
                        if (organize.AgreementMark != 1)
                        {
                            return RedirectToAction("Agreement", new { organizeId = organizeId });
                        }
                    }
                    return View();
                }
                else
                {
                    return Content("机构暂时未生效或不存在");
                }
            }
            else
            {
                return Content("链接不正确不或完整");
            }
        }

        /// <summary>
        /// 普号主页
        /// </summary>
        /// <returns></returns>
        public ActionResult Main(string organizeId, string city)
        {
            //添加靓号浏览实体
            //organizeId = "2287f3ae-e6b6-4a91-9b0c-66fc87658eaf";
            if (!string.IsNullOrEmpty(organizeId))
            {
                var organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    ViewBag.FullName = organize.FullName;
                    ViewBag.Tel = organize.OuterPhone;
                    ViewBag.Img1 = organize.Img1;
                    ViewBag.Img2 = organize.Img2;
                    ViewBag.Img3 = organize.Img3;
                    ViewBag.Img4 = organize.Img4;
                    ViewBag.city = city;
                    ViewBag.organizeId = organizeId;
                    ViewBag.InnerPhone = organize.InnerPhone;
                    ViewBag.Nature = organize.Nature;

                    //添加浏览主页记录
                    //如果不是自己查看，访客才添加查看记录CurrentWxUser.OpenId
                    //UserEntity userEntity = userBLL.CheckLogin(CurrentWxUser.NickName);
                    //if (userEntity == null)
                    if (organize.ManagerId != CurrentWxUser.OpenId)
                    {
                        TelphoneLiangSeeEntity seeEntity = new TelphoneLiangSeeEntity()
                        {
                            OrganizeId = organizeId,
                            IPAddress = Net.Ip,
                            IPAddressName = IPLocation.GetLocation(Net.Ip),
                            OpenId = CurrentWxUser.OpenId,
                            NickName = CurrentWxUser.NickName,
                            Sex = CurrentWxUser.Users.Sex,
                            HeadimgUrl = CurrentWxUser.Users.HeadimgUrl,
                            Province = CurrentWxUser.Users.Province,
                            City = CurrentWxUser.Users.City,//微信城市
                            Country = CurrentWxUser.Users.Country
                        };
                        telphoneliangseebll.SaveForm("", seeEntity);
                    }
                    else
                    {
                        //判断协议是否签写
                        if (organize.AgreementMark != 1)
                        {
                            return RedirectToAction("Agreement", new { organizeId = organizeId });
                        }
                    }
                    return View();
                }
                else
                {
                    return Content("机构暂时未生效或不存在");
                }
            }
            else
            {
                return Content("链接不正确不或完整");
            }

        }
        #endregion

        #region 搜索相关
        /// <summary>
        /// 模糊搜索等
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string organizeId, string city, string featuresname, string ico, string keyword)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                var organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    ViewBag.organizeId = organizeId;
                    ViewBag.Tel = organize.OuterPhone;


                    ViewBag.city = "";
                    ViewBag.cityName = "城市";
                    string cityName = "";
                    if (!string.IsNullOrEmpty(city))
                    {
                        AreaEntity base_Area = areaBLL.GetEntity(city);
                        if (base_Area != null)
                        {
                            ViewBag.city = base_Area.AreaId;
                            cityName = base_Area.AreaName;
                            ViewBag.cityName = cityName.Substring(0, 2) + "..";
                        }
                    }

                    switch (featuresname)
                    {
                        case "JP":
                            featuresname = "精品靓号";
                            break;
                        case "MS":
                            featuresname = "秒杀专区";
                            break;
                        case "6S-4S":
                            featuresname = "6顺-4顺";
                            break;
                        case "4(56)T1":
                            featuresname = "4(56)拖一";
                            break;
                        case "QH":
                            featuresname = "区号专区";
                            break;
                        case "DB":
                            featuresname = "打包专区";
                            break;
                        case "GX":
                            featuresname = "个性号";
                            break;
                        case "ELSE":
                            featuresname = "其它靓号";
                            break;
                        default:
                            break;
                    }
                    ViewBag.Title = organize.FullName + cityName + featuresname + keyword + "选号单";
                    if (!string.IsNullOrEmpty(ico))
                    {
                        ViewBag.Ico = "ico" + ico + ".png";
                    }
                    else
                    {
                        ViewBag.Ico = "liang.png";
                    }
                    return View();
                }
                else
                {
                    return Content("机构暂时未生效或不存在");
                }
            }
            else
            {
                return Content("链接不正确不或完整");
            }
        }
        /// <summary>
        /// 模糊搜索等 + '&price=' + price + '&except=' + except + '&yy=' + yuyi;
        /// </summary>
        /// <returns></returns>
        public ActionResult ListData(string keyword, string organizeId, string city, int page, string orderType, string price, string except, string Operator, string features, string ExistMark)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                var organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    JObject queryJson = new JObject { { "Telphone", keyword },
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city },
                        { "price", price },
                        { "except", except },
                        { "Operator", Operator },
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
                        sidx = "grade,right(Telphone,1)";//先按照类别排序，再按照最后一位排序
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

                    string styleStr = "";
                    foreach (var item in entityList)
                    {
                        string qian = item.Telphone.Substring(0, 3);
                        string zhong = item.Telphone.Substring(3, 4);
                        string hou = item.Telphone.Substring(7, 4);
                        string tt = "<font color='#E33F23'>" + qian + "</font><font color='#3A78F3'>" + zhong + "</font><font color='#E33F23'>" + hou + "</font>";
                        styleStr += @" 
                        <li onclick='f_search(" + item.TelphoneID + @")'>
                            <a >
                                <div class='fl'><p class='p1'>" + tt + @"</p><p>" + item.City + item.Operator + @"</p></div>
                                <div class='fr'><p><i>￥</i><b>" + Str.Num(item.Price) + @"</b></p><span>" + item.Description + @"</span></div>
                            </a>
                        </li>";
                    }
                    return Content(styleStr);
                }
                return Content("机构暂时未生效或不存在");
            }
            else
            {
                return Content("链接不正确不或完整");
            }
        }

        public ActionResult HaoDan(string keyword, string organizeId, string city, int page, string orderType, string price, string except, string Operator, string features, string ExistMark)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                var organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    JObject queryJson = new JObject { { "Telphone", keyword },
                        { "OrganizeIdH5", organizeId },
                        { "pid", organize.ParentId },
                        { "top", organize.TopOrganizeId },
                        { "city", city },
                        { "price", price },
                        { "except", except },
                        { "Operator", Operator },
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
                        sidx = "grade,right(Telphone,1)";//先按照类别排序，再按照最后一位排序
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

                    string styleStr = "";
                    foreach (var item in entityList)
                    {
                        styleStr += $"{item.Telphone}&nbsp;&nbsp;{item.City}{item.Operator}&nbsp;&nbsp;{item.Price}元<br/>";
                    }
                    return Content(styleStr+ "<br/><br/><br/><br/><br/>");
                }
                return Content("机构暂时未生效或不存在");
            }
            else
            {
                return Content("链接不正确不或完整");
            }
        }
        /// <summary>
        /// 自动匹配查询结果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TelAuto(string telphone, string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                var organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    var entity = tlbll.GetList(telphone, organizeId, city);
                    return Content(JsonConvert.SerializeObject(entity));
                }
                return Content("机构暂时未生效或不存在");
            }
            else
            {
                return Content("链接不正确不或完整");
            }

        }

        /// <summary>
        /// 查询页面弹窗，获取keyValue（点击下拉列表）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchTelId(int? telphoneId, string organizeId)
        {
            //是本机构的号码，是本机构的微信昵称
            int state = 3;
            string tel = "";
            TelphoneLiangEntity entity = tlbll.GetEntity(telphoneId);
            if (entity != null)
            {
                switch (entity.ExistMark)
                {
                    case 1:
                        entity.Description = "现卡";
                        break;
                    case 2:
                        entity.Description = "秒杀";
                        break;
                    default:
                        entity.Description = "预售";
                        break;
                }
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                //OrganizeEntity organize = organizebll.GetEntityByWxUser(organizeId, CurrentWxUser.NickName, CurrentWxUser.OpenId);
                tel = organize.OuterPhone;
                if (organize.ManagerId == CurrentWxUser.OpenId || organize.ShortName == CurrentWxUser.NickName)
                {
                    //类别0或1，并且本机构属于该号码的所属机构机1或本身0，非间接代售机构   ||加入平台的客户1级0级
                    //||(vipbll.IsShareMark(organize.ParentId) || vipbll.IsShareMark(organize.TopOrganizeId) || vipbll.IsShareMark(organize.OrganizeId))
                    //本机构只能下架本机构的号码
                    if (organize.Category < 2 && (entity.OrganizeId == organizeId || entity.OrganizeId == organize.ParentId))//0级一级可下架，可看价格
                    {
                        state = 0;//本机构，可看核算价，可编辑下架
                    }
                    else if (organize.Category == 0 && entity.OrganizeId != organizeId && entity.OrganizeId != organize.ParentId && (vipbll.IsShareMark(organize.ParentId) || vipbll.IsShareMark(organize.TopOrganizeId) || vipbll.IsShareMark(organize.OrganizeId)))
                    {
                        state = 1;//其它共享平台机构0级，可看核算价，不可编辑
                    }
                    else if (organize.Category > 2)
                    {
                        state = 3;//3级不能查看低价，普通客户
                    }
                    else
                    {
                        state = 2;//可看成本价，不可编辑下架
                    }
                }
            }
            var jsonData = new
            {
                entity = entity,
                state = state,
                tel = tel
            };
            return Content(JsonConvert.SerializeObject(jsonData));
        }

        /// <summary>
        /// 查询页面弹窗，获取keyValue（点击下拉列表）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchTel(string telphone, string organizeId)
        {
            //是本机构的号码，是本机构的微信昵称
            int state = 0;
            IEnumerable<TelphoneLiangEntity> entityList = tlbll.GetEntityByTel(organizeId, telphone);
            TelphoneLiangEntity entity = null;
            if (entityList.Count() != 0)
            {
                entity = entityList.First();

                OrganizeEntity organize = organizeCache.GetEntityByWxUser(organizeId, CurrentWxUser.NickName, CurrentWxUser.OpenId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    //类别0或1，并且本机构属于该号码的所属机构机1或本身0，非间接代售机构
                    if (organize.Category < 2 && (entity.OrganizeId == organizeId || entity.OrganizeId == organize.ParentId))//0级一级可下架，可看价格
                    {
                        state = 1;//可看低价可编辑下架
                    }
                    else if (organize.Category > 2)
                    {
                        state = 0;
                    }
                    else
                    {
                        state = 2;//可看低价不可编辑下架
                    }
                }
            }
            var jsonData = new
            {
                entity = entity,
                state = state,
            };
            return Content(JsonConvert.SerializeObject(jsonData));
        }

        /// <summary>
        /// 查询页面弹窗，获取keyValue（点击查询按钮）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchOrgTel(string telphone, string organizeId)
        {
            //是本机构的号码，是本机构的微信昵称
            int state = 0;
            IEnumerable<TelphoneLiangEntity> entityList = tlbll.GetEntityByOrgTel(organizeId, telphone);
            TelphoneLiangEntity entity = null;
            if (entityList.Count() != 0)
            {
                entity = entityList.First();

                OrganizeEntity organize = organizeCache.GetEntityByWxUser(organizeId, CurrentWxUser.NickName, CurrentWxUser.OpenId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    //类别0或1，并且本机构属于该号码的所属机构机1或本身0，非间接代售机构
                    if (organize.Category < 2 && (entity.OrganizeId == organizeId || entity.OrganizeId == organize.ParentId))//0级一级可下架，可看价格
                    {
                        state = 1;
                    }
                    else if (organize.Category > 2)
                    {
                        state = 0;
                    }
                    else
                    {
                        state = 2;
                    }
                }
            }
            var jsonData = new
            {
                entity = entity,
                state = state,
            };
            return Content(JsonConvert.SerializeObject(jsonData));
        }
        #endregion

        #region 下架编辑
        /// <summary>
        /// 号码内容页详情
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFormJson(int? keyValue)
        {
            TelphoneLiangEntity entity = tlbll.GetEntity(keyValue);
            return Content(JsonConvert.SerializeObject(entity));
        }

        /// <summary>
        /// 加载售出人列表
        /// </summary>
        /// <returns></returns>
        //[WeiXinLoginAuthorize]
        public ActionResult SearchForm(string organizeId)
        {
            //ViewBag.userList = userBLL.GetUsersByOrganizeId(organizeId);
            ViewBag.organizeId = organizeId;
            return View();
        }

        /// <summary>
        /// 保存靓号，修改状态售出
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="currOrg"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult SaveForm(int? keyValue, string currOrg, TelphoneLiangEntity entity)
        {
            OrganizeEntity organize = organizeCache.GetEntityByWxUser(currOrg, CurrentWxUser.NickName, CurrentWxUser.OpenId);
            if (!string.IsNullOrEmpty(organize.OrganizeId))
            {
                //类别0或1，并且本机构属于该号码的所属机构机1或本身0，非间接代售机构
                if (organize.Category < 2 && (entity.OrganizeId == currOrg || entity.OrganizeId == organize.ParentId))//0级一级可下架，可看价格
                {
                    entity.ModifyUserName = CurrentWxUser.NickName;
                    tlbll.SaveForm(keyValue, entity);
                    return Content(new JsonMessage { Success = true, Code = "1", Message = "保存成功！" }.ToString());
                }
            }
            return Content(new JsonMessage { Success = false, Code = "-1", Message = "你无权保存！" }.ToString());
        }
        #endregion

        #region 分享代码
        /// <summary>
        /// 分享
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Img"></param>
        /// <returns></returns>
        public PartialViewResult _PartialShare(string title, string Img)
        {
            //获得微信基础类
            BaseWxModel baseModel = GetWxModel();
            ShareModel fxModel = new ShareModel()
            {
                appid = baseModel.appid,
                nonce = baseModel.nonce,
                signature = baseModel.signature,
                thisUrl = baseModel.thisUrl,
                timestamp = baseModel.timestamp,
                fxTitle = title,
                fxContent = Config.GetValue("fxContent"),
                fxImg = Config.GetValue("Domain") + Url.Content("/Content/images/liang/" + Img),
            };
            return PartialView(fxModel);
        }
        /// <summary>
        /// 添加分享记录
        /// </summary>
        /// <param name="shareUrl"></param>
        /// <param name="shareTitle"></param>
        /// <param name="shareContent"></param>
        /// <param name="shareTo"></param>
        /// <param name="organizeId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddShare(string shareUrl, string shareTitle, string shareContent, string shareTo, string organizeId)
        {
            try
            {
                if (shareUrl.IndexOf("&amp;") > 0)
                {
                    //WeChatManage/Liang/Index?organizeId=4da3b884-ac5b-46fb-af10-150beb9e6c69&amp;amp;amp;amp;amp;from=timeline&amp;amp;amp;amp;from=timeline&amp;amp;amp;from=timeline&amp;amp;from=timeline&amp;from=timeline
                    //防止提示state参数过长问题,连接转发多次之后会带着一些转发到哪里的冗余信息
                    shareUrl = shareUrl.Substring(0, shareUrl.IndexOf("&amp;"));//WeChatManage/Liang/Index?organizeId=4da3b884-ac5b-46fb-af10-150beb9e6c69
                }

                TelphoneLiangShareEntity entity = new TelphoneLiangShareEntity()
                {
                    CreateDate = DateTime.Now,
                    //ShareContent = shareContent,
                    ShareTitle = shareTitle,
                    ShareTo = shareTo,
                    ShareUrl = shareUrl,
                    OpenId = CurrentWxUser.OpenId,
                    NickName = CurrentWxUser.NickName,
                    HeadimgUrl = CurrentWxUser.HeadimgUrl,
                    OrganizeId = organizeId
                };
                telphoneliangsharebll.SaveForm("", entity);
                return Json(new { iserror = false, message = "分享成功" });
            }
            catch (Exception ex)
            {
                return Json(new { iserror = true, message = ex.Message });
            }
        }
        #endregion

        #region 加盟申请代码
        /// <summary>
        /// 申请表单
        /// </summary>
        /// <returns></returns>
        public ActionResult Join(string organizeId)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                var organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    ViewBag.organizeId = organizeId;
                    return View();
                }
                return Content("机构暂时未生效或不存在");
            }
            else
            {
                return Content("链接不正确不或完整");
            }
        }
        /// <summary>
        /// 申请表单
        /// </summary>
        /// <returns></returns>
        public ActionResult SellerRegister(string organizeId)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                var organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    ViewBag.organizeId = organizeId;
                    return View();
                }
                return Content("机构暂时未生效或不存在");
            }
            else
            {
                return Content("链接不正确不或完整");
            }
        }

        /// <summary>
        /// 提交代理加盟
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveSellerRegister(TelphoneLiangJoinEntity entity)
        {
            var organize = organizeCache.GetEntityByTel(entity.Telphone);
            if (!string.IsNullOrEmpty(organize.OrganizeId))
            {
                return Content("该手机号申请的机构已经存在，请换个手机号重新申请！");
            }

            if (joinbll.NotExistTelphone(entity.Telphone))//号码个数为0true，并且机构列表中不存在此手机号
            {
                entity.OpenId = CurrentWxUser.OpenId;
                entity.NickName = CurrentWxUser.NickName;
                entity.Sex = CurrentWxUser.Users.Sex;
                entity.WxCity = CurrentWxUser.Users.City;
                entity.WXPro = CurrentWxUser.Users.Province;
                entity.HeadimgUrl = CurrentWxUser.HeadimgUrl;

                //插入加盟表
                joinbll.SaveForm("", entity);
                //微信提醒
                //WechatHelper.SendJoin(entity.CompanyName, entity.FullName, entity.Telphone);
                //订单提醒
                //WechatHelper.SendToTemplate(entity.OpenId);
                return Content("提交成功");
            }
            else
            {
                return Content("该手机号提交的申请正在等待审核，请不要重复提交！");
            }
        }
        #endregion

        #region 协议
        /// <summary>
        /// 协议页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Agreement(string organizeId)
        {
            ViewBag.organizeId = organizeId;
            return View();
        }

        /// <summary>
        ///  同意协议操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AgreementSaveForm(int mark, string organizeId)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                var organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    organize.AgreementMark = mark;
                    organizebll.UpdateAgreementState(organizeId, mark);
                    return Redirect("Index?organizeId=" + organizeId);
                    //return RedirectToAction("Index", new { organizeId = organizeId });//跳转的url会缺省Index
                }
                return Content("机构暂时未生效或不存在");
            }
            else
            {
                return Content("链接不正确不或完整");
            }
        }
        #endregion



        #region 未用页面
        /// <summary>
        /// 获取位置
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMap()
        {
            return View();
        }

        /// <summary>
        /// 查询页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Search(string organizeId)
        {
            ViewBag.organizeId = organizeId;

            string Tel = Config.GetValue("LiangTel");
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                Tel = organize.OuterPhone;
            }
            ViewBag.Tel = Tel;
            return View();
        }

        /// <summary>
        /// 联系方式页面
        /// </summary>
        /// <returns></returns>
        public ActionResult contact()
        {
            string organizeId = Request.QueryString["organizeId"];
            ViewBag.organizeId = organizeId;

            string OuterPhone = "";
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                OuterPhone = organize.OuterPhone;
            }
            if (string.IsNullOrEmpty(OuterPhone))
            {
                OuterPhone = Config.GetValue("LiangTel");
            }

            ViewBag.OuterPhone = OuterPhone;
            return View();
        }
        #endregion

    }
}
