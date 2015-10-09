using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using FreshFruit.Common.BaseCommon;

namespace FreshFruit.Data.Ext.Dapper
{
    public static class DapperExtend
    {
        /// <summary>
        /// 缓存实体字段 
        /// </summary>
        private static Dictionary<string, PropertyInfo[]> EntryProperties { get; set; }


        /// <summary>
        /// 返回数量
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public static int QueryCount(IDbConnection conn, string sqlstr, dynamic param = null)
        {
            string sql = string.Format("SELECT count(1) num FROM  ({0}) A", sqlstr);
            var cr = SqlMapper.Query(conn, sql, param);
            //var cr = SqlMapper.Query(conn, sql, param).SingleOrDefault();
            var singleOrDefault = ((IEnumerable<dynamic>)cr).SingleOrDefault();
            if (singleOrDefault != null)
            {
                return singleOrDefault.num; // (int)cr[0].num;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="dataCount"></param>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public static IEnumerable<T> Page<T>(IDbConnection conn, string order, string sqlstr, int pageIndex, int pageSize, out int count, dynamic param = null)
        {
            count = QueryCount(conn, sqlstr, param);
            string sql = string.Format(@"select * from( select top {0} row_number() over (order by {1}) row, w1.* from ({2})w1 )w2 where  w2.row > {3} ",
             pageIndex * pageSize, order, sqlstr, (pageIndex - 1) * pageSize);
            var cr = SqlMapper.Query<T>(conn, sql, param);
            return cr;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="dataCount"></param>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> Page(IDbConnection conn, string order, string sqlstr, int pageIndex, int pageSize, out int count, dynamic param = null)
        {
            count = QueryCount(conn, sqlstr, param);
            string sql = string.Format(@"select * from( select top {0} row_number() over (order by {1}) row, w1.* from ({2})w1 )w2 where  w2.row > {3} ",
             pageIndex * pageSize, order, sqlstr, (pageIndex - 1) * pageSize);
            var cr = SqlMapper.Query(conn, sql, param);
            return cr;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="entry"></param>
        /// <param name="newentry"></param>
        /// <returns></returns>
        public static int Insert<T>(IDbConnection conn, T entry, out T newentry, IDbTransaction transaction = null)
        {
            newentry = entry;
            return Insert<T>(conn, newentry, transaction);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="entry"></param>
        /// <param name="newentry"></param>
        /// <returns></returns>
        public static int Insert<T>(IDbConnection conn, T entry, IDbTransaction transaction = null)
        {
            StringBuilder sqlstr = new StringBuilder();
            var type = typeof(T);
            TableAttribute table = (TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), true)[0];
            sqlstr.Append(string.Format(" INSERT INTO {0}.{1}"
                , string.IsNullOrWhiteSpace(table.Schema) ? "dbo" : table.Schema
                , table.Name));
            PropertyInfo[] properties = CacheProperties(type);
            StringBuilder colstr = new StringBuilder();
            StringBuilder colvalue = new StringBuilder();
            bool haskey = false;
            DatabaseGeneratedAttribute Generate = null;
            foreach (var p in properties)
            {
                if (p.MemberType == MemberTypes.Property && p.GetCustomAttributes(typeof(NotMappedAttribute), true).Length == 0)
                {
                    if (!haskey)
                    {
                        if (p.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0)
                        {
                            haskey = true;//避免重复判断主键
                            if ((p.PropertyType).FullName == typeof(long).FullName)
                            {
                                p.SetValue(entry, IdGenerator.GenerateId());
                            }
                            else if ((p.PropertyType).FullName == typeof(Guid).FullName)
                            {
                                p.SetValue(entry, Guid.NewGuid());
                            }
                            if (p.GetCustomAttributes(typeof(DatabaseGeneratedAttribute), true).Length > 0)
                            {
                                Generate = (DatabaseGeneratedAttribute)p.GetCustomAttributes(typeof(DatabaseGeneratedAttribute), true)[0];
                            }
                        }
                    }
                    if (Generate == null || (Generate.DatabaseGeneratedOption != DatabaseGeneratedOption.Identity))//判断是否自增
                    {
                        colstr.Append(string.Format("{0},", p.Name));
                        if (p.PropertyType == typeof(TimeSpan) || p.PropertyType == typeof(TimeSpan?))
                        {
                            object obj = p.GetValue(entry);
                            if (obj == null)
                                colvalue.Append(string.Format("null,"));
                            else
                                colvalue.Append(string.Format("'{0}',", obj.ToString()));
                        }
                        else
                            colvalue.Append(string.Format("@{0},", p.Name));
                    }
                    else
                    {
                        Generate = null;
                    }
                }
            }
            sqlstr.Append(string.Format("({0}) values ({1})"
                , colstr.ToString().Trim(',')
                , colvalue.ToString().Trim(',')));
            return conn.Execute(sqlstr.ToString(), entry, transaction);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="entry"></param>
        /// <param name="newentry"></param>
        /// <returns></returns>
        public static int Update<T>(IDbConnection conn, T entry, IDbTransaction transaction = null)
        {
            StringBuilder sqlstr = new StringBuilder();
            var type = typeof(T);
            TableAttribute table = (TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), true)[0];
            sqlstr.Append(string.Format(" UPDATE {0}.{1} SET "
                , string.IsNullOrWhiteSpace(table.Schema) ? "dbo" : table.Schema
                , table.Name));
            PropertyInfo[] properties = CacheProperties(type);
            StringBuilder colstr = new StringBuilder();
            StringBuilder wheresql = new StringBuilder(" WHERE  ");

            bool haskey = false;
            foreach (var p in properties)
            {
                if (p.MemberType == MemberTypes.Property && p.GetCustomAttributes(typeof(NotMappedAttribute), true).Length == 0)
                {
                    if (p.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0)
                    {
                        wheresql.Append(string.Format(" {1} {0}=@{0} ", p.Name, haskey == true ? "and" : string.Empty));
                        haskey = true;
                    }
                    else
                    {
                        if (p.PropertyType == typeof(TimeSpan) || p.PropertyType == typeof(TimeSpan?))
                        {
                            object obj = p.GetValue(entry);
                            if (obj == null)
                                colstr.Append(string.Format("{0}=null,", p.Name));
                            else
                                colstr.Append(string.Format("{0}='{1}',", p.Name, obj.ToString()));
                        }
                        else
                            colstr.Append(string.Format("{0}=@{0},", p.Name));
                    }
                }
            }
            sqlstr.Append(string.Format(" {0} {1} "
                , colstr.ToString().TrimEnd(',')
                , wheresql.ToString()));

            return conn.Execute(sqlstr.ToString(), entry, transaction);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="entry"></param>
        /// <param name="newentry"></param>
        /// <returns></returns>
        public static int Update<T>(IDbConnection conn, T entry, dynamic parms, IDbTransaction transaction = null)
        {
            StringBuilder sqlstr = new StringBuilder();
            var type = typeof(T);
            TableAttribute table = (TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), true)[0];
            sqlstr.Append(string.Format(" UPDATE {0}.{1} SET "
                , string.IsNullOrWhiteSpace(table.Schema) ? "dbo" : table.Schema
                , table.Name));
            PropertyInfo[] properties = CacheProperties(type);
            StringBuilder colstr = new StringBuilder();
            StringBuilder wheresql = new StringBuilder(" where  ");
            PropertyInfo[] whereproperties = parms.GetType().GetProperties();
            foreach (var p in properties)
            {
                if (p.MemberType == MemberTypes.Property && p.GetCustomAttributes(typeof(NotMappedAttribute), true).Length == 0)
                {
                    if (p.GetCustomAttributes(typeof(KeyAttribute), true).Length == 0)
                    {
                        if (p.PropertyType == typeof(TimeSpan) || p.PropertyType == typeof(TimeSpan?))
                        {
                            object obj = p.GetValue(entry);
                            if (obj == null)
                                colstr.Append(string.Format("{0}=null,", p.Name));
                            else
                                colstr.Append(string.Format("{0}='{1}',", p.Name, obj.ToString()));
                        }
                        else
                            colstr.Append(string.Format("{0}=@{0},", p.Name));
                    }
                }
            }
            bool haswhere = false;
            foreach (var p in whereproperties)
            {
                wheresql.Append(string.Format("{1} {0}=@{0}", p.Name, haswhere == true ? "and" : string.Empty));
                haswhere = true;
            }

            sqlstr.Append(string.Format(" {0} {1} "
                , colstr.ToString().TrimEnd(',')
                , wheresql.ToString()));

            return conn.Execute(sqlstr.ToString(), entry, transaction);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="entry"></param>
        /// <param name="newentry"></param>
        /// <returns></returns>
        public static int Update<T>(IDbConnection conn, dynamic value, IDbTransaction transaction = null)
        {
            StringBuilder sqlstr = new StringBuilder();
            var type = typeof(T);
            TableAttribute table = (TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), true)[0];
            sqlstr.Append(string.Format(" UPDATE {0}.{1} SET "
                , string.IsNullOrWhiteSpace(table.Schema) ? "dbo" : table.Schema
                , table.Name));
            PropertyInfo[] properties = CacheProperties(type);
            StringBuilder colstr = new StringBuilder();
            StringBuilder wheresql = new StringBuilder();
            PropertyInfo[] valueproperties = value.GetType().GetProperties();

            bool haskey = false;
            foreach (var p in properties)
            {
                if (p.MemberType == MemberTypes.Property && p.GetCustomAttributes(typeof(NotMappedAttribute), true).Length == 0)
                {
                    if (p.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0)
                    {
                        wheresql.Append(string.Format(" {1} {0}=@{0} ", p.Name, haskey == true ? "and" : " where "));
                        haskey = true;
                    }
                    else
                    {
                        if (CheckHasProperty(p.Name, valueproperties))
                        {
                            colstr.Append(string.Format("{0}=@{0},", p.Name));
                        }
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(wheresql.ToString()))
            {
                return 0;
            }
            else
            {
                sqlstr.Append(string.Format(" {0} {1} "
                    , colstr.ToString().TrimEnd(',')
                    , wheresql.ToString()));
                return SqlMapper.Execute(conn, sqlstr.ToString(), value, transaction);
            }
        }


        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="entry"></param>
        /// <param name="newentry"></param>
        /// <returns></returns>
        public static int Delete<T>(IDbConnection conn, dynamic parms, IDbTransaction transaction = null)
        {
            StringBuilder sqlstr = new StringBuilder();
            var type = typeof(T);
            TableAttribute table = (TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), true)[0];
            sqlstr.Append(string.Format(" DELETE FROM {0}.{1} "
                , string.IsNullOrWhiteSpace(table.Schema) ? "dbo" : table.Schema
                , table.Name));

            StringBuilder wheresql = new StringBuilder(" where  ");
            bool haswhere = false;
            PropertyInfo[] pInfo = parms.GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo pio in pInfo)
            {
                wheresql.Append(string.Format(" {1} {0}=@{0} ", pio.Name, haswhere == true ? " and " : string.Empty));
                haswhere = true;
            }
            sqlstr.Append(string.Format(" {0} "
                , wheresql.ToString()));
            return SqlMapper.Execute(conn, sqlstr.ToString(), parms, transaction);
        }


        /// <summary>
        /// 查询单个实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static T QuerySingle<T>(IDbConnection conn, dynamic parms)
        {
            var type = typeof(T);
            TableAttribute table = (TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), true)[0];
            StringBuilder sqlstr = new StringBuilder("select top 1 ");

            PropertyInfo[] properties = CacheProperties(type);
            StringBuilder colstr = new StringBuilder();
            StringBuilder wheresql = new StringBuilder(" where 1=1 ");

            foreach (var p in properties)
            {
                if (p.MemberType == MemberTypes.Property && p.GetCustomAttributes(typeof(NotMappedAttribute), true).Length == 0)
                {
                    colstr.Append(string.Format("{0},", p.Name));
                }
            }

            properties = parms.GetType().GetProperties();
            foreach (var p in properties)
            {
                wheresql.Append(string.Format(" and {0}=@{0} ", p.Name));
            }
            sqlstr.Append(string.Format("{2} from {0}.{1} {3}"
                 , string.IsNullOrWhiteSpace(table.Schema) ? "dbo" : table.Schema
                  , table.Name
                , colstr.ToString().Trim(',')
                , wheresql.ToString().Trim()));
            return SqlMapper.Query<T>(conn, sqlstr.ToString(), parms).FirstOrDefault();
        }

        /// <summary>
        /// 查询实例集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(IDbConnection conn, dynamic parms)
        {
            var type = typeof(T);
            TableAttribute table = (TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), true)[0];
            StringBuilder sqlstr = new StringBuilder("select  ");

            PropertyInfo[] properties = CacheProperties(type);
            StringBuilder colstr = new StringBuilder();
            StringBuilder wheresql = new StringBuilder(" where 1=1 ");

            foreach (var p in properties)
            {
                if (p.MemberType == MemberTypes.Property && p.GetCustomAttributes(typeof(NotMappedAttribute), true).Length == 0)
                {
                    colstr.Append(string.Format("{0},", p.Name));
                }
            }

            properties = parms.GetType().GetProperties();
            foreach (var p in properties)
            {
                wheresql.Append(string.Format(" and {0}=@{0} ", p.Name));
            }
            sqlstr.Append(string.Format("{2} from {0}.{1} {3}"
                 , string.IsNullOrWhiteSpace(table.Schema) ? "dbo" : table.Schema
                  , table.Name
                , colstr.ToString().Trim(',')
                , wheresql.ToString().Trim()));
            return SqlMapper.Query<T>(conn, sqlstr.ToString(), parms);
        }

        #region 私有方法

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>属性集合</returns>
        private static PropertyInfo[] CacheProperties(Type type)
        {
            PropertyInfo[] properties;
            string key = type.FullName;
            if (EntryProperties == null)
                EntryProperties = new Dictionary<string, PropertyInfo[]>();

            if (!EntryProperties.ContainsKey(key))
            {
                properties = type.GetProperties();
                EntryProperties.Add(key, properties);
            }
            else
            {
                properties = EntryProperties[key];
            }
            return properties;
        }

        /// <summary>
        /// 判断是否存在属性名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        private static bool CheckHasProperty(string name, PropertyInfo[] properties)
        {
            bool flag = false;
            foreach (var p in properties)
            {
                if (p.Name.ToLower() == name.ToLower())
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }
        #endregion
    }
}
