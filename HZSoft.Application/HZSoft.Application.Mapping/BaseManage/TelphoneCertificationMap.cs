using HZSoft.Application.Entity.BaseManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.BaseManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-10-12 17:30
    /// 描 述：TelphoneCertification
    /// </summary>
    public class TelphoneCertificationMap : EntityTypeConfiguration<TelphoneCertificationEntity>
    {
        public TelphoneCertificationMap()
        {
            #region 表、主键
            //表
            this.ToTable("TelphoneCertification");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
