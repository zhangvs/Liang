using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using HZSoft.Util.Extension;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using HZSoft.Data;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.IService.BaseManage;
using HZSoft.Application.Service.AuthorizeManage;
using HZSoft.Application.IService.AuthorizeManage;
using HZSoft.Application.Code;

namespace HZSoft.Application.Service.BaseManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创建人：佘赐雄
    /// 日 期：2015.11.4 14:31
    /// 描 述：角色管理
    /// </summary>
    public class RoleService : RepositoryFactory<RoleEntity>, IRoleService
    {
        private IAuthorizeService<RoleEntity> iauthorizeservice = new AuthorizeService<RoleEntity>();

        #region 获取数据
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetList()
        {
            var expression = LinqExtensions.True<RoleEntity>();
            expression = expression.And(t => t.Category == 1).And(t => t.EnabledMark == 1).And(t => t.DeleteMark == 0);
            return this.BaseRepository().IQueryable(expression).OrderByDescending(t => t.CreateDate).ToList();
        }
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetListByIds()
        {
            string strSql = "SELECT * FROM Base_Role where DeleteMark <> 1 ";
            if (!OperatorProvider.Provider.Current().IsSystem)
            {
                string dataAutor = string.Format(OperatorProvider.Provider.Current().DataAuthorize.ReadAutorize, OperatorProvider.Provider.Current().UserId);
                strSql += " and RoleId IN( select RoleId from Base_Role where 1=1 and OrganizeId in (" + dataAutor + ") GROUP BY OrganizeId )";
            }
            strSql += " ORDER BY SortCode ";
            return this.BaseRepository().FindList(strSql.ToString());
        }
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetPageList(Pagination pagination, string queryJson)
        {
            //var expression = LinqExtensions.True<RoleEntity>();
            //var queryParam = queryJson.ToJObject();
            ////查询条件
            //if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            //{
            //    string condition = queryParam["condition"].ToString();
            //    string keyword = queryParam["keyword"].ToString();
            //    switch (condition)
            //    {
            //        case "EnCode":            //角色编号
            //            expression = expression.And(t => t.EnCode.Contains(keyword));
            //            break;
            //        case "FullName":          //角色名称
            //            expression = expression.And(t => t.FullName.Contains(keyword));
            //            break;
            //        default:
            //            break;
            //    }
                
            //}
            //expression = expression.And(t => t.Category == 1);
            //return this.BaseRepository().FindList(expression, pagination);

            string strSql = "SELECT * FROM Base_Role where DeleteMark <> 1 and category=1 ";
            var expression = LinqExtensions.True<RoleEntity>();
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyword = queryParam["keyword"].ToString();
                strSql += " and (EnCode like '%" + keyword + "%' or  FullName like '%" + keyword + "%') ";
            }

            string companyId = OperatorProvider.Provider.Current().CompanyId;
            if (!OperatorProvider.Provider.Current().IsSystem && companyId != "a5a962da-57e1-4ad4-87b2-bbdcd1b7cc92")
            {
                strSql += " and OrganizeId in(SELECT OrganizeId FROM Base_Organize WHERE OrganizeId='"+ OperatorProvider.Provider.Current().CompanyId + "' OR ParentId ='" + OperatorProvider.Provider.Current().CompanyId + "')";
            }
            strSql += " ORDER BY SortCode ";
            return this.BaseRepository().FindList(strSql.ToString());

        }
        /// <summary>
        /// 角色列表all
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetAllList()
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  r.RoleId ,
				                    o.FullName AS OrganizeId ,
				                    r.Category ,
				                    r.EnCode ,
				                    r.FullName ,
				                    r.SortCode ,
				                    r.EnabledMark ,
				                    r.Description ,
				                    r.CreateDate
                    FROM    Base_Role r
				                    LEFT JOIN Base_Organize o ON o.OrganizeId = r.OrganizeId
                    WHERE   o.FullName is not null and r.Category = 1 and r.EnabledMark =1
                    ORDER BY o.FullName, r.SortCode");
            return this.BaseRepository().FindList(strSql.ToString());
        }
        /// <summary>
        /// 角色实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RoleEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 角色编号不能重复
        /// </summary>
        /// <param name="enCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistEnCode(string enCode, string keyValue)
        {
            var expression = LinqExtensions.True<RoleEntity>();
            expression = expression.And(t => t.EnCode == enCode).And(t => t.Category == 1);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.RoleId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        /// <summary>
        /// 角色名称不能重复
        /// </summary>
        /// <param name="fullName">名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistFullName(string fullName, string keyValue)
        {
            var expression = LinqExtensions.True<RoleEntity>();
            expression = expression.And(t => t.FullName == fullName).And(t => t.Category == 1);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.RoleId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存角色表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="roleEntity">角色实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, RoleEntity roleEntity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                roleEntity.Modify(keyValue);
                this.BaseRepository().Update(roleEntity);
            }
            else
            {
                roleEntity.Create();
                roleEntity.Category = 1;
                this.BaseRepository().Insert(roleEntity);
            }
        }
        #endregion
    }
}
