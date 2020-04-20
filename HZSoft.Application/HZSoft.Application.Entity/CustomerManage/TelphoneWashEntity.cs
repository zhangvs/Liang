using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-09-22 16:27
    /// 描 述：洗号池
    /// </summary>
    public class TelphoneWashEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        public int? TelphoneID { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        /// <returns></returns>
        public string Telphone { get; set; }
        /// <summary>
        /// 号段
        /// </summary>
        /// <returns></returns>
        public string Number { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        /// <returns></returns>
        public string Grade { get; set; }
        /// <summary>
        /// 分配
        /// </summary>
        /// <returns></returns>
        public int? AssignMark { get; set; }
        /// <summary>
        /// 分配人Id
        /// </summary>
        /// <returns></returns>
        public string SellerId { get; set; }
        /// <summary>
        /// 分配人
        /// </summary>
        /// <returns></returns>
        public string SellerName { get; set; }
        /// <summary>
        /// 获取日期
        /// </summary>
        /// <returns></returns>
        public DateTime? ObtainDate { get; set; }
        /// <summary>
        /// 呼叫时间
        /// </summary>
        /// <returns></returns>
        public DateTime? CallTime { get; set; }
        /// <summary>
        /// 未接通方式
        /// </summary>
        /// <returns></returns>
        public int? NoConnectMark { get; set; }
        /// <summary>
        /// 不成交理由
        /// </summary>
        /// <returns></returns>
        public int? NoDealMark { get; set; }
        /// <summary>
        /// 贵姓
        /// </summary>
        /// <returns></returns>
        public string Surname { get; set; }
        /// <summary>
        /// 性别标识
        /// </summary>
        /// <returns></returns>
        public int? SexMark { get; set; }
        /// <summary>
        /// 年龄范围标识
        /// </summary>
        /// <returns></returns>
        public int? AgeMark { get; set; }
        /// <summary>
        /// 三区九县标识
        /// </summary>
        /// <returns></returns>
        public int? AreaMark { get; set; }
        /// <summary>
        /// 语气
        /// </summary>
        /// <returns></returns>
        public int? ToneMark { get; set; }
        /// <summary>
        /// 意向客户
        /// </summary>
        /// <returns></returns>
        public int? IntentionMark { get; set; }
        /// <summary>
        /// 售出
        /// </summary>
        /// <returns></returns>
        public int? SellMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string CallDescription { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        /// <returns></returns>
        public int? SortCode { get; set; }
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
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(int? keyValue)
        {
            this.TelphoneID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}