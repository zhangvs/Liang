using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using System.Web.Mvc;
using System;
using System.Data;
using System.Linq;

namespace HZSoft.Application.Web.Areas.CustomerManage.Controllers
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-03-05 10:43
    /// 描 述：共享机构表
    /// </summary>
    public class TelphoneVipShareController : MvcControllerBase
    {
        //选择共享机构
        private TelphoneVipShareBLL telphonevipsharebll = new TelphoneVipShareBLL();
        //所有vip机构
        private TelphoneLiangVipBLL telphoneliangvipbll = new TelphoneLiangVipBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneVipShareIndex()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneVipShareForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = telphonevipsharebll.GetPageList(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = telphonevipsharebll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = telphonevipsharebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// 用户列表
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetVipListJson(string vipId)
        {
            var existMember = telphonevipsharebll.GetVipList(vipId);
            var userdata = telphoneliangvipbll.GetTable();
            userdata.Columns.Add("ischeck", Type.GetType("System.Int32"));
            userdata.Columns.Add("isdefault", Type.GetType("System.Int32"));
            foreach (DataRow item in userdata.Rows)
            {
                string organizeid = item["organizeid"].ToString();
                int ischeck = existMember.Count(t => t.ShareId == organizeid);
                item["ischeck"] = ischeck;
            }
            userdata = DataHelper.DataFilter(userdata, "", "createdate desc");
            return Content(userdata.ToJson());
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            telphonevipsharebll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, TelphoneVipShareEntity entity)
        {
            telphonevipsharebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }


        /// <summary>
        /// 保存角色成员
        /// </summary>
        /// <param name="vipId">角色Id</param>
        /// <param name="shareIds">成员Id</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveMember(string vipId, string shareIds)
        {
            telphonevipsharebll.SaveMember(vipId, shareIds);
            return Success("保存成功。");
        }
        #endregion
    }
}
