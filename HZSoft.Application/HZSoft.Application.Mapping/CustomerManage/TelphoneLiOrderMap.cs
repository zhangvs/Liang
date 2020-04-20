using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-10-26 09:09
    /// 描 述：靓号订单
    /// </summary>
    public class TelphoneLiOrderMap : EntityTypeConfiguration<TelphoneLiOrderEntity>
    {
        public TelphoneLiOrderMap()
        {
            #region 表、主键
            //表
            this.ToTable("TelphoneLiOrder");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
