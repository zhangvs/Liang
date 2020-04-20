using HZSoft.Application.Busines.BaseManage;
using HZSoft.Application.Entity.BaseManage;
using HZSoft.Cache.Factory;
using HZSoft.Cache.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HZSoft.Application.Cache
{
    /// <summary>
    /// 版 本 6.1
    /// 
    /// 创建人：佘赐雄
    /// 日 期：2016.3.4 9:56
    /// 描 述：组织架构缓存
    /// </summary>
    public class DmsCache
    {

        /// <summary>
        /// 个数
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCount()
        {
            List<string> cache = RedisCache.Hash_GetAll<string>("Hmk_Count");
            return cache;
        }

    }
}
