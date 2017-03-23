using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge
{
    public class FieldsCommand : ZenCommand
    {
        public FieldsCommand(string text) : base(text)
        {

        }
        public override void Execute(ZenDbContext dbSet, string parameters)
        {
            parameters = parameters.Trim();
            foreach(var table in dbSet.Entities)
            {
                if (string.IsNullOrWhiteSpace(parameters) ||
                    table.Key.Equals(parameters, StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("-------------------------------------");
                    Console.WriteLine(string.Format("Search {0} with", table.Key));
                    Console.WriteLine(dbSet.DisplayFieldsForTable(table.Value.EntityType));
                }
            }
        }
    }
}
