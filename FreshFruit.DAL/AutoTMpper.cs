using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;
using FreshFruit.Model;

namespace FreshFruit.DAL
{
    public class TemplatesClassMapper : ClassMapper<TemplatesModel>
    {
        public TemplatesClassMapper()
        {
            Map(x => x.temp_id).Key(KeyType.Assigned);
            Table("temp_v1");
            AutoMap();
        }
    }
    
}
