using HZSoft.Application.Code;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.IService.SystemManage;
using HZSoft.Application.Service.SystemManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HZSoft.Application.Service.CustomerManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-04-16 09:07
    /// 描 述：订单表
    /// </summary>
    public class OrdersService : RepositoryFactory<OrdersEntity>, OrdersIService
    {
        private ICodeRuleService coderuleService = new CodeRuleService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<OrdersEntity> GetPageList(Pagination pagination, string queryJson)
        {
            string strSql = $"select * from Orders where 1=1 ";
            var expression = LinqExtensions.True<OrdersEntity>();
            var queryParam = queryJson.ToJObject();
            //成立日期
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and CreateDate >= '" + startTime + "' and CreateDate < '" + endTime + "'";
            }

            //单号
            if (!queryParam["OrderSn"].IsEmpty())
            {
                string OrderSn = queryParam["OrderSn"].ToString();
                strSql += " and OrderSn like '%" + OrderSn + "%'";
            }
            //靓号
            if (!queryParam["Tel"].IsEmpty())
            {
                string Tel = queryParam["Tel"].ToString();
                strSql += " and Tel like '%" + Tel + "%'";
            }

            //收件人
            if (!queryParam["Receiver"].IsEmpty())
            {
                string Receiver = queryParam["Receiver"].ToString();
                strSql += " and Receiver like '%" + Receiver + "%'";
            }
            //联系电话
            if (!queryParam["ContactTel"].IsEmpty())
            {
                string ContactTel = queryParam["ContactTel"].ToString();
                strSql += " and ContactTel like '%" + ContactTel + "%'";
            }
            //订单状态
            if (!queryParam["Status"].IsEmpty())
            {
                int Status = queryParam["Status"].ToInt();
                strSql += " and Status  = " + Status;
            }
            //支付状态
            if (!queryParam["PayStatus"].IsEmpty())
            {
                int PayStatus = queryParam["PayStatus"].ToInt();
                strSql += " and PayStatus  = " + PayStatus;
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OrdersEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OrdersEntity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="OrderSn">主键值</param>
        /// <returns></returns>
        public OrdersEntity GetEntityByOrderSn(string OrderSn)
        {
            return this.BaseRepository().FindEntity(t=>t.OrderSn == OrderSn);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="tel">主键值</param>
        /// <returns></returns>
        public OrdersEntity GetEntityByTel(string tel)
        {
            return this.BaseRepository().FindEntity(t => t.Tel == tel);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="contactTel">主键值</param>
        /// <returns></returns>
        public OrdersEntity GetEntityByContactTel(string contactTel)
        {
            return this.BaseRepository().FindEntity(t => t.ContactTel == contactTel);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(int? keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(int? keyValue, OrdersEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue.ToString()))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }


        /// <summary>
        /// 保存表单（新增）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public OrdersEntity SaveForm(OrdersEntity entity)
        {
            entity.Create();
            entity.OrderSn = string.Format("{0}{1}", "LX-", DateTime.Now.ToString("yyyyMMddHHmmss"));//,TenPayV3Util.BuildRandomStr(6)
            this.BaseRepository().Insert(entity);
            return entity;
        }




        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveSendForm(int? keyValue, OrdersEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue.ToString()))
            {
                entity.Modify(keyValue);
                entity.Status = 3;
                entity.DeliveryName = OperatorProvider.Provider.Current().UserName;
                entity.DeliveryDate = DateTime.Now;
                this.BaseRepository().Update(entity);
            }
        }
        /// <summary>
        /// 开卡
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public void UpdateSendState(int? keyValue)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue.ToString()))
                {
                    OrdersEntity entity = GetEntity(keyValue);
                    entity.Modify(keyValue);
                    entity.Status = 4;//开发，订单已完成
                    this.BaseRepository().Update(entity);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
