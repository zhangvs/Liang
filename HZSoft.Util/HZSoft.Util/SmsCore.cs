using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZSoft.Util
{
    /// <summary>
    /// 发送短信核心
    /// </summary>
    public class SmsCore
    {
        /// <summary>
        /// 获得纯数字验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetNumberCode(int length = 6)
        {
            string code = "";
            Random rnd = new Random();
            for (int i = 0; i < length; i++)
            {
                code += rnd.Next(0, 9);
            }
            return code;
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool SendSms(string phone, string code)
        {
            try
            {
                SmsSingleSenderResult singleResult;
                SmsSingleSender singleSender = new SmsSingleSender(SmsConfig.APPID, SmsConfig.APPKEY);


                List<string> templParams = new List<string>();
                templParams.Add(code);
                // 指定模板单发
                singleResult = singleSender.SendWithParam("86", phone, SmsConfig.TMPLID, templParams, "", "", "");

                if (singleResult.result == 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
/// <summary>
/// 短信配置表
/// </summary>
public class SmsConfig
{
    public const int APPID = 1400040861;
    public const string APPKEY = "a92c87d0d291698777a9b5f323c0388a";
    public const int TMPLID = 43313;
}