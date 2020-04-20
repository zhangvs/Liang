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
    /// 描 述：组织架构缓存
    /// </summary>
    public class OrganizeCache
    {
        private OrganizeBLL busines = new OrganizeBLL();

        /// <summary>
        /// 组织列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrganizeEntity> GetList()
        {
            var cacheList = CacheFactory.Cache().GetCache<IEnumerable<OrganizeEntity>>(busines.cacheKey);
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
        /// 组织列表
        /// </summary>
        /// <param name="organizeId">公司Id</param>
        /// <returns></returns>
        public OrganizeEntity GetEntity(string organizeId)
        {
            var data = this.GetList();
            if (!string.IsNullOrEmpty(organizeId))
            {
                var d = data.Where(t => t.OrganizeId == organizeId).ToList<OrganizeEntity>();
                if (d.Count > 0)
                {
                    return d[0];
                }
            }
            return new OrganizeEntity();
        }


        /// <summary>
        /// 获取自己和下级，从缓存中获取
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrganizeEntity> GetListByIds()
        {
            var data = this.GetList();
            string companyId = OperatorProvider.Provider.Current().CompanyId;
            if (!OperatorProvider.Provider.Current().IsSystem && companyId != "a5a962da-57e1-4ad4-87b2-bbdcd1b7cc92")
            {
                var d = data.Where(t => t.OrganizeId == companyId || t.ParentId == companyId).OrderBy(t => t.Category).ThenBy(t => t.CreateDate).ToList<OrganizeEntity>();
                return d;
            }
            else
            {
                return data;
            }
        }

        /// <summary>
        /// 根据当前微信昵称，openId,
        /// </summary>
        /// <param name="organizeId">公司Id</param>
        /// <returns></returns>
        public OrganizeEntity GetEntityByWxUser(string organizeId, string nickName, string openId)
        {
            var data = this.GetList();
            if (!string.IsNullOrEmpty(organizeId))
            {
                var d = data.Where(t => t.OrganizeId == organizeId && (t.ShortName == nickName || t.ManagerId == openId)).ToList<OrganizeEntity>();
                if (d.Count > 0)
                {
                    return d[0];
                }
            }
            return new OrganizeEntity();
        }


        /// <summary>
        /// 判断是否有相同电话号码和机构名称的机构，已存在则不再审核通过
        /// </summary>
        /// <param name="organizeId">公司Id</param>
        /// <returns></returns>
        public OrganizeEntity GetEntityByTel(string Telphone)
        {
            var data = this.GetList();
            var d = data.Where(t => t.OuterPhone == Telphone).ToList<OrganizeEntity>();
            if (d.Count > 0)
            {
                return d[0];
            }
            return new OrganizeEntity();
        }

    }
}
