using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.BaseManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.Service.BaseManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace HZSoft.Application.Service.CustomerManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-08-23 16:22
    /// 描 述：靓号主页浏览
    /// </summary>
    public class TelphoneLiangSeeService : RepositoryFactory<TelphoneLiangSeeEntity>, TelphoneLiangSeeIService
    {
        private IOrganizeService orgService = new OrganizeService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneLiangSeeEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var expression = LinqExtensions.True<TelphoneLiangSeeEntity>();
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneLiangSee where 1=1";
            //客户名称
            if (!queryParam["Keyword"].IsEmpty())
            {
                string Keyword = queryParam["Keyword"].ToString();
                strSql += " and NickName like '%" + Keyword + "%'";
            }
            //if (!queryParam["OrganizeId"].IsEmpty())
            //{
            //    string OrganizeId = queryParam["OrganizeId"].ToString();
            //    strSql += " and OrganizeId = '" + OrganizeId + "'";
            //}
            //else
            //{

            //}            
            if (!OperatorProvider.Provider.Current().IsSystem)
            {
                string companyId = OperatorProvider.Provider.Current().CompanyId;
                if (!string.IsNullOrEmpty(companyId))
                {
                    //strSql += " and OrganizeId IN( select OrganizeId from Base_Organize where ParentId='" + companyId +
                    //    "' OR OrganizeId='" + companyId + "')";
                    strSql += " and OrganizeId ='" + companyId + "'";
                }
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphoneLiangSeeEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TelphoneLiangSeeEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
        public void SaveForm(string keyValue, TelphoneLiangSeeEntity entity)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                db.Update(entity);
            }
            else
            {
                entity.Create();
                db.Insert(entity);
                //浏览+1
                orgService.UpdateSeeCount(entity.OrganizeId);
            }
            db.Commit();
        }
        #endregion
    }
}
