using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using System.Web.Mvc;
using HZSoft.Application.Busines.SystemManage;
using HZSoft.Application.Code;

namespace HZSoft.Application.Web.Areas.CustomerManage.Controllers
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-09-22 15:43
    /// 描 述：号码订单
    /// </summary>
    public class TelphoneOrderController : MvcControllerBase
    {
        private TelphoneOrderBLL telphoneorderbll = new TelphoneOrderBLL();
        private CodeRuleBLL codeRuleBLL = new CodeRuleBLL();

        #region 视图功能
        /// <summary>
        /// 订单发货汇总
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneOrderIndex()
        {
            return View();
        }
        /// <summary>
        /// 预定号码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneOrderReserve()
        {
            return View();
        }
        /// <summary>
        /// 待发货
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneOrderSend()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneOrderForm()
        {
            if (Request["keyValue"] == null)
            {
                ViewBag.OrderCode = codeRuleBLL.GetBillCode("c576c3f7-631d-4108-baaf-1495bdc0d6bb");
                //ViewBag.OrderCode = codeRuleBLL.GetBillCode(SystemInfo.CurrentUserId, "", ((int)CodeRuleEnum.Telphone_OrderCode).ToString());
            }
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
            var data = telphoneorderbll.GetPageList(pagination, queryJson);
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
            var data = telphoneorderbll.GetList(queryJson);
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
            var data = telphoneorderbll.GetEntity(keyValue);
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
            telphoneorderbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        private Kd100SubscribeDemo kd100 = new Kd100SubscribeDemo();
        private KdApiSubscribeDemo kdn = new KdApiSubscribeDemo();
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, TelphoneOrderEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                TelphoneOrderEntity oldentity = telphoneorderbll.GetEntity(keyValue);
                //老快递单号为空，新快递单号不为空，初次发货才请求快递接口
                if (!string.IsNullOrEmpty(entity.Numbers) && string.IsNullOrEmpty(oldentity.Numbers))
                {
                    kdn.orderTracesSubByJson(entity);//快递鸟免费接口，不传真实数据联系方式
                    entity.Sign = 0;
                }
            }
            else
            {
                //订阅接口
                if (!string.IsNullOrEmpty(entity.Numbers))
                {
                    //kd100.orderTracesSubByJson(entity.Express,entity.Numbers);
                    kdn.orderTracesSubByJson(entity);//快递鸟免费接口，不传真实数据联系方式
                    entity.Sign = 0;
                }
                entity.CheckMark = 1;//手动添加跳过审核，已审核状态
            }

            telphoneorderbll.SaveForm(keyValue, entity);
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
        public ActionResult UpdateCheckState(string keyValue)
        {
            telphoneorderbll.UpdateCheckState(keyValue, 1);
            return Success("核单成功。");
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
            telphoneorderbll.UpdateDeleteState(keyValue, 1);
            return Success("作废成功。");
        }
    }
}
