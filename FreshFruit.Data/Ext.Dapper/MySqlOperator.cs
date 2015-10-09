using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using FreshFruit.Common.DBUtils;
using MySql.Data.MySqlClient;

//********************************************************************************************************
//Copyright © 2015  Asa
//
//     Author：Asa
//       Date：2015-10-9 16:02:11
//Description：MySqlOperator
//    Version：1.00
//
//版　本　　　　　修改时间　　　　　　修改人　　　　　变更内容
//********************************************************************************************************
namespace FreshFruit.Data.Ext.Dapper
{
    public class MySqlOperator : DbOperator
    {
        private List<Assembly> _mappingAssemblies;

        public MySqlOperator(DataBaseName databaseName, List<Assembly> mappingAssemblies = null) : base(databaseName)
        {
            _mappingAssemblies = mappingAssemblies ?? new List<Assembly>()
            {
                Assembly.GetCallingAssembly()
            };

            var config = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), _mappingAssemblies, new MySqlDialect());
            SqlGenerator = new SqlGeneratorImpl(config);
        }

        protected override ISqlGenerator SqlGenerator { get; set; }

        protected override string GetPagingSql(
            string sql,
            IEnumerable<ISort> sorts,
            int page,
            int size,
            DynamicParameters parameters)
        {
            if (sorts == null)
            {
                throw new ArgumentNullException("sorts", "排序条件不能为空");
            }

            var orderBy = sorts.Select(s => s.PropertyName + " " + (s.Ascending ? "ASC" : "DESC")).AppendStrings();
            sql = string.Format("{0} ORDER BY {1}", sql, orderBy);

            parameters.Add("@startValue", (page - 1) * size);
            parameters.Add("@size", size);

            sql = string.Format("{0} LIMIT @startValue, @size", sql);
            return sql;
        }

        public override IDatabase GetDatabase(DatabaseMode mode)
        {
            var connection = new MySqlConnection(GetConnectionString(mode));

            return new Database(connection, SqlGenerator);
        }
    }
}
