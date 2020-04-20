using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.WeChatManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-11-21 14:46
    /// 描 述：用户表
    /// </summary>
    public class WeChat_UsersEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 微信 用户的唯一标识
        /// </summary>
        /// <returns></returns>
        public string OpenId { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        /// <returns></returns>
        public string NickName { get; set; }
        /// <summary>
        /// 用户的性别，1男性，2女性，0未知
        /// </summary>
        /// <returns></returns>
        public int? Sex { get; set; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。
        /// </summary>
        /// <returns></returns>
        public string HeadimgUrl { get; set; }
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
        /// 微信公众号
        /// </summary>
        /// <returns></returns>
        public string AppName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 员工id
        /// </summary>
        /// <returns></returns>
        public string UserId { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        /// <returns></returns>
        public string UserName { get; set; }
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
        public override void Modify(string keyValue)
        {
            this.OpenId = keyValue;
                                            }
        #endregion
    }
}