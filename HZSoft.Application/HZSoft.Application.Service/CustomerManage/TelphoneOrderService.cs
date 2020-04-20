using HZSoft.Application.Code;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.IService.SystemManage;
using HZSoft.Application.Service.SystemManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HZSoft.Application.Service.CustomerManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-09-22 15:43
    /// 描 述：号码订单
    /// </summary>
    public class TelphoneOrderService : RepositoryFactory<TelphoneOrderEntity>, TelphoneOrderIService
    {
        private ICodeRuleService coderuleService = new CodeRuleService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneOrderEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var expression = LinqExtensions.True<TelphoneOrderEntity>();
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneOrder where 1=1 ";
            //单据日期
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and CreateDate BETWEEN '" + startTime + "' and '" + endTime + "'";
            }
            //单据编号
            if (!queryParam["Telphone"].IsEmpty())
            {
                string Telphone = queryParam["Telphone"].ToString();
                strSql += " and Telphone = '" + Telphone + "'";
            }
            //销售人
            //if (!queryParam["SellerId"].IsEmpty())
            //{
            //    string SellerId = queryParam["SellerId"].ToString();
            //    strSql += " and SellerId = '" + SellerId + "'";
            //}
            //else
            //{
            //    if (!OperatorProvider.Provider.Current().IsSystem)
            //    {
            //        string dataAutor = string.Format(OperatorProvider.Provider.Current().DataAuthorize.ReadAutorize, OperatorProvider.Provider.Current().UserId);
            //        strSql += " and SellerId in (" + dataAutor + ")";
            //    }
            //}

            //联系人
            if (!queryParam["Consignee"].IsEmpty())
            {
                string Consignee = queryParam["Consignee"].ToString();
                strSql += " and Consignee = '" + Consignee + "'";
            }

            //审核
            if (!queryParam["CheckMark"].IsEmpty())
            {
                int CheckMark = queryParam["CheckMark"].ToInt();
                strSql += " and CheckMark = " + CheckMark;
            }

            //作废
            if (!queryParam["DeleteMark"].IsEmpty())
            {
                int DeleteMark = queryParam["DeleteMark"].ToInt();
                strSql += " and DeleteMark = " + DeleteMark;
            }

            //物流状态
            if (!queryParam["SendMark"].IsEmpty())
            {
                int SendMark = queryParam["SendMark"].ToInt();
                strSql += " and SendMark = " + SendMark;
            }

            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphoneOrderEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 最新的10条记录
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TelphoneOrderEntity> GetListTop10()
        {
            return this.BaseRepository().IQueryable().OrderByDescending(t => t.CreateDate).Take(10).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TelphoneOrderEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        public TelphoneOrderEntity GetEntityByNu(string Nu)
        {
            return this.BaseRepository().FindEntity(t => t.Numbers == Nu);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="Telphone">主键值</param>
        /// <returns></returns>
        public TelphoneOrderEntity GetEntityByTelphone(string Telphone)
        {
            return this.BaseRepository().FindEntity(t => t.Telphone == Telphone && t.DeleteMark == 0);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                TelphoneOrderEntity entity = this.BaseRepository().FindEntity(keyValue);
                var telphone_Data = db.FindEntity<TelphonePuEntity>(t => t.Telphone == entity.Telphone);
                if (telphone_Data != null)
                {
                    telphone_Data.SellMark = 0;
                    telphone_Data.SellerId = "";
                    telphone_Data.SellerName = "";
                    telphone_Data.Description = "";
                    telphone_Data.Modify(telphone_Data.TelphoneID);
                    db.Update(telphone_Data);
                }
                db.Commit();
                this.BaseRepository().Delete(keyValue);

            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }
        /// <summary>
        /// 修改物流状态
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveStateForm(string keyValue, TelphoneOrderEntity entity)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            if (!string.IsNullOrEmpty(keyValue))
            {
                this.BaseRepository().Update(entity);
            }
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, TelphoneOrderEntity entity)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();

            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                try
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                    //修改号码库中号码的售出状态
                    var telphone_Data = db.FindEntity<TelphonePuEntity>(t => t.Telphone == entity.Telphone);
                    if (telphone_Data != null)
                    {
                        telphone_Data.SellMark = 1;
                        telphone_Data.SellerId = entity.SellerId;
                        telphone_Data.SellerName = entity.SellerName;
                        telphone_Data.Description += "|" + entity.SellerName + "已预定";
                        telphone_Data.Modify(telphone_Data.TelphoneID);
                        db.Update(telphone_Data);
                    }
                    //占用单据号
                    coderuleService.UseRuleSeed("c576c3f7-631d-4108-baaf-1495bdc0d6bb", db);
                    //coderuleService.UseRuleSeed(entity.CreateUserId, "", ((int)CodeRuleEnum.Telphone_OrderCode).ToString(), db);//占用单据号
                    db.Commit();
                }
                catch (Exception)
                {
                    db.Rollback();
                    throw;
                }
            }
        }


        /// <summary>
        /// 核单
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="State">状态：1-启动；0-禁用</param>
        public void UpdateCheckState(string keyValue, int State)
        {
            TelphoneOrderEntity reserveEntity = new TelphoneOrderEntity();
            reserveEntity.Modify(keyValue);
            reserveEntity.CheckMark = State;
            this.BaseRepository().Update(reserveEntity);
        }

        /// <summary>
        /// 作废:还原未预定状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="State">状态：1-启动；0-禁用</param>
        public void UpdateDeleteState(string keyValue, int State)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();

            TelphoneOrderEntity reserveEntity = new TelphoneOrderEntity();
            reserveEntity.Modify(keyValue);
            reserveEntity.DeleteMark = State;
            db.Update(reserveEntity);

            //修改号码库中号码的售出状态
            TelphoneOrderEntity entity = this.BaseRepository().FindEntity(keyValue);
            var telphone_Data = db.FindEntity<TelphonePuEntity>(t => t.Telphone == entity.Telphone);
            if (telphone_Data != null)
            {
                telphone_Data.SellMark = 0;
                telphone_Data.SellerId = "";
                telphone_Data.SellerName = "";
                telphone_Data.Description = "";
                telphone_Data.Modify(telphone_Data.TelphoneID);
                db.Update(telphone_Data);
            }
            db.Commit();
        }
        #endregion
    }
}
