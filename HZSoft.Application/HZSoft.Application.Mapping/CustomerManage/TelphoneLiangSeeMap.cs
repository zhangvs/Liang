using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-08-23 16:22
    /// 描 述：靓号主页浏览
    /// </summary>
    public class TelphoneLiangSeeMap : EntityTypeConfiguration<TelphoneLiangSeeEntity>
    {
        public TelphoneLiangSeeMap()
        {
            #region 表、主键
            //表
            this.ToTable("TelphoneLiangSee");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
