using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using System.Web.Mvc;
using HZSoft.Application.Cache;

namespace HZSoft.Application.Web.Areas.CustomerManage.Controllers
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-09-05 17:58
    /// 描 述：靓号加盟代理
    /// </summary>
    public class TelphoneLiangJoinController : MvcControllerBase
    {
        private TelphoneLiangJoinBLL telphoneliangjoinbll = new TelphoneLiangJoinBLL();
        private OrganizeCache organizeCache = new OrganizeCache();

        #region 视图功能
        /// <summary>
        /// 加盟代理列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneLiangJoinIndex()
        {
            return View();
        }
        /// <summary>
        /// 来源机构级别0级机构列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneLiangJoinIndex1()
        {
            return View();
        }
        /// <summary>
        /// 供应商列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneLiangJoinIndex2()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneLiangJoinForm()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneLiangJoinForm1()
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
            var data = telphoneliangjoinbll.GetPageList(pagination, queryJson);
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
        /// 加盟来源机构级别
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson1(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = telphoneliangjoinbll.GetPageList1(pagination, queryJson);
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
        /// 供应商
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson2(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = telphoneliangjoinbll.GetPageList2(pagination, queryJson);
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
            var data = telphoneliangjoinbll.GetList(queryJson);
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
            var data = telphoneliangjoinbll.GetEntity(keyValue);
            return ToJsonResult(data);
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
            telphoneliangjoinbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, TelphoneLiangJoinEntity entity)
        {
            telphoneliangjoinbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion



        /// <summary>
        /// 核单
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult UpdateTopOrg(string keyValue)
        {
            var entity = telphoneliangjoinbll.GetEntity(keyValue);
            string msg = "";
            var organize = organizeCache.GetEntityByTel(entity.Telphone);
            if (!string.IsNullOrEmpty(organize.OrganizeId))
            {
                msg = "该手机号申请的机构已经存在，请换个手机号重新申请！";
            }
            //返回靓号商城链接，给下级
            msg = telphoneliangjoinbll.UpdateTopOrg(entity, 1);
            return Content(msg);
        }


        /// <summary>
        /// 核单
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult UpdateCheckState(string keyValue)
        {
            var entity = telphoneliangjoinbll.GetEntity(keyValue);
            string msg = "";
            var organize = organizeCache.GetEntityByTel(entity.Telphone);
            if (!string.IsNullOrEmpty(organize.OrganizeId))
            {
                msg= "该手机号申请的机构已经存在，请换个手机号重新申请！";
            }
            //返回靓号商城链接，给下级
            msg = telphoneliangjoinbll.UpdateCheckState(entity, 1);
            return Content(msg);
        }

        /// <summary>
        /// 核单-my
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult UpdateCheckStateMy(string keyValue)
        {
            var entity = telphoneliangjoinbll.GetEntity(keyValue);
            string msg = "";
            var organize = organizeCache.GetEntityByTel(entity.Telphone);
            if (!string.IsNullOrEmpty(organize.OrganizeId))
            {
                msg = "该手机号申请的机构已经存在，请换个手机号重新申请！";
            }
            //返回靓号商城链接，给下级
            msg = telphoneliangjoinbll.UpdateCheckState(entity, 2);
            return Content(msg);
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult UpdateDeleteState(string keyValue)
        {
            telphoneliangjoinbll.UpdateDeleteState(keyValue, 1);
            return Success("作废成功。");
        }
    }
}
