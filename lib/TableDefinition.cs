using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;


namespace System.Linq.Dynamic
{
    public class TableDefinition
    {
        private readonly ReadOnlyCollection<MetaDataMember> _members;
        private Int64? _nextId;
        private readonly Type _type;
        private readonly PropertyInfo _keyProperty;
        private readonly Dictionary<string, PropertyInfo> _properties;
        private List<PropertyInfo> _setterProperties;
        public string TableName;

        public Dictionary<string, PropertyInfo> Properties
        {
            get { return _properties; }
        }

        public TableDefinition(System.Type type, DataContext context)
        {
            _type = type;
            var table = context.Mapping.GetTable(type);
            this.TableName = table.TableName;
            _members = table.RowType.DataMembers;
            var properties = type.GetProperties();
            _properties = properties.ToDictionary(e => e.Name, e => e);
            _setterProperties = properties.Where(e => e.SetMethod != null && EntityExtensions.ValidTypesTocopy.Any(v => v == e.PropertyType.Name)).Select(e => e).ToList();
            _keyProperty = type.GetProperty(PrimaryKeyName);

        }

        public MetaDataMember PrimaryKeyMetaData
        {
            get
            {
                var key = _members.FirstOrDefault(e => e.IsPrimaryKey);
                return key;
            }
        }

        public string StringWherePrimaryKey
        {
            get
            {
                var key = _members.FirstOrDefault(e => e.IsPrimaryKey);
                return key != null ? string.Format("{0}=@0", key.Name) : "";
            }
        }

        public string PrimaryKeyName
        {
            get
            {
                var key = _members.FirstOrDefault(e => e.IsPrimaryKey);
                return key != null ? key.Name : "";
            }
        }

        public ReadOnlyCollection<MetaDataMember> ColumnMembers
        {
            get { return _members; }
        }

        public T CreateInstance<T>()
        {
            var constructor = _type.GetConstructor(new System.Type[0]);
            T entity = (T)constructor.Invoke(null);
            return entity;
        }

        public void SetNextId<T>(T entity, DataContext context) where T : class
        {

            var lastEntity = context.GetTable<T>().OrderBy(PrimaryKeyName + " DESC").FirstOrDefault();
            var keyProp = _type.GetProperty(PrimaryKeyName);
            Int64 nextId = 0;

            if (lastEntity != null)
            {
                nextId = Convert.ToInt64(keyProp.GetValue(lastEntity, null));
            }

            _nextId = (nextId >= (_nextId ?? 0) ? nextId : (_nextId ?? 0));
            _nextId++;

            _keyProperty.SetValue(entity, PropertyUtility.ConvertToType(_keyProperty.PropertyType.Name, _nextId),
                null);

        }

        public void SetId<T>(T entity, Int64 id) where T : class
        {
            _keyProperty.SetValue(entity, PropertyUtility.ConvertToType(_keyProperty.PropertyType.Name, _nextId),
                null);
        }

        public void Copy<T>(Dictionary<string, object> source, T destination)
        {
            var type = typeof(T);

            foreach (var value in source.Where(e => _setterProperties.Any(p => p.Name == e.Key)))
            {
                if (value.Key == PrimaryKeyName) continue;
                var prop = type.GetProperty(value.Key);
                prop.SetValue(destination, PropertyUtility.ConvertToType(prop.PropertyType.Name, value.Value), null);
            }
        }

        public void Copy<T>(T source, T destination)
        {
            var type = typeof(T);

            foreach (var prop in _setterProperties)
            {
                if (prop.Name == PrimaryKeyName) continue;
                var value = prop.GetValue(source, null);
                prop.SetValue(destination, PropertyUtility.ConvertToType(prop.PropertyType.Name, value), null);
            }
        }

        public void SetStringNullToEmpty<T>(T source)
        {
            var type = typeof(T);

            foreach (var prop in _setterProperties.Where(e => e.PropertyType.Name.ToLower() == "string"))
            {
                if (prop.Name == PrimaryKeyName) continue;
                var value = prop.GetValue(source, null);
                if (value == null)
                {
                    prop.SetValue(source, "", null);
                }
            }
        }
    }
}