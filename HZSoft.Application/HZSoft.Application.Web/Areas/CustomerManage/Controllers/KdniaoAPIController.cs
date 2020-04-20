using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using System.Web.Mvc;
using System;
using System.Collections.Generic;

namespace HZSoft.Application.Web.Areas.CustomerManage.Controllers
{
    public class KdniaoAPIController : Controller
    {
        private TelphoneOrderBLL telphoneorderbll = new TelphoneOrderBLL();


        #region 提交数据
        /// <summary>
        /// 推送接口
        /// </summary>
        /// <param name="requestType">101-轨迹查询结果</param>
        /// <param name="requestData">请求内容需进行URL(utf-8)编码。请求内容只支持JSON格式。</param>
        /// <param name="DataSign">	数据内容签名（把(请求内容(未编码)+AppKey)进行MD5加密，然后Base64编码）</param>
        /// <returns></returns>
        public ActionResult SavePushRecord(string requestData, string requestType, string DataSign)
        {
            string url = Request.Url.ToString();
            WriteInLog log = new WriteInLog();
            log.writeInLog(url);
            log.writeInLog(requestData);
            try
            {
                TracesPushRecord entity = Newtonsoft.Json.JsonConvert.DeserializeObject<TracesPushRecord>(requestData);
                log.writeInLog("序列化完毕");
                //保存跟进记录
                //telphoneorderbll.SavePushRecord(entity);
                
                foreach (var item in entity.Data)
                {
                    TelphoneOrderEntity telOrderEntity = telphoneorderbll.GetEntityByNu(item.LogisticCode);
                    if (telOrderEntity!=null)
                    {
                        telOrderEntity.Sign = item.State;//0-无轨迹，1-已揽收，2-在途中，3-签收,4-问题件   
                        var ItemTraces = item.Traces[item.Traces.Count - 1];//最后一个为最新状态
                        telOrderEntity.AcceptTime = ItemTraces.AcceptTime;
                        telOrderEntity.AcceptStation = ItemTraces.AcceptStation;
                        //保存物流状态
                        telphoneorderbll.SaveStateForm(telOrderEntity.ID, telOrderEntity);
                    }
                }
                return Content(new PushReturnMessage { EBusinessID = "1363273", UpdateTime = DateTime.Now, Success = true, Reason = "成功" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new PushReturnMessage { EBusinessID = "1363273", UpdateTime = DateTime.Now, Success = false, Reason = "写入数据库失败" }.ToString());
            }
        }
        #endregion

    }
}
