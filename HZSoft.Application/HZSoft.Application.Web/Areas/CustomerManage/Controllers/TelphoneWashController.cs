using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using System.Web.Mvc;
using System.Web;
using System.IO;
using System;
using System.Data.OleDb;
using System.Data;
using HZSoft.Util.Offices;
using System.Configuration;
using System.Data.SqlClient;

namespace HZSoft.Application.Web.Areas.CustomerManage.Controllers
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-09-16 16:41
    /// 描 述：号码库
    /// </summary>
    public class TelphoneWashController : MvcControllerBase
    {
        private TelphoneWashBLL telphonesourcebll = new TelphoneWashBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneWashIndex()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TelphoneWashForm()
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
            var data = telphonesourcebll.GetPageList(pagination, queryJson);
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
            var data = telphonesourcebll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(int keyValue)
        {
            var data = telphonesourcebll.GetEntity(keyValue);
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
        public ActionResult RemoveForm(int? keyValue)
        {
            telphonesourcebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(int? keyValue, TelphoneWashEntity entity)
        {
            telphonesourcebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 获取号码
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTelphone(string getCountStr)
        {
            int getCount = Convert.ToInt32(getCountStr);
            int state= telphonesourcebll.GetTelphone(getCount);
            if (state==0)
            {
                return Success("洗号池数据不足。");
            }
            else if (state == 1)
            {
                return Success("获取成功。");
            }
            else
            {
                return Success("今日已经超过最大获取数。");
            }
        }
        #endregion

        public FileResult GetFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Resource/ExcelTemplate/";
            string fileName = "呼叫任务导入模板.xlsx";
            return File(path + fileName, "application/ms-excel", fileName);
        }

        #region 批量导入呼叫任务
        public ActionResult WashImport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult WashImport(HttpPostedFileBase filebase)
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

            #region 传统
            ////string result = string.Empty;
            //string strConn;
            //strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + savePath + ";" + "Extended Properties=Excel 8.0";
            //OleDbConnection conn = new OleDbConnection(strConn);
            //conn.Open();
            //OleDbDataAdapter myCommand = new OleDbDataAdapter("select * from [Sheet1$]", strConn);
            //DataSet myDataSet = new DataSet();
            //try
            //{
            //    myCommand.Fill(myDataSet, "ExcelInfo");
            //}
            //catch (Exception ex)
            //{
            //    ViewBag.error = ex.Message;
            //    return View();
            //}
            //DataTable table = myDataSet.Tables["ExcelInfo"].DefaultView.ToTable();

            ////引用事务机制，出错时，事物回滚
            //using (TransactionScope transaction = new TransactionScope())
            //{
            //    for (int i = 0; i < table.Rows.Count; i++)
            //    {
            //        //获取地区名称
            //        string _areaName = table.Rows[i][0].ToString();
            //        //判断地区是否存在
            //        if (!_areaRepository.CheckAreaExist(_areaName))
            //        {
            //            ViewBag.error = "导入的文件中：" + _areaName + "地区不存在，请先添加该地区";
            //            return View();
            //        }
            //        else
            //        {
            //            Wash station = new Wash();
            //            station.AreaID = _areaRepository.GetIdByAreaName(_areaName).AreaID;
            //            station.WashName = table.Rows[i][1].ToString();
            //            station.TerminaAddress = table.Rows[i][2].ToString();
            //            station.CapacityGrade = table.Rows[i][3].ToString();
            //            station.OilEngineCapacity = decimal.Parse(table.Rows[i][4].ToString());
            //            _stationRepository.AddWash(station);
            //        }
            //    }
            //    transaction.Complete();
            //} 
            #endregion

            //excel转DataTable
            DataTable dtSource = ExcelHelper.ExcelImport(savePath);
            //批量插入TelphoneWash表
            //SqlBulkCopyByDatatable("TelphoneWash", dtSource);
            //一行行插入

            //引用事务机制，出错时，事物回滚
            ViewBag.error = telphonesourcebll.BatchAddEntity(dtSource);
            
            System.Threading.Thread.Sleep(2000);
            return View();
            //return RedirectToAction("TelphoneWashIndex");
        }

        /// <summary>
        /// 将datatable里面的大量数据批量复制到数据库中，而不用担心性能问题
        /// </summary>
        /// <param name="TableName">目标表</param>
        /// <param name="dt">源数据</param>
        private void SqlBulkCopyByDatatable(string TableName, DataTable dt)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BaseDb"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    try
                    {
                        sqlbulkcopy.DestinationTableName = TableName;
                        sqlbulkcopy.ColumnMappings.Add("Telphone", "Telphone");//自定义的DataTable和数据库表的字段相对应
                        //for (int i = 0; i < dt.Columns.Count; i++)
                        //{
                        //    sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                        //}
                        sqlbulkcopy.WriteToServer(dt);
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
        #endregion
    }
}
