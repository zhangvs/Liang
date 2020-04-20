using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using System.Web.Mvc;
using System;


namespace HZSoft.Application.Web.Areas.CustomerManage.Controllers
{
    public class KD100APIController : Controller
    {
        private TelphoneOrderBLL telphoneorderbll = new TelphoneOrderBLL();
        #region 保存推送数据
        /// <summary>
        /// 推送接口
        /// </summary>
        /// <param name="param">轨迹查询结果</param>
        /// <returns></returns>
        public ActionResult SavePushRecord(string param)
        {
            try
            {
                WriteInLog log = new WriteInLog();
                log.writeInLog("物流推送数据："+param);
                Kd100Push entity = Newtonsoft.Json.JsonConvert.DeserializeObject<Kd100Push>(param);
                LastResult dd = entity.lastResult;
                TelphoneOrderEntity telOrderEntity = telphoneorderbll.GetEntityByNu(dd.nu);
                if (telOrderEntity != null)
                {
                    telOrderEntity.Sign = int.Parse(dd.state);//0在途中、1已揽收、2疑难、3已签收、4退签、5同城派送中、6退回、7转单
                    telOrderEntity.ModifyDate = DateTime.Now;
                    telphoneorderbll.SaveStateForm(telOrderEntity.ID, telOrderEntity);
                }

                return Content(new PushReturnMessage100 { result = true, returnCode = "200", message = "成功" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new PushReturnMessage100 { result = false, returnCode = "200", message = ex.Message }.ToString());
            }
        }
        #endregion

    }
}
