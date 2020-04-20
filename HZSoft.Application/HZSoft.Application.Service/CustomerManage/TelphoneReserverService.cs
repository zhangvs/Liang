using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HZSoft.Application.Service.CustomerManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-07-25 11:57
    /// 描 述：预约号码
    /// </summary>
    public class TelphoneReserverService : RepositoryFactory<TelphoneReserverEntity>, TelphoneReserverIService
    {
        private TelphoneSourceIService sourceService = new TelphoneSourceService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneReserverEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var expression = LinqExtensions.True<TelphoneReserverEntity>();
            var queryParam = queryJson.ToJObject();
            //单据编号
            if (!queryParam["Reserve"].IsEmpty())
            {
                string Reserve = queryParam["Reserve"].ToString();
                expression = expression.And(t => t.Reserve.Contains(Reserve));
            }
            return this.BaseRepository().FindList(expression, pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphoneReserverEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 最新的10条记录
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TelphoneReserverEntity> GetListTop10()
        {
            return this.BaseRepository().IQueryable().OrderByDescending(t => t.CreateTime).Take(10).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TelphoneReserverEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="Reserve">主键值</param>
        /// <returns></returns>
        public TelphoneReserverEntity GetEntityByReserve(string Reserve)
        {
            return this.BaseRepository().FindEntity(t => t.Reserve == Reserve && t.DeleteMark == 0);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, TelphoneReserverEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion


        /// <summary>
        /// 核单
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="State">状态：1-启动；0-禁用</param>
        public void UpdateCheckState(string keyValue, int State)
        {
            TelphoneReserverEntity reserveEntity = new TelphoneReserverEntity();
            reserveEntity.Modify(keyValue);
            reserveEntity.CheckMark = State;
            this.BaseRepository().Update(reserveEntity);
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="State">状态：1-启动；0-禁用</param>
        public void UpdateDeleteState(string keyValue, int State)
        {
            TelphoneReserverEntity reserveEntity = new TelphoneReserverEntity();
            reserveEntity.Modify(keyValue);
            reserveEntity.DeleteMark = State;
            this.BaseRepository().Update(reserveEntity);
        }
    }
}
