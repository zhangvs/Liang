using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-04-16 09:07
    /// 描 述：订单表
    /// </summary>
    public class OrdersEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// Id
        /// </summary>
        /// <returns></returns>
        public int? Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        /// <returns></returns>
        public string OrderSn { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        /// <returns></returns>
        public string Host { get; set; }
        /// <summary>
        /// TelphoneID
        /// </summary>
        /// <returns></returns>
        public int? TelphoneID { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        /// <returns></returns>
        public string Tel { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        /// <returns></returns>
        public decimal? Price { get; set; }
        /// <summary>
        /// 发起时间
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 0 待付款 1 待发货 2 待开卡 3 已完成 
        /// </summary>
        /// <returns></returns>
        public int? Status { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        /// <returns></returns>
        public string PayType { get; set; }
        /// <summary>
        /// 0 未支付 1已支付
        /// </summary>
        /// <returns></returns>
        public int? PayStatus { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        /// <returns></returns>
        public DateTime? PayDate { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        /// <returns></returns>
        public string Receiver { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        /// <returns></returns>
        public string ContactTel { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        /// <returns></returns>
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        /// <returns></returns>
        public string City { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        /// <returns></returns>
        public string Country { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        /// <returns></returns>
        public string Address { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
        /// <summary>
        /// 发货人
        /// </summary>
        /// <returns></returns>
        public string DeliveryName { get; set; }
        /// <summary>
        /// 发货时间
        /// </summary>
        /// <returns></returns>
        public DateTime? DeliveryDate { get; set; }
        /// <summary>
        /// 快递公司
        /// </summary>
        /// <returns></returns>
        public string ExpressCompany { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        /// <returns></returns>
        public string ExpressSn { get; set; }
        /// <summary>
        /// 快递公司编号
        /// </summary>
        /// <returns></returns>
        public string ExpressCode { get; set; }
        /// <summary>
        /// 支付单号
        /// </summary>
        /// <returns></returns>
        public string PaySn { get; set; }
        /// <summary>
        /// 退款时间
        /// </summary>
        /// <returns></returns>
        public DateTime? ReturnDate { get; set; }
        /// <summary>
        /// 退款原因
        /// </summary>
        /// <returns></returns>
        public string ReturnRemark { get; set; }
        /// <summary>
        /// 退货编号
        /// </summary>
        /// <returns></returns>
        public string ReturnSn { get; set; }
        /// <summary>
        /// 0 手机（H5支付）  1 电脑（扫码Native支付），2微信浏览器（JSAPI）
        /// </summary>
        /// <returns></returns>
        public int? PC { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        /// <returns></returns>
        public int? DeleteMark { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.CreateDate = DateTime.Now;
            this.Status = 0;
            this.DeleteMark = 0;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(int? keyValue)
        {
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
        }
        #endregion
    }
}