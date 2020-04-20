using HZSoft.Application.Entity.AuthorizeManage;
using HZSoft.Application.Entity.ReportManage;
using System.Collections.Generic;
using System.Data;

namespace HZSoft.Application.IService.ReportManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创建人：刘晓雷
    /// 日 期：2016.1.14 14:27
    /// 描 述：报表模板管理
    /// </summary>
    public interface IDmsService
    {
        #region 获取数据
        /// <summary>
        /// 员工报表
        /// </summary>
        /// <param name="queryJson">起始，结束日期json</param>
        /// <returns></returns>
        DataTable GetDateOrder_emp(string queryJson);
        /// <summary>
        /// 销售号码分析报表
        /// </summary>
        /// <param name="queryJson">起始，结束日期json</param>
        /// <returns></returns>
        DataTable GetAnalysis(string queryJson);
        /// <summary>
        /// 话单通话时长
        /// </summary>
        /// <param name="queryJson">起始，结束日期json</param>
        /// <returns></returns>
        DataTable GetCallLog(string queryJson);
        #endregion

        #region 靓号部分
        /// <summary>
        /// 靓号浏览统计报表
        /// </summary>
        /// <param name="queryJson">起始，结束日期json</param>
        /// <returns></returns>
        DataTable GetDateLiang_See(string queryJson);

        /// <summary>
        /// 靓号分享统计报表
        /// </summary>
        /// <returns></returns>
        DataTable GetDateLiang_Share(string queryJson);

        /// <summary>
        /// 靓号加盟统计报表
        /// </summary>
        /// <returns></returns>
        DataTable GetDateLiang_Join(string queryJson);

        /// <summary>
        /// 首页数量统计
        /// </summary>
        /// <param name="queryJson">起始，结束日期json</param>
        /// <returns></returns>
        DataTable GetDateHome_Count();

        #endregion

        /// <summary>
        /// 猜
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetDateDms_Cai(string queryJson);



        /// <summary>
        /// 靓号销售报表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetLiangSales(string queryJson);
    }
}
