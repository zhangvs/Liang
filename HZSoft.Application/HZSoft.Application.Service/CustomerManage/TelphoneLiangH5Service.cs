using HZSoft.Application.Code;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.Entity.SystemManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
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
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneLiangH5Entity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
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
                        var liang_Data = db.FindEntity<TelphoneLiangEntity>(t => t.Telphone == telphone && t.DeleteMark != 1);//删除过的可以再次导入
                        if (liang_Data != null)
                        {
                            cf += telphone + ",";
                        }

                        //根据前7位确定城市和运营商
                        string Number7 = telphone.Substring(0, 7);
                        string City = "", CityId = "", Operator = "";
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
                        TelphoneLiangEntity entity = new TelphoneLiangEntity()
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
