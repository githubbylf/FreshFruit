using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using FreshFruit.Common.DBUtils;
using FreshFruit.Data.Ext.Dapper;
using MySql.Data.MySqlClient;

namespace FreshFruit.Data
{
    public class BaseDao
    {
        public BaseDao()
        { }

        #region Fields

        private string _writeDbConnectionString = string.Empty;
        private string _readOnlyDbConnectionString = string.Empty;
        //private const string ScreteKey = "Kuparts.COM_DataHelpers,./@";
        private const string ReadOnly = "ReadOnly_";
        private const string Write = "Write_";
        #endregion

        #region Ctor

        public BaseDao(DataBaseName dataBaseName)
        {
            InitConnectionString(GetConnectionName(dataBaseName));
        }

        #endregion

        protected string WriteDbConnectionString
        {
            get
            {
                return _writeDbConnectionString;
            }
        }

        protected string ReadOnlyDbConnectionString
        {
            get
            {
                return _readOnlyDbConnectionString;
            }
        }

        #region Assistant Methods

        private void InitConnectionString(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                //目前项目中读写是没有分离的，后期抛弃EF DbContext之后可以自定义扩展
                _readOnlyDbConnectionString = ConfigurationManager.ConnectionStrings[ReadOnly + groupName].ConnectionString;
                _writeDbConnectionString = ConfigurationManager.ConnectionStrings[Write + groupName].ConnectionString;
            }
        }

        #endregion


        /// <summary>
        /// 获取MsSql的连接数据库对象。SqlConnection
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        protected IDbConnection OpenMsSqlConnection(string connectionString)
        {
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                var connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            }

            throw new ArgumentException("传入的连接字符串为空");
        }

        /// <summary>
        /// 获取MySql的连接数据库对象。MySqlConnection
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        protected IDbConnection OpenMySqlConnection(string connectionString)
        {
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                var connection = new MySqlConnection(connectionString);
                connection.Open();
                return connection;
            }

            throw new ArgumentException("传入的连接字符串为空");
        }


        internal static string GetConnectionName(DataBaseName dbName)
        {
            switch (dbName)
            {
                case DataBaseName.FreshFruit:
                    return "FreshFruit";
                default:
                    throw new ArgumentException("未定义的数据库连接名称");
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> Query(string strSql)
        {
            IEnumerable<dynamic> list = null;
            using (IDbConnection dbConn = OpenMsSqlConnection(ReadOnlyDbConnectionString))
            {
                list = dbConn.Query(strSql);
            }
            return list;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string strSql)
        {
            IEnumerable<T> list = null;
            using (IDbConnection dbConn = OpenMsSqlConnection(ReadOnlyDbConnectionString))
            {
                list = dbConn.Query<T>(strSql);
            }
            return list;
        }


        /// <summary>
        /// 查询单个实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parms"></param>
        /// <returns></returns>
        public T QuerySingle<T>(dynamic parms)
        {
            T model = default(T);
            using (IDbConnection dbConn = OpenMsSqlConnection(ReadOnlyDbConnectionString))
            {
                model = DapperExtend.QuerySingle<T>(dbConn, parms);
            }
            return model;
        }

        /// <summary>
        /// 查询实例集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parms"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(dynamic parms)
        {
            IEnumerable<T> list = null;
            using (IDbConnection dbConn = OpenMsSqlConnection(ReadOnlyDbConnectionString))
            {
                list = DapperExtend.Query<T>(dbConn, parms);
            }
            return list;
        }
    }
}
