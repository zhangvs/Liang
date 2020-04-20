using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Busines.BaseManage;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using System.Web.Mvc;
using System.IO;
using System.Web;
using Ionic.Zip;
using System;
using System.Net;

namespace HZSoft.Application.Web.Areas.BaseManage.Controllers
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-10-12 17:30
    /// 描 述：TelphoneCertification
    /// </summary>
    public class TelphoneCertificationController : MvcControllerBase
    {
        private TelphoneCertificationBLL telphonecertificationbll = new TelphoneCertificationBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneCertificationIndex()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneCertificationForm()
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
            var data = telphonecertificationbll.GetPageList(pagination, queryJson);
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
            var data = telphonecertificationbll.GetList(queryJson);
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
            var data = telphonecertificationbll.GetEntity(keyValue);
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
            telphonecertificationbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, TelphoneCertificationEntity entity)
        {
            telphonecertificationbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult Export(string keyValue)
        {
            var data = telphonecertificationbll.GetEntity(keyValue);
            if (data!=null)
            {
                using (ZipFile zip = new ZipFile(System.Text.Encoding.Default))
                {
                    zip.AddFile(Server.MapPath(data.photo_z), "");
                    zip.AddFile(Server.MapPath(data.photo_b), "");
                    zip.AddFile(Server.MapPath(data.photo_s), "");

                    Stream ms = new MemoryStream();
                    zip.Save(ms);
                    ms.Seek(0, SeekOrigin.Begin);//将ms流重置，让后面调用可以获取到正确的流，否则可能会获取不到文件
                    return File(ms, "application/zip", data.mobileNumber + data.custName + ".zip");

                    //zip.Save(Server.MapPath("~/Resource/ZIP/" + data.mobileNumber + data.custName + ".zip"));
                    //return File(Server.MapPath("~/Resource/ZIP/" + data.mobileNumber + data.custName + ".zip"),
                    //              "application/zip");
                }
            }
            else
            {
                return Success("未选择。");
            }

        }
        


    }
}
