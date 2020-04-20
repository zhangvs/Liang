using HZSoft.Application.Entity.BaseManage;
using System.Collections.Generic;
using System.Data;

namespace HZSoft.Application.IService.BaseManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创建人：佘赐雄
    /// 日 期：2015.11.02 14:27
    /// 描 述：机构管理
    /// </summary>
    public interface IOrganizeService
    {
        #region 获取数据
        /// <summary>
        /// 机构列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<OrganizeEntity> GetList();
        //IEnumerable<OrganizeEntity> GetListByIds();
        DataTable GetOrgCount(string parentId);
        IEnumerable<OrganizeEntity> GetParentIdByOrgId(string orgId);
        /// <summary>
        /// 机构实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        OrganizeEntity GetEntity(string keyValue);
        #endregion

        #region 验证数据
        /// <summary>
        /// 公司名称不能重复
        /// </summary>
        /// <param name="organizeName">公司名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistFullName(string fullName, string keyValue);
        /// <summary>
        /// 外文名称不能重复
        /// </summary>
        /// <param name="enCode">外文名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistEnCode(string enCode, string keyValue);
        /// <summary>
        /// 中文名称不能重复
        /// </summary>
        /// <param name="shortName">中文名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistShortName(string shortName, string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除机构
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存机构表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="organizeEntity">机构实体</param>
        /// <returns></returns>
        void SaveForm(string keyValue, OrganizeEntity organizeEntity);
        /// <summary>
        /// 审批下级申请加盟
        /// </summary>
        /// <param name="organizeEntity"></param>
        /// <returns></returns>
        OrganizeEntity SaveReturnEntity(OrganizeEntity organizeEntity);
        /// <summary>
        /// 修改机构状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="State">状态：1-启动；0-禁用</param>
        void UpdateState(string keyValue, int State);
        /// <summary>
        /// 修改机构协议状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="State">状态：1-同意；0-拒绝</param>
        void UpdateAgreementState(string keyValue, int State);

        /// <summary>
        /// DMS统计次数
        /// </summary>
        /// <param name="keyValue"></param>
        void UpdateSeeCount(string keyValue);
        void UpdateShareCount(string keyValue);
        void UpdateJoinCount(string keyValue);

        void AddAdmin(string keyValue);
        #endregion
    }
}
