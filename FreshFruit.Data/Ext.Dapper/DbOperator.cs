using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using DapperExtensions.Sql;
using FreshFruit.Common.BaseCommon;
using FreshFruit.Common.DBUtils;

namespace FreshFruit.Data.Ext.Dapper
{
    public abstract class DbOperator
    {
        private readonly DataBaseName _dataBaseName;

        protected DbOperator(DataBaseName name)
        {
            _dataBaseName = name;
        }

        protected abstract ISqlGenerator SqlGenerator { get; set; }

        protected abstract string GetPagingSql(string sql, IEnumerable<ISort> sorts, int page, int size, DynamicParameters parameters);

        protected virtual string GetConnectionString(DatabaseMode mode)
        {
            string connectionName = BaseDao.GetConnectionName(_dataBaseName);

            if (mode != DatabaseMode.All)
            {
                connectionName = string.Format("{0}_{1}", mode, connectionName);
            }

            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        public abstract IDatabase GetDatabase(DatabaseMode mode);

        public virtual IDbConnection GetOpenedConnection(DatabaseMode mode)
        {
            var connection = GetDatabase(mode).Connection;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }

        public bool Insert<TEntity>(TEntity entity) where TEntity : class
        {
            using (var db = GetDatabase(DatabaseMode.Write))
            {
                db.Insert(entity);
                return true;
            }
        }

        public bool Update<TEntity>(TEntity entity) where TEntity : class
        {
            using (var db = GetDatabase(DatabaseMode.Write))
            {
                return db.Update(entity);
            }
        }

        public bool Update<TEntity>(TEntity entity, object parameters) where TEntity : class
        {
            using (var db = GetDatabase(DatabaseMode.Write))
            {
                return db.Update(entity, parameters);
            }
        }

        public bool Delete<TEntity>(object parameters) where TEntity : class
        {
            using (var db = GetDatabase(DatabaseMode.Write))
            {
                return db.Delete<TEntity>(parameters);
            }
        }

        public bool Delete<TEntity>(TEntity entity) where TEntity : class
        {
            using (var db = GetDatabase(DatabaseMode.Write))
            {
                return db.Delete(entity);
            }
        }

        public bool Execute(
            string sql,
            object param = null,
            CommandType? commandType = null)
        {
            using (var db = GetDatabase(DatabaseMode.Write))
            {
                return db.Connection.Execute(sql, param, commandType: commandType) > 0;
            }
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return GetList<TEntity>();
        }

        public virtual TEntity Get<TEntity, TIdentity>(TIdentity id, bool isConsistency = false) where TEntity : class
        {
            var mode = isConsistency ? DatabaseMode.Write : DatabaseMode.ReadOnly;

            using (var db = GetDatabase(mode))
            {
                return db.Get<TEntity>(id);
            }
        }

        public IEnumerable<TEntity> GetPage<TEntity>(
            object predicate,
            IList<ISort> sorts,
            int page = 1,
            int size = 10,
            bool isConsistency = false)
            where TEntity : class
        {
            if (size <= 0)
            {
                return Enumerable.Empty<TEntity>();
            }

            if (!sorts.IsNotNullEmpty())
            {
                throw new ArgumentNullException("sorts", "排序条件不能为空");
            }

            page = page > 1 ? page : 1;
            var mode = isConsistency ? DatabaseMode.Write : DatabaseMode.ReadOnly;

            using (var db = GetDatabase(mode))
            {
                return db.GetPage<TEntity>(predicate, sorts, page - 1, size);
            }
        }

        public IEnumerable<TEntity> GetList<TEntity>(
            object predicate = null,
            IList<ISort> sorts = null,
            bool isConsistency = false)
            where TEntity : class
        {
            var mode = isConsistency ? DatabaseMode.Write : DatabaseMode.ReadOnly;

            using (var db = GetDatabase(mode))
            {
                return db.GetList<TEntity>(predicate, sorts);
            }
        }

        public int Count<TEntity>(object predicate = null, bool isConsistency = false) where TEntity : class
        {
            var mode = isConsistency ? DatabaseMode.Write : DatabaseMode.ReadOnly;

            using (var db = GetDatabase(mode))
            {
                return db.Count<TEntity>(predicate);
            }
        }

        public int CountBySql(string sql, object parameters = null, bool isConsistency = false)
        {
            var mode = isConsistency ? DatabaseMode.Write : DatabaseMode.ReadOnly;
            string countSql = string.Format("SELECT COUNT(*) AS Total FROM ({0}) AS c", sql);

            using (var db = GetDatabase(mode))
            {
                var row = db.Connection.Query(countSql, parameters).SingleOrDefault();

                return row == null ? 0 : (int)row.Total;
            }
        }

        public TReturn Max<TEntity, TReturn>(string columnName, object parameters = null, bool isConsistency = false) where TEntity : class
        {
            return SqlFunctionValue<TEntity, TReturn>(columnName, "MAX", parameters, isConsistency);
        }

        public TReturn Min<TEntity, TReturn>(string columnName, object parameters = null, bool isConsistency = false) where TEntity : class
        {
            return SqlFunctionValue<TEntity, TReturn>(columnName, "MIN", parameters, isConsistency);
        }

        public TReturn Avg<TEntity, TReturn>(string columnName, object parameters = null, bool isConsistency = false) where TEntity : class
        {
            return SqlFunctionValue<TEntity, TReturn>(columnName, "AVG", parameters, isConsistency);
        }

        private TReturn SqlFunctionValue<TEntity, TReturn>(
            string columnName,
            string functionName,
            object parameters = null,
            bool isConsistency = false)
            where TEntity : class
        {
            if (string.IsNullOrEmpty(columnName))
            {
                throw new ArgumentNullException("columnName", "列名不能为空");
            }

            if (string.IsNullOrEmpty(functionName))
            {
                throw new ArgumentNullException("functionName", "函数名不能为空");
            }

            var mode = isConsistency ? DatabaseMode.Write : DatabaseMode.ReadOnly;

            using (var db = GetDatabase(mode))
            {
                var mapper = db.GetMap<TEntity>();
                string sql = string.Format(
                    "SELECT {0}({1}) AS FunctionValue FROM {2}",
                    functionName,
                    columnName,
                    mapper.TableName);

                var dapperParameters = new DynamicParameters();

                if (parameters != null)
                {
                    var predicate = DbOperatorExtensions.GetPredicate(mapper, parameters);
                    var dictionary = new Dictionary<string, object>();
                    string where = predicate.GetSql(SqlGenerator, dictionary);

                    foreach (var item in dictionary)
                    {
                        dapperParameters.Add(item.Key, item.Value);
                    }

                    sql = string.Format("{0} WHERE {1}", sql, where);
                }

                var row = db.Connection.Query(sql, dapperParameters).SingleOrDefault();
                var value = row == null ? default(TReturn) : (TReturn)row.FunctionValue;

                return value;
            }
        }

        public IEnumerable<TEntity> GetListBySql<TEntity>(
            string sql,
            object parameters = null,
            bool isConsistency = false)
            where TEntity : class
        {
            var mode = isConsistency ? DatabaseMode.Write : DatabaseMode.ReadOnly;

            using (var connection = GetDatabase(mode).Connection)
            {
                return connection.Query<TEntity>(sql, parameters);
            }
        }

        public IEnumerable<TReturn> GetListBySql<TFirst, TSecond, TReturn>(
            string sql,
            Func<TFirst, TSecond, TReturn> mapFunc,
            object parameters = null,
            string splitOn = "Id",
            bool isConsistency = false)
            where TReturn : class
        {
            var mode = isConsistency ? DatabaseMode.Write : DatabaseMode.ReadOnly;

            using (var connection = GetDatabase(mode).Connection)
            {
                return connection.Query(sql, mapFunc, parameters, splitOn: splitOn);
            }
        }

        public IEnumerable<TEntity> GetPageBySql<TEntity>(
            string sql,
            List<ISort> sorts,
            object parameters = null,
            int page = 1,
            int size = 10,
            bool isConsistency = false)
        {
            var mode = isConsistency ? DatabaseMode.Write : DatabaseMode.ReadOnly;
            var dapperParameters = new DynamicParameters(parameters ?? new { });
            var pagingSql = GetPagingSql(sql, sorts, page, size, dapperParameters);

            using (var db = GetDatabase(mode))
            {
                return db.Connection.Query<TEntity>(pagingSql, dapperParameters);
            }
        }

        public IEnumerable<TReturn> GetPageBySql<TFirst, TSecond, TReturn>(
            string sql,
            List<ISort> sorts,
            Func<TFirst, TSecond, TReturn> mapFunc,
            object parameters = null,
            int page = 1,
            int size = 10,
            string splitOn = "Id",
            bool isConsistency = false)
        {
            var mode = isConsistency ? DatabaseMode.Write : DatabaseMode.ReadOnly;
            var dapperParameters = new DynamicParameters(parameters ?? new { });
            var pagingSql = GetPagingSql(sql, sorts, page, size, dapperParameters);

            using (var db = GetDatabase(mode))
            {
                return db.Connection.Query<TFirst, TSecond, TReturn>(pagingSql, mapFunc, dapperParameters, splitOn: splitOn);
            }
        }

        public TReturn RunInTransaction<TReturn>(Func<IDatabase, TReturn> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func", "事务过程不能为空");
            }

            using (var db = GetDatabase(DatabaseMode.Write))
            {
                return db.RunInTransaction(() => func(db));
            }
        }

        public void RunInTransaction(Action<IDatabase> action)
        {
            if (action == null)
            {
                return;
            }

            using (var db = GetDatabase(DatabaseMode.Write))
            {
                db.RunInTransaction(() => action(db));
            }
        }
    }
}
