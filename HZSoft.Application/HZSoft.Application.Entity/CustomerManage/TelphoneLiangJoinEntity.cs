using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-09-05 17:58
    /// 描 述：靓号加盟代理
    /// </summary>
    public class TelphoneLiangJoinEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        /// <returns></returns>
        public string CompanyName { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        /// <returns></returns>
        public string FullName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        /// <returns></returns>
        public string Telphone { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        /// <returns></returns>
        public string Pro { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        /// <returns></returns>
        public string City { get; set; }
        /// <summary>
        /// 区县
        /// </summary>
        /// <returns></returns>
        public string Area { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        /// <returns></returns>
        public string Address { get; set; }
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
        /// 微信性别
        /// </summary>
        /// <returns></returns>
        public int? Sex { get; set; }
        /// <summary>
        /// 微信头像
        /// </summary>
        /// <returns></returns>
        public string HeadimgUrl { get; set; }
        /// <summary>
        /// 微信省
        /// </summary>
        /// <returns></returns>
        public string WXPro { get; set; }
        /// <summary>
        /// 微信市
        /// </summary>
        /// <returns></returns>
        public string WxCity { get; set; }
        /// <summary>
        /// 微信账号
        /// </summary>
        /// <returns></returns>
        public string WxAccount { get; set; }
        /// <summary>
        /// 微信二维码
        /// </summary>
        /// <returns></returns>
        public string WxQRcode { get; set; }
        /// <summary>
        /// 机构来源
        /// </summary>
        /// <returns></returns>
        public string OrganizeId { get; set; }
        /// <summary>
        /// 有靓号，成为供应商
        /// </summary>
        /// <returns></returns>
        public int? BoosMark { get; set; }
        /// <summary>
        /// 现有靓号数量
        /// </summary>
        /// <returns></returns>
        public int? LiangCount { get; set; }
        /// <summary>
        /// 没有靓号，成为代理
        /// </summary>
        /// <returns></returns>
        public int? AgentMark { get; set; }
        /// <summary>
        /// 0级标识
        /// </summary>
        /// <returns></returns>
        public int? TopMark { get; set; }
        /// <summary>
        /// 审核标识
        /// </summary>
        /// <returns></returns>
        public int? CheckMark { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        /// <returns></returns>
        public int? DeleteMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Description { get; set; }
        /// <summary>
        /// 跟进详情
        /// </summary>
        /// <returns></returns>
        public string FollowDes { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyTime { get; set; }
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
        /// <summary>
        /// beiz
        /// </summary>
        /// <returns></returns>
        public string Des1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Des2 { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = Guid.NewGuid().ToString();
            CreateTime = DateTime.Now;
            CheckMark = 0;
            DeleteMark = 0;
            TopMark = 0;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
            ModifyTime = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}