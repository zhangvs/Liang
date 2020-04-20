using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace HZSoft.Application.Web.Utility
{
    public class WebSiteConfig
    {
        /// <summary>
        /// 微信用户Session
        /// </summary>
        public const string WXUSER_SESSION_NAME = "_WXUSER_SESSION_NAME";
        /// <summary>
        /// 用户网页授权access_token
        /// </summary>
        public const string WXTOKEN_SESSION_NAME = "_WXTOKEN_SESSION_NAMEE";
        /// <summary>
        /// 基础接口的token 
        /// </summary>
        public const string WXTOKEN_SESSION_NAME_BASE = "_WXTOKEN_SESSION_NAME_BASE";

        /// <summary>
        /// 短信过期分钟
        /// </summary>
        public const int SMS_EXPIRE_MIN = 2;

        /// <summary>
        /// 每天短信发送次数
        /// </summary>
        public const int SMS_MAX_COUNT = 6;

        /// <summary>
        /// 匹配号码位数
        /// </summary>
        public const string MATCHING_TELLENGTH = "MatchingTelLength";
        /// <summary>
        /// 目标客户 0539 6539
        /// </summary>
        public const string TARGET_CUSTOMER = "TargetCustomer";
        /// <summary>
        /// 目标客户 砍价范围
        /// </summary>
        public const string TARGET_BARGAIN = "TargetBargain";
        /// <summary>
        /// 非目标客户砍价范围
        /// </summary>
        public const string COMMON_BARGAIN = "CommonBargain";
        /// <summary>
        /// 期初价
        /// </summary>
        public const string START_PRICE = "StartPrice";
        /// <summary>
        /// 最低价
        /// </summary>
        public const string LOW_PRICE = "LowPrice";
        /// <summary>
        /// 活动时间
        /// </summary>
        public const string ACTIVITY_TIME = "ActivityTime";
        /// <summary>
        /// 活动规则
        /// </summary>
        public const string ACTIVITY_RULE = "ActivityRule";

        private static object locker = new object();
        private static volatile int count = 0;

    }
}