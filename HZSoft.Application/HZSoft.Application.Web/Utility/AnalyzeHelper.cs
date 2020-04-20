using HZSoft.Application.Entity.WeChatManage;
using HZSoft.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HZSoft.Application.Web.Utility
{
    public class AnalyzeHelper
    {
        #region Get Function    
        /// <summary>  
        /// 发起GET请求  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="requestUrl"></param>  
        /// <returns></returns>  
        public static T Get<T>(String requestUrl) where T : WeixinResponse, new()
        {
            String resultJson = HttpClientHelper.Get(requestUrl);
            //LogHelper.AddLog(resultJson);
            return AnalyzeResult<T>(resultJson);
        }
        #endregion

        #region Post Function  
        /// <summary>  
        /// 发起POST请求  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="requestUrl"></param>  
        /// <param name="requestParamOfJsonStr"></param>  
        /// <returns></returns>  
        public static T Post<T>(String requestUrl, String requestParamOfJsonStr) where T : WeixinResponse, new()
        {
            String resultJson = HttpClientHelper.Post(requestUrl, requestParamOfJsonStr);
            return AnalyzeResult<T>(resultJson);
        }
        #endregion

        #region AnalyzeResult  
        /// <summary>  
        /// 分析结果  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="resultJson"></param>  
        /// <returns></returns>  
        private static T AnalyzeResult<T>(string resultJson) where T : WeixinResponse, new()
        {
            T result = null;
            if (!String.IsNullOrEmpty(resultJson))
            {
                result = JsonConvert.DeserializeObject<T>(resultJson);
            }

            if (result == null)
            {
                result = new T();
            }
            return result;
        }
        #endregion
    }
}