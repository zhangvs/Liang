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
    /// 日 期：2018-10-17 21:56
    /// 描 述：VIP服务机构
    /// </summary>
    public interface TelphoneLiangVipIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        IEnumerable<TelphoneLiangVipEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<TelphoneLiangVipEntity> GetList(string queryJson);
        /// <summary>
        /// 判断当前机构是否到期
        /// </summary>
        /// <param name="organizeId"></param>
        /// <returns></returns>
        bool GetVipByOrganizeId(string organizeId);
        List<string> GetVipOrgList(string organizeId, string pid, string top);
        /// <summary>
        /// 判断当前机构是否到期
        /// </summary>
        /// <param name="organizeId"></param>
        /// <returns></returns>
        bool IsShareMark(string organizeId);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        TelphoneLiangVipEntity GetEntity(string keyValue);
        /// <summary>
        /// vip机构
        /// </summary>
        /// <returns></returns>
        DataTable GetTable();
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, TelphoneLiangVipEntity entity);
        #endregion
    }
}
