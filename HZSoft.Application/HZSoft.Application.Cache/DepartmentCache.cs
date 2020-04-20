using HZSoft.Application.Busines.BaseManage;
using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Cache.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HZSoft.Application.Cache
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创建人：佘赐雄
    /// 日 期：2016.3.4 9:56
    /// 描 述：部门信息缓存
    /// </summary>
    public class DepartmentCache
    {
        private DepartmentBLL busines = new DepartmentBLL();

        /// <summary>
        /// 部门列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetList()
        {
            var cacheList = CacheFactory.Cache().GetCache<IEnumerable<DepartmentEntity>>(busines.cacheKey);
            if (cacheList == null)
            {
                var data = busines.GetList();
                CacheFactory.Cache().WriteCache(data, busines.cacheKey);
                return data;
            }
            else
            {
                return cacheList;
            }
        }
        /// <summary>
        /// 部门列表
        /// </summary>
        /// <param name="organizeId">公司Id</param>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetList(string organizeId)
        {
            var data = this.GetList();
            if (!string.IsNullOrEmpty(organizeId))
            {
                data = data.Where(t => t.OrganizeId == organizeId);
            }
            return data;
        }

        public DepartmentEntity GetEntity(string departmentId)
        {
            var data = this.GetList();
            if (!string.IsNullOrEmpty(departmentId))
            {
                var d = data.Where(t => t.DepartmentId == departmentId).ToList<DepartmentEntity>();
                if (d.Count > 0)
                {
                    return d[0];
                }
            }
            return new DepartmentEntity();
        }



        /// <summary>
        /// 获取自己和下级，从缓存中获取
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DepartmentEntity> GetListByIds()
        {
            var data = this.GetList();
            string companyId = OperatorProvider.Provider.Current().CompanyId;
            if (!OperatorProvider.Provider.Current().IsSystem && companyId != "a5a962da-57e1-4ad4-87b2-bbdcd1b7cc92")
            {
                var d = data.Where(t => t.OrganizeId == companyId).OrderBy(t => t.CreateDate).ToList<DepartmentEntity>();
                return d;
            }
            else
            {
                return data;
            }
        }
    }
}
