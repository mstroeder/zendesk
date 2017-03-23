using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeChallenge;

namespace UnitTests
{
    [TestClass]
    public class TicketUnitTests
    {
        private ZenDbContext dbContext = new ZenDbContext();
        [TestMethod]
        public void FindTicket()
        {
            var results = SearchEngine.Find(dbContext, "Tickets", dbContext.Tickets, "_id", "1a227508-9f39-427c-8f57-1b72f3fab87c");
            if(results.Length != 1)
            {
                throw new Exception("Ticket find failed");
            }
        }
        [TestMethod]
        public void FindTicket3333()
        {
            var results = SearchEngine.Find(dbContext, "Tickets", dbContext.Tickets, "_id", "3333");
            if (results.Length != 0)
            {
                throw new Exception("Ticket find for _id 3333 found a record");
            }
        }
        [TestMethod]
        public void FindTicketsSubmittedByUser71()
        {
            var results = SearchEngine.Find(dbContext, "Tickets", dbContext.Tickets, "submitter_id", "71");
            if (results.Length != 3)
            {
                throw new Exception(string.Format("Ticket find failed - {0} found, {1} expected", results.Length, 3));
            }
        }
    }
}
