using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge
{
    /// <summary>
    /// Contains the parsed options of a select command.
    /// </summary>
    class QueryOptions
    {
        public string Fields = "";
        public string Table = "";
        public string Where = "";
        public string SearchField = "";
        public string SearchOperator = "";
        public string SearchValue = "";
        public string FullText = "";
        public QueryOptions(string parameters)
        {
            this.FullText = parameters;
            string data = parameters.Trim();
            int pos;
            pos = data.IndexOf(" from ", StringComparison.CurrentCultureIgnoreCase);
            if(pos != -1)
            {
                this.Fields = data.Substring(0, pos).Trim();
                data = data.Substring(pos + 6);
            }
            pos = data.IndexOf(" where ", StringComparison.CurrentCultureIgnoreCase);
            if (pos != -1)
            {
                this.Table = data.Substring(0, pos).Trim();
                data = data.Substring(pos + 7);
                this.Where = data;
            }
            else
            {
                this.Table = data;
                this.Where = string.Empty;
            }
            //  Parse the where clause
            this.SearchField = string.Empty;
            this.SearchOperator = string.Empty;
            this.SearchValue = string.Empty;
            pos = this.Where.IndexOfAny(new char[] { ' ', '=', '<', '>' });
            if (pos != -1)
            {
                var whereData = this.Where;
                this.SearchField = whereData.Substring(0, pos);
                whereData = whereData.Substring(pos).TrimStart();
                if (whereData.Length > 1)
                {
                    this.SearchOperator = whereData.Substring(0, 1);
                    whereData = whereData.Substring(1).TrimStart().TrimEnd();
                }
                this.SearchValue = whereData;
                if (this.SearchValue.StartsWith("\"") && this.SearchValue.EndsWith("\"") && this.SearchValue.Length >= 2)
                {
                    this.SearchValue = this.SearchValue.Substring(1);
                    this.SearchValue = this.SearchValue.Substring(0, this.SearchValue.Length - 1);
                }
            }
            string searchTable = string.Empty;
            pos = this.SearchField.IndexOf('.');
            if (pos != -1)
            {
                this.Table = this.SearchField.Substring(0, pos);
                this.SearchField = this.SearchField.Substring(pos + 1);
            }
        }
    }
}
