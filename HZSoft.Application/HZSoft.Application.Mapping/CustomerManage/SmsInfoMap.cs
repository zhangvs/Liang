using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-10-30 20:25
    /// 描 述：短信验证码表
    /// </summary>
    public class SmsInfoMap : EntityTypeConfiguration<SmsInfoEntity>
    {
        public SmsInfoMap()
        {
            #region 表、主键
            //表
            this.ToTable("SmsInfo");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
