using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-09-06 10:25
    /// 描 述：靓号分享记录
    /// </summary>
    public class TelphoneLiangShareEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// Id
        /// </summary>
        /// <returns></returns>
        public int? Id { get; set; }
        /// <summary>
        /// 微信标识
        /// </summary>
        /// <returns></returns>
        public string OpenId { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        /// <returns></returns>
        public string NickName { get; set; }
        /// <summary>
        /// 微信头像
        /// </summary>
        /// <returns></returns>
        public string HeadimgUrl { get; set; }
        /// <summary>
        /// 分享url
        /// </summary>
        /// <returns></returns>
        public string ShareUrl { get; set; }
        /// <summary>
        /// 分享内容
        /// </summary>
        /// <returns></returns>
        public string ShareContent { get; set; }
        /// <summary>
        /// 分享标题
        /// </summary>
        /// <returns></returns>
        public string ShareTitle { get; set; }
        /// <summary>
        /// 分享到哪里
        /// </summary>
        /// <returns></returns>
        public string ShareTo { get; set; }
        /// <summary>
        /// 分享机构
        /// </summary>
        /// <returns></returns>
        public string OrganizeId { get; set; }        
        /// <summary>
        /// 分享时间
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
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