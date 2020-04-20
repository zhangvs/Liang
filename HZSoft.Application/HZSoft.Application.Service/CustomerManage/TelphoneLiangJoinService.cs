using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Service.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Data.Repository;
using HZSoft.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using HZSoft.Application.Code;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Application.IService.BaseManage;
using System;
using System.Data;

namespace HZSoft.Application.Service.CustomerManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-09-05 17:58
    /// 描 述：靓号加盟代理
    /// </summary>
    public class TelphoneLiangJoinService : RepositoryFactory<TelphoneLiangJoinEntity>, TelphoneLiangJoinIService
    {
        private IOrganizeService orgService = new OrganizeService();
        private TelphoneLiangVipIService telphoneLiangVipIService = new TelphoneLiangVipService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneLiangJoinEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var expression = LinqExtensions.True<TelphoneLiangJoinEntity>();
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneLiangJoin where DeleteMark <> 1 ";

            //客户名称
            if (!queryParam["Keyword"].IsEmpty())
            {
                string Keyword = queryParam["Keyword"].ToString();
                strSql += " and NickName like '%" + Keyword + "%'";
            }
            if (!queryParam["Telphone"].IsEmpty())
            {
                string Telphone = queryParam["Telphone"].ToString();
                strSql += " and Telphone = '" + Telphone + "'";
            }
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
                    if (!string.IsNullOrEmpty(companyId))
                    {
                        strSql += " and OrganizeId ='" + companyId + "' and BoosMark <> 1 ";//只显示代理，不显示供应商
                    }
                }
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }

        /// <summary>
        /// 加盟代理查看来源机构
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneLiangJoinEntity> GetPageList1(Pagination pagination, string queryJson)
        {
            var expression = LinqExtensions.True<TelphoneLiangJoinEntity>();
            var queryParam = queryJson.ToJObject();

            string strSql = @"select j.Id,j.CompanyName,j.FullName,j.Telphone,j.Pro,j.City,j.Area,j.Address,j.OpenId,j.NickName,
j.Sex,j.HeadimgUrl,j.WXPro,j.WxCity,j.WxAccount,j.WxQRcode,j.OrganizeId,j.BoosMark,j.LiangCount,j.AgentMark,j.TopMark,j.CheckMark,j.DeleteMark,j.Description,j.FollowDes,
j.CreateTime,j.ModifyTime,j.ModifyUserId,j.ModifyUserName,o.Category Des1,o.TopOrganizeId Des2 from TelphoneLiangJoin j
LEFT JOIN Base_Organize o ON j.OrganizeId = o.OrganizeId
where j.DeleteMark <> 1 and BoosMark <> 1";

            //客户名称
            if (!queryParam["Keyword"].IsEmpty())
            {
                string Keyword = queryParam["Keyword"].ToString();
                strSql += " and NickName like '%" + Keyword + "%'";
            }
            if (!queryParam["Telphone"].IsEmpty())
            {
                string Telphone = queryParam["Telphone"].ToString();
                strSql += " and Telphone = '" + Telphone + "'";
            }
            if (!queryParam["CheckMark"].IsEmpty())
            {
                string CheckMark = queryParam["CheckMark"].ToString();
                strSql += " and j.CheckMark = '" + CheckMark + "'";
            }
            else
            {
                strSql += " and j.CheckMark = 0";//默认显示未审核
            }
            if (!queryParam["Category"].IsEmpty())
            {
                string Category = queryParam["Category"].ToString();
                strSql += " and o.Category = '" + Category + "'";
            }

            if (!queryParam["OrganizeId"].IsEmpty())
            {
                string OrganizeId = queryParam["OrganizeId"].ToString();
                strSql += " and o.OrganizeId = '" + OrganizeId + "'";
            }
            else
            {
                if (!OperatorProvider.Provider.Current().IsSystem)
                {
                    string companyId = OperatorProvider.Provider.Current().CompanyId;
                    if (!string.IsNullOrEmpty(companyId))
                    {
                        strSql += " and j.OrganizeId ='" + companyId + "'";//只显示代理，不显示供应商
                    }
                }
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// 供应商查看
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneLiangJoinEntity> GetPageList2(Pagination pagination, string queryJson)
        {
            var expression = LinqExtensions.True<TelphoneLiangJoinEntity>();
            var queryParam = queryJson.ToJObject();
            string strSql = @"select j.Id,j.CompanyName,j.FullName,j.Telphone,j.Pro,j.City,j.Area,j.Address,j.OpenId,j.NickName,
j.Sex,j.HeadimgUrl,j.WXPro,j.WxCity,j.WxAccount,j.WxQRcode,j.OrganizeId,j.BoosMark,j.LiangCount,j.AgentMark,j.TopMark,j.CheckMark,j.DeleteMark,j.Description,
j.CreateTime,j.ModifyTime,j.ModifyUserId,j.ModifyUserName,o.Category Des1,o.TopOrganizeId Des2 from TelphoneLiangJoin j
LEFT JOIN Base_Organize o ON j.OrganizeId = o.OrganizeId
where j.DeleteMark <> 1 and BoosMark = 1";

            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphoneLiangJoinEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TelphoneLiangJoinEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 手机号不能重复
        /// </summary>
        /// <param name="telphone">公司名称</param>
        /// <returns></returns>
        public bool NotExistTelphone(string telphone)
        {
            var expression = LinqExtensions.True<TelphoneLiangJoinEntity>();
            expression = expression.And(t => t.Telphone == telphone);
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
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
        public void SaveForm(string keyValue, TelphoneLiangJoinEntity entity)
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
                //供应商不发送短信提醒
                if (entity.BoosMark != 1)
                {
                    //短信提醒上级审核
                    IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                    var parentOrg = db.FindEntity<OrganizeEntity>(t => t.OrganizeId == entity.OrganizeId);
                    //只给三级以上，0，1，2级发送短信
                    if (parentOrg != null && parentOrg.Category < 3)
                    {
                        SmsSingleSenderResult singleResult;
                        SmsSingleSender singleSender = new SmsSingleSender(1400040861, "a92c87d0d291698777a9b5f323c0388a");
                        List<string> templParams = new List<string>();
                        templParams.Add(parentOrg.FullName);
                        singleResult = singleSender.SendWithParam("86", parentOrg.OuterPhone, 205528, templParams, "", "", "");
                    }
                    //申请加盟+1
                    orgService.UpdateJoinCount(entity.OrganizeId);
                }
                else
                {
                    SmsSingleSenderResult singleResult;
                    SmsSingleSender singleSender = new SmsSingleSender(1400040861, "a92c87d0d291698777a9b5f323c0388a");
                    List<string> templParams = new List<string>();
                    templParams.Add("管理员");
                    singleResult = singleSender.SendWithParam("86", "18660996839", 205528, templParams, "", "", "");
                }
            }
        }

        /// <summary>
        /// 核单
        /// </summary>
        /// <param name="entity">主键值</param>
        /// <param name="State">状态：1-启动；0-禁用</param>
        public string UpdateTopOrg(TelphoneLiangJoinEntity entity, int State)
        {
            //获取0级机构id
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();

            //生成靓号商场平台
            OrganizeEntity orgEntity = new OrganizeEntity()
            {
                ParentId = "0",//0级机构
                FullName = entity.CompanyName,
                ShortName = entity.NickName,
                OuterPhone = entity.Telphone,
                InnerPhone = entity.WxAccount,//微信账号
                Nature = entity.WxQRcode,//微信二维码
                ManagerId = entity.OpenId,
                Manager = entity.FullName,
                Layer = 1,
                DeleteMark = 0
            };
            OrganizeEntity newEntity = orgService.SaveReturnEntity(orgEntity);

            //更新申请状态
            TelphoneLiangJoinEntity reserveEntity = new TelphoneLiangJoinEntity();
            reserveEntity.Modify(entity.Id);
            reserveEntity.TopMark = State;
            this.BaseRepository().Update(reserveEntity);

            //创建vip机构
            TelphoneLiangVipEntity telphoneLiangVipEntity = new TelphoneLiangVipEntity()
            {
                OrganizeId = newEntity.OrganizeId,
                FullName = newEntity.FullName,
                UploadMax = 1000,
                OtherMax = 0,
                OrgMax = 10,
                Price = 0,
                VipStartDate = DateTime.Now,
                VipEndDate = DateTime.Now.AddDays(7)
            };
            telphoneLiangVipIService.SaveForm(null, telphoneLiangVipEntity);

            //发送通过短信
            if (!string.IsNullOrEmpty(newEntity.Description))
            {
                SmsSingleSenderResult singleResult;
                SmsSingleSender singleSender = new SmsSingleSender(1400040861, "a92c87d0d291698777a9b5f323c0388a");
                List<string> templParams = new List<string>();
                templParams.Add(entity.FullName);
                templParams.Add(newEntity.Description);
                //成功                
                singleResult = singleSender.SendWithParam("86", entity.Telphone, 205617, templParams, "", "", "");
            }

            return $"已通过短信的方式通知了你的下级，其靓号商城为：{newEntity.Description}";
        }

        /// <summary>
        /// 核单
        /// </summary>
        /// <param name="entity">主键值</param>
        /// <param name="State">状态：1-启动；0-禁用</param>
        public string UpdateCheckState(TelphoneLiangJoinEntity entity, int State)
        {
            //获取0级机构id
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();

            if (OperatorProvider.Provider.Current().Account != "System")
            {
                var checkEntity = db.FindEntity<OrganizeEntity>(t => t.OuterPhone == entity.Telphone);
                if (checkEntity != null)
                {
                    return "该申请人的手机号已经通过审核，请刷新机构列表查看！";
                }

                var parentOrg0 = orgService.GetParentIdByOrgId(OperatorProvider.Provider.Current().CompanyId);//获得当前机构的所属0级机构
                if (!string.IsNullOrEmpty(parentOrg0.First().OrganizeId))
                {
                    string parentOrg = parentOrg0.First().OrganizeId;
                    //获取0级vip表
                    var vipEntity = db.FindEntity<TelphoneLiangVipEntity>(t => t.OrganizeId == parentOrg && t.VipEndDate > DateTime.Now);
                    if (vipEntity == null)
                    {
                        return "平台机构未开放【申请加盟】功能！";
                    }
                    else
                    {
                        int? orgMax = vipEntity.OrgMax;
                        DataTable dt = orgService.GetOrgCount(parentOrg0.First().OrganizeId);//获取0级机构数量
                        int parentOrgCount = Convert.ToInt32(dt.Rows[0][0].ToString());
                        //如果0级机构，超过了设置的机构上限
                        if (parentOrgCount > orgMax)
                        {
                            return "平台机构已超过上限，请联系上级增加机构容量！";
                        }
                    }
                }
            }

            if (State==2)
            {
                entity.OrganizeId = "e21c39be-da56-4f1c-9120-a0926a520947";//修改审核机构为我
            }

            //判断2,3级数量是个超限

            //生成靓号商场平台
            OrganizeEntity orgEntity = new OrganizeEntity()
            {
                ParentId = entity.OrganizeId,
                FullName = entity.CompanyName,
                ShortName = entity.NickName,
                OuterPhone = entity.Telphone,
                InnerPhone = entity.WxAccount,//微信账号
                Nature = entity.WxQRcode,//微信二维码
                ManagerId = entity.OpenId,
                Manager = entity.FullName,
                Layer = 1,
                DeleteMark = 0
            };
            OrganizeEntity newEntity = orgService.SaveReturnEntity(orgEntity);

            //更新申请状态
            TelphoneLiangJoinEntity reserveEntity = new TelphoneLiangJoinEntity();
            reserveEntity.Modify(entity.Id);
            reserveEntity.CheckMark = State;
            reserveEntity.OrganizeId = entity.OrganizeId;//审核机构为我
            this.BaseRepository().Update(reserveEntity);


            //发送通过短信
            if (!string.IsNullOrEmpty(newEntity.Description))
            {
                SmsSingleSenderResult singleResult;
                SmsSingleSender singleSender = new SmsSingleSender(1400040861, "a92c87d0d291698777a9b5f323c0388a");
                List<string> templParams = new List<string>();
                templParams.Add(entity.FullName);
                templParams.Add(newEntity.Description);
                //成功                
                singleResult = singleSender.SendWithParam("86", entity.Telphone, 205617, templParams, "", "", "");
            }

            return $"已通过短信的方式通知了你的下级，其靓号商城为：{newEntity.Description}";
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="State">状态：1-启动；0-禁用</param>
        public void UpdateDeleteState(string keyValue, int State)
        {
            TelphoneLiangJoinEntity reserveEntity = new TelphoneLiangJoinEntity();
            reserveEntity.Modify(keyValue);
            reserveEntity.DeleteMark = State;
            this.BaseRepository().Update(reserveEntity);
            var entity = this.BaseRepository().FindEntity(keyValue);
            if (entity != null)
            {
                SmsSingleSenderResult singleResult;
                SmsSingleSender singleSender = new SmsSingleSender(1400040861, "a92c87d0d291698777a9b5f323c0388a");
                List<string> templParams = new List<string>();
                templParams.Add(entity.FullName);
                //申请失败提醒             
                singleResult = singleSender.SendWithParam("86", entity.Telphone, 205531, templParams, "", "", "");
            }
        }
        #endregion


    }
}
