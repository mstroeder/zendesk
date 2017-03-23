using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge
{
    public class TablesCommand : ZenCommand
    {
        public TablesCommand(string text) : base(text)
        {

        }
        public override void Execute(ZenDbContext dbSet, string parameters)
        {
            var builder = new StringBuilder();
            foreach (var table in dbSet.Entities)
            {
                builder.AppendLine(table.Key);
            }
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Searchable tables");
            Console.WriteLine(builder.ToString());
        }
    }
}
