using HZSoft.Application.Busines.BaseManage;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Code;
using HZSoft.Cache.Factory;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using HZSoft.Application.Entity.WebApp;
using HZSoft.Util.Extension;
using System;

namespace HZSoft.Application.Cache
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创建人：佘赐雄
    /// 日 期：2016.3.4 9:56
    /// 描 述：用户信息缓存
    /// </summary>
    public class UserCache
    {
        private UserBLL busines = new UserBLL();

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserEntity> GetList()
        {
            IEnumerable<UserEntity> data = new List<UserEntity>();
            var cacheList = CacheFactory.Cache().GetCache<IEnumerable<UserEntity>>(busines.cacheKey);
            if (cacheList == null)
            {
                data = busines.GetList();
                CacheFactory.Cache().WriteCache(data, busines.cacheKey);
            }
            else
            {
                data = cacheList;
            }
            return data;
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="departmentId">部门Id</param>
        /// <returns></returns>
        public IEnumerable<UserEntity> GetList(string departmentId)
        {
            var data = this.GetList();
            if (!string.IsNullOrEmpty(departmentId))
            {
                data = data.Where(t => t.DepartmentId == departmentId);
            }
            return data;
        }
        public Dictionary<string,appUserInfoModel> GetListToApp()
        {
            Dictionary<string, appUserInfoModel> data = new Dictionary<string,appUserInfoModel>();
            var datalist = this.GetList();
            foreach (var item in datalist)
            {
                appUserInfoModel one = new appUserInfoModel {
                    UserId = item.UserId,
                    Account = item.Account,
                    RealName = item.RealName,
                    OrganizeId = item.OrganizeId,
                    DepartmentId = item.DepartmentId
                };
                data.Add(item.UserId, one);
            }

            return data;
        }


        /// <summary>
        /// 获取自己和下级，从缓存中获取
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserEntity> GetListByIds()
        {
            var data = this.GetList();
            string companyId = OperatorProvider.Provider.Current().CompanyId;
            if (!OperatorProvider.Provider.Current().IsSystem && companyId != "a5a962da-57e1-4ad4-87b2-bbdcd1b7cc92")
            {
                var d = data.Where(t => t.UserId != "System" && t.DeleteMark==0 && t.EnabledMark==1 && t.OrganizeId == companyId).OrderBy(t => t.CreateDate).ToList<UserEntity>();
                return d;
            }
            else
            {
                return data;
            }
        }
    }
}
