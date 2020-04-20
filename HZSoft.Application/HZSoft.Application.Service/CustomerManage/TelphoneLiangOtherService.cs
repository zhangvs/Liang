using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
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
    /// 日 期：2018-10-09 20:14
    /// 描 述：代售靓号库
    /// </summary>
    public class TelphoneLiangOtherService : RepositoryFactory<TelphoneLiangOtherEntity>, TelphoneLiangOtherIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneLiangOtherEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string companyId = OperatorProvider.Provider.Current().CompanyId;
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            var organizeData = db.FindEntity<OrganizeEntity>(t => t.OrganizeId == companyId);
            if (!OperatorProvider.Provider.Current().IsSystem)
            {
                if (!string.IsNullOrEmpty(organizeData.OtherOrganizeId))
                {
                    //默认EnabledMark =1为上架SELECT top 100 percent * FROM (
                    string strSql = "SELECT TelphoneID,Telphone,EnabledMark,ExistMark,Grade,Operator,City,Price,MinPrice,ChaPrice,ModifyUserName,ModifyDate,Package,CreateOrganizeId FROM TelphoneLiangOther" +
                                        " WHERE CreateOrganizeId='" + companyId + "'" +
                                        //关联代售机构的所有号码
                                        " UNION ALL " +
                                        " select TelphoneID,Telphone,EnabledMark,ExistMark,Grade,Operator,City,Price,MinPrice,ChaPrice,ModifyUserName,ModifyDate,Package,'' CreateOrganizeId " +
                                        " from TelphoneLiang where DeleteMark <> 1 and EnabledMark <> 1 and SellMark <> 1  and OrganizeId='" + organizeData.OtherOrganizeId + "' " +
                                        " and telphone not in (select Telphone from TelphoneLiangOther where CreateOrganizeId in ('" + organizeData.ParentId + "','" + organizeData.TopOrganizeId + "') ) ";
                    //单据编号
                    if (!queryParam["Telphone"].IsEmpty())
                    {
                        string Telphone = queryParam["Telphone"].ToString();
                        strSql += " and Telphone like '%" + Telphone + "%'";
                    }
                    else
                    {
                        //不在TelphoneLiangOther表内
                        strSql += " AND telphone NOT IN (SELECT Telphone FROM TelphoneLiangOther WHERE CreateOrganizeId='" + companyId + "')";
                    }
                    //分类
                    if (!queryParam["Grade"].IsEmpty())
                    {
                        string Grade = queryParam["Grade"].ToString();
                        strSql += " and Grade = '" + Grade + "'";
                    }
                    //分运营商
                    if (!queryParam["City"].IsEmpty())
                    {
                        string City = queryParam["City"].ToString();
                        strSql += " and City = '" + City + "'";
                    }
                    //上架标识
                    if (!queryParam["EnabledMark"].IsEmpty())
                    {
                        string EnabledMark = queryParam["EnabledMark"].ToString();
                        strSql += " and EnabledMark = " + EnabledMark;
                    }
                    return this.BaseRepository().FindList(strSql.ToString(), pagination);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                string strSql = "SELECT TelphoneID,Telphone,EnabledMark,ExistMark,Grade,Operator,City,Price,MinPrice,ChaPrice,ModifyUserName,ModifyDate,Package,CreateOrganizeId FROM TelphoneLiangOther";
                return this.BaseRepository().FindList(strSql.ToString(), pagination);
            }
        }

        /// <summary>
        /// 机构现代售号码数量
        /// </summary>
        /// <param name="organizeId">机构</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetLiangOtherCountByOrg(string organizeId)
        {
            if (string.IsNullOrEmpty(organizeId))
            {
                return null;
            }
            else
            {
                string strSql = $"select count(1) from TelphoneLiangOther where CreateOrganizeId='{organizeId}'";
                return this.BaseRepository().FindTable(strSql.ToString());
            }

        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphoneLiangOtherEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TelphoneLiangOtherEntity GetEntity(string keyValue)
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
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, TelphoneLiangOtherEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
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

        #region 判断号码上限
        /// <summary>
        /// 导入号码个数
        /// </summary>
        /// <param name="rowsCount"></param>
        /// <returns></returns>
        public string IsGreater(int rowsCount)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            string companyid = OperatorProvider.Provider.Current().CompanyId;
            var vipEntity = db.FindEntity<TelphoneLiangVipEntity>(t => t.OrganizeId == companyid && t.VipEndDate > DateTime.Now && t.DeleteMark != 1);
            if (vipEntity==null)
            {
                return "【代售其它机构号码】权限未开通或已过期,请联系管理员开通！";
            }
            else
            {
                int otherMax = Convert.ToInt32(vipEntity.OtherMax);//最大号码上传上限
                DataTable dt = GetLiangOtherCountByOrg(companyid);
                int nowLiangCount = Convert.ToInt32(dt.Rows[0][0].ToString());
                if (nowLiangCount + rowsCount > otherMax)
                {
                    return $"代售号码已超过上限{otherMax}，请联系管理员追加号码上限！";
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        /// <summary>
        /// 批量上架
        /// </summary>
        /// <param name="upTelphones">主键</param>
        public string UpsForm(string upTelphones)
        {
            if (!string.IsNullOrEmpty(upTelphones))
            {

                IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                string[] custIds = upTelphones.Split(',');
                int rowsCount = custIds.Length;
                //判断号码上限
                string greaterMsg = IsGreater(rowsCount);
                if (!string.IsNullOrEmpty(greaterMsg))
                {
                    //throw new Exception(greaterMsg);
                    return greaterMsg;
                }
                
                for (int i = 0; i < rowsCount; i++)
                {
                    int? id = Convert.ToInt32(custIds[i]);
                    var liangEntity = db.FindEntity<TelphoneLiangEntity>(t => t.TelphoneID == id);
                    if (liangEntity != null)
                    {
                        TelphoneLiangOtherEntity entity = new TelphoneLiangOtherEntity()
                        {
                            Telphone = liangEntity.Telphone,
                            EnabledMark = liangEntity.EnabledMark,
                            ExistMark = liangEntity.ExistMark,
                            Grade = liangEntity.Grade,
                            City = liangEntity.City,
                            Operator = liangEntity.Operator,
                            Price = liangEntity.Price,
                            MinPrice = liangEntity.MinPrice,
                            ChaPrice = liangEntity.ChaPrice,
                            ModifyUserName = liangEntity.ModifyUserName,
                            ModifyDate = liangEntity.ModifyDate,
                            Package = liangEntity.Package,
                            OrganizeId = liangEntity.OrganizeId
                        };
                        entity.Create();
                        this.BaseRepository().Insert(entity);
                    }

                }
                return "操作成功";
            }
            else
            {
                return "未选择上架号码！";
            }
        }


        /// <summary>
        /// 批量下架
        /// </summary>
        /// <param name="downTelphones">主键</param>
        public void DownsForm(string downTelphones)
        {
            if (!string.IsNullOrEmpty(downTelphones))
            {
                string[] custIds = downTelphones.Split(',');
                for (int i = 0; i < custIds.Length; i++)
                {
                    int? id = Convert.ToInt32(custIds[i]);
                    TelphoneLiangOtherEntity entity = this.BaseRepository().FindEntity(id);
                    if (entity!=null)
                    {
                        this.BaseRepository().Delete(entity);
                    }
                    
                }
            }
        }
        #endregion
    }
}
