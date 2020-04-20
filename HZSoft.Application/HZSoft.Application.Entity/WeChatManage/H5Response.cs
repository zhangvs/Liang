using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZSoft.Application.Entity.WeChatManage
{
    public class H5ResponseData
    {
        /// <summary>
        /// 
        /// </summary>
        public string html { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gotoUrl { get; set; }


        
    }

    public class H5Response
    {
        /// <summary>
        /// 
        /// </summary>
        public bool code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool status { get; set; }
        /// <summary>
        /// 操作成功
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public dynamic data { get; set; }
    }

    public class H5PayData
    {
        /// <summary>
        /// 
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string code_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string prepay_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string result_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string return_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string return_msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string trade_no { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string payid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string wx_query_href { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string wx_query_over { get; set; }
    }


    /// <summary>
    /// 订单状态:0 待付款 1 待审核 2 待发货 3 待开卡 4 已完成
    /// </summary>
    public enum OrderStatus
    {
        待付款 = 0,
        未发货 = 1,
        已发货 = 2,
        退款 = 3,
        已结束 = 4
    }

    /// <summary>
    /// 支付状态
    /// </summary>
    public enum PayStatus
    {
        未支付 = 0,
        已支付 = 1
    }
}
