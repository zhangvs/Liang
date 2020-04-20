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
    public class TelphonePuService : RepositoryFactory<TelphonePuEntity>, TelphonePuIService
    {
        private IOrganizeService orgService = new OrganizeService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphonePuEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphonePu where DeleteMark <> 1 and EnabledMark <> 1";
            //单据编号
            if (!queryParam["Telphone"].IsEmpty())
            {
                string Telphone = queryParam["Telphone"].ToString();
                strSql += " and Telphone like '%" + Telphone + "%'";
            }
            //机构
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
                    //一级机构可以查看上级0级的号码库，因为自己人员工
                    strSql += " and OrganizeId IN( select OrganizeId from Base_Organize where OrganizeId='" + companyId
                        + "' or OrganizeId =(SELECT ParentId FROM Base_Organize WHERE OrganizeId='" + companyId + "'))";
                }
            }
            //分类
            if (!queryParam["Grade"].IsEmpty())
            {
                string Grade = queryParam["Grade"].ToString();
                strSql += " and Grade like '%" + Grade + "%'";
            }
            //分运营商
            if (!queryParam["City"].IsEmpty())
            {
                string City = queryParam["City"].ToString();
                strSql += " and City = '" + City + "'";
            }
            //售出标识
            if (!queryParam["SellMark"].IsEmpty())
            {
                string SellMark = queryParam["SellMark"].ToString();
                strSql += " and SellMark = " + SellMark;
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// 机构现售号码数量
        /// </summary>
        /// <param name="organizeId">机构</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPuCountByOrg(string organizeId)
        {
            if (string.IsNullOrEmpty(organizeId))
            {
                return null;
            }
            else
            {
                string strSql = $"select count(1) from TelphonePu where DeleteMark <> 1 and EnabledMark <> 1 and SellMark <> 1 and OrganizeId='{organizeId}'";
                return this.BaseRepository().FindTable(strSql.ToString());
            }

        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphonePuEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 号码智能自动补全
        /// </summary>
        /// <param name="telphone"></param>
        /// <param name="organizeId"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public IEnumerable<TelphonePuEntity> GetList(string telphone, string organizeId, string city)
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
                if (!string.IsNullOrEmpty(city) && city != "全国")
                {
                    citySql = " and city like '" + city.Substring(0, 2) + "%'";
                }
                //1.本机构 2.代售直属上级 3.顶级机构
                string orgSql = "and OrganizeId IN('" + organizeId + "','" + organizeData.ParentId + "','" + organizeData.TopOrganizeId + "')";


                //输入号码，显示前20个号码
                string strSql = @" SELECT TOP 20 * FROM (
 SELECT Telphone,City,Grade,ExistMark,price,EnabledMark FROM TelphoneLiang
  WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 "
                    + orgSql
                    + citySql
                    + telSql
 + @"  UNION ALL 
 SELECT Telphone,City,Grade,ExistMark,price,EnabledMark FROM TelphonePu
  WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 "
                    + citySql
                    + telSql
                    + " )t ORDER BY price desc";

                return this.BaseRepository().FindList(strSql.ToString());
            }
            else
            {
                return null;
            }


        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="organizeId">靓号公司</param>
        /// <param name="Grade">查询参数</param>
        /// <param name="city"></param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphonePuEntity> GetGrade(string organizeId, string Grade, string city)
        {
            if (string.IsNullOrEmpty(organizeId))
            {
                return null;
            }
            var organizeData = orgService.GetEntity(organizeId);
            if (!string.IsNullOrEmpty(organizeData.OrganizeId))
            {
                //分类条件
                string gradeSql = "";
                if (!Grade.IsEmpty())
                {
                    gradeSql += " and Grade like '%," + Grade + "%'";//,105,202,203数据库谱号类别都会加个逗号
                }
                //城市条件
                string citySql = "";
                if (!string.IsNullOrEmpty(city) && city != "全国")
                {
                    citySql = " and city like '" + city.Substring(0, 2) + "%'";
                }
                //1.本机构 2.代售直属上级 3.顶级机构
                //string orgSql = "and OrganizeId IN('" + organizeId + "','" + organizeData.ParentId + "','" + organizeData.TopOrganizeId + "')";

                string strSql = @" SELECT * FROM (
 SELECT Telphone,City,Operator,Price,Grade,CASE ExistMark WHEN '1' THEN '现卡' ELSE '预售' END Description,Package,EnabledMark FROM TelphonePu 
 WHERE SellMark<>1 AND DeleteMark<>1 and EnabledMark <> 1 "
                    //+ orgSql   谱号不检查机构
                    + gradeSql
                    + citySql
                    + " )t ORDER BY price desc";
                return this.BaseRepository().FindList(strSql.ToString());

            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TelphonePuEntity GetEntity(int? keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// 手机端点击号码的时候,直接全库搜索，因为既然可以点击号码，就必然在权限范围内，不再重复校验该号码是否属于本机构范围
        /// </summary>
        /// <param name="organizeId">当前机构</param>
        /// <param name="telphone">手机号</param>
        /// <returns></returns>
        public IEnumerable<TelphonePuEntity> GetEntityByOrgTel(string organizeId, string telphone)
        {
            if (string.IsNullOrEmpty(organizeId))
            {
                return null;
            }
            string strSql = "select TelphoneID,Telphone,City,Operator,MinPrice,Price,ChaPrice,Grade,CASE ExistMark WHEN '1' THEN '现卡' ELSE '预售' END Description,Package,OrganizeId from TelphoneLiang"
               + " where DeleteMark <> 1 and EnabledMark <> 1 and SellMark <> 1";
            if (!telphone.IsEmpty())
            {
                strSql += " and Telphone='" + telphone + "'";
            }
            strSql += " UNION ALL " +
               "select TelphoneID,Telphone,City,Operator,MinPrice,Price,ChaPrice,Grade,CASE ExistMark WHEN '1' THEN '现卡' ELSE '预售' END Description,Package,OrganizeId from TelphonePu"
               + " where DeleteMark <> 1 and EnabledMark <> 1 and SellMark <> 1";
            if (!telphone.IsEmpty())
            {
                strSql += " and Telphone='" + telphone + "'";
            }
            return this.BaseRepository().FindList(strSql);
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
                    TelphonePuEntity entity = this.BaseRepository().FindEntity(id);
                    entity.Modify(custIds[i]);
                    entity.DeleteMark = 1;//软删除
                    this.BaseRepository().Update(entity);
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
                    TelphonePuEntity entity = this.BaseRepository().FindEntity(id);
                    entity.Modify(custIds[i]);
                    entity.SellMark = 0;//销售状态
                    this.BaseRepository().Update(entity);
                    
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
                    TelphonePuEntity entity = this.BaseRepository().FindEntity(id);
                    entity.Modify(custIds[i]);
                    entity.ExistMark = 1;//现卡
                    this.BaseRepository().Update(entity);
                    
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
        public void SaveForm(int? keyValue, TelphonePuEntity entity)
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
        #endregion

        /// <summary>
        /// 尾号匹配：不再预定记录中（预定不作废的），没有售出，尾号
        /// </summary>
        /// <param name="end4"></param>
        /// <returns></returns>
        public IEnumerable<TelphonePuEntity> GetListEnd4(string end4)
        {
            string strSql = @"SELECT telphone,city,price,operator,'0' Description FROM TelphonePu WHERE SellMark =0 and DeleteMark =0 and substring(telphone,4,8)='0539" + end4 + "'" +
@"UNION ALL
SELECT telphone, city, price,operator,'1' Description FROM TelphoneLiang WHERE SellMark = 0 and DeleteMark =0 and substring(telphone,4, 8)= '0539" + end4 + "' ";
            return this.BaseRepository().FindList(strSql.ToString());
        }

        /// <summary>
        /// 批量（新增）
        /// </summary>
        /// <param name="dtSource">实体对象</param>
        /// <returns></returns>
        public string BatchAddEntity(DataTable dtSource)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            int rowsCount = dtSource.Rows.Count;

            for (int i = 0; i < rowsCount; i++)
            {
                try
                {
                    string telphone = dtSource.Rows[i][0].ToString();
                    if (telphone.Length == 11)
                    {
                        var liang_Data = db.FindEntity<TelphonePuEntity>(t => t.Telphone == telphone && t.DeleteMark != 1);//删除过的可以再次导入
                        if (liang_Data != null)
                        {
                            return telphone + "重复导入！";
                        }

                        //根据前7位确定城市和运营商
                        string Number7 = telphone.Substring(0, 7);
                        string City = "";
                        string Operator = "";
                        var TelphoneData = db.FindEntity<TelphoneDataEntity>(t => t.Number7 == Number7);
                        if (TelphoneData != null)
                        {
                            City = TelphoneData.City;
                            Operator = TelphoneData.Operate;
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
                        //if (string.IsNullOrEmpty(dtSource.Rows[i][2].ToString()))
                        //{
                        //    return telphone + "成本价为空";
                        //}
                        //decimal MinPrice = Convert.ToDecimal(dtSource.Rows[i][2].ToString());
                        decimal MinPrice = Convert.ToDecimal(Price* (decimal)0.55);
                        //利润
                        decimal ChaPrice = Price - MinPrice;

                        //类别
                        if (string.IsNullOrEmpty(dtSource.Rows[i][3].ToString()))
                        {
                            return telphone + "类别为空";
                        }
                        string itemName = dtSource.Rows[i][3].ToString();
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
                        string Package = dtSource.Rows[i][4].ToString();
                        //状态
                        if (string.IsNullOrEmpty(dtSource.Rows[i][5].ToString()))
                        {
                            return telphone + "现卡/代售状态为空";
                        }
                        string existStr = dtSource.Rows[i][5].ToString();
                        int existMark = 0;
                        if (existStr == "现卡")
                        {
                            existMark = 1;
                        }
                        else
                        {
                            existMark = 0;
                        }

                        //添加靓号
                        TelphonePuEntity entity = new TelphonePuEntity()
                        {
                            Telphone = telphone,
                            Price = Price,
                            MinPrice = MinPrice,
                            ChaPrice = ChaPrice,
                            City = City,
                            Operator = Operator,
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
                    return ex.Message;
                }

            }
            db.Commit();
            return "导入成功";
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

                        string companyid = OperatorProvider.Provider.Current().CompanyId;
                        var entity = db.FindEntity<TelphonePuEntity>(t => t.Telphone == telphone && t.SellMark == 0 && t.EnabledMark == 0 && t.DeleteMark == 0 && t.OrganizeId == companyid);
                        if (entity != null)
                        {
                            var userEntity = db.FindEntity<UserEntity>(t => t.RealName == name);
                            if (userEntity != null)
                            {
                                entity.SellerName = userEntity.RealName;
                                entity.SellerId = userEntity.UserId;
                            }
                            else
                            {
                                return name + " 不存在该用户！";
                            }

                            entity.SellMark = 1;
                            entity.Modify(entity.TelphoneID);
                            db.Update(entity);
                            returnMsg += telphone + " 已下架</br>";
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
                        //string[] telphonePrice = telphoneTxt.Split(' ');
                        string[] telphonePrice = Regex.Split(telphoneTxt, "\t|\\s+", RegexOptions.IgnoreCase);//正则表达式
                        string telphone = telphoneTxt.Substring(0, 11);

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

                        decimal chaPrice = price - minPrice;
                        if (chaPrice < 0)
                        {
                            return telphone + " 成本价高于售价，价格顺序颠倒！";
                        }

                        string companyid = OperatorProvider.Provider.Current().CompanyId;
                        var entity = db.FindEntity<TelphonePuEntity>(t => t.Telphone == telphone && t.SellMark == 0 && t.EnabledMark == 0 && t.DeleteMark == 0 && t.OrganizeId == companyid);
                        if (entity != null)
                        {
                            entity.MinPrice = minPrice;
                            entity.Price = price;
                            entity.ChaPrice = chaPrice;
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
