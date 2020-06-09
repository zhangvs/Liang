using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.SystemManage;
using HZSoft.Application.IService.BaseManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.Service.BaseManage;
using HZSoft.Cache.Redis;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace HZSoft.Application.Service.CustomerManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-10-23 14:11
    /// 描 述：靓号库
    /// </summary>
    public class TelphoneLiangService : RepositoryFactory<TelphoneLiangEntity>, TelphoneLiangIService
    {
        private IOrganizeService orgService = new OrganizeService();
        private TelphoneLiangVipIService vipService = new TelphoneLiangVipService();
        private TelphoneVipShareIService vipShareService = new TelphoneVipShareService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneLiangEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneLiang where DeleteMark <> 1 and EnabledMark <> 1";
            //机构后台
            if (!queryParam["OrganizeId"].IsEmpty())
            {
                string OrganizeId = queryParam["OrganizeId"].ToString();
                strSql += " and OrganizeId = '" + OrganizeId + "'";
            }
            else
            {
                if (!OperatorProvider.Provider.Current().IsSystem && OperatorProvider.Provider.Current().UserId != "3303254b-7cd3-4a25-abd3-bb2542a08df9")//龙哥可以查看到所有号码
                {
                    string companyId = OperatorProvider.Provider.Current().CompanyId;
                    //一级机构可以查看上级0级的号码库，因为自己人员工
                    strSql += " and OrganizeId IN( select OrganizeId from Base_Organize where OrganizeId='" + companyId
                        + "' or OrganizeId =(SELECT ParentId FROM Base_Organize WHERE OrganizeId='" + companyId + "'))";
                }
            }
            strSql += GetSql(queryJson);
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }

        /// <summary>
        /// 查号码归属代理页面
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<TelphoneLiangEntity> GetPageListJoin(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneLiang where DeleteMark <> 1 and EnabledMark <> 1";
            //机构后台
            if (!queryParam["OrganizeId"].IsEmpty())
            {
                string OrganizeId = queryParam["OrganizeId"].ToString();
                strSql += " and OrganizeId = '" + OrganizeId + "'";
            }
            else
            {
                if (!OperatorProvider.Provider.Current().IsSystem)
                {
                    string companyId = OperatorProvider.Provider.Current().CompanyId;
                    var organizeData = orgService.GetEntity(companyId);
                    if (!string.IsNullOrEmpty(organizeData.OrganizeId))
                    {
                        //string inOrg = GetInOrg(companyId, organizeData.ParentId, organizeData.TopOrganizeId);
                        List<string> vipList= vipService.GetVipOrgList(companyId, organizeData.ParentId, organizeData.TopOrganizeId);
                        string inOrg = GetOtherOrg(vipList);//自定义优先，共享平台其次
                        if (!string.IsNullOrEmpty(inOrg))
                        {
                            strSql += " and OrganizeId IN(" + inOrg + ")";
                        }
                    }
                }
            }
            strSql += GetSql(queryJson);
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }

        /// <summary>
        /// 根据json拼接sql条件，除机构外的条件
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        private string GetSql(string queryJson)
        {
            string strSql = "";
            var queryParam = queryJson.ToJObject();

            //单据编号
            if (!queryParam["Telphone"].IsEmpty())
            {
                string Telphone = queryParam["Telphone"].ToString();
                if (Telphone.Contains("|"))
                {
                    strSql += " and Telphone like '%" + Telphone.Replace("|", "") + "'";
                }
                else
                {
                    strSql += " and Telphone like '%" + Telphone + "%'";
                }

            }
            //分类
            if (!queryParam["Grade"].IsEmpty())
            {
                string Grade = queryParam["Grade"].ToString();
                if (Grade.Contains(","))
                {
                    strSql += " and Grade in (" + Grade + ")";
                }
                else
                {
                    strSql += " and Grade = '" + Grade + "'";
                }
            }
            //城市
            if (!queryParam["city"].IsEmpty())
            {
                string city = queryParam["city"].ToString();
                if (city.Contains("0000"))
                {
                    strSql += " and cityid like '" + city.Substring(0,2) + "%'";
                }
                else if (city!="0")
                {
                    strSql += " and cityid = '" + city + "'";
                }
            }
            //城市名称
            if (!queryParam["City"].IsEmpty())
            {
                string City = queryParam["City"].ToString();
                strSql += " and City like '%" + City + "%'";
            }
            //价格区间
            if (!queryParam["price"].IsEmpty())
            {
                string price = queryParam["price"].ToString();
                string[] jgqj = price.Split('-');
                if (!string.IsNullOrEmpty(jgqj[0]))
                {
                    strSql += " and price >= '" + jgqj[0] + "'";
                }
                if (!string.IsNullOrEmpty(jgqj[1]))
                {
                    strSql += " and price <= '" + jgqj[1] + "'";
                }
            }
            //价格区间
            if (!queryParam["MaxPrice"].IsEmpty())
            {
                string MaxPrice = queryParam["MaxPrice"].ToString();
                string[] jgqj = MaxPrice.Split('-');
                if (!string.IsNullOrEmpty(jgqj[0]))
                {
                    strSql += " and MaxPrice >= '" + jgqj[0] + "'";
                }
                if (!string.IsNullOrEmpty(jgqj[1]))
                {
                    strSql += " and MaxPrice <= '" + jgqj[1] + "'";
                }
            }
            //排除
            if (!queryParam["except"].IsEmpty())
            {
                string except = queryParam["except"].ToString();
                strSql += " and Telphone not like '%" + except.Replace("e", "") + "%'";
            }
            //寓意
            if (!queryParam["yuyi"].IsEmpty())
            {
                string yuyi = queryParam["yuyi"].ToString();
                if (yuyi == "1")
                {//1349风水能量
                    strSql += " and Telphone like '%1349%'";
                }
                else if (yuyi == "2")
                {//
                    strSql += " and (Telphone like '%520%' or Telphone like '%521%' or Telphone like '%1314%')";
                }
            }
            //售出标识
            if (!queryParam["SellMark"].IsEmpty())
            {
                string SellMark = queryParam["SellMark"].ToString();
                strSql += " and SellMark = " + SellMark;
            }

            //状态条件
            if (!queryParam["ExistMark"].IsEmpty())
            {
                string ExistMark = queryParam["ExistMark"].ToString();
                strSql += " and ExistMark = " + ExistMark;
            }

            //网络
            if (!queryParam["Operator"].IsEmpty())
            {
                string Operator = queryParam["Operator"].ToString();
                strSql += " and Operator = '" + Operator + "'";
            }
            return strSql;
        }


        /// <summary>
        /// 所有相关的机构，包括共享平台
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="pid"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        private string GetInOrg(string organizeId, string pid, string top)
        {
            string inOrg = "";
            string shareOrg = RedisCache.Get<string>("ShareOrg");
            bool isJoin = false;
            //判断vip到期情况
            //1.本机构
            if (vipService.GetVipByOrganizeId(organizeId))
            {
                if (!string.IsNullOrEmpty(shareOrg))
                {
                    if (shareOrg.Contains(organizeId))
                    {
                        isJoin = true;
                        shareOrg = shareOrg.Replace("'" + organizeId + "',", "").Replace("'" + organizeId + "'", "");
                    }
                }
                inOrg += "'" + organizeId + "',";
            }
            else
            {
                organizeId = "";
            }
            //2.代售直属上级
            if (!string.IsNullOrEmpty(pid) && organizeId != pid)
            {
                if (vipService.GetVipByOrganizeId(pid))
                {
                    if (!string.IsNullOrEmpty(shareOrg))
                    {
                        if (shareOrg.Contains(pid))
                        {
                            isJoin = true;
                            shareOrg = shareOrg.Replace("'" + pid + "',", "").Replace("'" + pid + "'", "");
                        }
                        inOrg += "'" + pid + "',";
                    }
                }
                else
                {
                    pid = "";
                }
            }
            else
            {
                pid = "";
            }
            // 3.顶级机构
            if (!string.IsNullOrEmpty(top) && top != organizeId && top != pid)
            {
                if (vipService.GetVipByOrganizeId(top))
                {
                    if (!string.IsNullOrEmpty(shareOrg))
                    {
                        if (shareOrg.Contains(top))
                        {
                            isJoin = true;
                            shareOrg = shareOrg.Replace("'" + top + "',", "").Replace("'" + top + "'", "");
                        }
                    }
                    inOrg += "'" + top + "',";
                }
                else
                {
                    top = "";
                }
            }
            else
            {
                top = "";
            }

            //如果机构全部过期，返回空
            if (string.IsNullOrEmpty(inOrg))
            {
                return null;
            }

            //是否加入共享平台
            if (isJoin)
            {
                inOrg += shareOrg;
            }

            return inOrg.Trim(',');
        }

        /// <summary>
        /// 从过滤共享平台中的vip机构
        /// </summary>
        /// <param name="vipList">vip机构</param>
        /// <returns></returns>
        private string GetOtherOrg(List<string> vipList)
        {
            string shareOrg = "";
            //1.自定义共享机构
            foreach (var item in vipList)
            {
                var sharelist = vipShareService.GetVipList(item);
                if (sharelist != null)
                {
                    foreach (var item2 in sharelist)
                    {
                        shareOrg += "'" + item2.ShareId + "',";//所有自定义选择的共享机构
                    }
                }
            }
            //如果没有自定义选择共享机构，则再判断是否加入共享平台
            if (string.IsNullOrEmpty(shareOrg))
            {
                //2.共享平台
                shareOrg = RedisCache.Get<string>("ShareOrg");
                bool isJoin = false;
                if (!string.IsNullOrEmpty(shareOrg))
                {
                    foreach (var item in vipList)
                    {
                        //如果也加入了共享平台，把本身机构去重
                        if (shareOrg.Contains(item))
                        {
                            shareOrg = shareOrg.Replace("'" + item + "',", "").Replace("'" + item + "'", "");
                            isJoin = true;//加入共享平台
                        }
                    }
                }
                //没有加入的shareOrg设置为空
                if (!isJoin)
                {
                    shareOrg = "";
                }
            }

            return shareOrg.Trim(',');
        }

        /// <summary>
        /// H5端查询按钮，List页面，分页加载
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<TelphoneLiangEntity> GetPageListH5LX(Pagination pagination, string queryJson)
        {
            //机构前台H5
            List<string> viplist = null;
            //本机构sql
            string vipOrg = "";
            string ownOrgSql = "";//默认为空
            //分享平台机构sql
            string shareOrg = "";
            string shareOrgSql = "";//默认为空

            var queryParam = queryJson.ToJObject();
            if (queryParam["OrganizeIdH5"].IsEmpty())
            {
                return null;
            }

            string organizeId = queryParam["OrganizeIdH5"].ToString();
            string pid = queryParam["pid"].ToString();
            string top = queryParam["top"].ToString();

            string allOrg = "";
            //过滤出vip机构
            viplist = vipService.GetVipOrgList(organizeId, pid, top);
            foreach (var item in viplist)
            {
                vipOrg += "'" + item + "',";
            }
            //本身机构判断为空，返回null，vip全部过期不查了
            if (vipOrg == "")
            {
                return null;
            }

            //1.自身机构体系
            vipOrg = vipOrg.Trim(',');
            ownOrgSql = " and OrganizeId IN(" + vipOrg + ")";
            string ownWhere = ownOrgSql + GetSql(queryJson);

            //2.共享平台，限制类型
            string shareSql = "";
            shareOrg = GetOtherOrg(viplist);
            if (!shareOrg.IsEmpty())
            {
                allOrg = vipOrg + "," + shareOrg;
                shareOrgSql = " and OrganizeId IN(" + shareOrg + ")";
                string shareWhere = shareOrgSql + GetSql(queryJson);
                shareSql = @" UNION all
 SELECT TelphoneID,Telphone,City,Operator,Price,MaxPrice,Grade,ExistMark,CASE ExistMark WHEN '2' THEN '平台秒杀' WHEN '1' THEN '平台现卡' ELSE '平台预售' END Description,Package,EnabledMark,OrganizeId OrganizeId FROM TelphoneLiang
 WHERE  SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 and ExistMark=1 "//平台只卖现卡的
    + shareWhere;
            }
            else
            {
                allOrg = vipOrg;
            }

            //自身，父，0级
            string strSql = @" SELECT * FROM (
 SELECT TelphoneID,Telphone,City,Operator,Price,MaxPrice,Grade,ExistMark,CASE ExistMark WHEN '2' THEN '自营秒杀' WHEN '1' THEN '自营现卡' ELSE '自营预售' END Description,Package,EnabledMark,OrganizeId FROM TelphoneLiang 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 " + ownWhere//自身可以卖秒杀，现卡，预售
//+ otherOrgSql//代售功能省略
+ shareSql
                + " )t ";


            //按照机构排序
            string[] orderbyOrg = allOrg.Split(',');
            string orderbySql = " case ";
            for (int i = 0; i < orderbyOrg.Length; i++)
            {
                orderbySql += " when OrganizeId=" + orderbyOrg[i] + " then " + i;
            }
            orderbySql += " end ASC,";
            pagination.sidx = orderbySql + pagination.sidx + " " + pagination.sord;

            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }

        /// <summary>
        /// H5端查询按钮，List页面，分页加载
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<TelphoneLiangEntity> GetPageListH5(Pagination pagination, string queryJson)
        {
            //机构前台H5
            List<string> viplist = null;
            //本机构sql
            string vipOrg = "";
            string ownOrgSql = "";//默认为空
            //分享平台机构sql
            string shareOrg = "";
            string shareOrgSql = "";//默认为空

            var queryParam = queryJson.ToJObject();
            if (queryParam["OrganizeIdH5"].IsEmpty())
            {
                return null;
            }

            string organizeId = queryParam["OrganizeIdH5"].ToString();
            string pid = queryParam["pid"].ToString();
            string top = queryParam["top"].ToString();

            string allOrg = "";
            //过滤出vip机构
            viplist = vipService.GetVipOrgList(organizeId, pid, top);
            foreach (var item in viplist)
            {
                vipOrg += "'" + item + "',";
            }
            //本身机构判断为空，返回null，vip全部过期不查了
            if (vipOrg == "")
            {
                return null;
            }

            //1.自身机构体系
            vipOrg = vipOrg.Trim(',');
            ownOrgSql = " and OrganizeId IN(" + vipOrg + ")";
            string ownWhere = ownOrgSql + GetSql(queryJson);

            //2.代售机构表分支
            string otherWhere = ownWhere.Replace("OrganizeId", "CreateOrganizeId");
            string otherOrgSql = @" UNION all
 SELECT TelphoneID,Telphone,City,Operator,Price,MaxPrice,Grade,CASE ExistMark WHEN '2' THEN '代售秒杀' WHEN '1' THEN '代售现卡' ELSE '代售预售' END Description,Package,EnabledMark,CreateOrganizeId OrganizeId FROM TelphoneLiangOther
 WHERE EnabledMark = 1  "
+ otherWhere;

            //3.共享平台，限制类型
            string shareSql = "";
            shareOrg = GetOtherOrg(viplist);
            if (!shareOrg.IsEmpty())
            {
                allOrg = vipOrg + "," + shareOrg;
                shareOrgSql = " and OrganizeId IN(" + shareOrg + ")";
                string shareWhere = shareOrgSql + GetSql(queryJson);
                shareSql = @" UNION all
 SELECT TelphoneID,Telphone,City,Operator,Price,MaxPrice,Grade,CASE ExistMark WHEN '2' THEN '平台秒杀' WHEN '1' THEN '平台现卡' ELSE '平台预售' END Description,Package,EnabledMark,OrganizeId OrganizeId FROM TelphoneLiang
 WHERE  SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 "// and Grade IN (0,1,2,3,11,12,4,5,6,9,13,14,15,17,18,19,20,903,902,901,904,905,906,907,908,909,910,911,912,913,914,915,1301,1302,1303,1304,1308)
    + shareWhere;
            }
            else
            {
                allOrg = vipOrg;
            }

            //自身，父，0级
            string strSql = @" SELECT * FROM (
 SELECT TelphoneID,Telphone,City,Operator,Price,MaxPrice,Grade,CASE ExistMark WHEN '2' THEN '自营秒杀' WHEN '1' THEN '自营现卡' ELSE '自营预售' END Description,Package,EnabledMark,OrganizeId FROM TelphoneLiang 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 " + ownWhere
//+ otherOrgSql//代售功能省略
+ shareSql
                + " )t ";


            //按照机构排序
            string[] orderbyOrg = allOrg.Split(',');
            string orderbySql = " case ";
            for (int i = 0; i < orderbyOrg.Length; i++)
            {
                orderbySql += " when OrganizeId=" + orderbyOrg[i] + " then " + i;
            }
            orderbySql += " end ASC,";
            pagination.sidx = orderbySql+ pagination.sidx+" "+ pagination.sord;

            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
// Select * From (Select ROW_NUMBER() Over (Order By  case  when OrganizeId='485b4733-c958-4f46-94dd-3ff8ce444d2c' then 0 when OrganizeId='a5a962da-57e1-4ad4-87b2-bbdcd1b7cc92' then 1 when OrganizeId='2d418431-d2cd-41d7-a2f9-a749ca4ad907' then 2 end ASC,TelphoneID desc) As rowNum, * From ( SELECT * FROM (
// SELECT TelphoneID,Telphone,City,Operator,Price,Grade,CASE ExistMark WHEN '2' THEN '自营秒杀' WHEN '1' THEN '自营现卡' ELSE '自营预售' END Description,Package,EnabledMark,OrganizeId FROM TelphoneLiang 
// WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1  and OrganizeId IN('485b4733-c958-4f46-94dd-3ff8ce444d2c') and Grade in (1,2) and SellMark = 0 
// UNION all
// SELECT TelphoneID,Telphone,City,Operator,Price,Grade,CASE ExistMark WHEN '2' THEN '代售秒杀' WHEN '1' THEN '代售现卡' ELSE '代售预售' END Description,Package,EnabledMark,CreateOrganizeId OrganizeId FROM TelphoneLiangOther
// WHERE EnabledMark = 1 and CreateOrganizeId IN('485b4733-c958-4f46-94dd-3ff8ce444d2c') and Grade in (1,2) and SellMark = 0 
// UNION all
// SELECT TelphoneID,Telphone,City,Operator,Price,Grade,CASE ExistMark WHEN '2' THEN '平台秒杀' WHEN '1' THEN '平台现卡' ELSE '平台预售' END Description,Package,EnabledMark,OrganizeId OrganizeId FROM TelphoneLiang
// WHERE  SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 and Grade IN (0,1,2,3,11,12,4,5,6,9,13,14,15,17,18,19,20,903,902,901,904,905,906,907,908,909,910,911,912,913,914,915,1301,1302,1303,1304,1308)  and OrganizeId IN('a5a962da-57e1-4ad4-87b2-bbdcd1b7cc92','2d418431-d2cd-41d7-a2f9-a749ca4ad907') and Grade in (1,2) and SellMark = 0 )t ) As T ) As N Where rowNum > 0 And rowNum <= 40
        /// <summary>
        /// 首页部分精品推荐和秒杀
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphoneLiangEntity> GetList(string queryJson)
        {
            //机构前台H5
            List<string> viplist = null;
            //本机构sql
            string vipOrg = "";
            string ownOrgSql = "";//默认为空
            //分享平台机构sql
            string shareOrg = "";
            string shareOrgSql = "";//默认为空

            var queryParam = queryJson.ToJObject();
            if (queryParam["OrganizeIdH5"].IsEmpty())
            {
                return null;
            }

            string organizeId = queryParam["OrganizeIdH5"].ToString();
            string pid = queryParam["pid"].ToString();
            string top = queryParam["top"].ToString();
            //过滤出vip机构
            viplist = vipService.GetVipOrgList(organizeId, pid, top);
            foreach (var item in viplist)
            {
                vipOrg += "'" + item + "',";
            }
            //本身机构判断为空，返回null，vip全部过期不查了
            if (vipOrg == "")
            {
                return null;
            }


            //1.自身机构体系
            vipOrg = vipOrg.Trim(',');
            ownOrgSql = " and OrganizeId IN(" + vipOrg + ")";
            string ownWhere = ownOrgSql + GetSql(queryJson);

            //2.代售机构表分支
            string otherWhere = ownWhere.Replace("OrganizeId", "CreateOrganizeId");
            string otherOrgSql = @" UNION all
 SELECT TelphoneID,Telphone,City,Operator,Price,Grade,CASE ExistMark WHEN '2' THEN '代售秒杀' WHEN '1' THEN '代售现卡' ELSE '代售预售' END Description,Package,EnabledMark,CreateOrganizeId OrganizeId FROM TelphoneLiangOther
 WHERE EnabledMark = 1  "
+ otherWhere;

            //3.共享平台，限制类型
            string shareSql = "";
            shareOrg = GetOtherOrg(viplist);
            if (!shareOrg.IsEmpty())
            {
                shareOrgSql = " and OrganizeId IN(" + shareOrg + ")";
                string shareWhere = shareOrgSql + GetSql(queryJson);
                shareSql = @" UNION all
 SELECT TelphoneID,Telphone,City,Operator,Price,Grade,CASE ExistMark WHEN '2' THEN '平台秒杀' WHEN '1' THEN '平台现卡' ELSE '平台预售' END Description,Package,EnabledMark,OrganizeId OrganizeId FROM TelphoneLiang
 WHERE  SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 "// and Grade IN (0,1,2,3,11,12,4,5,6,9,13,14,15,17,18,19,20,903,902,901,904,905,906,907,908,909,910,911,912,913,914,915,1301,1302,1303,1304,1308)
    + shareWhere;
            }

            //自身，父，0级
            string strSql = @" SELECT TOP(10) * FROM (
 SELECT TelphoneID,Telphone,City,Operator,Price,Grade,CASE ExistMark WHEN '2' THEN '自营秒杀' WHEN '1' THEN '自营现卡' ELSE '自营预售' END Description,Package,EnabledMark,OrganizeId FROM TelphoneLiang 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 " + ownWhere
//+ otherOrgSql//代售功能省略
+ shareSql
                + " )t  ORDER BY Description desc,price ";//ORDER BY EnabledMark, right(Telphone,1),Description,price desc

            return this.BaseRepository().FindList(strSql.ToString());
        }

        /// <summary>
        /// 机构现售号码数量
        /// </summary>
        /// <param name="organizeId">机构</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetLiangCountByOrg(string organizeId)
        {
            if (string.IsNullOrEmpty(organizeId))
            {
                return null;
            }
            else
            {
                string strSql = $"select count(1) from TelphoneLiang where DeleteMark <> 1 and EnabledMark <> 1 and SellMark <> 1 and OrganizeId='{organizeId}'";
                return this.BaseRepository().FindTable(strSql.ToString());
            }

        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TelphoneLiangEntity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取分类列表（弃用）
        /// </summary>
        /// <param name="organizeId">靓号公司</param>
        /// <param name="Grade">查询参数</param>
        /// <param name="city"></param>
        /// <param name="ExistMark"></param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphoneLiangEntity> GetGrade(string organizeId, string Grade, string city, int? ExistMark)
        {
            if (string.IsNullOrEmpty(organizeId))
            {
                return null;
            }
            var organizeData = orgService.GetEntity(organizeId);
            if (!string.IsNullOrEmpty(organizeData.OrganizeId))
            {
                string orgSql = "";
                //分类条件
                string gradeSql = "";
                if (!Grade.IsEmpty())
                {
                    gradeSql += " and Grade = '" + Grade + "'";
                }
                //状态条件
                string existSql = "";
                if (!ExistMark.IsEmpty())
                {
                    existSql += " and ExistMark = " + ExistMark;
                }
                //城市条件
                string citySql = "";
                if (!string.IsNullOrEmpty(city))
                {
                    if (city.Contains("0000"))
                    {
                        citySql += " and cityid like '" + city.Substring(0, 2) + "%'";
                    }
                    else if (city != "0")
                    {
                        citySql += " and cityid = '" + city + "'";
                    }
                }
                //1.本机构 2.代售直属上级 3.顶级机构
                //orgSql = "and OrganizeId IN('" + organizeId + "','" + organizeData.ParentId + "','" + organizeData.TopOrganizeId + "')";
                //判断vip到期情况
                //1.本机构
                if (vipService.GetVipByOrganizeId(organizeId))
                {
                    organizeId = "'" + organizeId + "',";
                }
                else
                {
                    organizeId = "";
                }
                //2.代售直属上级
                string pid = "";
                if (!string.IsNullOrEmpty(organizeData.ParentId) && organizeData.ParentId != organizeData.OrganizeId)
                {
                    if (vipService.GetVipByOrganizeId(organizeData.ParentId))
                    {
                        pid = "'" + organizeData.ParentId + "',";
                    }
                }
                // 3.顶级机构
                string top = "";
                if (!string.IsNullOrEmpty(organizeData.TopOrganizeId) && organizeData.TopOrganizeId != organizeData.OrganizeId && organizeData.TopOrganizeId != organizeData.ParentId)
                {
                    if (vipService.GetVipByOrganizeId(organizeData.TopOrganizeId))
                    {
                        top = "'" + organizeData.TopOrganizeId + "',";
                    }
                }
                string inOrg = organizeId + pid + top;
                //如果机构全部过期，返回空
                if (string.IsNullOrEmpty(inOrg))
                {
                    return null;
                }
                orgSql = " and OrganizeId IN(" + inOrg + ")";
                orgSql = orgSql.Replace(",)", ")");


                //4.如果代售机构vip服务没有到期
                string otherOrgSql = @" UNION all
 SELECT Telphone,City,Operator,Price,Grade,CASE ExistMark WHEN '2' THEN '秒杀' WHEN '1' THEN '现卡' ELSE '预售' END Description,Package,EnabledMark FROM TelphoneLiangOther
 WHERE EnabledMark = 1 "
+ orgSql.Replace("OrganizeId", "CreateOrganizeId")
+ gradeSql
+ existSql
+ citySql;

                string strSql = @" SELECT * FROM (
 SELECT Telphone,City,Operator,Price,Grade,CASE ExistMark WHEN '2' THEN '秒杀' WHEN '1' THEN '现卡' ELSE '预售' END Description,Package,EnabledMark FROM TelphoneLiang 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 "
                    + orgSql
                    + gradeSql
                    + existSql
                    + citySql
                    + otherOrgSql
                    + " )t ORDER BY EnabledMark, right(Telphone,1),Description,price desc";
                return this.BaseRepository().FindList(strSql.ToString());

            }
            else
            {
                return null;
            }
        }

        
        /// <summary>
        /// 号码智能自动补全（老版本存在，新版弃用作废）
        /// </summary>
        /// <param name="telphone"></param>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public IEnumerable<TelphoneLiangEntity> GetList(string telphone, string organizeId,string city)
        {
            if (string.IsNullOrEmpty(organizeId))
            {
                return null;
            }
            var organizeData = orgService.GetEntity(organizeId);
            if (!string.IsNullOrEmpty(organizeData.OrganizeId))
            {
                //号码条件
                string telSql = "";
                if (!string.IsNullOrEmpty(telphone))
                {
                    telSql = " and Telphone like '%" + telphone + "%' ";
                }
                //城市条件
                string citySql = "";
                if (!string.IsNullOrEmpty(city))
                {
                    if (city.Contains("0000"))
                    {
                        citySql += " and cityid like '" + city.Substring(0, 2) + "%'";
                    }
                    else if (city != "0")
                    {
                        citySql += " and cityid = '" + city + "'";
                    }
                }

                string orgSql = "";
                //判断vip到期情况
                //1.本机构
                if (vipService.GetVipByOrganizeId(organizeId))
                {
                    organizeId = "'" + organizeId + "',";
                }
                else
                {
                    organizeId = "";
                }
                //2.代售直属上级
                string pid = "";
                if (!string.IsNullOrEmpty(organizeData.ParentId) && organizeData.ParentId != organizeData.OrganizeId)
                {
                    if (vipService.GetVipByOrganizeId(organizeData.ParentId))
                    {
                        pid = "'" + organizeData.ParentId + "',";
                    }
                }
                // 3.顶级机构
                string top = "";
                if (!string.IsNullOrEmpty(organizeData.TopOrganizeId) && organizeData.TopOrganizeId != organizeData.OrganizeId && organizeData.TopOrganizeId != organizeData.ParentId)
                {
                    if (vipService.GetVipByOrganizeId(organizeData.TopOrganizeId))
                    {
                        top = "'" + organizeData.TopOrganizeId + "',";
                    }
                }

                string inOrg = organizeId + pid + top;
                //如果机构全部过期，返回空
                if (string.IsNullOrEmpty(inOrg))
                {
                    return null;
                }
                orgSql = " and OrganizeId IN(" + inOrg + ")";
                orgSql = orgSql.Replace(",)", ")");

                //如果机构全部过期，返回空
                if (string.IsNullOrEmpty(organizeId) && string.IsNullOrEmpty(pid) && string.IsNullOrEmpty(top))
                {
                    return null;
                }
                //4.其它代售机构
                string otherOrgSql = "";
                //如果代售机构vip服务没有到期
                otherOrgSql = @" UNION all
 SELECT Telphone,City,Grade,ExistMark,price,EnabledMark FROM TelphoneLiangOther
 WHERE EnabledMark = 1 "
+ orgSql.Replace("OrganizeId", "CreateOrganizeId")
+ citySql
+ telSql;
                
                //输入号码，显示前20个号码
                string strSql = @" SELECT TOP 20 * FROM (
 SELECT Telphone,City,Grade,ExistMark,price,EnabledMark FROM TelphoneLiang
  WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 "
                    + orgSql
                    + citySql
                    + telSql
                    + otherOrgSql
                    + " )t ORDER BY EnabledMark,Grade,ExistMark desc,price desc";

                return this.BaseRepository().FindList(strSql.ToString());
            }
            else
            {
                return null;
            }
            

        }

        /// <summary>
        /// 手机端点击下拉列表号码的时候,直接全库搜索，因为既然可以点击号码，就必然在权限范围内，不再重复校验该号码是否属于本机构范围
        /// </summary>
        /// <param name="organizeId">当前机构</param>
        /// <param name="telphone">手机号</param>
        /// <returns></returns>
        public IEnumerable<TelphoneLiangEntity> GetEntityByTel(string organizeId, string telphone)
        {
            if (string.IsNullOrEmpty(organizeId))
            {
                return null;
            }
            string strSql = "select TelphoneID,Telphone,City,Operator,MinPrice,Price,ChaPrice,Grade,CASE ExistMark WHEN '2' THEN '秒杀' WHEN '1' THEN '现卡' ELSE '预售' END Description,Package,OrganizeId from TelphoneLiang"
               + " where DeleteMark <> 1 and EnabledMark <> 1 and SellMark <> 1";
            if (!telphone.IsEmpty())
            {
                strSql += " and Telphone='" + telphone + "'";
            }
            return this.BaseRepository().FindList(strSql);

        }
        public TelphoneLiangEntity GetEntityByOrgTel(string telphone)
        {
            return this.BaseRepository().FindEntity(t => t.Telphone == telphone);
        }

        /// <summary>
        /// 手机端点击查询按钮号码的时候，防止查询到其它机构的号码，加入机构判断（老版本存在，新版弃用作废）
        /// </summary>
        /// <param name="organizeId">当前机构</param>
        /// <param name="telphone">手机号</param>
        /// <returns></returns>
        public IEnumerable<TelphoneLiangEntity> GetEntityByOrgTel(string organizeId, string telphone)
        {

            if (string.IsNullOrEmpty(organizeId))
            {
                return null;
            }
            var organizeData = orgService.GetEntity(organizeId);
            if (!string.IsNullOrEmpty(organizeData.OrganizeId))
            {
                string telSql = "";
                if (!telphone.IsEmpty())
                {
                    telSql += " and Telphone='" + telphone + "'";
                }
                string orgSql = "";
                //1.本机构 2.代售直属上级 3.顶级机构
                //orgSql = "and OrganizeId IN('" + organizeId + "','" + organizeData.ParentId + "','" + organizeData.TopOrganizeId + "')";
                //判断vip到期情况
                //1.本机构
                if (vipService.GetVipByOrganizeId(organizeId))
                {
                    organizeId = "'" + organizeId + "',";
                }
                else
                {
                    organizeId = "";
                }
                //2.代售直属上级
                string pid = "";
                if (!string.IsNullOrEmpty(organizeData.ParentId) && organizeData.ParentId != organizeData.OrganizeId)
                {
                    if (vipService.GetVipByOrganizeId(organizeData.ParentId))
                    {
                        pid = "'" + organizeData.ParentId + "',";
                    }
                }
                // 3.顶级机构
                string top = "";
                if (!string.IsNullOrEmpty(organizeData.TopOrganizeId) && organizeData.TopOrganizeId != organizeData.OrganizeId && organizeData.TopOrganizeId != organizeData.ParentId)
                {
                    if (vipService.GetVipByOrganizeId(organizeData.TopOrganizeId))
                    {
                        top = "'" + organizeData.TopOrganizeId + "',";
                    }
                }
                string inOrg = organizeId + pid + top;
                //如果机构全部过期，返回空
                if (string.IsNullOrEmpty(inOrg))
                {
                    return null;
                }
                orgSql = " and OrganizeId IN(" + inOrg + ")";
                orgSql = orgSql.Replace(",)", ")");
                //4.其它代售机构
                string otherOrgSql = "";
                //如果代售机构vip服务没有到期
                otherOrgSql = @" UNION all
 SELECT TelphoneID,Telphone,City,Operator,MinPrice,Price,ChaPrice,Grade,CASE ExistMark WHEN '2' THEN '秒杀' WHEN '1' THEN '现卡' ELSE '预售' END Description,Package,OrganizeId FROM TelphoneLiangOther
 WHERE EnabledMark = 1 "
+ orgSql.Replace("OrganizeId", "CreateOrganizeId")
+ telSql;

                string strSql = @" 
 SELECT TelphoneID,Telphone,City,Operator,MinPrice,Price,ChaPrice,Grade,CASE ExistMark WHEN '2' THEN '秒杀' WHEN '1' THEN '现卡' ELSE '预售' END Description,Package,OrganizeId FROM TelphoneLiang 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 "
                    + telSql
                    + orgSql
                    + otherOrgSql;
                return this.BaseRepository().FindList(strSql.ToString());

            }
            else
            {
                return null;
            }
        }



        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValues">主键</param>
        public void RemoveForm(string keyValues)
        {
            //this.BaseRepository().Delete(keyValues);
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            if (!string.IsNullOrEmpty(keyValues))
            {
                string[] custIds = keyValues.Split(',');
                for (int i = 0; i < custIds.Length; i++)
                {
                    int? id = Convert.ToInt32(custIds[i]);
                    TelphoneLiangEntity entity = this.BaseRepository().FindEntity(id);
                    entity.Modify(custIds[i]);
                    entity.DeleteMark = 1;//软删除
                    this.BaseRepository().Update(entity);

                    //删除代售表相同号码
                    var otherList = db.FindList<TelphoneLiangOtherEntity>(t => t.Telphone == entity.Telphone);
                    foreach (var item in otherList)
                    {
                        db.Delete(item);
                    }
                }
                db.Commit();
            }
        }
        /// <summary>
        /// 上架数据
        /// </summary>
        /// <param name="keyValues">主键</param>
        public void UpForm(string keyValues)
        {
            //this.BaseRepository().Delete(keyValues);
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            if (!string.IsNullOrEmpty(keyValues))
            {
                string[] custIds = keyValues.Split(',');
                for (int i = 0; i < custIds.Length; i++)
                {
                    int? id = Convert.ToInt32(custIds[i]);
                    TelphoneLiangEntity entity = this.BaseRepository().FindEntity(id);
                    entity.Modify(custIds[i]);
                    entity.SellMark = 0;//销售状态
                    this.BaseRepository().Update(entity);

                    //上架代售表相同号码
                    var otherList = db.FindList<TelphoneLiangOtherEntity>(t => t.Telphone == entity.Telphone);
                    foreach (var item in otherList)
                    {
                        item.SellMark = 0;//销售状态
                        db.Update(item);
                    }
                }
                db.Commit();
            }
        }
        /// <summary>
        /// 现卡数据
        /// </summary>
        /// <param name="keyValues">主键</param>
        public void ExistForm(string keyValues)
        {
            //this.BaseRepository().Delete(keyValues);
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            if (!string.IsNullOrEmpty(keyValues))
            {
                string[] custIds = keyValues.Split(',');
                for (int i = 0; i < custIds.Length; i++)
                {
                    int? id = Convert.ToInt32(custIds[i]);
                    TelphoneLiangEntity entity = this.BaseRepository().FindEntity(id);
                    entity.Modify(custIds[i]);
                    entity.ExistMark = 1;//现卡
                    this.BaseRepository().Update(entity);

                    //现卡代售表相同号码
                    var otherList = db.FindList<TelphoneLiangOtherEntity>(t => t.Telphone == entity.Telphone);
                    foreach (var item in otherList)
                    {
                        item.ExistMark = 1;//现卡
                        db.Update(item);
                    }
                }
                db.Commit();
            }
        }

        /// <summary>
        /// 秒杀数据
        /// </summary>
        /// <param name="keyValues">主键</param>
        public void MiaoShaForm(string keyValues)
        {
            //this.BaseRepository().Delete(keyValues);
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            if (!string.IsNullOrEmpty(keyValues))
            {
                string[] custIds = keyValues.Split(',');
                for (int i = 0; i < custIds.Length; i++)
                {
                    int? id = Convert.ToInt32(custIds[i]);
                    TelphoneLiangEntity entity = this.BaseRepository().FindEntity(id);
                    entity.Modify(custIds[i]);
                    entity.ExistMark = 2;//秒杀
                    this.BaseRepository().Update(entity);

                    //现卡代售表相同号码
                    var otherList = db.FindList<TelphoneLiangOtherEntity>(t => t.Telphone == entity.Telphone);
                    foreach (var item in otherList)
                    {
                        item.ExistMark = 2;//秒杀
                        db.Update(item);
                    }
                }
                db.Commit();
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(int? keyValue, TelphoneLiangEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue.ToString()))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
                if (entity.SellMark==1)
                {
                    //删除代售表相同号码
                    IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                    var otherList = db.FindList<TelphoneLiangOtherEntity>(t => t.Telphone == entity.Telphone);
                    foreach (var item in otherList)
                    {
                        db.Delete(item);
                    }
                    db.Commit();
                }
            }
            else
            {
                //判断号码上限
                string greaterMsg = IsGreater(1);
                if (!string.IsNullOrEmpty(greaterMsg))
                {
                    throw new Exception(greaterMsg);
                }

                //根据前7位确定城市网络
                IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                string Number7 = entity.Telphone.Substring(0, 7);

                var TelphoneData = db.FindEntity<TelphoneDataEntity>(t => t.Number7 == Number7);
                if (TelphoneData != null)
                {
                    entity.City = TelphoneData.City.Replace("市", "");
                    entity.CityId = TelphoneData.CityId;
                    entity.Operator = TelphoneData.Operate;
                }
                else
                {
                    throw new Exception("号段不存在：" + Number7);
                }
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion

        #region 判断号码上限
        /// <summary>
        /// 导入号码个数
        /// </summary>
        /// <param name="rowsCount"></param>
        /// <returns></returns>
        public string IsGreater(int rowsCount)
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "";
            }
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            string companyid = OperatorProvider.Provider.Current().CompanyId;
            var vipEntity = db.FindEntity<TelphoneLiangVipEntity>(t => (t.OrganizeId == companyid || t.Description.Contains(companyid)) && t.VipEndDate > DateTime.Now);
            if (vipEntity==null)
            {
                return "未开通本功能或服务已到期，请联系管理员开通！";
            }
            if (string.IsNullOrEmpty(vipEntity.Id))
            {
                return "未开通本功能或服务已到期，请联系管理员开通！";
            }
            else
            {
                int uploadMax = Convert.ToInt32(vipEntity.UploadMax);//最大号码上传上限
                DataTable dt= GetLiangCountByOrg(companyid);
                int nowLiangCount = Convert.ToInt32(dt.Rows[0][0].ToString());
                if (nowLiangCount + rowsCount > uploadMax)
                {
                    return $"已上传号码{nowLiangCount}，本次上传号码{rowsCount}， 已超过上限{uploadMax}，请联系管理员追加套餐包！";
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        /// <summary>
        /// 批量（新增）
        /// </summary>
        /// <param name="dtSource">实体对象</param>
        /// <returns></returns>
        public string BatchAddEntity(DataTable dtSource)
        {
            int rowsCount = dtSource.Rows.Count;
            //判断号码上限
            string greaterMsg = IsGreater(rowsCount);
            if (!string.IsNullOrEmpty(greaterMsg))
            {
                return greaterMsg;
            }

            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();

            int columns = dtSource.Columns.Count;
            string cf = "";
            for (int i = 0; i < rowsCount; i++)
            {
                try
                {
                    string telphone = dtSource.Rows[i][0].ToString();
                    if (telphone.Length == 11)
                    {
                        var liang_Data = db.FindEntity<TelphoneLiangEntity>(t => t.Telphone == telphone && t.DeleteMark!=1);//删除过的可以再次导入
                        if (liang_Data != null)
                        {
                            cf+= telphone + ",";
                        }

                        //根据前7位确定城市和运营商
                        string Number7 = telphone.Substring(0, 7);
                        string City = "",CityId = "", Operator = "", Brand = "";
                        var TelphoneData = db.FindEntity<TelphoneDataEntity>(t => t.Number7 == Number7);
                        if (TelphoneData != null)
                        {
                            if (string.IsNullOrEmpty(TelphoneData.City))
                            {
                                return "号段城市为空：" + Number7;
                            }
                            else
                            {
                                City = TelphoneData.City.Replace("市", "");
                                CityId = TelphoneData.CityId;
                                Operator = TelphoneData.Operate;
                                Brand = TelphoneData.Brand;
                            }
                        }
                        else
                        {
                            return "号段不存在：" + Number7;
                        }

                        //价格
                        if (string.IsNullOrEmpty(dtSource.Rows[i][1].ToString()))
                        {
                            return telphone + "价格为空";
                        }
                        decimal Price = Convert.ToDecimal(dtSource.Rows[i][1].ToString());
                        //成本价
                        if (string.IsNullOrEmpty(dtSource.Rows[i][2].ToString()))
                        {
                            return telphone + "成本价为空";
                        }
                        decimal MinPrice = Convert.ToDecimal(dtSource.Rows[i][2].ToString());
                        //利润
                        decimal ChaPrice = Price - MinPrice;

                        //核算价
                        decimal CheckPrice = Convert.ToDecimal(dtSource.Rows[i][3].ToString());

                        //类别
                        string itemName = dtSource.Rows[i][4].ToString();
                        if (string.IsNullOrEmpty(itemName))
                        {
                            return telphone + "类别为空";
                        }
                        string itemNCode = "";
                        var DataItemDetail = db.FindEntity<DataItemDetailEntity>(t => t.ItemName == itemName);
                        if (DataItemDetail != null)
                        {
                            itemNCode = DataItemDetail.ItemValue;
                        }
                        else
                        {
                            return "类型不存在：" + itemName+",请在数据字典里维护此类型。";
                        }

                        //套餐
                        string Package = dtSource.Rows[i][5].ToString();
                        //状态
                        string existStr = dtSource.Rows[i][6].ToString();

                        if (string.IsNullOrEmpty(existStr))
                        {
                            return telphone + "现卡/代售/秒杀状态为空";
                        }
                        if (existStr!= "秒杀" && existStr != "现卡" && existStr != "预售")
                        {
                            return telphone + "现卡/代售/秒杀状态填写错误";
                        }
                        int existMark = 0;
                        if (existStr == "秒杀")
                        {
                            existMark = 2;
                        }
                        else if (existStr == "现卡")
                        {
                            existMark = 1;
                        }
                        else
                        {
                            existMark = 0;
                        }

                        //防止不含推广价此列 报错提示：无法找到列 7
                        decimal? MaxPrice = null;
                        if (columns==8)
                        {
                            //推广价
                            string maxPriceStr = dtSource.Rows[i][7].ToString();
                            //如果当前列的单元格报错，也会转类型错误
                            if (!string.IsNullOrEmpty(maxPriceStr))
                            {
                                MaxPrice = Convert.ToDecimal(maxPriceStr);
                            }
                        }


                        //添加靓号
                        TelphoneLiangEntity entity = new TelphoneLiangEntity()
                        {
                            Telphone = telphone,
                            Price = Price,
                            MaxPrice = MaxPrice,
                            MinPrice = MinPrice,
                            ChaPrice = ChaPrice,
                            CheckPrice= CheckPrice,
                            City = City,
                            CityId = CityId,
                            Operator = Operator,
                            Grade = itemNCode,
                            Package = Package,
                            Brand = Brand,
                            ExistMark = existMark,
                            SellMark = 0,
                            DeleteMark = 0,
                            OrganizeId = OperatorProvider.Provider.Current().CompanyId,
                        };
                        entity.Create();
                        db.Insert(entity);
                    }

                }
                catch (Exception ex)
                {
                    LogHelper.AddLog(ex.Message);
                    return ex.Message;
                }

            }
            db.Commit();
            if (cf!="")
            {
                LogHelper.AddLog("跳过重复导入号码：" + cf);
                return "跳过重复导入号码：" + cf;
            }
            else
            {
                return "导入成功";
            }
            
        }

        /// <summary>
        /// 批量下架
        /// </summary>
        /// <returns></returns>
        public string DownTelphone(string downTelphones)
        {
            string returnMsg = "";
            if (!string.IsNullOrEmpty(downTelphones))
            {
                IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                //string[] telphones = downTelphones.Split(new string[] {"%0A"}, StringSplitOptions.RemoveEmptyEntries);
                string[] telphones = downTelphones.Split('\n');
                for (int i = 0; i < telphones.Length; i++)
                {
                    string telphoneName = telphones[i];
                    if (!string.IsNullOrEmpty(telphoneName))
                    {
                        string telphone = telphoneName.Substring(0, 11);
                        string name = telphoneName.Replace(telphone, "").Trim();

                        //string companyid = OperatorProvider.Provider.Current().CompanyId;
                        var entity = db.FindEntity<TelphoneLiangEntity>(t => t.Telphone == telphone && t.SellMark == 0 && t.EnabledMark == 0 && t.DeleteMark == 0 );//&& t.OrganizeId == companyid
                        if (entity != null)
                        {
                            //var userEntity = db.FindEntity<UserEntity>(t => t.RealName == name);
                            //if (userEntity != null)
                            //{
                            //    entity.SellerName = userEntity.RealName;
                            //    entity.SellerId = userEntity.UserId;
                            //}
                            //else
                            //{
                            //    return name + " 不存在该用户！";
                            //}
                            entity.SellerName = name;

                            entity.SellMark = 1;
                            entity.Modify(entity.TelphoneID);
                            db.Update(entity);
                            returnMsg += telphone + " 已下架</br>";
                        }
                        else
                        {
                            returnMsg += telphone + " 不属于本公司或已售出</br>";
                        }
                        //删除代售表相同号码
                        var otherList = db.FindList<TelphoneLiangOtherEntity>(t => t.Telphone == telphone);
                        foreach (var item in otherList)
                        {
                            db.Delete(item);
                        }
                    }

                }
                db.Commit();
            }
            else
            {
                returnMsg = "未接受到任何数据";
            }

            return returnMsg;
        }

        /// <summary>
        /// 批量调价
        /// </summary>
        /// <returns></returns>
        public string PriceTelphone(string priceTelphones)
        {
            string returnMsg = "";
            if (!string.IsNullOrEmpty(priceTelphones))
            {
                IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                string[] telphones = priceTelphones.Split('\n');
                for (int i = 0; i < telphones.Length; i++)
                {
                    string telphoneTxt = telphones[i];
                    if (!string.IsNullOrEmpty(telphoneTxt))
                    {
                        string telphone = telphoneTxt.Substring(0, 11);
                        string[] telphonePrice = Regex.Split(telphoneTxt, "\t|\\s+", RegexOptions.IgnoreCase);//正则表达式
                        //售价
                        string priceTxt = telphonePrice[1].Trim();
                        decimal price = 0;
                        try
                        {
                            price = Convert.ToDecimal(priceTxt);
                        }
                        catch (Exception)
                        {
                            return telphone + " 售价转换格式错误！";
                        }
                        //成本价
                        string minPriceTxt = telphonePrice[2].Trim();
                        decimal minPrice = 0;
                        try
                        {
                            minPrice = Convert.ToDecimal(minPriceTxt);
                        }
                        catch (Exception)
                        {
                            return telphone + " 成本价转换格式错误！";
                        }
                        //利润
                        decimal chaPrice = price - minPrice;
                        if (chaPrice<0)
                        {
                            return telphone + " 成本价高于售价，价格顺序颠倒！";
                        }

                        //核算价
                        decimal checkPrice = 0;
                        if (telphonePrice.Length==4)
                        {
                            string checkPriceTxt = telphonePrice[3].Trim();
                            try
                            {
                                checkPrice = Convert.ToDecimal(checkPriceTxt);
                            }
                            catch (Exception)
                            {
                                return telphone + " 核算价价转换格式错误！";
                            }

                            decimal checkChaPrice = price - checkPrice;
                            if (checkChaPrice < 0)
                            {
                                return telphone + " 核算价高于售价，价格顺序颠倒！";
                            }
                        }


                        //string companyid = OperatorProvider.Provider.Current().CompanyId;
                        var entity = db.FindEntity<TelphoneLiangEntity>(t => t.Telphone == telphone && t.SellMark == 0 && t.EnabledMark == 0 && t.DeleteMark == 0 );//&& t.OrganizeId == companyid//一级机构可以操作
                        if (entity != null)
                        {
                            entity.MinPrice = minPrice;//成本价
                            entity.Price = price;//售价
                            entity.ChaPrice = chaPrice;//利润
                            entity.CheckPrice = checkPrice;//核算价
                            entity.Modify(entity.TelphoneID);
                            db.Update(entity);
                            returnMsg += telphone + " 已调价</br>";
                        }
                        else
                        {
                            returnMsg += telphone + " 不属于本公司或已售出</br>";
                        }
                    }

                }
                db.Commit();
            }
            else
            {
                returnMsg = "未接受到任何数据";
            }

            return returnMsg;
        }
        
    }
}
