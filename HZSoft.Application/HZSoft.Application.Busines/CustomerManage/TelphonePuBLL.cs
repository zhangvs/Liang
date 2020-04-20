using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.Service.CustomerManage;
using HZSoft.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace HZSoft.Application.Busines.CustomerManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-10-23 14:11
    /// 描 述：靓号库
    /// </summary>
    public class TelphonePuBLL
    {
        private TelphonePuIService service = new TelphonePuService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphonePuEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphonePuEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphonePuEntity> GetList(string telphone, string organizeId, string city)
        {
            return service.GetList(telphone, organizeId, city);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphonePuEntity> GetGrade(string organizeId, string Grade, string city)
        {
            return service.GetGrade(organizeId, Grade, city);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphonePuEntity> GetListEnd4(string end4)
        {
            return service.GetListEnd4(end4);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TelphonePuEntity GetEntity(int? keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 权限内号码
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public IEnumerable<TelphonePuEntity> GetEntityByOrgTel(string organizeId, string telphone)
        {
            return service.GetEntityByOrgTel(organizeId, telphone);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValues)
        {
            try
            {
                service.RemoveForm(keyValues);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 上架数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void UpForm(string keyValues)
        {
            try
            {
                service.UpForm(keyValues);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 现卡数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void ExistForm(string keyValues)
        {
            try
            {
                service.ExistForm(keyValues);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 批量下架
        /// </summary>
        /// <returns></returns>
        public string DownTelphone(string downTelphones)
        {
            try
            {
                return service.DownTelphone(downTelphones);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 批量调价
        /// </summary>
        /// <returns></returns>
        public string PriceTelphone(string priceTelphones)
        {
            try
            {
                return service.PriceTelphone(priceTelphones);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(int? keyValue, TelphonePuEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <returns></returns>
        public string BatchAddEntity(DataTable dtSource)
        {
            try
            {
                string returnMsg = service.BatchAddEntity(dtSource);
                return returnMsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
