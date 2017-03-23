using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeChallenge
{
    [DebuggerDisplay("id: {_id}, name: {name}, email: {email}, url: {url}")]
    public class User : IIsExpanded
    {
        public User()
        {
            this.tags = new List<string>();
            this.SubmittedTickets = new List<Ticket>();
            this.AssignedTickets = new List<Ticket>();
        }
        public virtual int _id { get; set; }
        public virtual string url { get; set; }
        public virtual string external_id { get; set; }
        public virtual string name { get; set; }
        public virtual string alias { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual bool active { get; set; }
        public virtual bool verified { get; set; }
        public virtual bool shared { get; set; }
        public virtual string locale { get; set; }
        public virtual string timezone { get; set; }
        public virtual DateTime last_login_at { get; set; }
        public virtual string email { get; set; }
        public virtual string phone { get; set; }
        public virtual string signature { get; set; }
        public virtual int organization_id { get; set; }
        [ForeignKey("organization_id")]     // Field in this object
        public virtual Organization Organization { get; set; }
        public virtual List<string> tags { get; set; }
        public virtual bool suspended { get; set; }
        public virtual string role { get; set; }
        [ForeignKey("submitter_id")]    //  Field in ticket object.
        public virtual ICollection<Ticket> SubmittedTickets { get; set; }
        [ForeignKey("assignee_id")]    //  Field in ticket object.
        public virtual ICollection<Ticket> AssignedTickets { get; set; }
        private bool _IsExpanded = false;
        bool IIsExpanded.IsExpanded {  get { return _IsExpanded; } set { _IsExpanded = value; } }
    }
}
