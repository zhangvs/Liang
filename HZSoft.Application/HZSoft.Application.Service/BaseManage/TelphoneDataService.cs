using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.BaseManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.Service.CustomerManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace HZSoft.Application.Service.BaseManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-09-19 17:45
    /// 描 述：号码库
    /// </summary>
    public class TelphoneDataService : RepositoryFactory<TelphoneDataEntity>, TelphoneDataIService
    {
        private TelphoneLiangH5IService telphoneLiangH5IService = new TelphoneLiangH5Service();
        private TelphoneLiangIService telphoneLiangIService = new TelphoneLiangService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneDataEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneData where 1=1 ";
            //单据编号
            if (!queryParam["Number7"].IsEmpty())
            {
                string Number7 = queryParam["Number7"].ToString();
                strSql += " and Number7 = '" + Number7 + "'";
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphoneDataEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TelphoneDataEntity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(int? keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(int? keyValue, TelphoneDataEntity entity)
        {
            if (keyValue != null)
            {
                IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                TelphoneDataEntity oldEntity = GetEntity(keyValue);
                //添加品牌信息
                if (oldEntity.Brand==null && entity.Brand!=null)
                {
                    //修改头条靓号库品牌
                    var listH5 = db.FindList<TelphoneLiangH5Entity>(t => t.Telphone.Contains(entity.Number7) && t.DeleteMark!=1);
                    foreach (var item in listH5)
                    {
                        item.Brand = entity.Brand;
                        db.Update<TelphoneLiangH5Entity>(item);
                    }
                    //修改靓号库品牌
                    var list = db.FindList<TelphoneLiangEntity>(t => t.Telphone.Contains(entity.Number7) && t.DeleteMark != 1);
                    foreach (var item in list)
                    {
                        item.Brand = entity.Brand;
                        db.Update<TelphoneLiangEntity>(item);
                    }
                }
                entity.Modify(keyValue);
                db.Update<TelphoneDataEntity>(entity);
                db.Commit();
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
