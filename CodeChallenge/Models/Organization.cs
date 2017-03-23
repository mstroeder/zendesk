using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CodeChallenge
{
    [DebuggerDisplay("id: {_id}, name: {name}, details: {details}, url: {url}")]
    public class Organization : IIsExpanded
    {
        public Organization()
        {
            this.domain_names = new List<string>();
            this.tags = new List<string>();
        }
        public virtual int _id { get; set; }
        public virtual string url { get; set; }
        public virtual string external_id { get; set; }
        public virtual string name { get; set; }
        public virtual List<string> domain_names { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual string details { get; set; }
        public virtual bool shared_tickets { get; set; }
        public virtual List<string> tags { get; set; }
        private bool _IsExpanded = false;
        bool IIsExpanded.IsExpanded { get { return _IsExpanded; } set { _IsExpanded = value; } }
    }
}
