using System;
using System.Collections.Generic;
using System.Reflection;
using FreshFruit.Common.DBUtils;
using FreshFruit.Data;
using FreshFruit.Data.Ext.Dapper;
using FreshFruit.IDAL;
using FreshFruit.Model;

namespace FreshFruit.DAL
{
    public class Templates_T1 : BaseDao,ITemplates_T1
    {

        private MySqlOperator db;

        public class MockMySqlOperator : MySqlOperator
        {
            public MockMySqlOperator(List<Assembly> assemblies = null)
                : base(DataBaseName.FreshFruit, assemblies)
            {
            }
        }

        public Templates_T1()
            : base(DataBaseName.FreshFruit)
        {
            db = new MockMySqlOperator(new List<Assembly>() { Assembly.GetExecutingAssembly() });
        }

        public int AddObject(TemplatesModel obj)
        {
            return db.Insert(obj) == true ? 1 : 0;
        }

        public int DeleteObject(int temp_id)
        {
            throw new NotImplementedException();
        }

        public List<TemplatesModel> GetObject()
        {
            throw new NotImplementedException();
        }

        public int UpdateObject(TemplatesModel obj)
        {
            throw new NotImplementedException();
        }
    }
}
