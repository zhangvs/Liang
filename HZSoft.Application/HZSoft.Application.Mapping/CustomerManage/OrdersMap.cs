using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-04-16 09:07
    /// 描 述：订单表
    /// </summary>
    public class OrdersMap : EntityTypeConfiguration<OrdersEntity>
    {
        public OrdersMap()
        {
            #region 表、主键
            //表
            this.ToTable("Orders");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
