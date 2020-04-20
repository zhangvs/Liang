using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-07-28 10:05
    /// 描 述：预定查询记录
    /// </summary>
    public class TelphoneReserveSearchMap : EntityTypeConfiguration<TelphoneReserveSearchEntity>
    {
        public TelphoneReserveSearchMap()
        {
            #region 表、主键
            //表
            this.ToTable("TelphoneReserveSearch");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
