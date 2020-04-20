using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-10-09 20:14
    /// 描 述：代售靓号库
    /// </summary>
    public class TelphoneLiangOtherMap : EntityTypeConfiguration<TelphoneLiangOtherEntity>
    {
        public TelphoneLiangOtherMap()
        {
            #region 表、主键
            //表
            this.ToTable("TelphoneLiangOther");
            //主键
            this.HasKey(t => t.TelphoneID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
