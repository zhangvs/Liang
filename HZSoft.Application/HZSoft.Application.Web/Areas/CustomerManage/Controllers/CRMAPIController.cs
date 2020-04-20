using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Util;
using HZSoft.Util.WebControl;
using System.Web.Mvc;

namespace HZSoft.Application.Web.Areas.CustomerManage.Controllers
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-11-04 16:22
    /// 描 述：话单
    /// </summary>
    public class CRMAPIController : Controller
    {
        private CRM_CallLogBLL crm_calllogbll = new CRM_CallLogBLL();


        #region 提交数据
        /// <summary>
        /// 上传话单
        /// </summary>
        /// <param name="calllog">接受话单json字符串</param>
        /// <returns></returns>

        public ActionResult PushFollowUpRecord(string calllog)
        {
            string url = Request.Url.ToString();
            //WriteInLog log = new WriteInLog();
            //log.writeInLog(url);
            string calllogBit = calllog.Replace(":true", ":1").Replace(":false", ":0");
            //log.writeInLog(calllog);
            
            try
            {
                CRM_CallLogEntity entity = Newtonsoft.Json.JsonConvert.DeserializeObject<CRM_CallLogEntity>(calllogBit);
                int row = crm_calllogbll.SaveForm(entity);
                if (row == 1)
                {
                    return Content(new ReturnMessage { Success = true, Message = "上传成功", Value = null, ErrorCode = 0 }.ToString());
                }
                else
                {
                    return Content(new ReturnMessage { Success = false, Message = "上传失败", Value = null, ErrorCode = 0 }.ToString());
                }
            }
            catch (System.Exception)
            {
                return Content(new ReturnMessage { Success = false, Message = "写入数据库失败", Value = null, ErrorCode = 0 }.ToString());
            }

        }
        #endregion
    }
}
