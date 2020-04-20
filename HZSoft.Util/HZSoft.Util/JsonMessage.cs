using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HZSoft.Util
{
    /// <summary>
    /// 返回消息
    /// </summary>
    public class JsonMessage
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 结果编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 结果消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        public string sthid { get; set; }
        public override string ToString()
        {
            return Json.ToJson(this);
        }
    }
}

