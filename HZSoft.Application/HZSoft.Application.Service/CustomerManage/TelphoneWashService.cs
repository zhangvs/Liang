using HZSoft.Application.Code;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Transactions;

namespace HZSoft.Application.Service.CustomerManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-09-16 16:41
    /// 描 述：号码库
    /// </summary>
    public class TelphoneWashService : RepositoryFactory<TelphoneWashEntity>, TelphoneWashIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneWashEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var expression = LinqExtensions.True<TelphoneWashEntity>();
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneWash where 1=1";

            //导入日期
            if (!queryParam["CreateStartTime"].IsEmpty() && !queryParam["CreateEndTime"].IsEmpty())
            {
                DateTime createStartTime = DateTime.Now.Date;
                DateTime createEndTime = DateTime.Now.Date.AddDays(1);
                strSql += " and CreateDate BETWEEN '" + createStartTime + "' and '" + createEndTime + "'";
            }
            //分配状态
            if (!queryParam["AssignMark"].IsEmpty())
            {
                int AssignMark = queryParam["AssignMark"].ToInt();
                strSql += " and AssignMark = " + AssignMark;
            }

            if (queryParam["CreateStartTime"].IsEmpty() && queryParam["CreateEndTime"].IsEmpty() && queryParam["AssignMark"].IsEmpty())
            {
                //单据日期
                if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
                {
                    DateTime startTime = queryParam["StartTime"].ToDate();
                    DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                    strSql += " and ObtainDate BETWEEN '" + startTime + "' and '" + endTime + "'";
                }
                else
                {
                    //默认显示当前销售人的未呼出号码
                    DateTime startTime = DateTime.Now.Date;
                    DateTime endTime = DateTime.Now.Date.AddDays(1);
                    strSql += " and CallTime IS NULL ";
                }
                //手机号
                if (!queryParam["Telphone"].IsEmpty())
                {
                    string Telphone = queryParam["Telphone"].ToString();
                    strSql += " and Telphone like '%" + Telphone + "%'";
                }
                //号段
                if (!queryParam["Number"].IsEmpty())
                {
                    string Telphone = queryParam["Number"].ToString();
                    strSql += " and Number = '" + Telphone + "'";
                }
                //呼叫结果
                if (!queryParam["CallResult"].IsEmpty())
                {
                    int CallResult = queryParam["CallResult"].ToInt();
                    strSql += " and CallResult = " + CallResult;
                }

                //意向客户
                if (!queryParam["IntentionMark"].IsEmpty())
                {
                    int IntentionMark = queryParam["IntentionMark"].ToInt();
                    strSql += " and IntentionMark = " + IntentionMark;
                }
                //售出状态
                if (!queryParam["SellMark"].IsEmpty())
                {
                    int SellMark = queryParam["SellMark"].ToInt();
                    strSql += " and SellMark = " + SellMark;
                }
                //未接通方式
                if (!queryParam["NoConnectSelect"].IsEmpty())
                {
                    string NoConnectSelect = queryParam["NoConnectSelect"].ToString();
                    strSql += " and NoConnectMark = " + NoConnectSelect;
                }
                //不成交理由
                if (!queryParam["NoDealSelect"].IsEmpty())
                {
                    string NoDealSelect = queryParam["NoDealSelect"].ToString();
                    strSql += " and NoDealMark = " + NoDealSelect;
                }

                //分配人
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
                        strSql += " and SellerId in (" + dataAutor + ")";

                    }
                }

            }



            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphoneWashEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TelphoneWashEntity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
        public void SaveForm(int? keyValue, TelphoneWashEntity entity)
        {
            if (keyValue != null)
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
        /// 批量（新增）
        /// </summary>
        /// <param name="dtSource">实体对象</param>
        /// <returns></returns>
        public string BatchAddEntity(DataTable dtSource)
        {

            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            
            try
            {
                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    TelphoneWashEntity entity = new TelphoneWashEntity();
                    string telphone = dtSource.Rows[i][0].ToString();
                    if (telphone.Length == 11)
                    {
                        var wash_Data = db.FindEntity<TelphoneWashEntity>(t => t.Telphone == telphone);
                        if (wash_Data!=null)
                        {
                            return telphone+"重复导入！";
                        }
                        entity.Telphone = telphone.ToString();
                        entity.Grade = telphone.Substring(3, 4);
                        entity.Number = telphone.Substring(7);
                        entity.DeleteMark = 0;
                        entity.SellMark = 0;
                        entity.IntentionMark = 0;
                        entity.AssignMark = 0;
                        entity.Create();
                        db.Insert(entity);
                    }

                }
                db.Commit();
                return "导入成功";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 获取号码
        /// </summary>
        /// <returns></returns>
        public int GetTelphone(int getCount)
        {
            //1.先查看该员工今天是否已经获取过(是否大于300)
            string userid = OperatorProvider.Provider.Current().UserId;
            string username = OperatorProvider.Provider.Current().UserName;
            string strSql = "SELECT COUNT(*) FROM TelphoneWash WHERE 1=1 AND SellerId ='" + userid + "' AND   datediff(day,[ObtainDate],getdate())=0";
            DataTable dt = this.BaseRepository().FindTable(strSql.ToString());
            //已经获取的个数
            int ges = int.Parse(dt.Rows[0][0].ToString());
            //每日最大获取数
            int maxGet = int.Parse(Config.GetValue("maxGet"));
            //小于等于最大获取数300才可以获取
            if (ges + getCount <= maxGet)
            {


                //获取一遍数据库中没有分配的号码，
                string strSql1 = "SELECT * FROM TelphoneWash WHERE 1=1 AND AssignMark <>1  AND DeleteMark<>1 ";
                DataTable dts = this.BaseRepository().FindTable(strSql1.ToString());
                //dts.Rows.Count号码库剩余号码个数
                if (dts.Rows.Count > getCount)
                {
                    #region 顺序获取
                    string strSql2 = " set rowcount " + getCount + " update TelphoneWash set sellerid='" + userid + "',sellername='" + username + "',obtaindate=getdate() ,AssignMark=1 WHERE AssignMark  <>1 set rowcount 0 ";
                    int dts2 = this.BaseRepository().ExecuteBySql(strSql2.ToString());

                    #endregion
                    #region 随机获取
                    //2.没获取过，随机获取100个号码分配给当前员工
                    //Random rd = new Random();
                    //List<int> gint = new List<int>();
                    //for (int i = 0; i < maxGet; i++)
                    //{
                    //    //随机获取一个数
                    //    int dd = rd.Next(dts.Rows.Count);
                    //    //判断当前行是否被用过
                    //    if (gint.Contains(dd))
                    //    {
                    //        i--;
                    //        continue;
                    //    }
                    //    else
                    //    {
                    //        //不重复，修改当前实体，进行分配
                    //        gint.Add(dd);
                    //        DataRow dtr = dts.Rows[dd];
                    //        int keyValue = int.Parse(dtr["TelphoneID"].ToString());
                    //        TelphoneWashEntity oldEntity = this.BaseRepository().FindEntity(keyValue);
                    //        oldEntity.SellerId = userid;
                    //        oldEntity.SellerName = username;
                    //        oldEntity.AssignMark = 1;
                    //        oldEntity.ObtainDate = DateTime.Now;
                    //        oldEntity.Modify(keyValue);
                    //        this.BaseRepository().Update(oldEntity);
                    //    }
                    //}
                    #endregion
                    return 1;
                }
                else
                {
                    return 0;//洗号池数据不足
                }
            }
            else
            {
                return 2;//今日已经获取过
            }
        }
        #endregion
    }
}
