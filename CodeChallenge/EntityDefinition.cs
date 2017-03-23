using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge
{
    public class EntityDefinition
    {
        public EntityDefinition(string name, Type entityType, Type listType)
        {
            this.Name = name;
            this.EntityType = entityType;
            this.ListType = listType;
        }
        public string Name;
        public Type EntityType;
        public Type ListType;
        public IEnumerable EntityList;
        public Dictionary<string, object> KeyedEntities;
        public object FindByKey(object id)
        {
            if (KeyedEntities == null)
            {
                KeyedEntities = new Dictionary<string, object>();
                var prop = EntityType.GetProperty("_id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
                foreach (var item in EntityList)
                {
                    KeyedEntities.Add(prop.GetValue(item).ToString(), item);
                }
            }
            if (KeyedEntities.ContainsKey(id.ToString()))
            {
                return KeyedEntities[id.ToString()];
            }
            else
            {
                return null;
            }
        }
    }
}
