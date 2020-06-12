using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.SystemManage;
using HZSoft.Application.IService.BaseManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.Service.BaseManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace HZSoft.Application.Service.CustomerManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2020-06-07 14:11
    /// 描 述：头条靓号库
    /// </summary>
    public class TelphoneLiangH5Service : RepositoryFactory<TelphoneLiangH5Entity>, TelphoneLiangH5IService
    {
        private IOrganizeService orgService = new OrganizeService();
        private TelphoneLiangVipIService vipService = new TelphoneLiangVipService();
        private TelphoneVipShareIService vipShareService = new TelphoneVipShareService();
        #region 获取数据

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
                    strSql += " and cityid like '" + city.Substring(0, 2) + "%'";
                }
                else if (city != "0")
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
            return shareOrg.Trim(',');
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneLiangH5Entity> GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneLiangH5 where DeleteMark <> 1 and EnabledMark <> 1";
            ////机构后台
            //if (!queryParam["OrganizeId"].IsEmpty())
            //{
            //    string OrganizeId = queryParam["OrganizeId"].ToString();
            //    strSql += " and OrganizeId = '" + OrganizeId + "'";
            //}
            //else
            //{
            //    if (!OperatorProvider.Provider.Current().IsSystem)
            //    {
            //        string companyId = OperatorProvider.Provider.Current().CompanyId;
            //        var organizeData = orgService.GetEntity(companyId);
            //        if (!string.IsNullOrEmpty(organizeData.OrganizeId))
            //        {
            //            //string inOrg = GetInOrg(companyId, organizeData.ParentId, organizeData.TopOrganizeId);
            //            List<string> vipList = vipService.GetVipOrgList(companyId, organizeData.ParentId, organizeData.TopOrganizeId);
            //            string inOrg = GetOtherOrg(vipList);//自定义优先，共享平台其次
            //            if (!string.IsNullOrEmpty(inOrg))
            //            {
            //                strSql += " and OrganizeId IN(" + inOrg + ")";
            //            }
            //        }
            //    }
            //}
            strSql += GetSql(queryJson);
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }


        /// <summary>
        /// H5端查询按钮，List页面，分页加载
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<TelphoneLiangH5Entity> GetPageListH5LX(Pagination pagination, string queryJson)
        {
            //本机构sql
            string ownOrgSql = "";//默认为空

            var queryParam = queryJson.ToJObject();
            if (queryParam["OrganizeIdH5"].IsEmpty())
            {
                return null;
            }

            string organizeId = queryParam["OrganizeIdH5"].ToString();
            ownOrgSql = " and OrganizeId ='" + organizeId + "'";
            string ownWhere = ownOrgSql + GetSql(queryJson);
            
            //自身，父，0级
            string strSql = @" SELECT * FROM (
 SELECT TelphoneID,Telphone,City,Operator,Price,MaxPrice,Grade,ExistMark,CASE ExistMark WHEN '2' THEN '自营秒杀' WHEN '1' THEN '自营现卡' ELSE '自营预售' END Description,
Package,EnabledMark,OrganizeId FROM TelphoneLiangH5 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 " + ownWhere
                + " )t ";
            
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphoneLiangH5Entity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TelphoneLiangH5Entity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
                    this.BaseRepository().Delete(id);
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
        public void SaveForm(int? keyValue, TelphoneLiangH5Entity entity)
        {
            if (keyValue!=0)
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
            int rowsCount = dtSource.Rows.Count;
            //判断号码上限
            //string greaterMsg = IsGreater(rowsCount);
            //if (!string.IsNullOrEmpty(greaterMsg))
            //{
            //    return greaterMsg;
            //}

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
                        var liang_Data = db.FindEntity<TelphoneLiangH5Entity>(t => t.Telphone == telphone && t.DeleteMark != 1);//删除过的可以再次导入
                        if (liang_Data != null)
                        {
                            cf += telphone + ",";
                        }

                        //根据前7位确定城市和运营商
                        string Number7 = telphone.Substring(0, 7);
                        string City = "", CityId = "", Operator = "", Brand = "";
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
                            return "类型不存在：" + itemName + ",请在数据字典里维护此类型。";
                        }

                        //套餐
                        string Package = dtSource.Rows[i][5].ToString();
                        //状态
                        string existStr = dtSource.Rows[i][6].ToString();

                        if (string.IsNullOrEmpty(existStr))
                        {
                            return telphone + "现卡/代售/秒杀状态为空";
                        }
                        if (existStr != "秒杀" && existStr != "现卡" && existStr != "预售")
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
                        if (columns == 8)
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
                        TelphoneLiangH5Entity entity = new TelphoneLiangH5Entity()
                        {
                            Telphone = telphone,
                            Price = Price,
                            MaxPrice = MaxPrice,
                            MinPrice = MinPrice,
                            ChaPrice = ChaPrice,
                            CheckPrice = CheckPrice,
                            City = City,
                            CityId = CityId,
                            Operator = Operator,
                            Brand = Brand,
                            Grade = itemNCode,
                            Package = Package,
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
            if (cf != "")
            {
                LogHelper.AddLog("跳过重复导入号码：" + cf);
                return "跳过重复导入号码：" + cf;
            }
            else
            {
                return "导入成功";
            }

        }
        #endregion
    }
}
