using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge
{
    public class ZenCommand
    {
        public ZenCommand(string helpText)
        {
            this.HelpText = helpText;
        }
        public string HelpText;
        public virtual void Execute(ZenDbContext dbSet, string parameters)
        {
        }
    }
}
