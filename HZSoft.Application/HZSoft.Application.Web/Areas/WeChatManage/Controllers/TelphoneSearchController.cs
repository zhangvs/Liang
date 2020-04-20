using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HZSoft.Application.Web.Areas.WeChatManage.Controllers
{
    /// <summary>
    /// 公众号号码查询页面
    /// </summary>
    public class TelphoneSearchController : Controller
    {
        private TelphoneSourceBLL telphonesourcebll = new TelphoneSourceBLL();
        private TelphoneReserveSearchBLL reserverSearchbll = new TelphoneReserveSearchBLL();
        private TelphoneOrderBLL orderBll = new TelphoneOrderBLL();
        /// <summary>
        /// 查询页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var list0539 = telphonesourcebll.GetList0539();
            ViewBag.list0539 = list0539;
            return View();
        }

        /// <summary>
        /// 查询结果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetEntity(string telphone)
        {
            TelphoneSourceEntity entity = telphonesourcebll.GetEntity(telphone);
            return Content(JsonConvert.SerializeObject(entity));
        }

        /// <summary>
        /// 查询结果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetEntityByTelphone(string Telphone)
        {
            TelphoneOrderEntity entity = orderBll.GetEntityByTelphone(Telphone);
            return Content(JsonConvert.SerializeObject(entity));
        }

        /// <summary>
        /// 查询结果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TelAuto(string telphone)
        {
            var entity = telphonesourcebll.GetList(telphone);
            return Content(JsonConvert.SerializeObject(entity));
        }

        /// <summary>
        /// 尾号匹配
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetListEnd4(string end4,string number)
        {
            //添加查询记录表
            TelphoneReserveSearchEntity searchEntity = new TelphoneReserveSearchEntity()
            {
                IPAddress = Net.Ip,
                IPAddressName = IPLocation.GetLocation(Net.Ip),
                SearchNumber=end4
            };
            searchEntity.Create();
            reserverSearchbll.SaveForm("", searchEntity);

            //查询尾号相同的号码
            var entity = telphonesourcebll.GetListEnd4(end4);
            return Content(JsonConvert.SerializeObject(entity));
        }

        /// <summary>
        /// 预定状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TelReserve(string telphone)
        {
            telphonesourcebll.TelReserve(telphone);
            return Content("true");
        }
        private TelphonePuBLL telphonepubll = new TelphonePuBLL();
        /// <summary>
        /// 尾号匹配
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Get0539End4(string end4, string number)
        {
            //添加查询记录表
            TelphoneReserveSearchEntity searchEntity = new TelphoneReserveSearchEntity()
            {
                IPAddress = Net.Ip,
                IPAddressName = IPLocation.GetLocation(Net.Ip),
                SearchNumber = end4
            };
            searchEntity.Create();
            reserverSearchbll.SaveForm("", searchEntity);

            //查询尾号相同的号码
            var entity = telphonepubll.GetListEnd4(end4);
            return Content(JsonConvert.SerializeObject(entity));
        }
    }
}
