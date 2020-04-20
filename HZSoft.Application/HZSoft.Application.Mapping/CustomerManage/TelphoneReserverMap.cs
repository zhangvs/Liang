using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-07-25 11:57
    /// 描 述：预约号码
    /// </summary>
    public class TelphoneReserverMap : EntityTypeConfiguration<TelphoneReserverEntity>
    {
        public TelphoneReserverMap()
        {
            #region 表、主键
            //表
            this.ToTable("TelphoneReserver");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
