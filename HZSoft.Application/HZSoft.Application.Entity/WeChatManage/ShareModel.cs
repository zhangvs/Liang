using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZSoft.Application.Entity.WeChatManage
{
    public class ShareModel : BaseWxModel
    {
        /// <summary>
        /// 分享图片
        /// </summary>
        public string fxImg { get; set; }
        /// <summary>
        /// 当前URL
        /// </summary>
        public string thisUrl { get; set; }
        /// <summary>
        /// 分享链接
        /// </summary>
        public string fxUrl { get; set; }
        /// <summary>
        /// 分享标题
        /// </summary>
        public string fxTitle { get; set; }
        /// <summary>
        /// 分享内容
        /// </summary>
        public string fxContent { get; set; }
    }
}
