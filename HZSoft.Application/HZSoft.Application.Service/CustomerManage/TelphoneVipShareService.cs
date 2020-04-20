using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Data.Repository;
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
    /// 日 期：2020-03-05 10:43
    /// 描 述：共享机构表
    /// </summary>
    public class TelphoneVipShareService : RepositoryFactory<TelphoneVipShareEntity>, TelphoneVipShareIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneVipShareEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphoneVipShareEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TelphoneVipShareEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取成员列表
        /// </summary>
        /// <param name="vipId">对象Id</param>
        /// <returns></returns>
        public IEnumerable<TelphoneVipShareEntity> GetVipList(string vipId)
        {
            return this.BaseRepository().IQueryable(t => t.VipId == vipId).OrderByDescending(t => t.CreateDate).ToList();
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
        public void SaveForm(string keyValue, TelphoneVipShareEntity entity)
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
        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="vipId">对象Id</param>
        /// <param name="shareIds">成员Id</param>
        public void SaveMember(string vipId, string[] shareIds)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                db.Delete<TelphoneVipShareEntity>(t => t.VipId == vipId);
                int SortCode = 1;
                foreach (string item in shareIds)
                {
                    TelphoneVipShareEntity vipShareEntity = new TelphoneVipShareEntity();
                    vipShareEntity.Create();
                    vipShareEntity.VipId = vipId;
                    vipShareEntity.ShareId = item;
                    vipShareEntity.SortCode = SortCode++;
                    db.Insert(vipShareEntity);
                }
                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }
        #endregion
    }
}
