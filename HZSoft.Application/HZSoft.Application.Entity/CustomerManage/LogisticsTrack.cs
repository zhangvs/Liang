using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZSoft.Application.Entity.CustomerManage
{
    /// <summary>
    /// 轨迹推送记录
    /// </summary>
    public class TracesPushRecord
    {
        /// <summary>
        /// 用户电商ID
        /// </summary>
        public string EBusinessID { get; set; }
        /// <summary>
        /// 推送物流单号轨迹个数
        /// </summary>
        public string Count { get; set; }
        /// <summary>
        /// 推送时间
        /// </summary>
        public string PushTime { get; set; }
        /// <summary>
        /// 推送物流单号轨迹集合
        /// </summary>
        public List<TracesData> Data { get; set; }
    }
    /// <summary>
    /// 轨迹单号数据
    /// </summary>
    public class TracesData
    {
        /// <summary>
        /// 快递单号(主键)
        /// </summary>
        public string LogisticCode { get; set; }
        /// <summary>
        /// 商户ID
        /// </summary>
        public string EBusinessID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 快递公司编码
        /// </summary>
        public string ShipperCode { get; set; }
        /// <summary>
        /// 成功与否：true,false
        /// </summary>
        public string Success { get; set; }
        /// <summary>
        /// 失败原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 物流状态: 0-无轨迹，1-已揽收，2-在途中，3-签收,4-问题件
        /// </summary>
        public int? State { get; set; }
        /// <summary>
        /// 订阅接口的Bk值
        /// </summary>
        public string CallBack { get; set; }
        /// <summary>
        /// 踪迹列表
        /// </summary>
        public List<TracesItem> Traces { get; set; }
    }
    /// <summary>
    /// 轨迹详情
    /// </summary>
    public class TracesItem
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string AcceptTime { get; set; }
        /// <summary>
        /// 深圳市横岗速递营销部已收件，（揽投员姓名：钟定基;联系电话：）
        /// </summary>
        public string AcceptStation { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }


    ///// <summary>
    ///// 轨迹推送记录
    ///// </summary>
    //public class TracesPushRecordEntity
    //{
    //    /// <summary>
    //    /// 推送ID
    //    /// </summary>
    //    public string Id { get; set; }
    //    /// <summary>
    //    /// 用户电商ID
    //    /// </summary>
    //    public string EBusinessID { get; set; }
    //    /// <summary>
    //    /// 推送物流单号轨迹个数
    //    /// </summary>
    //    public string Count { get; set; }
    //    /// <summary>
    //    /// 推送时间
    //    /// </summary>
    //    public string PushTime { get; set; }
    //}

    ///// <summary>
    ///// 轨迹单号数据
    ///// </summary>
    //public class TracesDataEntity
    //{
    //    /// <summary>
    //    /// 快递单号(主键)
    //    /// </summary>
    //    public string LogisticCode { get; set; }
    //    /// <summary>
    //    /// 推送ID
    //    /// </summary>
    //    public string PushId { get; set; }
    //    /// <summary>
    //    /// 商户ID
    //    /// </summary>
    //    public string EBusinessID { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string OrderCode { get; set; }
    //    /// <summary>
    //    /// 快递公司编码
    //    /// </summary>
    //    public string ShipperCode { get; set; }
    //    /// <summary>
    //    /// 成功与否：true,false
    //    /// </summary>
    //    public string Success { get; set; }
    //    /// <summary>
    //    /// 失败原因
    //    /// </summary>
    //    public string Reason { get; set; }
    //    /// <summary>
    //    /// 物流状态: 0-无轨迹，1-已揽收，2-在途中，3-签收,4-问题件
    //    /// </summary>
    //    public int? State { get; set; }
    //    /// <summary>
    //    /// 订阅接口的Bk值
    //    /// </summary>
    //    public string CallBack { get; set; }
    //}

    ///// <summary>
    ///// 轨迹详情
    ///// </summary>
    //public class TracesItemEntity
    //{
    //    /// <summary>
    //    /// 快递单号(主键)
    //    /// </summary>
    //    public string LogisticCode { get; set; }
    //    /// <summary>
    //    /// 时间
    //    /// </summary>
    //    public string AcceptTime { get; set; }
    //    /// <summary>
    //    /// 深圳市横岗速递营销部已收件，（揽投员姓名：钟定基;联系电话：）
    //    /// </summary>
    //    public string AcceptStation { get; set; }
    //    /// <summary>
    //    /// 备注
    //    /// </summary>
    //    public string Remark { get; set; }
    //}

}
