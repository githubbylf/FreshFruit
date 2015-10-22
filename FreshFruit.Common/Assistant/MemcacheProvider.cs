using FreshFruit.Common.Interface;
using Memcached.ClientLibrary;
using System.Configuration;

namespace FreshFruit.Common.Assistant
{
    public sealed class MemcacheProvider : IMemcacheProvider
    {
        private MemcachedClient mc;

        private SockIOPool pool;

        private string[] servers = new string[]
        {
            ""
        };

        /// <summary>
        ///
        /// </summary>
        public MemcacheProvider()
        {
            string text = ConfigurationManager.AppSettings["MemcachedHost"];
            if (text != null)
            {
                this.servers = text.ToString().Split(new char[]
                {
                    ','
                });
            }
            this.pool = SockIOPool.GetInstance();
            this.pool.SetServers(this.servers);
            this.pool.InitConnections = 100;
            this.pool.MinConnections = 3;
            this.pool.MaxConnections = 1024;
            this.pool.SocketConnectTimeout = 1000;
            this.pool.SocketTimeout = 3000;
            this.pool.MaintenanceSleep = 30L;
            this.pool.Failover = true;
            this.pool.Nagle = false;
            this.pool.Initialize();
            this.mc = new MemcachedClient
            {
                EnableCompression = false
            };
        }

        /// <summary>
        /// 数据不存在时,新增缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add(string key, object value)
        {
            return this.mc.Add(key, value);
        }

        public bool Add(string key, object value, double expiry = 1.0)
        {
            return this.mc.Add(key, value, System.DateTime.Now.AddHours(expiry));
        }

        public bool Add(string key, object value, int hashCode)
        {
            return this.mc.Add(key, value, hashCode);
        }

        public bool Add(string key, object value, int hashCode, double expiry = 1.0)
        {
            return this.mc.Add(key, value, System.DateTime.Now.AddHours(expiry), hashCode);
        }

        public long Decrement(string key)
        {
            return this.mc.Decrement(key);
        }

        public long Decrement(string key, long inc)
        {
            return this.mc.Decrement(key, inc);
        }

        public long Decrement(string key, long inc, int hashCode)
        {
            return this.mc.Decrement(key, inc, hashCode);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(string key)
        {
            return this.mc.Delete(key);
        }

        public bool Delete(string key, double expiry = 1.0)
        {
            return this.mc.Delete(key, System.DateTime.Now.AddHours(expiry));
        }

        public bool Delete(string key, object hashCode, double expiry = 1.0)
        {
            return this.mc.Delete(key, hashCode, System.DateTime.Now.AddHours(expiry));
        }

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        /// <returns></returns>
        public bool FlushAll()
        {
            return this.mc.FlushAll();
        }

        public bool FlushAll(System.Collections.ArrayList servers)
        {
            return this.mc.FlushAll(servers);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            return this.mc.Get(key);
        }

        public object Get(string key, int hashCode)
        {
            return this.mc.Get(key, hashCode);
        }

        public object Get(string key, object hashCode, bool asString)
        {
            return this.mc.Get(key, hashCode, asString);
        }

        public long GetCounter(string key)
        {
            return this.mc.GetCounter(key);
        }

        public long GetCounter(string key, object hashCode)
        {
            return this.mc.GetCounter(key, hashCode);
        }

        public System.Collections.Hashtable GetMultiple(string[] keys)
        {
            return this.mc.GetMultiple(keys);
        }

        public System.Collections.Hashtable GetMultiple(string[] keys, int[] hashCodes)
        {
            return this.mc.GetMultiple(keys, hashCodes);
        }

        public System.Collections.Hashtable GetMultiple(string[] keys, int[] hashCodes, bool asString)
        {
            return this.mc.GetMultiple(keys, hashCodes, asString);
        }

        public object[] GetMultipleArray(string[] keys)
        {
            return this.mc.GetMultipleArray(keys);
        }

        public object[] GetMultipleArray(string[] keys, int[] hashCodes)
        {
            return this.mc.GetMultipleArray(keys, hashCodes);
        }

        public object[] GetMultipleArray(string[] keys, int[] hashCodes, bool asString)
        {
            return this.mc.GetMultipleArray(keys, hashCodes, asString);
        }

        public long Increment(string key)
        {
            return this.mc.Increment(key);
        }

        public long Increment(string key, long inc)
        {
            return this.mc.Increment(key, inc);
        }

        public long Increment(string key, long inc, int hashCode)
        {
            return this.mc.Increment(key, inc, hashCode);
        }

        /// <summary>
        /// 缓存是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return this.mc.KeyExists(key);
        }

        /// <summary>
        /// 数据存在时,替换缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Replace(string key, object value)
        {
            return this.mc.Replace(key, value);
        }

        public bool Replace(string key, object value, double expiry = 1.0)
        {
            return this.mc.Replace(key, value, System.DateTime.Now.AddHours(expiry));
        }

        public bool Replace(string key, object value, int hashCode)
        {
            return this.mc.Replace(key, value, hashCode);
        }

        public bool Replace(string key, object value, int hashCode, double expiry = 1.0)
        {
            return this.mc.Replace(key, value, System.DateTime.Now.AddHours(expiry), hashCode);
        }

        /// <summary>
        /// 设置缓存，如果存在则更新，不存在则新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set(string key, object value)
        {
            return this.mc.Set(key, value);
        }

        public bool Set(string key, object value, double expiry = 1.0)
        {
            return this.mc.Set(key, value, System.DateTime.Now.AddHours(expiry));
        }

        public bool Set(string key, object value, int hashCode)
        {
            return this.mc.Set(key, value, hashCode);
        }

        public bool Set(string key, object value, int hashCode, double expiry = 1.0)
        {
            return this.mc.Set(key, value, System.DateTime.Now.AddHours(expiry), hashCode);
        }

        public System.Collections.Hashtable Stats()
        {
            return this.mc.Stats();
        }

        public System.Collections.Hashtable Stats(System.Collections.ArrayList servers)
        {
            return this.mc.Stats(servers);
        }

        public bool StoreCounter(string key, long counter)
        {
            return this.mc.StoreCounter(key, counter);
        }

        public bool StoreCounter(string key, long counter, int hashCode)
        {
            return this.mc.StoreCounter(key, counter, hashCode);
        }
    }
}
