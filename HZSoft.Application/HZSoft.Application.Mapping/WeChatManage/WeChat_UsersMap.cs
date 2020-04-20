using HZSoft.Application.Entity.WeChatManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.WeChatManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-11-21 14:46
    /// 描 述：用户表
    /// </summary>
    public class WeChat_UsersMap : EntityTypeConfiguration<WeChat_UsersEntity>
    {
        public WeChat_UsersMap()
        {
            #region 表、主键
            //表
            this.ToTable("WeChat_Users");
            //主键
            this.HasKey(t => t.OpenId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
