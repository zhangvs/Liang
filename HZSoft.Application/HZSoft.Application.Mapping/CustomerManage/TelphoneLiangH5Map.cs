using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-06-07 14:11
    /// 描 述：头条靓号库
    /// </summary>
    public class TelphoneLiangH5Map : EntityTypeConfiguration<TelphoneLiangH5Entity>
    {
        public TelphoneLiangH5Map()
        {
            #region 表、主键
            //表
            this.ToTable("TelphoneLiangH5");
            //主键
            this.HasKey(t => t.TelphoneID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
