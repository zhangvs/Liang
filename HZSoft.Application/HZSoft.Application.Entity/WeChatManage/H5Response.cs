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
        /// H5连接 <mweb_url><![CDATA[https://wx.tenpay.com/cgi-bin/mmpayweb-bin/checkmweb?prepay_id=wx0821504501009699fc47ba7d1821679000&package=3205204241]]></mweb_url>
        /// </summary>
        public string mweb_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string notify_url { get; set; }
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
        /// <summary>
        /// 
        /// </summary>
        public string form { get; set; }
    }


    /// <summary>
    /// 订单状态:0 待付款 1 待发货 2 待开卡 3 已完成 
    /// </summary>
    public enum OrderStatus
    {
        待付款 = 0,
        未发货 = 1,
        待开卡 = 2,
        已完成 = 4
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
