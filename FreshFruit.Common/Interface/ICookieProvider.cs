using System;

namespace FreshFruit.Common.Interface
{
    /// <summary>
	/// 提供对 Http Cookie 的存取操作接口。
	/// </summary>
	public interface ICookieProvider
    {
        /// <summary>
        /// 新增Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        void Add(string key, string value);

        /// <summary>
        /// 新增Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="domain">Cookie域</param>
        void Add(string key, string value, string domain);

        /// <summary>
        /// 新增Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="domain">Cookie域</param>
        /// <param name="expminute">有效时长(分)</param>
        void Add(string key, string value, string domain, double expminute);

        /// <summary>
        /// 新增Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="domain">Cookie域</param>
        /// <param name="expminute">有效时长(分)</param>
        void Add<T>(string key, T value, string domain, double expminute) where T : class, new();

        /// <summary>
        /// 新增Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cryptFunc">加密方法</param>
        /// <param name="domain">Cookie域</param>
        /// <param name="expminute">有效时长(分)</param>
        void Add<T>(string key, T value, Func<string, string> cryptFunc, string domain, double expminute) where T : class, new();

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="key">键</param>
        void Delete(string key);

        /// <summary>
        /// 删除指定Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="domain">Cookie域</param>
        void Delete(string key, string domain);

        /// <summary>
        /// 清除所有Cookie
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// 清除指定域下所有Cookie
        /// </summary>
        /// <param name="domain">Cookie域</param>
        void DeleteAll(string domain);

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>字符串值</returns>
        string Get(string key);

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>对象</returns>
        T Get<T>(string key) where T : class, new();

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="decryptFunc">解密方法</param>
        /// <returns>对象</returns>
        T Get<T>(string key, Func<string, string> decryptFunc) where T : class, new();
    }
}
