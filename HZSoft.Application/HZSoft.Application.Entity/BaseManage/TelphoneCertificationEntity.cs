using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.BaseManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-10-12 17:30
    /// 描 述：TelphoneCertification
    /// </summary>
    public class TelphoneCertificationEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 唯一标识
        /// </summary>
        /// <returns></returns>
        public string ID { get; set; }
        /// <summary>
        /// 认证手机号
        /// </summary>
        /// <returns></returns>
        public string mobileNumber { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        /// <returns></returns>
        public string custName { get; set; }
        /// <summary>
        /// 客户电话
        /// </summary>
        /// <returns></returns>
        public string custTelphone { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        /// <returns></returns>
        public string custCertCode { get; set; }
        /// <summary>
        /// 身份证有效期
        /// </summary>
        /// <returns></returns>
        public string custCertDate { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        /// <returns></returns>
        public string custCertAddress { get; set; }
        /// <summary>
        /// 正面照
        /// </summary>
        /// <returns></returns>
        public string photo_z { get; set; }
        /// <summary>
        /// 反面照
        /// </summary>
        /// <returns></returns>
        public string photo_b { get; set; }
        /// <summary>
        /// 半身照
        /// </summary>
        /// <returns></returns>
        public string photo_s { get; set; }
        /// <summary>
        /// 提交标识
        /// </summary>
        /// <returns></returns>
        public int? loadMark { get; set; }
        /// <summary>
        /// 失败原因
        /// </summary>
        /// <returns></returns>
        public string description { get; set; }
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <returns></returns>
        public int? sendMessage { get; set; }
        /// <summary>
        /// 提交次数
        /// </summary>
        /// <returns></returns>
        public int? loadCount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime? createTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        public DateTime? modifyTime { get; set; }
        /// <summary>
        /// 微信openid
        /// </summary>
        /// <returns></returns>
        public string createId { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        /// <returns></returns>
        public string createName { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = Guid.NewGuid().ToString();
            loadCount = 1;
            loadMark = 0;//激活标识，0：未操作，1：成功，-1：失败
            createTime = DateTime.Now;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            modifyTime = DateTime.Now;
                                            }
        #endregion
    }
}