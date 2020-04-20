using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.Service.CustomerManage;
using HZSoft.Util.WebControl;
using System.Collections.Generic;
using System;
using HZSoft.Cache.Factory;

namespace HZSoft.Application.Busines.CustomerManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-09-05 17:58
    /// 描 述：靓号加盟代理
    /// </summary>
    public class TelphoneLiangJoinBLL
    {
        private TelphoneLiangJoinIService service = new TelphoneLiangJoinService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneLiangJoinEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        public IEnumerable<TelphoneLiangJoinEntity> GetPageList1(Pagination pagination, string queryJson)
        {
            return service.GetPageList1(pagination, queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneLiangJoinEntity> GetPageList2(Pagination pagination, string queryJson)
        {
            return service.GetPageList2(pagination, queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphoneLiangJoinEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TelphoneLiangJoinEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public bool NotExistTelphone(string telphone)
        {
            return service.NotExistTelphone(telphone);
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
        public void SaveForm(string keyValue, TelphoneLiangJoinEntity entity)
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
        public string UpdateCheckState(TelphoneLiangJoinEntity entity, int State)
        {
            try
            {
                string msg= service.UpdateCheckState(entity, State);
                CacheFactory.Cache().RemoveCache("OrganizeCache");
                CacheFactory.Cache().RemoveCache("DepartmentCache");
                CacheFactory.Cache().RemoveCache("RoleCache");
                CacheFactory.Cache().RemoveCache("userCache");
                return msg;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 修改0级机构
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="State">状态：1-启动；0-禁用</param>
        public string UpdateTopOrg(TelphoneLiangJoinEntity entity, int State)
        {
            try
            {
                string msg = service.UpdateTopOrg(entity, State);
                CacheFactory.Cache().RemoveCache("OrganizeCache");
                CacheFactory.Cache().RemoveCache("DepartmentCache");
                CacheFactory.Cache().RemoveCache("RoleCache");
                CacheFactory.Cache().RemoveCache("userCache");
                return msg;
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
