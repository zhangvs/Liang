using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Application.Busines.WeChatManage;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace HZSoft.Application.Web.Areas.WeChatManage.Controllers
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-11-21 14:46
    /// 描 述：用户表
    /// </summary>
    public class WeChat_UsersController : MvcControllerBase
    {
        private WeChat_UsersBLL wechat_usersbll = new WeChat_UsersBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WeChat_UsersIndex()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WeChat_UsersForm()
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
            var data = wechat_usersbll.GetPageList(pagination, queryJson);
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
            var data = wechat_usersbll.GetList(queryJson);
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
            var data = wechat_usersbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 自动匹配查询结果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NickNameAuto(string nickName)
        {
            //WeChat_UsersEntity queryJson = new WeChat_UsersEntity()
            //{
            //    NickName = nickName,
            //};

            var entity = wechat_usersbll.GetList(nickName);
            return Content(JsonConvert.SerializeObject(entity));
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
            wechat_usersbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, WeChat_UsersEntity entity)
        {
            wechat_usersbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
