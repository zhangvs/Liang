using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-03-05 10:43
    /// 描 述：共享机构表
    /// </summary>
    public class TelphoneVipShareEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 共享机构主键
        /// </summary>
        /// <returns></returns>
        public string VipShareId { get; set; }
        /// <summary>
        /// 机构主键
        /// </summary>
        /// <returns></returns>
        public string VipId { get; set; }
        /// <summary>
        /// 共享机构主键
        /// </summary>
        /// <returns></returns>
        public string ShareId { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        /// <returns></returns>
        public int? SortCode { get; set; }
        /// <summary>
        /// 创建时间
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
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.VipShareId = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.VipShareId = keyValue;
                                            }
        #endregion
    }
}