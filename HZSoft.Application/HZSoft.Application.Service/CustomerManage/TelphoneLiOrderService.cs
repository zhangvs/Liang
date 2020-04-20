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
    public class TelphoneLiOrderService : RepositoryFactory<TelphoneLiOrderEntity>, TelphoneLiOrderIService
    {
        private ICodeRuleService coderuleService = new CodeRuleService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneLiOrderEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var expression = LinqExtensions.True<TelphoneLiOrderEntity>();
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneLiOrder where 1=1";
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
            if (!queryParam["SellerId"].IsEmpty())
            {
                string SellerId = queryParam["SellerId"].ToString();
                strSql += " and SellerId = '" + SellerId + "'";
            }
            else
            {
                if (!OperatorProvider.Provider.Current().IsSystem)
                {
                    string dataAutor = string.Format(OperatorProvider.Provider.Current().DataAuthorize.ReadAutorize, OperatorProvider.Provider.Current().UserId);
                    //strSql += " and SellerId in (" + dataAutor + ")";
                    strSql += " and OrganizeId in(SELECT OrganizeId FROM Base_Organize WHERE OrganizeId='" + OperatorProvider.Provider.Current().CompanyId + "' OR ParentId ='" + OperatorProvider.Provider.Current().CompanyId + "')";

                }
            }
            //收货人
            if (!queryParam["Consignee"].IsEmpty())
            {
                int SellMark = queryParam["Consignee"].ToInt();
                strSql += " and Consignee = " + SellMark;
            }

            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphoneLiOrderEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TelphoneLiOrderEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
                TelphoneLiOrderEntity entity = this.BaseRepository().FindEntity(keyValue);
                var telphone_Data = db.FindEntity<TelphoneLiangEntity>(t => t.Telphone == entity.Telphone);
                if (telphone_Data != null)
                {
                    telphone_Data.SellMark = 0;
                    telphone_Data.SellerId = "";
                    telphone_Data.SellerName = "";
                    telphone_Data.Description = entity.SellerName + "退回";
                    telphone_Data.Modify(telphone_Data.TelphoneID);
                    db.Update(telphone_Data);
                }
                //修改洗号池中号码的售出状态
                //TelphoneWashService tsw = new TelphoneWashService();
                //var telphone_wash = db.FindEntity<TelphoneWashEntity>(t => t.Telphone == entity.Contact);
                //if (telphone_wash != null)
                //{
                //    telphone_wash.SellMark = 0;
                //    telphone_wash.SellerId = null;
                //    telphone_wash.SellerName = null;
                //    telphone_wash.CallDescription = entity.SellerName + "退回";
                //    telphone_wash.Modify(telphone_wash.TelphoneID);
                //    db.Update(telphone_wash);
                //}

                db.Commit();
                //this.BaseRepository().Delete(keyValue);
                //软删除
                entity.Modify(keyValue);
                entity.DeleteMark = 1;
                this.BaseRepository().Update(entity);

            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, TelphoneLiOrderEntity entity)
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
                    TelphoneLiangService tss = new TelphoneLiangService();
                    var telphone_Data = db.FindEntity<TelphoneLiangEntity>(t => t.Telphone == entity.Telphone);
                    if (telphone_Data != null)
                    {
                        telphone_Data.SellMark = 1;
                        telphone_Data.SellerId = entity.SellerId;
                        telphone_Data.SellerName = entity.SellerName;
                        telphone_Data.Description = entity.SellerName + "已售出";
                        telphone_Data.Modify(telphone_Data.TelphoneID);
                        db.Update(telphone_Data);
                    }
                    //修改洗号池中号码的售出状态
                    //TelphoneWashService tsw = new TelphoneWashService();
                    //var telphone_wash = db.FindEntity<TelphoneWashEntity>(t => t.Telphone == entity.Contact);
                    //if (telphone_wash != null)
                    //{
                    //    telphone_wash.SellMark = 1;
                    //    telphone_wash.SellerId = entity.SellerId;
                    //    telphone_wash.SellerName = entity.SellerName;
                    //    telphone_wash.CallDescription = entity.SellerName + "已售出";
                    //    telphone_wash.Modify(telphone_wash.TelphoneID);
                    //    db.Update(telphone_wash);
                    //}
                    //占用单据号
                    coderuleService.UseRuleSeed(SystemInfo.CurrentModuleId, db);
                    db.Commit();
                }
                catch (Exception)
                {
                    db.Rollback();
                    throw;
                }
            }

        }
        #endregion
    }
}
