using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge
{
    /// <summary>
    /// This is a container for all the tables accessible from this application, similar in concept to 
    /// an entity framework dbcontext. Individual tables will be loaded on first use.
    /// </summary>
    public class ZenDbContext
    {
        public ZenDbContext()
        {
            this.Entities = new Dictionary<string, EntityDefinition>(StringComparer.CurrentCultureIgnoreCase);
            this.Entities.Add("Users", new EntityDefinition("Users", typeof(User), typeof(List<User>)));
            this.Entities.Add("Tickets", new EntityDefinition("Tickets", typeof(Ticket), typeof(List<Ticket>)));
            this.Entities.Add("Organizations", new EntityDefinition("Organizations", typeof(Organization), typeof(List<Organization>)));
        }
        public Dictionary<string, EntityDefinition> Entities { get; set; }
        public IEnumerable Users
        {
            get
            {
                return GetListFromTableName("Users");
            }
        }
        public IEnumerable Tickets
        {
            get
            {
                return GetListFromTableName("Tickets");
            }
        }
        public IEnumerable Organizations
        {
            get
            {
                return GetListFromTableName("Organizations");
            }
        }
        public string GetEntityNameFromModelType(Type modelType)
        {
            foreach(var entityDef in this.Entities)
            {
                if(entityDef.Value.EntityType == modelType)
                {
                    return entityDef.Key;
                }
            }
            return null;
        }
        public EntityDefinition GetEntityDefinition(string table)
        {
            if (this.Entities.ContainsKey(table))
            {
                return this.Entities[table];
            }
            else
            {
                throw new Exception(string.Format("Unable to find table '{0}' on the data context", table));
            }
        }
        public IEnumerable GetListFromTableName(string table)
        {
            var entityDef = this.GetEntityDefinition(table);
            if(entityDef.EntityList == null)
            {
                entityDef.EntityList = (IEnumerable)JsonConvert.DeserializeObject(LoadFile(entityDef.Name), entityDef.ListType);
            }
            return entityDef.EntityList;
        }
        public string DisplayFieldsForTable(Type modelType)
        {
            var builder = new StringBuilder();
            foreach (var prop in modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty))
            {
                builder.AppendLine(prop.Name);
            }
            return builder.ToString();
        }
        private string LoadFile(string table)
        {
            return File.ReadAllText(Path.Combine(GetPath(), table + ".json"));
        }
        private string GetPath()
        {
            int pos;
            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            while (true)
            {
                if (Directory.Exists(Path.Combine(location, "Data")))
                {
                    return Path.Combine(location, "Data");
                }
                else
                {
                    pos = location.LastIndexOf(Path.DirectorySeparatorChar);
                    if (pos != -1)
                    {
                        location = location.Substring(0, pos);
                    }
                    else
                    {
                        throw new Exception("Unable to find json files in the current or parent directories.");
                    }
                }
            }
        }
    }
}
