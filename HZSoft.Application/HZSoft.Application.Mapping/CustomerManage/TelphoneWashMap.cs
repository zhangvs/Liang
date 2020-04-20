using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-09-22 16:27
    /// 描 述：洗号池
    /// </summary>
    public class TelphoneWashMap : EntityTypeConfiguration<TelphoneWashEntity>
    {
        public TelphoneWashMap()
        {
            #region 表、主键
            //表
            this.ToTable("TelphoneWash");
            //主键
            this.HasKey(t => t.TelphoneID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
