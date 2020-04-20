using HZSoft.Application.Busines.CustomerManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Web.Utility;
using HZSoft.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HZSoft.Application.Web.Areas.WeChatManage.Controllers
{
    public class SmsController : Controller
    {
        //
        // GET: /WeChatManage/Sms/
        SmsInfoBLL smsBll = new SmsInfoBLL();
        //号码预定
        public JsonResult SendReserve(string tel)
        {
            try
            {
                //1.判断是否已发送 是否过期
                //2.判断是否超过一天的发送次数
                var sms = smsBll.GetList("{}").FirstOrDefault(t => t.Tel == tel && t.Type == (int)SmsType.号码预定);
                if (sms != null)
                {
                    if (sms.CreateDate.AddMinutes(WebSiteConfig.SMS_EXPIRE_MIN) > DateTime.Now)
                        throw new Exception("请1分钟后再次发送");
                    if (sms.SendCount >= WebSiteConfig.SMS_MAX_COUNT)
                        throw new Exception("一天内只能发送" + WebSiteConfig.SMS_MAX_COUNT + "次");
                }

                //6位的随机验证码
                string code = SmsCore.GetNumberCode();
                var bo = SmsCore.SendSms(tel, code);
                if (bo == false)
                    throw new Exception("短信发送失败");
                if (sms == null)
                {
                    sms = new SmsInfoEntity()
                    {
                        Captcha = code,
                        CreateDate = DateTime.Now,
                        Tel = tel,
                        SendCount = 1,
                        Status = bo ? 1 : 0,
                        Type = (int)SmsType.号码预定
                    };
                    smsBll.SaveForm(null, sms);
                }
                else
                {
                    int day = sms.CreateDate.Subtract(DateTime.Now).Days;
                    sms.CreateDate = DateTime.Now;
                    sms.Status = bo ? 1 : 0;
                    sms.Captcha = code;
                    if (day == 0)
                    {
                        sms.SendCount += 1;
                    }
                    else
                    {
                        sms.SendCount = 1;
                    }
                    smsBll.SaveForm(sms.Id, sms);
                }
                return Json(new { success = true, message = "发送成功" });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }


        //申请加盟
        public JsonResult SendJoin(string tel)
        {
            try
            {
                //1.判断是否已发送 是否过期
                //2.判断是否超过一天的发送次数
                var sms = smsBll.GetList("{}").FirstOrDefault(t => t.Tel == tel && t.Type == (int)SmsType.申请加盟);
                if (sms != null)
                {
                    if (sms.CreateDate.AddMinutes(WebSiteConfig.SMS_EXPIRE_MIN) > DateTime.Now)
                        throw new Exception("请1分钟后再次发送");
                    if (sms.SendCount >= WebSiteConfig.SMS_MAX_COUNT)
                        throw new Exception("一天内只能发送" + WebSiteConfig.SMS_MAX_COUNT + "次");
                }

                //6位的随机验证码
                string code = SmsCore.GetNumberCode();
                var bo = SmsCore.SendSms(tel, code);
                if (bo == false)
                    throw new Exception("短信发送失败");
                if (sms == null)
                {
                    sms = new SmsInfoEntity()
                    {
                        Captcha = code,
                        CreateDate = DateTime.Now,
                        Tel = tel,
                        SendCount = 1,
                        Status = bo ? 1 : 0,
                        Type = (int)SmsType.申请加盟
                    };
                    smsBll.SaveForm(null, sms);
                }
                else
                {
                    int day = sms.CreateDate.Subtract(DateTime.Now).Days;
                    sms.CreateDate = DateTime.Now;
                    sms.Status = bo ? 1 : 0;
                    sms.Captcha = code;
                    if (day == 0)
                    {
                        sms.SendCount += 1;
                    }
                    else
                    {
                        sms.SendCount = 1;
                    }
                    smsBll.SaveForm(sms.Id, sms);
                }
                return Json(new { success = true, message = "发送成功" });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

    }

}


    public enum SmsType
    {
        号码预定 = 1,
        申请加盟 = 2
    }