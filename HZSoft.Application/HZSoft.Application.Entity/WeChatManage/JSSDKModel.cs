using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZSoft.Application.Entity.WeChatManage
{
    public class JSSDKModel
    {
        public string appId { set; get; }
        public string timestamp { set; get; }
        public string nonceStr { set; get; }
        public string jsapiTicket { set; get; }
        public string signature { set; get; }
        public string shareUrl { set; get; }
        public string shareImg { set; get; }
        public string string1 { set; get; }
    }
}
