using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge
{
    /// <summary>
    /// Json resolver to only include the specified fields in the output json string.
    /// </summary>
    public class FieldsContractResolver : DefaultContractResolver
    {
        private string[] _Fields;
        private string _Table;
        public FieldsContractResolver(string table, string fields)
        {
            this._Table = table;
            fields = fields.Replace(" ", "");
            if(string.IsNullOrWhiteSpace(fields) || fields.Equals("*"))
            {
                _Fields = null;
            }
            else
            {
                _Fields = fields.Split(',');
            }
        }
        protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization)
        {
            var items = base.CreateProperties(type, memberSerialization);
            //  Only filter fields for specified entity type (top level).
            if ((type.Name + "s").Equals(_Table, StringComparison.CurrentCultureIgnoreCase))
            {
                if (_Fields != null && _Fields.Length != 0)
                {
                    for (int loop = items.Count - 1; loop >= 0; loop--)
                    {
                        if (!_Fields.Contains<string>(items[loop].PropertyName, StringComparer.CurrentCultureIgnoreCase))
                        {
                            items.RemoveAt(loop);
                        }
                    }
                }
            }
            return items;
        }
    }
}
