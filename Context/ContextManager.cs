using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guayaba.LinqHelper.Context
{
    public class ContextManager
    {
        public T SelectByPK<T>(string id) where T : class
        {
            var table = LinqHelper.DataContext.GetTable<T>();
            MetaModel modelMap = LinqHelper.DataContext.Mapping;

            //Obtieme proriedades de la entidad
            ReadOnlyCollection<MetaDataMember> dataMembers = modelMap.GetMetaType(typeof(T)).DataMembers;

            string PrimaryKeyName = (dataMembers.FirstOrDefault<MetaDataMember>(m => m.IsPrimaryKey)).Name;

            return table.FirstOrDefault<T>(delegate (T t)
            {
                String memberId = t.GetType().GetProperty(PrimaryKeyName).GetValue(t, null).ToString();
                return memberId.ToString() == id.ToString();
            });
        }
    }
}
