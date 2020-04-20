using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-09-16 16:41
    /// 描 述：号码库
    /// </summary>
    public class TelphoneSourceMap : EntityTypeConfiguration<TelphoneSourceEntity>
    {
        public TelphoneSourceMap()
        {
            #region 表、主键
            //表
            this.ToTable("TelphoneSource");
            //主键
            this.HasKey(t => t.TelphoneID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
