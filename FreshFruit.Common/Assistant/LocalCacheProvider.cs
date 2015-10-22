using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace FreshFruit.Common.Assistant
{
    //— 参数“key”代表缓存数据项的键值，必须是唯一的。
    //— 参数“value”代表缓存数据的内容，可以是任意类型。
    //— 参数“dependencies”表示缓存的依赖项，也就是此项的更改意味着缓存内容已经过期。如果没有依赖项，可将此值设置为NULL。
    //— 参数“absoluteExpiration”是日期型数据，表示缓存过期的时间，.NET 2.0提供的缓存在过期后是可以使用的，能使用多长时间，就看这个参数的设置。
    //— 参数“slidingExpiration”的类型表示一段时间间隔，表示缓存参数将在多长时间以后被删除，此参数与absoluteExpiration参数相关联。
    //— 参数“priority”表示撤销缓存的优先值，此参数的值取自枚举变量“CacheItemPriority”，优先级低的数据项将先被删除。此参数主要用在缓存退出对象时。
    //— 参数“onRemoveCallback”表示缓存删除数据对象时调用的事件，一般用做通知程序。

    /// <summary>
    /// 本地缓存帮助类
    /// </summary>
    public class LocalCacheProvider
    {
        /// <summary>
        /// 获取数据缓存
        /// </summary>
        /// <param name="key">键</param>
        public static object Get(string key)
        {
            Cache objCache = HttpRuntime.Cache;
            return objCache[key];
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void Set(string key, object value)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(key, value);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void Set(string key, object value, TimeSpan timeout)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(key, value, null, DateTime.MaxValue, timeout, CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration"></param>
        public static void Set(string key, object value, DateTime absoluteExpiration)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(key, value, null, absoluteExpiration, TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void Set(string key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(key, value, null, absoluteExpiration, slidingExpiration);
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        public static void Remove(string key)
        {
            Cache cache = HttpRuntime.Cache;
            cache.Remove(key);
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAll()
        {
            Cache cache = HttpRuntime.Cache;
            IDictionaryEnumerator cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cache.Remove(cacheEnum.Key.ToString());
            }
        }
    }
}
