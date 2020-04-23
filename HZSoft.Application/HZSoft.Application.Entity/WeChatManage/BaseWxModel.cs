using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZSoft.Application.Entity.WeChatManage
{
    public class BaseWxModel
    {
        /// <summary>
        /// 公众号的唯一标识
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; }
        /// <summary>
        /// 生成签名的随机串
        /// </summary>
        public string nonce { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string signature { get; set; }

        /// <summary>
        /// 当前URL
        /// </summary>
        public string thisUrl { get; set; }


        public string Package { get; set; }
    }


    public class BaseWxModel2
    {
        /// <summary>
        /// 公众号的唯一标识
        /// </summary>
        public string appId { get; set; }
        /// <summary>
        /// 生成签名的随机串
        /// </summary>
        public string nonceStr { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; }
        /// <summary>
        /// 当前URL
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string signature { get; set; }
        public string rawString { get; set; }
    }

    public class WFTWxModel
    {
        /// <summary>
        /// 公众号的唯一标识
        /// </summary>
        public string appId { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string timeStamp { get; set; }
        /// <summary>
        /// 生成签名的随机串
        /// </summary>
        public string nonceStr { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string paySign { get; set; }

        /// <summary>
        /// 微信签名方式
        /// </summary>
        public string signType { get; set; }

        /// <summary>
        /// 当前URL
        /// </summary>
        public string callback_url { get; set; }

        public string package { get; set; }
    }
}
