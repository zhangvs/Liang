using System;
using HZSoft.Application.Code;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// 版 本
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-07-28 10:05
    /// 描 述：预定查询记录
    /// </summary>
    public class TelphoneReserveSearchEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; }
        /// <summary>
        /// 查询号码
        /// </summary>
        /// <returns></returns>
        public string SearchNumber { get; set; }
        /// <summary>
        /// 访问Ip
        /// </summary>
        /// <returns></returns>
        public string IPAddress { get; set; }
        /// <summary>
        /// 访问Ip地区
        /// </summary>
        /// <returns></returns>
        public string IPAddressName { get; set; }
        /// <summary>
        /// 查询时间
        /// </summary>
        /// <returns></returns>
        public DateTime? SearchDate { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = Guid.NewGuid().ToString();
            this.SearchDate = DateTime.Now;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
                                            }
        #endregion
    }
}