using HZSoft.Application.Entity.ReportManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.ReportManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2019-09-08 19:35
    /// 描 述：Dms_Cai
    /// </summary>
    public class Dms_CaiMap : EntityTypeConfiguration<Dms_CaiEntity>
    {
        public Dms_CaiMap()
        {
            #region 表、主键
            //表
            this.ToTable("Dms_Cai");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
