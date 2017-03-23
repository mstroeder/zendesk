using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeChallenge;

namespace UnitTests
{
    [TestClass]
    public class OrganizationUnitTests
    {
        private ZenDbContext dbContext = new ZenDbContext();
        [TestMethod]
        public void FindOrganization121()
        {
            var results = SearchEngine.Find(dbContext, "Organizations", dbContext.Organizations, "_id", "121");
            if(results.Length != 1)
            {
                throw new Exception("Organization find for _id 121 failed");
            }
        }
        [TestMethod]
        public void FindOrganization3333()
        {
            var results = SearchEngine.Find(dbContext, "Organizations", dbContext.Organizations, "_id", "3333");
            if (results.Length != 0)
            {
                throw new Exception("Organization find for _id 3333 found a record");
            }
        }
    }
}
