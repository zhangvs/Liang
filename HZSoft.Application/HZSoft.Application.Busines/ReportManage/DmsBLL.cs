using HZSoft.Application.Entity.AuthorizeManage;
using HZSoft.Application.Entity.ReportManage;
using HZSoft.Application.IService.ReportManage;
using HZSoft.Application.Service.ReportManage;
using System;
using System.Collections.Generic;
using System.Data;

namespace HZSoft.Application.Busines.ReportManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创建人：刘晓雷
    /// 日 期：2016.1.14 14:27
    /// 描 述：报表模板管理
    /// </summary>
    public class DmsBLL
    {
        private IDmsService service = new DmsService();
        /// <summary>
        /// 员工报表
        /// </summary>
        /// <param name="postData">起始，结束日期json</param>
        /// <returns></returns>
        public DataTable GetDateOrder_emp(string queryJson)
        {
            return service.GetDateOrder_emp(queryJson);
        }
        /// <summary>
        /// 销售号码分析报表
        /// </summary>
        /// <param name="postData">起始，结束日期json</param>
        /// <returns></returns>
        public DataTable GetAnalysis(string queryJson)
        {
            return service.GetAnalysis(queryJson);
        }
        /// <summary>
        /// 话单通话时长
        /// </summary>
        /// <param name="postData">起始，结束日期json</param>
        /// <returns></returns>
        public DataTable GetCallLog(string queryJson)
        {
            return service.GetCallLog(queryJson);
        }



        #region 靓号部分
        /// <summary>
        /// 靓号浏览排名
        /// </summary>
        /// <param name="postData">起始，结束日期json</param>
        /// <returns></returns>
        public DataTable GetDateLiang_See(string queryJson)
        {
            return service.GetDateLiang_See(queryJson);
        }
        /// <summary>
        /// 靓号转发排名
        /// </summary>
        /// <param name="postData">起始，结束日期json</param>
        /// <returns></returns>
        public DataTable GetDateLiang_Share(string queryJson)
        {
            return service.GetDateLiang_Share(queryJson);
        }
        /// <summary>
        /// 靓号加盟排名
        /// </summary>
        /// <param name="postData">起始，结束日期json</param>
        /// <returns></returns>
        public DataTable GetDateLiang_Join(string queryJson)
        {
            return service.GetDateLiang_Join(queryJson);
        }

        /// <summary>
        /// 首页数量统计指标
        /// </summary>
        /// <returns></returns>
        public DataTable GetDateHome_Count()
        {
            return service.GetDateHome_Count();
        }

        #endregion



        /// <summary>
        /// 猜
        /// </summary>
        /// <param name="postData">起始，结束日期json</param>
        /// <returns></returns>
        public DataTable GetDateDms_Cai(string queryJson)
        {
            return service.GetDateDms_Cai(queryJson);
        }

        /// <summary>
        /// 销售报表
        /// </summary>
        /// <param name="postData">起始，结束日期json</param>
        /// <returns></returns>
        public DataTable GetLiangSales(string queryJson)
        {
            return service.GetLiangSales(queryJson);
        }

    }
}
