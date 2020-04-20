using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-10-23 14:11
    /// 描 述：靓号库
    /// </summary>
    public class TelphoneLiangMap : EntityTypeConfiguration<TelphoneLiangEntity>
    {
        public TelphoneLiangMap()
        {
            #region 表、主键
            //表
            this.ToTable("TelphoneLiang");
            //主键
            this.HasKey(t => t.TelphoneID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
