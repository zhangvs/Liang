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

namespace HZSoft.Application.Web.Areas.WeChatManage.Controllers
{
    /// <summary>
    /// 靓号菜单
    /// </summary>
    [HandlerWXAuthorizeAttribute(LoginMode.Enforce)]
    public class LiangMenu2Controller : BaseWxUserController
    {
        private UserBLL userBLL = new UserBLL();
        private TelphoneLiangBLL tlbll = new TelphoneLiangBLL();
        private TelphonePuBLL publl = new TelphonePuBLL();
        private OrganizeBLL organizebll = new OrganizeBLL();
        private TelphoneReserveSearchBLL reserverSearchbll = new TelphoneReserveSearchBLL();

        private OrganizeCache organizeCache = new OrganizeCache();

        /// <summary>
        /// 普号主页
        /// </summary>
        /// <returns></returns>
        public ActionResult Main(string organizeId, string city)
        {
            //添加靓号浏览实体
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

                    TelphoneLiangSeeEntity seeEntity = new TelphoneLiangSeeEntity()
                    {
                        OrganizeId = organizeId,
                        IPAddress = Net.Ip,
                        IPAddressName = IPLocation.GetLocation(Net.Ip),
                    };
                    telphoneliangseebll.SaveForm("", seeEntity);
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
                    ViewBag.list0 = tlbll.GetGrade(organizeId, "0", city, null);
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
        /// 6A-5A-4A
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
                    var list6A = tlbll.GetGrade(organizeId, "1", city, null);
                    ViewBag.list6A = list6A;
                    var list5A = tlbll.GetGrade(organizeId, "2", city, null);
                    ViewBag.list5A = list5A;
                    var list4A = tlbll.GetGrade(organizeId, "3", city, null);
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
        /// 3A
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
                    var list3A3B_05 = tlbll.GetGrade(organizeId, "4", city, null);
                    ViewBag.list3A3B_05 = list3A3B_05;
                    var list3A_05 = tlbll.GetGrade(organizeId, "11", city, null);
                    ViewBag.list3A_05 = list3A_05;

                    var list3A3B_69 = tlbll.GetGrade(organizeId, "20", city, null);
                    ViewBag.list3A3B_69 = list3A3B_69;
                    var list3A_69 = tlbll.GetGrade(organizeId, "12", city, null);
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
        /// 个性号
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
                    var listABABAB = tlbll.GetGrade(organizeId, "17", city, null);
                    ViewBag.listABABAB = listABABAB;
                    var listABCDABCD = tlbll.GetGrade(organizeId, "13", city, null);
                    ViewBag.listABCDABCD = listABCDABCD;
                    var listABACAD = tlbll.GetGrade(organizeId, "16", city, null);
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
        /// （6顺-4顺）
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
                    var list6shun = tlbll.GetGrade(organizeId, "9", city, null);
                    ViewBag.list6shun = list6shun;
                    var list5shun = tlbll.GetGrade(organizeId, "5", city, null);
                    ViewBag.list5shun = list5shun;
                    var list4shun = tlbll.GetGrade(organizeId, "6", city, null);
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
        /// （ABC）
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
                    var listABC = publl.GetGrade(organizeId, "303", city);
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
        /// 纪念（寓意）号
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu6(string organizeId, string city)
        {
            return View();
        }

        private TelphoneLiangSeeBLL telphoneliangseebll = new TelphoneLiangSeeBLL();
        /// <summary>
        /// 1314专区
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu7(string organizeId, string city)
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

        #region 搜索相关
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
                    var entity = publl.GetList(telphone, organizeId, city);
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
        /// 查询页面弹窗，获取keyValue
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchTel(string telphone, string organizeId)
        {
            //添加查询记录表
            TelphoneReserveSearchEntity searchEntity = new TelphoneReserveSearchEntity()
            {
                IPAddress = Net.Ip,
                IPAddressName = IPLocation.GetLocation(Net.Ip),
                SearchNumber = telphone
            };
            searchEntity.Create();
            reserverSearchbll.SaveForm("", searchEntity);


            //是本机构的号码，是本机构的微信昵称
            int state = 0;
            IEnumerable<TelphonePuEntity> entityList = publl.GetEntityByOrgTel(organizeId, telphone);
            TelphonePuEntity entity = null;
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

        /// <summary>
        /// 查询页面弹窗，获取keyValue
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchTel1314(string telphone, string organizeId)
        {
            //添加查询记录表
            TelphoneReserveSearchEntity searchEntity = new TelphoneReserveSearchEntity()
            {
                IPAddress = Net.Ip,
                IPAddressName = IPLocation.GetLocation(Net.Ip),
                SearchNumber = telphone
            };
            searchEntity.Create();
            reserverSearchbll.SaveForm("", searchEntity);


            //是本机构的号码，是本机构的微信昵称
            IEnumerable<TelphonePuEntity> entityList = publl.GetEntityByOrgTel(organizeId, telphone);
            TelphonePuEntity entity = null;
            if (entityList.Count() != 0)
            {
                entity = entityList.First();
            }
            var jsonData = new
            {
                entity = entity,
                state = 2,//1314广告投放页面状态为2，只看价格
            };
            return Content(JsonConvert.SerializeObject(jsonData));
        }
        #endregion

        /// <summary>
        /// 寓意号、爱情号
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu71(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    var listPu101 = publl.GetGrade(organizeId, "101", city);
                    ViewBag.listPu101 = listPu101;
                    var listPu102 = publl.GetGrade(organizeId, "102", city);
                    ViewBag.listPu102 = listPu102;
                    var listPu103 = publl.GetGrade(organizeId, "103", city);
                    ViewBag.listPu103 = listPu103;
                    var listPu104 = publl.GetGrade(organizeId, "104", city);
                    ViewBag.listPu104 = listPu104;
                    var listPu105 = publl.GetGrade(organizeId, "105", city);
                    ViewBag.listPu105 = listPu105;
                    var listPu106 = publl.GetGrade(organizeId, "106", city);
                    ViewBag.listPu106 = listPu106;

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
        /// 生日号（纪念号）、年份号
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu72(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {

                    var listPu201 = publl.GetGrade(organizeId, "201", city);
                    ViewBag.listPu201 = listPu201;
                    var listPu202 = publl.GetGrade(organizeId, "202", city);
                    ViewBag.listPu202 = listPu202;
                    var listPu203 = publl.GetGrade(organizeId, "203", city);
                    ViewBag.listPu203 = listPu203;

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
        /// 成双成对爱情小靓号
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu73(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    var listPu302 = publl.GetGrade(organizeId, "302", city);
                    ViewBag.listPu302 = listPu302;

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
        /// 成双成对爱情小靓号
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu74(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                OrganizeEntity organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    var listPu301 = publl.GetGrade(organizeId, "301", city);
                    ViewBag.listPu301 = listPu301;

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
        /// 0539专区
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu8(string organizeId, string city)
        {
            return View();
        }

        /// <summary>
        /// 特价号码
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu9(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                var organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    ViewBag.organizeId = organizeId;
                    ViewBag.FullName = organize.FullName;
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
        /// 号码定制
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menu10(string organizeId, string city)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                var organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    ViewBag.organizeId = organizeId;
                    ViewBag.FullName = organize.FullName;
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
        public ActionResult Menu11(string organizeId)
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
        /// 流量卡
        /// </summary>
        /// <returns></returns>
        public ActionResult Menu12(string organizeId)
        {
            if (!string.IsNullOrEmpty(organizeId))
            {
                var organize = organizebll.GetEntity(organizeId);
                if (!string.IsNullOrEmpty(organize.OrganizeId))
                {
                    ViewBag.organizeId = organizeId;
                    ViewBag.FullName = organize.FullName;
                    return View();
                }
                return Content("机构暂时未生效或不存在");
            }
            else
            {
                return Content("链接不正确不或完整");
            }
        }
        

        private TelphoneLiangJoinBLL joinbll = new TelphoneLiangJoinBLL();
        SmsInfoBLL smsBll = new SmsInfoBLL();
        /// <summary>
        /// 提交代理加盟
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveSellerRegister2(TelphoneLiangJoinEntity entity,string smsCode)
        {
            //1.验证手机验证码是否有效，是否过期
            var sms = smsBll.GetList("{}").Where(t => t.Tel == entity.Telphone && t.Type == (int)SmsType.申请加盟).OrderByDescending(t => t.CreateDate).FirstOrDefault();
            if (sms != null)
            {
                ValidateSmsCode(sms, smsCode);

                if (joinbll.NotExistTelphone(entity.Telphone))//号码个数为0true
                {
                    entity.OpenId = CurrentWxUser.OpenId;
                    entity.NickName = CurrentWxUser.NickName;
                    entity.Sex = CurrentWxUser.Users.Sex;
                    entity.WxCity = CurrentWxUser.Users.City;
                    entity.WXPro = CurrentWxUser.Users.Province;
                    entity.HeadimgUrl = CurrentWxUser.HeadimgUrl;

                    //插入加盟表
                    joinbll.SaveForm("", entity);
                    return Content("提交成功");
                }
                else
                {
                    return Content("该手机号提交的申请正在等待审核！请不要重复提交，着急请拨打电话咨询");
                }
            }
            else
            {
                return Content("验证码有误");
            }
        }

        /// <summary>
        /// 提交代理加盟
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveSellerRegister1314(TelphoneLiangJoinEntity entity, string smsCode)
        {
            //1.验证手机验证码是否有效，是否过期
            var sms = smsBll.GetList("{}").Where(t => t.Tel == entity.Telphone && t.Type == (int)SmsType.申请加盟).OrderByDescending(t => t.CreateDate).FirstOrDefault();
            if (sms != null)
            {
                ValidateSmsCode(sms, smsCode);

                if (joinbll.NotExistTelphone(entity.Telphone))//号码个数为0true
                {
                    //插入加盟表
                    joinbll.SaveForm("", entity);
                    return Content("提交成功");
                }
                else
                {
                    return Content("该手机号提交的申请正在等待审核！请不要重复提交，着急请拨打电话咨询");
                }
            }
            else
            {
                return Content("验证码有误");
            }
        }


        #region 谱号下架
        /// <summary>
        /// 号码内容页详情
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFormJson(int? keyValue)
        {
            TelphonePuEntity entity = publl.GetEntity(keyValue);
            return Content(JsonConvert.SerializeObject(entity));
        }

        /// <summary>
        /// 加载售出人列表
        /// </summary>
        /// <returns></returns>
        //[WeiXinLoginAuthorize]
        public ActionResult SearchForm(string organizeId)
        {
            ViewBag.userList = userBLL.GetUsersByOrganizeId(organizeId);
            ViewBag.organizeId = organizeId;
            return View();
        }
        /// <summary>
        /// 保存靓号，修改状态售出
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult SaveForm(int? keyValue, TelphonePuEntity entity)
        {
            entity.ModifyUserName = CurrentWxUser.NickName;
            publl.SaveForm(keyValue, entity);
            string Message = "保存成功！";
            return Content(new JsonMessage { Success = false, Code = "-1", Message = Message }.ToString());
        }
        #endregion


    }
}
