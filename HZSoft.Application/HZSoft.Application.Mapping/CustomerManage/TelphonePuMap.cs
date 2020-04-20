using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-11-06 20:39
    /// 描 述：普号库
    /// </summary>
    public class TelphonePuMap : EntityTypeConfiguration<TelphonePuEntity>
    {
        public TelphonePuMap()
        {
            #region 表、主键
            //表
            this.ToTable("TelphonePu");
            //主键
            this.HasKey(t => t.TelphoneID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
