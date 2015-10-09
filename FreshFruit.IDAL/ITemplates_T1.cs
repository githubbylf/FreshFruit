using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshFruit.Model;

namespace FreshFruit.IDAL
{
    public interface ITemplates_T1
    {
        int AddObject(TemplatesModel obj);

        int UpdateObject(TemplatesModel obj);

        int DeleteObject(int temp_id);

        List<TemplatesModel> GetObject();
    }
}
