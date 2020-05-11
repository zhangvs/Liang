using HZSoft.Application.Entity.AuthorizeManage;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Application.Entity.CustomerManage;
using HZSoft.Application.IService.CustomerManage;
using HZSoft.Application.Service.BaseManage;
using HZSoft.Cache.Redis;
using HZSoft.Data.Repository;
using HZSoft.Util.Extension;
using HZSoft.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace HZSoft.Application.Service.CustomerManage
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创 建：超级管理员
    /// 日 期：2018-10-17 21:56
    /// 描 述：VIP服务机构
    /// </summary>
    public class TelphoneLiangVipService : RepositoryFactory<TelphoneLiangVipEntity>, TelphoneLiangVipIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<TelphoneLiangVipEntity> GetPageList(Pagination pagination, string queryJson)
        {
            Expression<Func<TelphoneLiangVipEntity, bool>> expression = LinqExtensions.True<TelphoneLiangVipEntity>();
            expression = expression.And(t => t.DeleteMark != 1);
            return this.BaseRepository().FindList(expression,pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TelphoneLiangVipEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 判断当前机构是否到期
        /// </summary>
        /// <param name="organizeId"></param>
        /// <returns></returns>
        public bool GetVipByOrganizeId(string organizeId)
        {
            int count = this.BaseRepository().IQueryable(t => t.OrganizeId == organizeId && t.VipEndDate > DateTime.Now).Count();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 判断机构是否逾期，所售机构范围
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="pid"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<string> GetVipOrgList(string organizeId, string pid, string top)
        {
            List<string> vipList = new List<string>();
            //判断vip到期情况
            //1.本机构
            if (GetVipByOrganizeId(organizeId))
            {
                vipList.Add(organizeId);
            }
            else
            {
                organizeId = "";
            }
            //2.代售直属上级
            if (!string.IsNullOrEmpty(pid) && organizeId != pid)
            {
                if (GetVipByOrganizeId(pid))
                {
                    vipList.Add(pid);
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
                if (GetVipByOrganizeId(top))
                {
                    vipList.Add(top);
                }
            }

            return vipList;
        }

        /// <summary>
        /// 判断当前机构是否加入共享平台
        /// </summary>
        /// <param name="organizeId"></param>
        /// <returns></returns>
        public bool IsShareMark(string organizeId)
        {
            int count = this.BaseRepository().IQueryable(t => t.OrganizeId == organizeId && t.ShareMark == 1).Count();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TelphoneLiangVipEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable()
        {
            return this.BaseRepository().FindTable("SELECT * FROM TelphoneLiangVip");
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
        public void SaveForm(string keyValue, TelphoneLiangVipEntity entity)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            if (!string.IsNullOrEmpty(keyValue))
            {
                var oldEntity = db.FindEntity<TelphoneLiangVipEntity>(t => t.Id == keyValue);
                //加入共享平台
                if (entity.ShareMark == 1 && oldEntity.ShareMark != 1)
                {
                    string shareOrg = RedisCache.Get<string>("ShareOrg");
                    if (!string.IsNullOrEmpty(shareOrg))
                    {
                        if (!shareOrg.Contains(entity.OrganizeId))
                        {
                            RedisCache.Set<string>("ShareOrg", shareOrg + ",'" + entity.OrganizeId + "'");
                        }
                    }
                }

                //取消共享
                if (entity.ShareMark != 1 && oldEntity.ShareMark == 1)
                {
                    string shareOrg = RedisCache.Get<string>("ShareOrg");
                    if (!string.IsNullOrEmpty(shareOrg))
                    {
                        RedisCache.Set<string>("ShareOrg", shareOrg.Replace(",'" + entity.OrganizeId + "'", ""));
                    }
                }

                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
                //修改机构表中的vip标识位
                var orgEntity = db.FindEntity<OrganizeEntity>(t => t.OrganizeId == entity.OrganizeId);
                orgEntity.VipMark = 1;
                db.Update(orgEntity);
                
                //根据机构id获取角色id
                var roleEntity=db.FindEntity<RoleEntity>(t => t.OrganizeId == entity.OrganizeId);
                //如果上传号码上限>0，添加'靓号库'菜单,Base_Module菜单id
                if (entity.UploadMax>0)
                {
                    var authorize = db.FindEntity<AuthorizeEntity>(t => t.ObjectId == roleEntity.RoleId && t.ItemId== "16cea332-0204-4f7d-a510-0426329205ff");
                    if (authorize==null)
                    {
                        AuthorizeEntity authorizeEntity = new AuthorizeEntity();
                        authorizeEntity.Create();
                        authorizeEntity.Category = 2;  //1 - 部门2 - 角色3 - 岗位4 - 职位5 - 工作组
                        authorizeEntity.ObjectId = roleEntity.RoleId;//角色id，角色限定了机构和部门
                        authorizeEntity.ItemType = 1; //项目类型: 1 - 菜单2 - 按钮3 - 视图4表单
                        authorizeEntity.ItemId = "16cea332-0204-4f7d-a510-0426329205ff";//项目主键
                        authorizeEntity.SortCode = 100;
                        db.Insert(authorizeEntity);
                    }
                }

                //如果代售号码上限>0，添加'代售靓号库'菜单,Base_Module菜单id
                if (entity.OtherMax>0)
                {
                    var authorize = db.FindEntity<AuthorizeEntity>(t => t.ObjectId == roleEntity.RoleId && t.ItemId == "effbaf18-e4d1-4250-9aa0-370422634a21");
                    if (authorize == null)
                    {
                        AuthorizeEntity authorizeEntity2 = new AuthorizeEntity();
                        authorizeEntity2.Create();
                        authorizeEntity2.Category = 2;  //1 - 部门2 - 角色3 - 岗位4 - 职位5 - 工作组
                        authorizeEntity2.ObjectId = roleEntity.RoleId;//角色id，角色限定了机构和部门
                        authorizeEntity2.ItemType = 1; //项目类型: 1 - 菜单2 - 按钮3 - 视图4表单
                        authorizeEntity2.ItemId = "effbaf18-e4d1-4250-9aa0-370422634a21";//项目主键
                        authorizeEntity2.SortCode = 100;
                        db.Insert(authorizeEntity2);
                    }
                }
                //报表
                var authorize6 = db.FindEntity<AuthorizeEntity>(t => t.ObjectId == roleEntity.RoleId && t.ItemId == "6");
                if (authorize6 == null)
                {
                    AuthorizeEntity authorizeEntity2 = new AuthorizeEntity();
                    authorizeEntity2.Create();
                    authorizeEntity2.Category = 2;  //1 - 部门2 - 角色3 - 岗位4 - 职位5 - 工作组
                    authorizeEntity2.ObjectId = roleEntity.RoleId;//角色id，角色限定了机构和部门
                    authorizeEntity2.ItemType = 1; //项目类型: 1 - 菜单2 - 按钮3 - 视图4表单
                    authorizeEntity2.ItemId = "6";//项目主键
                    authorizeEntity2.SortCode = 12;
                    db.Insert(authorizeEntity2);
                }
                //靓号销售报表
                var authorize61 = db.FindEntity<AuthorizeEntity>(t => t.ObjectId == roleEntity.RoleId && t.ItemId == "44498bc7-33ac-418f-a0f8-c88241afa118");
                if (authorize61 == null)
                {
                    AuthorizeEntity authorizeEntity2 = new AuthorizeEntity();
                    authorizeEntity2.Create();
                    authorizeEntity2.Category = 2;  //1 - 部门2 - 角色3 - 岗位4 - 职位5 - 工作组
                    authorizeEntity2.ObjectId = roleEntity.RoleId;//角色id，角色限定了机构和部门
                    authorizeEntity2.ItemType = 1; //项目类型: 1 - 菜单2 - 按钮3 - 视图4表单
                    authorizeEntity2.ItemId = "44498bc7-33ac-418f-a0f8-c88241afa118";//项目主键
                    authorizeEntity2.SortCode = 13;
                    db.Insert(authorizeEntity2);
                }

                db.Commit();

                //添加缓存分享平台
                if (entity.ShareMark==1)
                {
                    string shareOrg = RedisCache.Get<string>("ShareOrg");
                    if (!string.IsNullOrEmpty(shareOrg))
                    {
                        RedisCache.Set<string>("ShareOrg", shareOrg + ",'" + entity.OrganizeId + "'");
                    }
                }
            }
        }
        #endregion
    }
}
