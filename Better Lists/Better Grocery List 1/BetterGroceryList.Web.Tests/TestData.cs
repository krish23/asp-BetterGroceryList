using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterGroceryList.Web.Models;

namespace BetterGroceryList.Web.Tests
{
    class TestData
    {
        public static IQueryable<UserProfile> Users
        {
            get
            {
                var testUserList = new List<UserProfile>();
                var testUser = new UserProfile();
                testUser.UserId = 1;
                testUser.UserName = "Test User 1";
                testUserList.Add(testUser);
                return testUserList.AsQueryable();
               
            }

        }
    }
}
