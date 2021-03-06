﻿using HZSoft.Application.Entity.FlowManage;
using System.Data.Entity.ModelConfiguration;
namespace HZSoft.Application.Mapping.FlowManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创建人：陈彬彬
    /// 日 期：2016.03.18 09:58
    /// 描 述：工作流委托规则与工作流模板对应表
    /// </summary>
    public class WFDelegateRuleSchemeInfoMap : EntityTypeConfiguration<WFDelegateRuleSchemeInfoEntity>
    {
        public WFDelegateRuleSchemeInfoMap()
        {
            #region 表、主键
            //表
            this.ToTable("WF_DelegateRuleSchemeInfo");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
