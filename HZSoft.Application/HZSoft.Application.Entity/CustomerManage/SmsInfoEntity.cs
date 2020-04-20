using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-10-30 20:25
    /// 描 述：短信验证码表
    /// </summary>
    public class SmsInfoEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// Id
        /// </summary>
        /// <returns></returns>
        public int? Id { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        /// <returns></returns>
        public string Tel { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public string Captcha { get; set; }
        /// <summary>
        /// 发送次数
        /// </summary>
        /// <returns></returns>
        public int? SendCount { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        public int? Status { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        /// <returns></returns>
        public int? Type { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        /// <returns></returns>
        public string CreateName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 短信内容
        /// </summary>
        /// <returns></returns>
        public string SmsContent { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.CreateDate = DateTime.Now;
                                }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(int? keyValue)
        {
            this.Id = keyValue;
                                            }
        #endregion
    }
}