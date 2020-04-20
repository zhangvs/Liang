using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HZSoft.Application.Web.Areas.CustomerManage.Controllers
{
    public class Kd100SubscribeDemo
    {
        //电商加密私钥，快递鸟提供，注意保管，不要泄漏
        private string Key = "tEstynxGxGuN4344";
        private string CallBackUrl = "http://hmk.lywenkai.com/CustomerManage/KD100API/SavePushRecord";
        //测试请求url
        //private string ReqURL = "https://poll.kuaidi100.com/test/poll";
        //正式请求url
        private string ReqURL = "https://poll.kuaidi100.com/poll";

        public String output;
        /// <summary>
        /// Json方式  物流信息订阅
        /// </summary>
        /// <returns></returns>
        public void orderTracesSubByJson(string company,string number)
        {
            System.Net.WebClient WebClientObj = new System.Net.WebClient();
            System.Collections.Specialized.NameValueCollection PostVars =
                    new System.Collections.Specialized.NameValueCollection();

            String param = "";
            param += "{";
            param += "\"company\":\""+ company + "\",";
            param += "\"number\":\"" + number + "\",";
            param += "\"key\":\""+ Key + "\",";
            param += "\"parameters\":{\"callbackurl\":\""+ CallBackUrl + "\"}";
            param += "}";

            PostVars.Add("schema", "json");
            PostVars.Add("param", param);

            try
            {
                byte[] byRemoteInfo = WebClientObj.UploadValues(ReqURL, "POST", PostVars);
                output = System.Text.Encoding.UTF8.GetString(byRemoteInfo);
                //注意返回的信息，只有result=true的才是成功
            }
            catch
            {

            }
        }
    }


}