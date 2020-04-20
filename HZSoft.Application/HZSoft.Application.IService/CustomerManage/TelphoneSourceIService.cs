using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Util.WebControl;
using System.Collections.Generic;

namespace HZSoft.Application.IService.CustomerManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-09-16 16:41
    /// 描 述：号码库
    /// </summary>
    public interface TelphoneSourceIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        IEnumerable<TelphoneSourceEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<TelphoneSourceEntity> GetList(string queryJson);
        IEnumerable<TelphoneSourceEntity> GetListEnd4(string end4);
        IEnumerable<TelphoneSourceEntity> GetListQian8(string Qian8);
        IEnumerable<TelphoneSourceEntity> GetList0539();
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        TelphoneSourceEntity GetEntity(int? keyValue);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        TelphoneSourceEntity GetEntity(string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(int? keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(int? keyValue, TelphoneSourceEntity entity);
        /// <summary>
        /// 获取号码
        /// </summary>
        /// <returns></returns>
        int GetTelphone();

        /// <summary>
        /// 预定状态修改
        /// </summary>
        /// <param name="telphone">实体对象</param>
        /// <returns></returns>
        void TelReserve(string telphone);
        #endregion
    }
}
