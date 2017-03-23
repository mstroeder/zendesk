using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeChallenge;

namespace UnitTests
{
    [TestClass]
    public class UserUnitTests
    {
        private ZenDbContext dbContext = new ZenDbContext();
        [TestMethod]
        public void FindUser71()
        {
            var results = SearchEngine.Find(dbContext, "Users", dbContext.Users, "_id", "71");
            if(results.Length != 1)
            {
                throw new Exception("User find for _id 71 failed");
            }
        }
        [TestMethod]
        public void FindUser3333()
        {
            var results = SearchEngine.Find(dbContext, "Users", dbContext.Users, "_id", "3333");
            if (results.Length != 0)
            {
                throw new Exception("User find for _id 3333 found a record");
            }
        }
    }
}
