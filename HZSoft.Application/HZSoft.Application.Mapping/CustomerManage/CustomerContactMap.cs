﻿using HZSoft.Application.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace HZSoft.Application.Mapping.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：佘赐雄
    /// 日 期：2016-03-19 14:25
    /// 描 述：客户联系人
    /// </summary>
    public class CustomerContactMap : EntityTypeConfiguration<CustomerContactEntity>
    {
        public CustomerContactMap()
        {
            #region 表、主键
            //表
            this.ToTable("Client_CustomerContact");
            //主键
            this.HasKey(t => t.CustomerContactId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}