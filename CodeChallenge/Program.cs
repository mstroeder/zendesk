using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace CodeChallenge
{
    class Program
    {
        /// <summary>
        /// Build a list of commands available from the search application.
        /// </summary>
        /// <param name="dbSet"></param>
        /// <returns></returns>
        static Dictionary<string, ZenCommand> GetCommands()
        {
            var commands = new Dictionary<string, ZenCommand>(StringComparer.CurrentCultureIgnoreCase);
            commands.Add("select", new SelectCommand("search for items using the following syntax" + 
                "\r\n\tselect [fields] from [table] [where [field]=[value]]" + 
                "\r\n\t\tfields must be * or a comma delimited list of fields in the table" +
                "\r\n\t\ttable must be a single table name from the searchable list of tables" +
                "\r\n\t\twhere is optional, but if specified, must be fieldname = or < or > value" +
                "\r\n\tselect * from Users where _id=71" + 
                "\r\n\tselect * from Tickets where status=\"hold\"" + 
                "\r\n\tselect _id,name,organization from Users where verified=true" +
                "\r\n\tselect _id,name,verified from Users" +
                "\r\n\tselect _id,name,details,tags from Organizations where created_at>2016-05-05" + 
                "\r\n\tValid operators are:" + 
                    "\r\n\t\t= (for all types)" + 
                    "\r\n\t\t< (for int and DateTime types)" + 
                    "\r\n\t\t> (for int and DateTime types)"));
            commands.Add("fields", new FieldsCommand("view a list of searchable fields\r\n\tOptionally pass a table name to only show fields for that table\r\n\tfields Users"));
            commands.Add("tables", new TablesCommand("view a list of searchable tables"));
            return commands;
        }
        static void Main(string[] args)
        {
            var dbSet = new ZenDbContext();
            var commands = GetCommands();
            string userCommand;
            string parameters;
            int pos;
            Console.Write("Welcome to Zendesk Search\r\nType 'quit' to exit at any time, Type 'help' to get a list of available commands, Press 'Enter' to continue\r\n");
            while (true)
            {
                try
                {
                    //  Display current menu options each time through the loop.
                    Console.Write("Zendesk Search>");
                    userCommand = Console.ReadLine();
                    pos = userCommand.IndexOf(' ');
                    if(pos != -1)
                    {
                        parameters = userCommand.Substring(pos + 1);
                        userCommand = userCommand.Substring(0, pos);
                    }
                    else
                    {
                        parameters = string.Empty;
                    }
                    //  quit and 0 are hard coded always available options.
                    if(userCommand.Equals("quit"))
                    {
                        return;
                    }
                    else if(userCommand.Equals("help"))
                    {
                        Console.WriteLine("Available commands:");
                        foreach(var item in commands)
                        {
                            Console.WriteLine(item.Key + "\t" + item.Value.HelpText);
                        }
                    }
                    else if (commands.ContainsKey(userCommand))
                    {
                        commands[userCommand].Execute(dbSet, parameters);
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Unknown command '{0}'", userCommand));
                    }
                }
                catch (Exception ex)
                {
                  Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
