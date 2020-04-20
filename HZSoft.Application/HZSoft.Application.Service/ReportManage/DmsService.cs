using HZSoft.Application.Entity.ReportManage;
using HZSoft.Application.IService.ReportManage;
using HZSoft.Data;
using HZSoft.Data.Repository;
using HZSoft.Util.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using HZSoft.Util;
using HZSoft.Application.Entity.AuthorizeManage;
using HZSoft.Application.Code;

namespace HZSoft.Application.Service.ReportManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创建人：佘赐雄
    /// 日 期：2016.1.14 14:27
    /// 描 述：报表模板管理
    /// </summary>
    public class DmsService : RepositoryFactory, IDmsService
    {
        #region 获取数据
        /// <summary>
        /// 员工报表
        /// </summary>
        /// <param name="queryJson">起始，结束日期json</param>
        /// <returns></returns>
        public DataTable GetDateOrder_emp(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string strSql = "SELECT SellerName AS name, SUM(Amount) AS emp_amount, count(1) AS emp_count FROM TelphoneOrder where 1=1";
            if (!OperatorProvider.Provider.Current().IsSystem)
            {
                string dataAutor = string.Format(OperatorProvider.Provider.Current().DataAuthorize.ReadAutorize, OperatorProvider.Provider.Current().UserId);
                strSql += " and SellerId in (" + dataAutor + ")";
            }
            //单据日期
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and CreateDate BETWEEN '" + startTime + "' and '" + endTime + "'";
            }
            strSql += " group by SellerName order by emp_amount desc";

            return this.BaseRepository().FindTable(strSql.ToString());
        }
        /// <summary>
        /// 销售号码分析报表
        /// </summary>
        /// <returns></returns>
        public DataTable GetAnalysis(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string whereSql = "";
            //单据日期
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                whereSql += " and TelphoneOrder.CreateDate BETWEEN '" + startTime + "' and '" + endTime + "'";
            }

            string strSql = @"SELECT fx,hd,count,zb FROM (
SELECT 'b2' fx,substring(contact,1,2) hd,count(1) count,CONVERT( DECIMAL(18,2),cast(count(1) as float)/CAST((SELECT count(1) total FROM TelphoneOrder WHERE Contact IS not NULL ) AS FLOAT)*100) zb FROM TelphoneOrder 
WHERE Contact IS not NULL " + whereSql+
@" GROUP BY substring(contact,1, 2) 
UNION
SELECT 'b3' fx,substring(contact,1,3) hd,count(1) count,CONVERT( DECIMAL(18,2),cast(count(1) as float)/CAST((SELECT count(1) total FROM TelphoneOrder WHERE Contact IS not NULL ) AS FLOAT)*100) zb  FROM TelphoneOrder 
WHERE Contact IS not NULL " + whereSql +
@" GROUP BY substring(contact,1, 3) 
UNION
SELECT 'operate' fx,TelphoneData.operate hd,count(1) count,CONVERT( DECIMAL(18,2),cast(count(1) as float)/CAST((SELECT count(1) total FROM TelphoneOrder WHERE Contact IS not NULL ) AS FLOAT)*100) zb  FROM TelphoneOrder 
LEFT JOIN TelphoneData  ON substring(TelphoneOrder.contact,1,7)=TelphoneData.Number7 
WHERE Contact IS not NULL  " + whereSql +
@" GROUP BY TelphoneData.Operate 
UNION
SELECT 'w1' fx, substring(contact,8,1) hd,count(1) count,CONVERT( DECIMAL(18,2),cast(count(1) as float)/CAST((SELECT count(1) total FROM TelphoneOrder WHERE Contact IS not NULL ) AS FLOAT)*100) zb  FROM TelphoneOrder 
WHERE Contact IS not NULL " + whereSql +
@" GROUP BY substring(contact,8,1) 
UNION
SELECT 'w2' fx,substring(contact,9,1) hd,count(1) count,CONVERT( DECIMAL(18,2),cast(count(1) as float)/CAST((SELECT count(1) total FROM TelphoneOrder WHERE Contact IS not NULL ) AS FLOAT)*100) zb  FROM TelphoneOrder 
WHERE Contact IS not NULL " + whereSql +
@" GROUP BY substring(contact,9,1) 
UNION
SELECT 'w3' fx,substring(contact,10,1) hd,count(1) count,CONVERT( DECIMAL(18,2),cast(count(1) as float)/CAST((SELECT count(1) total FROM TelphoneOrder WHERE Contact IS not NULL ) AS FLOAT)*100) zb  FROM TelphoneOrder 
WHERE Contact IS not NULL " + whereSql +
@" GROUP BY substring(contact,10,1) 
UNION
SELECT 'w4' fx,substring(contact,11,1) hd,count(1) count,CONVERT( DECIMAL(18,2),cast(count(1) as float)/CAST((SELECT count(1) total FROM TelphoneOrder WHERE Contact IS not NULL ) AS FLOAT)*100) zb  FROM TelphoneOrder 
WHERE Contact IS not NULL " + whereSql +
@" GROUP BY substring(contact,11,1) 
) t
ORDER BY fx,count desc";
            return this.BaseRepository().FindTable(strSql.ToString());
        }

        /// <summary>
        /// 员工报表
        /// </summary>
        /// <param name="queryJson">起始，结束日期json</param>
        /// <returns></returns>
        public DataTable GetCallLog(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string whereCallTime = "";
            string whereObtainDate = "";
            //单据日期
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                whereCallTime += " and a.CallTime BETWEEN '" + startTime + "' and '" + endTime + "'";
                whereObtainDate += " and ObtainDate BETWEEN '" + startTime + "' and '" + endTime + "'";
            }
            // CAST(CONVERT( DECIMAL(18,2),yescallcount/CAST(ISNULL(NULLIF(callcount,0),1) AS FLOAT)*100)AS VARCHAR(10))+'%' jtl
            //秒转分秒 CAST(sum(CallDuration) / 60 as nvarchar(50)) + ':' + SUBSTRING(CAST(100 + sum(CallDuration) % 60 as nvarchar(3)), 1, 2) minutesecond
            //            string strSql =
            //@"SELECT workerusername,minute,callcount,yescallcount,nocallcount,CONVERT( DECIMAL(18,2),yescallcount/CAST(ISNULL(NULLIF(callcount,0),1) AS FLOAT)*100) jtl,itemname name FROM(
            //    SELECT workerusername, sum(CallDuration)/60 minute, count(1) callcount,
            //    (SELECT count(1)  FROM CRM_CallLog b WHERE CallDuration>0 AND a.WorkerUserName=b.WorkerUserName " + whereSql + @")yescallcount,
            //    (SELECT count(1)  FROM CRM_CallLog b WHERE CallDuration<=0 AND a.WorkerUserName=b.WorkerUserName " + whereSql + @")nocallcount
            //    FROM CRM_CallLog a Where 1=1" + whereSql +@" GROUP BY workerusername
            //) main
            // LEFT JOIN(
            // SELECT detail.itemvalue, detail.itemname FROM Base_DataItem item
            // LEFT JOIN Base_DataItemDetail detail
            // ON item.ItemId = detail.ItemId where item.ItemCode = 'zxh'
            //) items ON items.itemvalue = main.workerusername";

            string strSql =
@"SELECT gg.workerusername,gg.workername,
ISNULL(minute, 0) minute,
ISNULL(callcount, 0) callcount,
ISNULL(yescallcount, 0) yescallcount,
ISNULL(nocallcount, 0) nocallcount,
CONVERT(DECIMAL(18, 2), yescallcount / CAST(ISNULL(NULLIF(callcount, 0), 1) AS FLOAT) * 100) jtl,
ISNULL(getcount, 0) getcount,
ISNULL(surpluscount, 0) surpluscount FROM(
    SELECT workerusername, workername, count(1) callcount FROM CRM_CallLog a
    WHERE 1=1 " + whereCallTime + @"
    GROUP BY workerusername, workername)gg
LEFT JOIN(
    SELECT workerusername, workername, sum(CallDuration) / 60 minute FROM CRM_CallLog a
    LEFT JOIN TelphoneWash w ON a.CallNumber = w.Telphone
    Where w.NoConnectMark = 0 " + whereCallTime + @"
    GROUP BY workerusername, workername) aa ON aa.workerusername = gg.workerusername
LEFT JOIN (
    SELECT workerusername, workername, count(1) yescallcount FROM CRM_CallLog a
    LEFT JOIN TelphoneWash w ON a.CallNumber = w.Telphone
    WHERE CallDuration > 0 AND a.CallNumber = w.Telphone and w.NoConnectMark = 0 " + whereCallTime + @"
    GROUP BY workerusername, workername) bb ON gg.workerusername = bb.workerusername
LEFT JOIN (
    SELECT workerusername, workername, count(1) nocallcount FROM CRM_CallLog a
    LEFT JOIN TelphoneWash w ON a.CallNumber = w.Telphone
    WHERE a.CallNumber = w.Telphone and(CallDuration <= 0 or w.NoConnectMark <> 0) " + whereCallTime + @"
    GROUP BY workerusername, workername) cc ON gg.workerusername = cc.workerusername
LEFT JOIN (
    SELECT SellerName, count(1) getcount FROM TelphoneWash
    WHERE 1=1 " + whereObtainDate + @" GROUP BY SellerName
) dd ON gg.workername = dd.SellerName
LEFT JOIN (
    SELECT SellerName, count(1) surpluscount FROM TelphoneWash
    WHERE 1=1 " + whereObtainDate + @" AND CallTime IS NULL  GROUP BY SellerName
) ee ON gg.workername = ee.SellerName
ORDER BY minute desc";

            return this.BaseRepository().FindTable(strSql.ToString());
        }
        #endregion



        #region 靓号部分
        /// <summary>
        /// 查看靓号主页访问数据量
        /// </summary>
        /// <param name="queryJson">起始，结束日期json</param>
        /// <returns></returns>
//        public DataTable GetDateLiang_See(string queryJson)
//        {
//            var queryParam = queryJson.ToJObject();
//            string strSql = @" SELECT TOP 50 CASE WHEN org.FullName IS NULL THEN '总公司' ELSE org.FullName END name,count(1) see_count 
//FROM TelphoneLiangSee see 
//LEFT JOIN Base_Organize org ON see.OrganizeId=org.OrganizeId WHERE 1=1 ";
//            if (!OperatorProvider.Provider.Current().IsSystem)
//            {
//                strSql += " and org.OrganizeId IN(select OrganizeId from Base_Organize where ParentId='" + OperatorProvider.Provider.Current().CompanyId + "' OR OrganizeId='" + OperatorProvider.Provider.Current().CompanyId + "')";
//            }
//            //单据日期
//            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
//            {
//                DateTime startTime = queryParam["StartTime"].ToDate();
//                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
//                strSql += " and SeeDate BETWEEN '" + startTime + "' and '" + endTime + "'";
//            }
//            strSql += " GROUP BY org.FullName ORDER BY see_count desc";
//            return this.BaseRepository().FindTable(strSql.ToString());
//        }

        public DataTable GetDateLiang_See(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string strSql = @" SELECT TOP 40 FullName,seecount FROM Base_Organize  WHERE 1=1 ";
            if (!OperatorProvider.Provider.Current().IsSystem)
            {
                strSql += " and OrganizeId IN(select OrganizeId from Base_Organize where ParentId='" + OperatorProvider.Provider.Current().CompanyId + "' OR OrganizeId='" + OperatorProvider.Provider.Current().CompanyId + "')";
            }
            strSql += " ORDER BY seecount desc";
            return this.BaseRepository().FindTable(strSql.ToString());
        }

        /// <summary>
        /// 转发
        /// </summary>
        /// <param name="queryJson">起始，结束日期json</param>
        /// <returns></returns>
//        public DataTable GetDateLiang_Share(string queryJson)
//        {
//            var queryParam = queryJson.ToJObject();
//            string strSql = @" SELECT TOP 50 org.FullName,count(1) share_count 
//FROM TelphoneLiangShare share
//LEFT JOIN Base_Organize org ON share.OrganizeId=org.OrganizeId WHERE 1=1 ";

//            if (!OperatorProvider.Provider.Current().IsSystem)
//            {
//                strSql += " and org.OrganizeId IN(select OrganizeId from Base_Organize where ParentId='" + OperatorProvider.Provider.Current().CompanyId + "' OR OrganizeId='" + OperatorProvider.Provider.Current().CompanyId + "')";
//            }
//            //单据日期
//            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
//            {
//                DateTime startTime = queryParam["StartTime"].ToDate();
//                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
//                strSql += " and SeeDate BETWEEN '" + startTime + "' and '" + endTime + "'";
//            }
//            strSql += " GROUP BY org.FullName ORDER BY share_count desc ";
//            return this.BaseRepository().FindTable(strSql.ToString());
//        }
        public DataTable GetDateLiang_Share(string queryJson)
        {
            string strSql = @" SELECT TOP 40 FullName,sharecount FROM Base_Organize  WHERE 1=1 ";
            if (!OperatorProvider.Provider.Current().IsSystem)
            {
                strSql += " and OrganizeId IN(select OrganizeId from Base_Organize where ParentId='" + OperatorProvider.Provider.Current().CompanyId + "' OR OrganizeId='" + OperatorProvider.Provider.Current().CompanyId + "')";
            }
            strSql += " ORDER BY sharecount desc";
            return this.BaseRepository().FindTable(strSql.ToString());
        }

        /// <summary>
        /// 加盟
        /// </summary>
        /// <param name="queryJson">起始，结束日期json</param>
        /// <returns></returns>
        //        public DataTable GetDateLiang_Join(string queryJson)
        //        {
        //            var queryParam = queryJson.ToJObject();
        //            string strSql = @" SELECT TOP 50 org.FullName,count(1) join_count 
        //FROM TelphoneLiangJoin j
        //LEFT JOIN Base_Organize org ON j.OrganizeId=org.OrganizeId WHERE 1=1 ";
        //            if (!OperatorProvider.Provider.Current().IsSystem)
        //            {
        //                strSql += " and org.OrganizeId IN(select OrganizeId from Base_Organize where ParentId='" + OperatorProvider.Provider.Current().CompanyId + "' OR OrganizeId='" + OperatorProvider.Provider.Current().CompanyId + "')";
        //            }
        //            //单据日期
        //            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
        //            {
        //                DateTime startTime = queryParam["StartTime"].ToDate();
        //                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
        //                strSql += " and SeeDate BETWEEN '" + startTime + "' and '" + endTime + "'";
        //            }
        //            strSql += " GROUP BY org.FullName ORDER BY join_count desc ";
        //            return this.BaseRepository().FindTable(strSql.ToString());
        //        }
        public DataTable GetDateLiang_Join(string queryJson)
        {
            string strSql = @" SELECT TOP 40 FullName,joincount FROM Base_Organize  WHERE 1=1 ";
            if (!OperatorProvider.Provider.Current().IsSystem)
            {
                strSql += " and OrganizeId IN(select OrganizeId from Base_Organize where ParentId='" + OperatorProvider.Provider.Current().CompanyId + "' OR OrganizeId='" + OperatorProvider.Provider.Current().CompanyId + "')";
            }
            strSql += " ORDER BY joincount desc";
            return this.BaseRepository().FindTable(strSql.ToString());
        }

        /// <summary>
        /// 首页数量展示
        /// </summary>
        /// <returns></returns>
        public DataTable GetDateHome_Count()
        {
            string strSql = @" SELECT count(1) seecount FROM TelphoneLiangSee
                                UNION All
                                SELECT count(1) seecount FROM TelphoneLiangShare
                                UNION All
                                SELECT count(1) seecount FROM TelphoneLiangJoin
                                UNION All
                                SELECT count(1) seecount FROM Base_Organize
                                UNION All
                                SELECT count(1) seecount FROM TelphoneLiang ";
            return this.BaseRepository().FindTable(strSql.ToString());
        }
        #endregion


        public DataTable GetDateDms_Cai(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string max = "310";
            if (!queryParam["Max"].IsEmpty())
            {
                max = queryParam["Max"].ToString();
            }
            string sqlWhere = "";
            if (!queryParam["Id"].IsEmpty())
            {
                sqlWhere = " where Id='"+ queryParam["Id"]+"'";
            }

            string strSql0 = " SELECT id,[_12_05],[_12_10],[_12_15],[_12_20],[_12_25],[_12_30],[_12_35],[_12_41],[_12_46],[_12_50],[_12_56],[_13_03],[_13_06],[_13_11],[_13_15],[_13_21],[_13_25],[_13_31],[_13_36],[_13_41],[_13_47],[_13_51],[_13_55],[_14_01],[_14_05],[_14_12],[_14_17],[_14_22],[_14_27],[_14_31],[_14_35],[_14_41],[_14_46],[_14_51],[_14_55],[_15_01],[_15_06],[_15_10],[_15_15],[_15_20],[_15_26],[_15_30],[_15_35],[_15_41],[_15_45],[_15_51],[_15_55],[_16_00],[_16_05],[_16_11],[_16_15],[_16_21],[_17_30],[_17_36],[_17_40],[_17_46],[_17_50],[_17_55],[_18_00],[_18_05],[_18_12],[_18_15],[_18_20],[_18_25],[_18_30],[_18_35],[_18_40],[_18_45],[_18_50],[_18_56],[_19_00],[_19_05],[_19_10],[_19_16],[_19_21],[_19_27],[_19_31],[_19_37],[_19_42],[_19_47],[_19_51],[_19_57],[_20_00],[_20_05],[_20_11],[_20_15],[_20_21],[_20_25],[_20_32],[_20_36],[_20_40],[_20_45],[_20_51],[_20_56],[_21_00],[_21_05],[_21_11],[_21_15],[_21_21],[_21_26],[_21_32],[_21_36],[_21_41],[_21_46],[_21_50],[_21_55],[_22_00],[_22_05],[_22_10] FROM[HZSoftFramework_Base_2016].[dbo].[Dms_Cai]" + sqlWhere;
            string strSql = @" SELECT id,
 CASE WHEN [_12_05]>" + max + @" THEN " + max + @" ELSE [_12_05] END [_12_05]
,CASE WHEN [_12_10]>" + max + @" THEN " + max + @" ELSE [_12_10] END [_12_10]
,CASE WHEN [_12_15]>" + max + @" THEN " + max + @" ELSE [_12_15] END [_12_15]
,CASE WHEN [_12_20]>" + max + @" THEN " + max + @" ELSE [_12_20] END [_12_20]
,CASE WHEN [_12_25]>" + max + @" THEN " + max + @" ELSE [_12_25] END [_12_25]
,CASE WHEN [_12_30]>" + max + @" THEN " + max + @" ELSE [_12_30] END [_12_30]
,CASE WHEN [_12_35]>" + max + @" THEN " + max + @" ELSE [_12_35] END [_12_35]
,CASE WHEN [_12_41]>" + max + @" THEN " + max + @" ELSE [_12_41] END [_12_41]
,CASE WHEN [_12_46]>" + max + @" THEN " + max + @" ELSE [_12_46] END [_12_46]
,CASE WHEN [_12_50]>" + max + @" THEN " + max + @" ELSE [_12_50] END [_12_50]
,CASE WHEN [_12_56]>" + max + @" THEN " + max + @" ELSE [_12_56] END [_12_56]
,CASE WHEN [_13_03]>" + max + @" THEN " + max + @" ELSE [_13_03] END [_13_03]
,CASE WHEN [_13_06]>" + max + @" THEN " + max + @" ELSE [_13_06] END [_13_06]
,CASE WHEN [_13_11]>" + max + @" THEN " + max + @" ELSE [_13_11] END [_13_11]
,CASE WHEN [_13_15]>" + max + @" THEN " + max + @" ELSE [_13_15] END [_13_15]
,CASE WHEN [_13_21]>" + max + @" THEN " + max + @" ELSE [_13_21] END [_13_21]
,CASE WHEN [_13_25]>" + max + @" THEN " + max + @" ELSE [_13_25] END [_13_25]
,CASE WHEN [_13_31]>" + max + @" THEN " + max + @" ELSE [_13_31] END [_13_31]
,CASE WHEN [_13_36]>" + max + @" THEN " + max + @" ELSE [_13_36] END [_13_36]
,CASE WHEN [_13_41]>" + max + @" THEN " + max + @" ELSE [_13_41] END [_13_41]
,CASE WHEN [_13_47]>" + max + @" THEN " + max + @" ELSE [_13_47] END [_13_47]
,CASE WHEN [_13_51]>" + max + @" THEN " + max + @" ELSE [_13_51] END [_13_51]
,CASE WHEN [_13_55]>" + max + @" THEN " + max + @" ELSE [_13_55] END [_13_55]
,CASE WHEN [_14_01]>" + max + @" THEN " + max + @" ELSE [_14_01] END [_14_01]
,CASE WHEN [_14_05]>" + max + @" THEN " + max + @" ELSE [_14_05] END [_14_05]
,CASE WHEN [_14_12]>" + max + @" THEN " + max + @" ELSE [_14_12] END [_14_12]
,CASE WHEN [_14_17]>" + max + @" THEN " + max + @" ELSE [_14_17] END [_14_17]
,CASE WHEN [_14_22]>" + max + @" THEN " + max + @" ELSE [_14_22] END [_14_22]
,CASE WHEN [_14_27]>" + max + @" THEN " + max + @" ELSE [_14_27] END [_14_27]
,CASE WHEN [_14_31]>" + max + @" THEN " + max + @" ELSE [_14_31] END [_14_31]
,CASE WHEN [_14_35]>" + max + @" THEN " + max + @" ELSE [_14_35] END [_14_35]
,CASE WHEN [_14_41]>" + max + @" THEN " + max + @" ELSE [_14_41] END [_14_41]
,CASE WHEN [_14_46]>" + max + @" THEN " + max + @" ELSE [_14_46] END [_14_46]
,CASE WHEN [_14_51]>" + max + @" THEN " + max + @" ELSE [_14_51] END [_14_51]
,CASE WHEN [_14_55]>" + max + @" THEN " + max + @" ELSE [_14_55] END [_14_55]
,CASE WHEN [_15_01]>" + max + @" THEN " + max + @" ELSE [_15_01] END [_15_01]
,CASE WHEN [_15_06]>" + max + @" THEN " + max + @" ELSE [_15_06] END [_15_06]
,CASE WHEN [_15_10]>" + max + @" THEN " + max + @" ELSE [_15_10] END [_15_10]
,CASE WHEN [_15_15]>" + max + @" THEN " + max + @" ELSE [_15_15] END [_15_15]
,CASE WHEN [_15_20]>" + max + @" THEN " + max + @" ELSE [_15_20] END [_15_20]
,CASE WHEN [_15_26]>" + max + @" THEN " + max + @" ELSE [_15_26] END [_15_26]
,CASE WHEN [_15_30]>" + max + @" THEN " + max + @" ELSE [_15_30] END [_15_30]
,CASE WHEN [_15_35]>" + max + @" THEN " + max + @" ELSE [_15_35] END [_15_35]
,CASE WHEN [_15_41]>" + max + @" THEN " + max + @" ELSE [_15_41] END [_15_41]
,CASE WHEN [_15_45]>" + max + @" THEN " + max + @" ELSE [_15_45] END [_15_45]
,CASE WHEN [_15_51]>" + max + @" THEN " + max + @" ELSE [_15_51] END [_15_51]
,CASE WHEN [_15_55]>" + max + @" THEN " + max + @" ELSE [_15_55] END [_15_55]
,CASE WHEN [_16_00]>" + max + @" THEN " + max + @" ELSE [_16_00] END [_16_00]
,CASE WHEN [_16_05]>" + max + @" THEN " + max + @" ELSE [_16_05] END [_16_05]
,CASE WHEN [_16_11]>" + max + @" THEN " + max + @" ELSE [_16_11] END [_16_11]
,CASE WHEN [_16_15]>" + max + @" THEN " + max + @" ELSE [_16_15] END [_16_15]
,CASE WHEN [_16_21]>" + max + @" THEN " + max + @" ELSE [_16_21] END [_16_21]
,CASE WHEN [_17_30]>" + max + @" THEN " + max + @" ELSE [_17_30] END [_17_30]
,CASE WHEN [_17_36]>" + max + @" THEN " + max + @" ELSE [_17_36] END [_17_36]
,CASE WHEN [_17_40]>" + max + @" THEN " + max + @" ELSE [_17_40] END [_17_40]
,CASE WHEN [_17_46]>" + max + @" THEN " + max + @" ELSE [_17_46] END [_17_46]
,CASE WHEN [_17_50]>" + max + @" THEN " + max + @" ELSE [_17_50] END [_17_50]
,CASE WHEN [_17_55]>" + max + @" THEN " + max + @" ELSE [_17_55] END [_17_55]
,CASE WHEN [_18_00]>" + max + @" THEN " + max + @" ELSE [_18_00] END [_18_00]
,CASE WHEN [_18_05]>" + max + @" THEN " + max + @" ELSE [_18_05] END [_18_05]
,CASE WHEN [_18_12]>" + max + @" THEN " + max + @" ELSE [_18_12] END [_18_12]
,CASE WHEN [_18_15]>" + max + @" THEN " + max + @" ELSE [_18_15] END [_18_15]
,CASE WHEN [_18_20]>" + max + @" THEN " + max + @" ELSE [_18_20] END [_18_20]
,CASE WHEN [_18_25]>" + max + @" THEN " + max + @" ELSE [_18_25] END [_18_25]
,CASE WHEN [_18_30]>" + max + @" THEN " + max + @" ELSE [_18_30] END [_18_30]
,CASE WHEN [_18_35]>" + max + @" THEN " + max + @" ELSE [_18_35] END [_18_35]
,CASE WHEN [_18_40]>" + max + @" THEN " + max + @" ELSE [_18_40] END [_18_40]
,CASE WHEN [_18_45]>" + max + @" THEN " + max + @" ELSE [_18_45] END [_18_45]
,CASE WHEN [_18_50]>" + max + @" THEN " + max + @" ELSE [_18_50] END [_18_50]
,CASE WHEN [_18_56]>" + max + @" THEN " + max + @" ELSE [_18_56] END [_18_56]
,CASE WHEN [_19_00]>" + max + @" THEN " + max + @" ELSE [_19_00] END [_19_00]
,CASE WHEN [_19_05]>" + max + @" THEN " + max + @" ELSE [_19_05] END [_19_05]
,CASE WHEN [_19_10]>" + max + @" THEN " + max + @" ELSE [_19_10] END [_19_10]
,CASE WHEN [_19_16]>" + max + @" THEN " + max + @" ELSE [_19_16] END [_19_16]
,CASE WHEN [_19_21]>" + max + @" THEN " + max + @" ELSE [_19_21] END [_19_21]
,CASE WHEN [_19_27]>" + max + @" THEN " + max + @" ELSE [_19_27] END [_19_27]
,CASE WHEN [_19_31]>" + max + @" THEN " + max + @" ELSE [_19_31] END [_19_31]
,CASE WHEN [_19_37]>" + max + @" THEN " + max + @" ELSE [_19_37] END [_19_37]
,CASE WHEN [_19_42]>" + max + @" THEN " + max + @" ELSE [_19_42] END [_19_42]
,CASE WHEN [_19_47]>" + max + @" THEN " + max + @" ELSE [_19_47] END [_19_47]
,CASE WHEN [_19_51]>" + max + @" THEN " + max + @" ELSE [_19_51] END [_19_51]
,CASE WHEN [_19_57]>" + max + @" THEN " + max + @" ELSE [_19_57] END [_19_57]
,CASE WHEN [_20_00]>" + max + @" THEN " + max + @" ELSE [_20_00] END [_20_00]
,CASE WHEN [_20_05]>" + max + @" THEN " + max + @" ELSE [_20_05] END [_20_05]
,CASE WHEN [_20_11]>" + max + @" THEN " + max + @" ELSE [_20_11] END [_20_11]
,CASE WHEN [_20_15]>" + max + @" THEN " + max + @" ELSE [_20_15] END [_20_15]
,CASE WHEN [_20_21]>" + max + @" THEN " + max + @" ELSE [_20_21] END [_20_21]
,CASE WHEN [_20_25]>" + max + @" THEN " + max + @" ELSE [_20_25] END [_20_25]
,CASE WHEN [_20_32]>" + max + @" THEN " + max + @" ELSE [_20_32] END [_20_32]
,CASE WHEN [_20_36]>" + max + @" THEN " + max + @" ELSE [_20_36] END [_20_36]
,CASE WHEN [_20_40]>" + max + @" THEN " + max + @" ELSE [_20_40] END [_20_40]
,CASE WHEN [_20_45]>" + max + @" THEN " + max + @" ELSE [_20_45] END [_20_45]
,CASE WHEN [_20_51]>" + max + @" THEN " + max + @" ELSE [_20_51] END [_20_51]
,CASE WHEN [_20_56]>" + max + @" THEN " + max + @" ELSE [_20_56] END [_20_56]
,CASE WHEN [_21_00]>" + max + @" THEN " + max + @" ELSE [_21_00] END [_21_00]
,CASE WHEN [_21_05]>" + max + @" THEN " + max + @" ELSE [_21_05] END [_21_05]
,CASE WHEN [_21_11]>" + max + @" THEN " + max + @" ELSE [_21_11] END [_21_11]
,CASE WHEN [_21_15]>" + max + @" THEN " + max + @" ELSE [_21_15] END [_21_15]
,CASE WHEN [_21_21]>" + max + @" THEN " + max + @" ELSE [_21_21] END [_21_21]
,CASE WHEN [_21_26]>" + max + @" THEN " + max + @" ELSE [_21_26] END [_21_26]
,CASE WHEN [_21_32]>" + max + @" THEN " + max + @" ELSE [_21_32] END [_21_32]
,CASE WHEN [_21_36]>" + max + @" THEN " + max + @" ELSE [_21_36] END [_21_36]
,CASE WHEN [_21_41]>" + max + @" THEN " + max + @" ELSE [_21_41] END [_21_41]
,CASE WHEN [_21_46]>" + max + @" THEN " + max + @" ELSE [_21_46] END [_21_46]
,CASE WHEN [_21_50]>" + max + @" THEN " + max + @" ELSE [_21_50] END [_21_50]
,CASE WHEN [_21_55]>" + max + @" THEN " + max + @" ELSE [_21_55] END [_21_55]
,CASE WHEN [_22_00]>" + max + @" THEN " + max + @" ELSE [_22_00] END [_22_00]
,CASE WHEN [_22_05]>" + max + @" THEN " + max + @" ELSE [_22_05] END [_22_05]
,CASE WHEN [_22_10]>" + max + @" THEN " + max + @" ELSE [_22_10] END [_22_10]
  FROM[HZSoftFramework_Base_2016].[dbo].[Dms_Cai]"+sqlWhere;
            string strSql1 = "SELECT * FROM[HZSoftFramework_Base_2016].[dbo].[Dms_Cai]" + sqlWhere;
            return this.BaseRepository().FindTable(strSql1.ToString());
        }

        /// <summary>
        /// 首页数量展示
        /// </summary>
        /// <returns></returns>
        public DataTable GetLiangSales(string queryJson)
        {
            string strSql = @"SELECT ModifyDate,Telphone,Grade,City,Price,MinPrice,SalePrice,SellerName FROM TelphoneLiang WHERE SellMark=1 AND OrganizeId='" + OperatorProvider.Provider.Current().CompanyId + "' ";
            //string strSql = @"SELECT ModifyDate,Telphone,Grade,City,Price,MinPrice,ChaPrice,SalePrice,SellerName FROM TelphoneLiang WHERE SellMark=1 AND OrganizeId='a5a962da-57e1-4ad4-87b2-bbdcd1b7cc92' ";
            var queryParam = queryJson.ToJObject();
            if (queryJson==null)
            {
                DateTime startTime = DateTime.Now.Date;
                DateTime endTime = DateTime.Now.Date.AddDays(1);
                strSql += " and ModifyDate BETWEEN '" + startTime + "' and '" + endTime + "'";
            }
            //单据日期
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                strSql += " and ModifyDate BETWEEN '" + startTime + "' and '" + endTime + "'";
            }

            if (!queryParam["Telphone"].IsEmpty())
            {
                string Telphone = queryParam["Telphone"].ToString();
                strSql += " and Telphone like '%" + Telphone + "%'";
            }
            if (!queryParam["City"].IsEmpty())
            {
                string City = queryParam["City"].ToString();
                strSql += " and City like '%" + City + "%'";
            }
            if (!queryParam["Grade"].IsEmpty())
            {
                string Grade = queryParam["Grade"].ToString();
                strSql += " and Grade =" + Grade;
            }
            if (!queryParam["SellerName"].IsEmpty())
            {
                string SellerName = queryParam["SellerName"].ToString();
                strSql += " and SellerName like '%" + SellerName + "%'";
            }

            ////查询条件
            //if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            //{
            //    string condition = queryParam["condition"].ToString();
            //    string keyword = queryParam["keyword"].ToString();
            //    switch (condition)
            //    {
            //        case "City":              //城市
            //            strSql += " and City like '%" + queryParam["City"].ToString() + "%'";
            //            break;
            //        case "Grade":              //类别
            //            strSql += " and Grade like '%" + queryParam["Grade"].ToString() + "%'";
            //            break;
            //        case "SellerName":              //销售人
            //            strSql += " and SellerName like '%" + queryParam["SellerName"].ToString() + "%'";
            //            break;

            //        default:
            //            break;
            //    }
            //}

            return this.BaseRepository().FindTable(strSql.ToString());
        }
    }
}
