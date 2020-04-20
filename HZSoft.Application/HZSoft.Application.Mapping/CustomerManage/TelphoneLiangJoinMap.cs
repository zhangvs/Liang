using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-09-05 17:58
    /// 描 述：靓号加盟代理
    /// </summary>
    public class TelphoneLiangJoinMap : EntityTypeConfiguration<TelphoneLiangJoinEntity>
    {
        public TelphoneLiangJoinMap()
        {
            #region 表、主键
            //表
            this.ToTable("TelphoneLiangJoin");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
