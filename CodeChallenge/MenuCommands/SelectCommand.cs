using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge
{
    public class SelectCommand : ZenCommand
    {
        public SelectCommand(string text) : base(text)
        {
        }
        public override void Execute(ZenDbContext dbContext, string parameters)
        {
            var queryOptions = new QueryOptions(parameters);
            var result = SearchEngine.Find(dbContext, queryOptions.Table, queryOptions.SearchField, queryOptions.SearchOperator, queryOptions.SearchValue);
            var settings = new JsonSerializerSettings()
            {
                //  Only include the specified fields in the output.
                ContractResolver = new FieldsContractResolver(queryOptions.Table, queryOptions.Fields)
            };
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented, settings));
            Console.WriteLine(string.Format("Found {0} results searching {1} for {2}", result.Length, queryOptions.Table, queryOptions.Where));
        }
    }
}
