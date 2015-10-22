using FreshFruit.Common.Interface;
using FreshFruit.Common.Serialization;
using System;
using System.Web;

namespace FreshFruit.Common.Assistant
{
    /// <summary>
	/// Cookie操作类
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class CookieProvider : ICookieProvider
    {
        private HttpContextBase httpContext;

        private static readonly object objLocker = new object();

        private readonly IObjectSerializer serializer;

        /// <summary>
        ///
        /// </summary>
        /// <param name="aspNetHttpContext"></param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        public CookieProvider(HttpContext aspNetHttpContext)
        {
            if (aspNetHttpContext == null)
            {
                throw new System.ArgumentNullException("aspNetHttpContext");
            }
            this.httpContext = new HttpContextWrapper(aspNetHttpContext);
            this.serializer = new ObjectJsonSerializer();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aspNetMvcHttpContext"></param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        public CookieProvider(HttpContextBase aspNetMvcHttpContext)
        {
            if (aspNetMvcHttpContext == null)
            {
                throw new System.ArgumentNullException("aspNetMvcHttpContext", "在 MVC 中的 HttpContext 上下文为空");
            }
            this.httpContext = aspNetMvcHttpContext;
            this.serializer = new ObjectJsonSerializer();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, string value)
        {
            this.Add(key, value, "kuparts.com");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="domain"></param>
        public void Add(string key, string value, string domain)
        {
            this.Add(key, value, domain, 0.0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="domain"></param>
        /// <param name="expminute"></param>
        public void Add(string key, string value, string domain, double expminute)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new System.ArgumentNullException("key");
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new System.ArgumentNullException("value");
            }
            HttpCookie httpCookie = this.GetCookie(key);
            if (httpCookie == null || httpCookie.Expires < System.DateTime.Now)
            {
                httpCookie = new HttpCookie(key);
            }
            lock (CookieProvider.objLocker)
            {
                if (expminute > 0.0)
                {
                    httpCookie.Expires = System.DateTime.Now.AddMinutes(expminute);
                }
                if (!string.IsNullOrWhiteSpace(domain))
                {
                    httpCookie.Domain = domain;
                }
                httpCookie.Value = HttpUtility.UrlEncode(value);
                this.httpContext.Response.Cookies.Add(httpCookie);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        public void Delete(string key)
        {
            HttpCookie cookie = this.GetCookie(key);
            if (cookie != null)
            {
                cookie.Expires = System.DateTime.Now.AddYears(-1);
                this.httpContext.Response.Cookies.Add(cookie);
            }
        }

        public void Delete(string key, string domain)
        {
            HttpCookie cookie = this.GetCookie(key);
            if (cookie != null)
            {
                cookie.Expires = System.DateTime.Now.AddYears(-1);
                cookie.Domain = domain;
                this.httpContext.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void DeleteAll()
        {
            HttpCookieCollection cookies = this.httpContext.Request.Cookies;
            if (cookies != null && cookies.Count > 0)
            {
                System.Collections.Generic.List<HttpCookie> list = new System.Collections.Generic.List<HttpCookie>();
                foreach (string key in cookies)
                {
                    HttpCookie cookie = this.GetCookie(key);
                    list.Add(cookie);
                }
                foreach (HttpCookie current in list)
                {
                    current.Expires = System.DateTime.Now.AddYears(-1);
                    this.httpContext.Response.Cookies.Add(current);
                }
            }
        }

        public void DeleteAll(string domain)
        {
            HttpCookieCollection cookies = this.httpContext.Request.Cookies;
            if (cookies != null && cookies.Count > 0)
            {
                System.Collections.Generic.List<HttpCookie> list = new System.Collections.Generic.List<HttpCookie>();
                foreach (string key in cookies)
                {
                    HttpCookie cookie = this.GetCookie(key);
                    list.Add(cookie);
                }
                foreach (HttpCookie current in list)
                {
                    current.Expires = System.DateTime.Now.AddYears(-1);
                    current.Domain = domain;
                    this.httpContext.Response.Cookies.Add(current);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            HttpCookie cookie = this.GetCookie(key);
            if (cookie != null)
            {
                return HttpUtility.UrlDecode(cookie.Value);
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private HttpCookie GetCookie(string key)
        {
            return this.httpContext.Request.Cookies[key];
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="domain"></param>
        /// <param name="expminute"></param>
        public void Add<T>(string key, T value, string domain = "kuparts.com", double expminute = 30.0) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new System.ArgumentNullException("key");
            }
            if (value == null)
            {
                throw new System.ArgumentNullException("value");
            }
            this.Add<T>(key, value, (string val) => HttpUtility.UrlEncode(val), domain, expminute);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cryptFunc"></param>
        /// <param name="domain"></param>
        /// <param name="expminute"></param>
        public void Add<T>(string key, T value, Func<string, string> cryptFunc, string domain = "kuparts.com", double expminute = 30.0) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new System.ArgumentNullException("key");
            }
            if (value == null)
            {
                throw new System.ArgumentNullException("value");
            }
            if (cryptFunc == null)
            {
                throw new System.InvalidOperationException("cryptFunc");
            }
            byte[] bytes = this.serializer.Serialize<T>(value);
            string @string = System.Text.Encoding.UTF8.GetString(bytes);
            HttpCookie httpCookie = this.GetCookie(key);
            if (httpCookie == null)
            {
                httpCookie = new HttpCookie(key);
            }
            lock (CookieProvider.objLocker)
            {
                httpCookie.Expires = System.DateTime.Now.AddMinutes((expminute <= 0.0) ? 30.0 : expminute);
                if (!string.IsNullOrWhiteSpace(domain))
                {
                    httpCookie.Domain = domain;
                }
                httpCookie.Value = cryptFunc(@string);
                this.httpContext.Response.Cookies.Add(httpCookie);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new System.ArgumentNullException("key");
            }
            return this.Get<T>(key, (string val) => HttpUtility.UrlDecode(val));
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="decryptFunc"></param>
        /// <returns></returns>
        public T Get<T>(string key, Func<string, string> decryptFunc) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new System.ArgumentNullException("key");
            }
            if (decryptFunc == null)
            {
                throw new System.InvalidOperationException("decryptFunc");
            }
            HttpCookie cookie = this.GetCookie(key);
            if (cookie != null)
            {
                string text = decryptFunc(cookie.Value);
                if (!string.IsNullOrWhiteSpace(text))
                {
                    return this.serializer.Deserialize<T>(System.Text.Encoding.UTF8.GetBytes(text));
                }
            }
            return default(T);
        }
    }
}
