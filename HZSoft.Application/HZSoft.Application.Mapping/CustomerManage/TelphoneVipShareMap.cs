using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-03-05 10:43
    /// 描 述：共享机构表
    /// </summary>
    public class TelphoneVipShareMap : EntityTypeConfiguration<TelphoneVipShareEntity>
    {
        public TelphoneVipShareMap()
        {
            #region 表、主键
            //表
            this.ToTable("TelphoneVipShare");
            //主键
            this.HasKey(t => t.VipShareId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
