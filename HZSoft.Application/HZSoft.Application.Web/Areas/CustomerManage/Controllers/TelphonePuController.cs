using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using System.Web.Mvc;
using System.Web;
using System.IO;
using System;
using HZSoft.Util.Offices;
using System.Data;

namespace HZSoft.Application.Web.Areas.CustomerManage.Controllers
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-10-23 14:11
    /// 描 述：靓号库
    /// </summary>
    public class TelphonePuController : MvcControllerBase
    {
        private TelphonePuBLL telphoneliangbll = new TelphonePuBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphonePuIndex()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphonePuForm()
        {
            return View();
        }
        /// <summary>
        /// 批量导入靓号
        /// </summary>
        /// <returns></returns>
        public ActionResult TelphonePuImport()
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
            var data = telphoneliangbll.GetPageList(pagination, queryJson);
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
            var data = telphoneliangbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(int? keyValue)
        {
            var data = telphoneliangbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult TelphonePuOrg(int? keyValue)
        {
            var data = telphoneliangbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValues">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValues)
        {
            telphoneliangbll.RemoveForm(keyValues);
            return Success("删除成功。");
        }

        /// <summary>
        /// 批量上架
        /// </summary>
        /// <returns></returns>
        public ActionResult UpForm(string keyValues)
        {
            telphoneliangbll.UpForm(keyValues);
            return Success("上架成功。");
        }

        /// <summary>
        /// 批量现卡
        /// </summary>
        /// <returns></returns>
        public ActionResult ExistForm(string keyValues)
        {
            telphoneliangbll.ExistForm(keyValues);
            return Success("现卡成功。");
        }

        /// <summary>
        /// 批量下架
        /// </summary>
        /// <returns></returns>
        public ActionResult DownTelphone(string downTelphones)
        {
            string returnMsg = telphoneliangbll.DownTelphone(downTelphones);
            return Success(returnMsg);
        }

        /// <summary>
        /// 批量调价
        /// </summary>
        /// <returns></returns>
        public ActionResult PriceTelphone(string priceTelphones)
        {
            string returnMsg = telphoneliangbll.PriceTelphone(priceTelphones);
            return Success(returnMsg);
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
        public ActionResult SaveForm(int? keyValue, TelphonePuEntity entity)
        {
            telphoneliangbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion


        #region 批量导入靓号
        public FileResult GetFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Resource/ExcelTemplate/";
            string fileName = "普号批量导入模板.xlsx";
            return File(path + fileName, "application/ms-excel", fileName);
        }

        [HttpPost]
        public ActionResult TelphonePuImport(HttpPostedFileBase filebase)
        {
            HttpPostedFileBase file = Request.Files["files"];
            string FileName;
            string savePath;
            if (file == null || file.ContentLength <= 0)
            {
                ViewBag.error = "文件不能为空";
                return View();
            }
            else
            {
                string filename = Path.GetFileName(file.FileName);
                int filesize = file.ContentLength;//获取上传文件的大小单位为字节byte
                string fileEx = System.IO.Path.GetExtension(filename);//获取上传文件的扩展名
                string NoFileName = System.IO.Path.GetFileNameWithoutExtension(filename);//获取无扩展名的文件名
                int Maxsize = 4000 * 1024;//定义上传文件的最大空间大小为4M
                string FileType = ".xls,.xlsx";//定义上传文件的类型字符串

                FileName = NoFileName + DateTime.Now.ToString("yyyyMMddhhmmss") + fileEx;
                if (!FileType.Contains(fileEx))
                {
                    ViewBag.error = "文件类型不对，只能导入xls和xlsx格式的文件";
                    return View();
                }
                if (filesize >= Maxsize)
                {
                    ViewBag.error = "上传文件超过4M，不能上传";
                    return View();
                }
                string path = AppDomain.CurrentDomain.BaseDirectory + "Resource/ExcelData/";
                savePath = Path.Combine(path, FileName);
                file.SaveAs(savePath);
            }

            //excel转DataTable
            DataTable dtSource = ExcelHelper.ExcelImport(savePath);
            //批量插入TelphoneWash表
            //SqlBulkCopyByDatatable("TelphoneWash", dtSource);
            //一行行插入

            //引用事务机制，出错时，事物回滚
            ViewBag.error = telphoneliangbll.BatchAddEntity(dtSource);

            System.Threading.Thread.Sleep(2000);
            return View();
        }
        #endregion
    }
}
