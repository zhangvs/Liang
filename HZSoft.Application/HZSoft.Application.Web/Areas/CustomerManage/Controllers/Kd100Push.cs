using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HZSoft.Application.Web.Areas.CustomerManage.Controllers
{
    public class LastResultDataItem
    {
        /// <summary>
        /// 上海分拨中心/装件入车扫描 
        /// </summary>
        public string context { get; set; }

        /// <summary>
        /// 时间，原始格式
        /// </summary>
        public string time { get; set; }

        /// <summary>
        /// 格式化后时间
        /// </summary>
        public string ftime { get; set; }

        /// <summary>
        /// 在途  本数据元对应的签收状态。只有在开通签收状态服务（见上面"status"后的说明）且在订阅接口中提交resultv2标记后才会出现
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 本数据元对应的行政区域的编码，只有在开通签收状态服务（见上面"status"后的说明）且在订阅接口中提交resultv2标记后才会出现
        /// </summary>
        public string areaCode { get; set; }

        /// <summary>
        /// 上海市 本数据元对应的行政区域的名称，开通签收状态服务（见上面"status"后的说明）且在订阅接口中提交resultv2标记后才会出现
        /// </summary>
        public string areaName { get; set; }

    }



    public class LastResult
    {
        /// <summary>
        /// 消息体，请忽略
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 快递单当前签收状态，包括0在途中、1已揽收、2疑难、3已签收、4退签、5同城派送中、6退回、7转单等7个状态，其中4-7需要另外开通才有效
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// 通讯状态，请忽略
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 快递单明细状态标记，暂未实现，请忽略
        /// </summary>
        public string condition { get; set; }

        /// <summary>
        /// 是否签收标记
        /// </summary>
        public string ischeck { get; set; }

        /// <summary>
        /// 快递公司编码,一律用小写字母
        /// </summary>
        public string com { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string nu { get; set; }

        /// <summary>
        /// 数组，包含多个对象，每个对象字段如展开所示
        /// </summary>
        public List<LastResultDataItem> data { get; set; }

    }



    public class DestResultDataItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string context { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ftime { get; set; }

        /// <summary>
        /// 签收
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string areaCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string areaName { get; set; }

    }



    public class DestResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string condition { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ischeck { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string com { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string nu { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<DestResultDataItem> data { get; set; }

    }



    public class Kd100Push
    {
        /// <summary>
        /// 监控状态:polling:监控中，shutdown:结束，abort:中止，updateall：重新推送。其中当快递单为已签收时status=shutdown，当message为“3天查询无记录”或“60天无变化时”status= abort ，对于stuatus=abort的状度，需要增加额外的处理逻辑
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 包括got、sending、check三个状态，由于意义不大，已弃用，请忽略
        /// </summary>
        public string billstatus { get; set; }

        /// <summary>
        /// 监控状态相关消息，如:3天查询无记录，60天无变化
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 	快递公司编码是否出错，0为本推送信息对应的是贵司提交的原始快递公司编码，1为本推送信息对应的是我方纠正后的新的快递公司编码
        /// </summary>
        public string autoCheck { get; set; }

        /// <summary>
        /// 贵司提交的原始的快递公司编码。
        /// </summary>
        public string comOld { get; set; }

        /// <summary>
        /// 我司纠正后的新的快递公司编码。
        /// </summary>
        public string comNew { get; set; }

        /// <summary>
        /// 最新查询结果，若在订阅报文中通过interCom字段开通了国际版，则此lastResult表示出发国的查询结果，全量，倒序（即时间最新的在最前）
        /// </summary>
        public LastResult lastResult { get; set; }

        /// <summary>
        /// 表示最新的目的国家的查询结果，只有在订阅报文中通过interCom=1字段开通了国际版才会显示此数据元，全量，倒序（即时间最新的在最前）
        /// </summary>
        public DestResult destResult { get; set; }

    }
}