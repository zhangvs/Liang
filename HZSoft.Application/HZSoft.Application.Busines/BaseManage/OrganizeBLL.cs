using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.IService.BaseManage;
using HZSoft.Application.Service.BaseManage;
using HZSoft.Cache.Factory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HZSoft.Application.Busines.BaseManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创建人：佘赐雄
    /// 日 期：2015.11.02 14:27
    /// 描 述：机构管理
    /// </summary>
    public class OrganizeBLL
    {
        private IOrganizeService service = new OrganizeService();
        /// <summary>
        /// 缓存key
        /// </summary>
        public string cacheKey = "OrganizeCache";

        #region 获取数据
        /// <summary>
        /// 机构列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrganizeEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// 机构列表
        /// </summary>
        /// <returns></returns>
        //public IEnumerable<OrganizeEntity> GetListByIds()
        //{
        //    return service.GetListByIds();
        //}
        /// <summary>
        /// 机构实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OrganizeEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 公司名称不能重复
        /// </summary>
        /// <param name="organizeName">公司名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistFullName(string fullName, string keyValue)
        {
            return service.ExistFullName(fullName, keyValue);
        }
        /// <summary>
        /// 外文名称不能重复
        /// </summary>
        /// <param name="enCode">外文名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistEnCode(string enCode, string keyValue)
        {
            return service.ExistEnCode(enCode, keyValue);
        }
        /// <summary>
        /// 中文名称不能重复
        /// </summary>
        /// <param name="shortName">中文名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistShortName(string shortName, string keyValue)
        {
            return service.ExistShortName(shortName, keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除机构
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
                CacheFactory.Cache().RemoveCache(cacheKey);
                CacheFactory.Cache().RemoveCache("DepartmentCache");
                CacheFactory.Cache().RemoveCache("RoleCache");
                CacheFactory.Cache().RemoveCache("userCache");
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存机构表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="organizeEntity">机构实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, OrganizeEntity organizeEntity)
        {
            try
            {
                service.SaveForm(keyValue, organizeEntity);
                CacheFactory.Cache().RemoveCache(cacheKey);
                CacheFactory.Cache().RemoveCache("DepartmentCache");
                CacheFactory.Cache().RemoveCache("RoleCache");
                CacheFactory.Cache().RemoveCache("userCache");
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="State">状态：1-启动；0-禁用</param>
        public void UpdateState(string keyValue, int State)
        {
            try
            {
                service.UpdateState(keyValue, State);
                CacheFactory.Cache().RemoveCache(cacheKey);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 修改机构协议状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="State">状态：1-同意；0-拒绝</param>
        public void UpdateAgreementState(string keyValue, int State)
        {
            try
            {
                service.UpdateAgreementState(keyValue, State);
                CacheFactory.Cache().RemoveCache(cacheKey);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion



        /// <summary>
        /// 保存机构表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="organizeEntity">机构实体</param>
        /// <returns></returns>
        public void AddAdmin(string keyValue)
        {
            try
            {
                service.AddAdmin(keyValue);
                CacheFactory.Cache().RemoveCache(cacheKey);
                CacheFactory.Cache().RemoveCache("DepartmentCache");
                CacheFactory.Cache().RemoveCache("RoleCache");
                CacheFactory.Cache().RemoveCache("userCache");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
