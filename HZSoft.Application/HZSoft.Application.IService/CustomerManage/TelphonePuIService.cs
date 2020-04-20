using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace HZSoft.Application.IService.CustomerManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-10-23 14:11
    /// 描 述：靓号库
    /// </summary>
    public interface TelphonePuIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        IEnumerable<TelphonePuEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<TelphonePuEntity> GetList(string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<TelphonePuEntity> GetList(string telphone, string organizeId, string city);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<TelphonePuEntity> GetGrade(string organizeId, string Grade, string city);
        IEnumerable<TelphonePuEntity> GetListEnd4(string end4);
        /// <summary>
        /// 是否是权限内的号码
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        IEnumerable<TelphonePuEntity> GetEntityByOrgTel(string organizeId, string telphone);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        TelphonePuEntity GetEntity(int? keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValues);
        /// <summary>
        /// 上架数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void UpForm(string keyValues);
        /// <summary>
        /// 现卡数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void ExistForm(string keyValues);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(int? keyValue, TelphonePuEntity entity);
        /// <summary>
        /// 批量（新增）
        /// </summary>
        /// <param name="dtSource">实体对象</param>
        /// <returns></returns>
        string BatchAddEntity(DataTable dtSource);
        //批量下架
        string DownTelphone(string downTelphones);
        //批量调价
        string PriceTelphone(string priceTelphones);
        #endregion
    }
}
