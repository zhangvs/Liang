using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-10-26 09:09
    /// 描 述：靓号订单
    /// </summary>
    public class TelphoneLiOrderEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// ID
        /// </summary>
        /// <returns></returns>
        public string ID { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        /// <returns></returns>
        public string OrderCode { get; set; }
        /// <summary>
        /// 售出号码
        /// </summary>
        /// <returns></returns>
        public string Telphone { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        /// <returns></returns>
        public string SellerId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        /// <returns></returns>
        public string SellerName { get; set; }
        /// <summary>
        /// 售出金额
        /// </summary>
        /// <returns></returns>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 已付
        /// </summary>
        /// <returns></returns>
        public decimal? Paid { get; set; }
        /// <summary>
        /// 预付款转公司账号时间
        /// </summary>
        /// <returns></returns>
        public string PaidDate { get; set; }
        /// <summary>
        /// 尾款/全款
        /// </summary>
        /// <returns></returns>
        public decimal? ToPay { get; set; }
        /// <summary>
        /// 尾款/全款转公司账号时间
        /// </summary>
        /// <returns></returns>
        public string ToPayDate { get; set; }
        /// <summary>
        /// 到付服务费
        /// </summary>
        /// <returns></returns>
        public decimal? ToPayCharge { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        /// <returns></returns>
        public string Consignee { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        /// <returns></returns>
        public string Contact { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        /// <returns></returns>
        public string Address { get; set; }
        /// <summary>
        /// 快递
        /// </summary>
        /// <returns></returns>
        public string Express { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        /// <returns></returns>
        public string Numbers { get; set; }
        /// <summary>
        /// 是否签收
        /// </summary>
        /// <returns></returns>
        public int? Sign { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        /// <returns></returns>
        public int? ActivationMark { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        /// <returns></returns>
        public int? DeleteMark { get; set; }
        /// <summary>
        /// 有效标志
        /// </summary>
        /// <returns></returns>
        public int? EnabledMark { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>
        /// <returns></returns>
        public string OrganizeId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Description { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>
        /// <returns></returns>
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        public string ModifyUserName { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.OrganizeId = OperatorProvider.Provider.Current().CompanyId;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}