using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-09-06 10:25
    /// 描 述：靓号分享记录
    /// </summary>
    public class TelphoneLiangShareMap : EntityTypeConfiguration<TelphoneLiangShareEntity>
    {
        public TelphoneLiangShareMap()
        {
            #region 表、主键
            //表
            this.ToTable("TelphoneLiangShare");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
