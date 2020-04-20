using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-07-27 17:06
    /// 描 述：号码预定访问次数
    /// </summary>
    public class TelphoneReserveSeeMap : EntityTypeConfiguration<TelphoneReserveSeeEntity>
    {
        public TelphoneReserveSeeMap()
        {
            #region 表、主键
            //表
            this.ToTable("TelphoneReserveSee");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
