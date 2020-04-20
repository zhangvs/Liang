using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.BaseManage;
using HZSoft.Data.Repository;
using HZSoft.Util;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HZSoft.Application.Service.BaseManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2017-10-12 17:30
    /// 描 述：TelphoneCertification
    /// </summary>
    public class TelphoneCertificationService : RepositoryFactory<TelphoneCertificationEntity>, TelphoneCertificationIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneCertificationEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string strSql = "select * from TelphoneCertification where 1=1 ";
            //单据编号
            if (!queryParam["custName"].IsEmpty())
            {
                string custName = queryParam["custName"].ToString();
                strSql += " and custName = '" + custName + "'";
            }
            return this.BaseRepository().FindList(strSql.ToString(), pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphoneCertificationEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TelphoneCertificationEntity GetEntity(string keyValue)
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
            //删除老照片
            TelphoneCertificationEntity oldEntity = this.BaseRepository().FindEntity(keyValue);
            DirFileHelper.DeleteFile(oldEntity.photo_z);
            DirFileHelper.DeleteFile(oldEntity.photo_b);
            DirFileHelper.DeleteFile(oldEntity.photo_s);
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public string SaveForm(string keyValue, TelphoneCertificationEntity entity)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            SmsSingleSenderResult singleResult;
            SmsSingleSender singleSender = new SmsSingleSender(1400040861, "a92c87d0d291698777a9b5f323c0388a");

            TelphonePuEntity puData=db.FindEntity<TelphonePuEntity>(t => t.Telphone == entity.mobileNumber);
            TelphoneLiangEntity liangData = db.FindEntity<TelphoneLiangEntity>(t => t.Telphone == entity.mobileNumber);
            if (puData == null && liangData == null)
            {
                return "Error！认证的手机号码不存在！";
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                //发送短信
                if (entity.sendMessage==1)
                {
                    TelphoneCertificationEntity oldEntity = this.BaseRepository().FindEntity(keyValue);
                    if (oldEntity.loadMark == 0 && entity.loadMark == 2)
                    {
                        List<string> templParams = new List<string>();
                        templParams.Add(entity.mobileNumber);
                        //成功                
                        singleResult = singleSender.SendWithParam("86", entity.custTelphone, 172611, templParams, "", "", "");
                        //微信提醒模板:审核通过
                        WechatHelper.SendToOK(oldEntity.createId);
                    }
                    else if (oldEntity.loadMark == 0 && entity.loadMark == 1)
                    {
                        List<string> templParams = new List<string>();
                        templParams.Add(entity.mobileNumber);
                        templParams.Add(entity.description);
                        //失败
                        singleResult = singleSender.SendWithParam("86", entity.custTelphone, 172613, templParams, "", "", "");
                        //微信提醒模板：审核不通过
                        WechatHelper.SendToFail(oldEntity.createId,entity.description);
                    }
                }
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
                return "OK！操作成功";
            }
            else
            {
                //已经提交过的号码，覆盖
                var CertificationData = db.FindEntity<TelphoneCertificationEntity>(t => t.mobileNumber == entity.mobileNumber);
                if (CertificationData!=null)
                {
                    if (CertificationData.loadMark==2)
                    {
                        return "Error！该号码已经激活，无需再次提交。";
                    }
                    else if (CertificationData.loadMark == 0)
                    {
                        return "Error！正在认证中，请先耐心等待认证结果，不要重复提交！";
                    }
                    else
                    {
                        //失败的再处理
                        entity.loadCount= CertificationData.loadCount+1;//提交次数加1
                        entity.loadMark = 0;
                        entity.description = "";
                        entity.Modify(CertificationData.ID);
                        this.BaseRepository().Update(entity);
                        //删除老照片
                        DirFileHelper.DeleteFile(CertificationData.photo_z);
                        DirFileHelper.DeleteFile(CertificationData.photo_b);
                        DirFileHelper.DeleteFile(CertificationData.photo_s);
                        return "OK！再次上传成功！请耐心等待审核结果……";
                    }
                }
                else
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                    return "OK！提交成功！请耐心等待审核结果……";
                }

            }
        }
        #endregion
    }
}
