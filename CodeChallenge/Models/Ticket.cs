using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeChallenge
{
    [DebuggerDisplay("id: {_id}, subject: {subject}, status: {status}, url: {url}")]
    public class Ticket : IIsExpanded
    {
        public Ticket()
        {
            this.tags = new List<string>();
        }
        public virtual string _id { get; set; }
        public virtual string url { get; set; }
        public virtual string external_id { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual string type { get; set; }
        public virtual string subject { get; set; }
        public virtual string description { get; set; }
        public virtual string priority { get; set; }
        public virtual string status { get; set; }
        public virtual int submitter_id { get; set; }
        //[ForeignKey("submitter_id")]
        //public virtual User Submitter { get; set; }
        public virtual int assignee_id { get; set; }
        //[ForeignKey("assignee_id")]
        //public virtual User Assignee { get; set; }
        public virtual int organization_id { get; set; }
        [ForeignKey("organization_id")]
        public virtual Organization Organization { get; set; }
        public virtual List<string> tags { get; set; }
        public virtual bool has_incidents { get; set; }
        public virtual DateTime due_at { get; set; }
        public virtual string via { get; set; }
        private bool _IsExpanded = false;
        bool IIsExpanded.IsExpanded { get { return _IsExpanded; } set { _IsExpanded = value; } }
    }
}
