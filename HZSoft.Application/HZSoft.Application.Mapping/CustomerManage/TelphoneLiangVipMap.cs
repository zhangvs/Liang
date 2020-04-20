using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-10-17 21:56
    /// 描 述：VIP服务机构
    /// </summary>
    public class TelphoneLiangVipMap : EntityTypeConfiguration<TelphoneLiangVipEntity>
    {
        public TelphoneLiangVipMap()
        {
            #region 表、主键
            //表
            this.ToTable("TelphoneLiangVip");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
