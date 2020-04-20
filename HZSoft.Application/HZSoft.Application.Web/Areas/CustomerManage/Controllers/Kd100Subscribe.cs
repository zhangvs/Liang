using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HZSoft.Application.Web.Areas.CustomerManage.Controllers
{
    public class Parameters
    {
        /// <summary>
        /// 您的回调接口的地址，如http://www.您的域名.com/kuaidi?callbackid=...
        /// </summary>
        public string callbackurl { get; set; }

        /// <summary>
        /// 签名用随机字符串
        /// </summary>
        public string salt { get; set; }

        /// <summary>
        /// 添加此字段表示开通行政区域解析功能（仅对开通签收状态服务用户有效）
        /// </summary>
        public string resultv2 { get; set; }

        /// <summary>
        /// 添加此字段且将此值设为1，则表示开始智能判断单号所属公司的功能，开启后，company字段可为空,即只传运单号（number字段），我方收到后会根据单号判断出其所属的快递公司（即company字段）。建议只有在无法知道单号对应的快递公司（即company的值）的情况下才开启此功能
        /// </summary>
        public string autoCom { get; set; }

        /// <summary>
        /// 添加此字段表示开启国际版，开启后，若订阅的单号（即number字段）属于国际单号，会返回出发国与目的国两个国家的跟踪信息，本功能暂时只支持邮政体系（国际类的邮政小包、EMS）内的快递公司，若单号我方识别为非国际单，即使添加本字段，也不会返回destResult元素组
        /// </summary>
        public string interCom { get; set; }

        /// <summary>
        /// 	出发国家编码
        /// </summary>
        public string departureCountry { get; set; }

        /// <summary>
        /// 	出发的快递公司的编码
        /// </summary>
        public string departureCom { get; set; }

        /// <summary>
        /// 目的国家编码
        /// </summary>
        public string destinationCountry { get; set; }

        /// <summary>
        /// 目的的快递公司的编码
        /// </summary>
        public string destinationCom { get; set; }

    }



    public class Kd100Subscribe
    {
        /// <summary>
        /// 快递公司的编码
        /// </summary>
        public string company { get; set; }

        /// <summary>
        /// 订阅的快递单号
        /// </summary>
        public string number { get; set; }

        /// <summary>
        /// 广东省深圳市南山区
        /// </summary>
        public string from { get; set; }

        /// <summary>
        /// 北京市朝阳区
        /// </summary>
        public string to { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Parameters parameters { get; set; }

    }


}