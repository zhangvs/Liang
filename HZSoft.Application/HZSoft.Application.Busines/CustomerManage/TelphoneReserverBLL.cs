using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.Service.CustomerManage;
using HZSoft.Util.WebControl;
using System.Collections.Generic;
using System;

namespace HZSoft.Application.Busines.CustomerManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-07-25 11:57
    /// 描 述：预约号码
    /// </summary>
    public class TelphoneReserverBLL
    {
        private TelphoneReserverIService service = new TelphoneReserverService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneReserverEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphoneReserverEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        public IEnumerable<TelphoneReserverEntity> GetListTop10()
        {
            return service.GetListTop10();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TelphoneReserverEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public TelphoneReserverEntity GetEntityByReserve(string Reserve)
        {
            return service.GetEntityByReserve(Reserve);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, TelphoneReserverEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// 修改核单状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="State">状态：1-启动；0-禁用</param>
        public void UpdateCheckState(string keyValue, int State)
        {
            try
            {
                service.UpdateCheckState(keyValue, State);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="State">状态：1-启动；0-禁用</param>
        public void UpdateDeleteState(string keyValue, int State)
        {
            try
            {
                service.UpdateDeleteState(keyValue, State);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
