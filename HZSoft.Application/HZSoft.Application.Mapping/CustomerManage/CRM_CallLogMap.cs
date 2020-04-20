using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-11-04 16:22
    /// 描 述：话单
    /// </summary>
    public class CRM_CallLogMap : EntityTypeConfiguration<CRM_CallLogEntity>
    {
        public CRM_CallLogMap()
        {
            #region 表、主键
            //表
            this.ToTable("CRM_CallLog");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
