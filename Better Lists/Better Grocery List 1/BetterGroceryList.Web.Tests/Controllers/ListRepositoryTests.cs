using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BetterGroceryList.Web.Controllers;
using BetterGroceryList.Web.Models;

namespace BetterGroceryList.Web.Tests.Controllers
{
    [TestClass]
    public class ListRepositoryTests
    {
        //IBetterListContext fakeContext = null;
        IBetterListContext testContext = null;
        IListRepository r = null;
        
        #region List Tests

        [TestMethod]
        public void CreateListTest()
        {
            this.testContext = RepositoryHelpers.getTestContext();
            using (this.r = new ListRepository(this.testContext))
            {
                UserProfile testUserProfile = this.testContext.UserProfiles.Where(u => u.UserName.StartsWith("UserName")).FirstOrDefault();
                int userId = testUserProfile.UserId;

                string listName = "TestList from CreateListTest";

                this.r.CreateList(listName, userId);

                IList<BetterList> lists = this.r.GetLists(userId);

                BetterList addedList = null;

                //is the new list in the list of lists for the user.
                foreach (BetterList l in lists)
                {
                    if (l.Name == listName && l.UserId == userId)
                    {
                        addedList = l;
                        break;
                    }
                }

                Assert.IsNotNull(addedList);
                Assert.AreEqual(addedList.UserId, userId);
                Assert.AreEqual(addedList.Name, listName);
            }
        }
        
        [TestMethod]
        public void GetListsTest()
        {
            this.testContext = RepositoryHelpers.getTestContext();
            using (this.r = new ListRepository(this.testContext))
            {
                int countOfListsFromContext = this.testContext.BetterLists.Count();

                IList<BetterList> lists = r.GetLists();

                Assert.AreEqual(lists.Count, countOfListsFromContext);
            }
        }

        [TestMethod]
        public void GetListsWUserIdTest()
        {
            this.testContext = RepositoryHelpers.getTestContext();
            using (this.r = new ListRepository(this.testContext))
            {
                UserProfile testUserProfile = this.testContext.UserProfiles.Where(u => u.UserName.StartsWith("UserName")).FirstOrDefault();
                int userId = testUserProfile.UserId;
                int countOfTestUserLists = this.testContext.BetterLists.Where(l => l.UserId == userId).Count();

                IList<BetterList> lists = this.r.GetLists(userId);

                Assert.AreEqual(lists.Count, countOfTestUserLists);
                this.testContext.BetterLists.Where(l => l.UserId == userId).ToList().ForEach(l => Assert.AreEqual(l.UserId, userId));
            }
        }

        [TestMethod]
        public void GetListWithItemsTest()
        {
            this.testContext = RepositoryHelpers.getTestContext();
            using (this.r = new ListRepository(this.testContext))
            {
                int testListId = this.testContext.BetterLists.FirstOrDefault().Id;
                BetterList testList = this.testContext.BetterLists.Include(l => l.ListMembers).Where(l => l.Id == testListId).FirstOrDefault();
                int itemCount = testList.ListMembers.Count();

                BetterList listWItems = this.r.GetListWithItems(testListId);

                Assert.IsNotNull(listWItems);
                Assert.IsNotNull(listWItems.ListMembers);
                Assert.AreEqual(listWItems.Id, testListId);
                Assert.AreEqual(listWItems.ListMembers.Count, itemCount);
            }
        }

        [TestMethod]
        public void SaveListTest()
        {
            this.testContext = RepositoryHelpers.getTestContext();
            using (this.r = new ListRepository(this.testContext))
            {
                string testListName = "TestList from SaveListTest";
                int userId = 1;
                BetterList listToSave = new BetterList();
                listToSave.Name = testListName;
                listToSave.UserId = userId;
                
                this.r.SaveList(listToSave);

                IList<BetterList> retrievedLists = this.r.GetLists(userId);

                BetterList retrievedList = null;

                foreach (BetterList l in retrievedLists)
                {
                    if (l.Name == listToSave.Name)
                    {
                        retrievedList = l;
                        break;
                    }
                }

                Assert.IsNotNull(retrievedList);
                Assert.AreEqual(retrievedList.Name, testListName);
                Assert.AreEqual(retrievedList.UserId, userId);
            }
        }

        #endregion

        #region List Item Tests

        [TestMethod]
        public void CreateListItemTest()
        {
            this.testContext = RepositoryHelpers.getTestContext();
            using (this.r = new ListRepository(this.testContext))
            {
                string itemName = "Test List Item for CreateListItemTest";
                int userId = this.testContext.UserProfiles.Where(u => u.UserName.StartsWith("UserName")).FirstOrDefault().UserId;
                int listId = this.testContext.BetterLists.Where(l => l.UserId == userId).FirstOrDefault().Id;

                this.r.CreateListItem(itemName, userId, listId);

                BetterListItem testItem = this.testContext.BetterListItems.Where(i => i.Name == itemName && i.UserId == userId).FirstOrDefault();

                BetterList testList = this.r.GetListWithItems(listId);

                BetterListMember testMembership = null;

                foreach (BetterListMember m in testList.ListMembers)
                {
                    if (m.ListMembershipId == listId && m.ListItemId == testItem.Id)
                    {
                        testMembership = m;
                        break;
                    }
                }

                Assert.IsNotNull(testMembership);
                Assert.AreEqual(testMembership.ListMembershipId, listId);
                Assert.AreEqual(testMembership.ListItemId, testItem.Id);
            }
        }

        [TestMethod]
        public void GetUserListItemsForUserTest()
        {
            this.testContext = RepositoryHelpers.getTestContext();
            using (this.r = new ListRepository(this.testContext))
            {
                int userId = this.testContext.UserProfiles.Where(u => u.UserName.StartsWith("UserName")).FirstOrDefault().UserId;

                IList<BetterListItem> testItems = this.testContext.BetterListItems.Where(i => i.UserId == userId).ToList();

                IList<BetterListItem> testUserItems = this.r.GetUserListItems(userId);

                foreach (BetterListItem i in testItems)
                {
                    Assert.IsTrue(testUserItems.Contains(i));
                }
            }
        }

        [TestMethod]
        public void GetUserListItemsQueryTest()
        {
            this.testContext = RepositoryHelpers.getTestContext();
            using (this.r = new ListRepository(this.testContext))
            {
                int userId = this.testContext.UserProfiles.Where(u => u.UserName.StartsWith("UserName")).FirstOrDefault().UserId;

                string queryText = "item";

                IList<BetterListItem> testItems = this.testContext.BetterListItems.Where(i => i.Name.ToLower().Contains(queryText.ToLower()) && i.UserId == userId).ToList();

                IList<BetterListItem> testUserItems = this.r.GetUserListItems(userId, queryText);

                foreach (BetterListItem i in testItems)
                {
                    Assert.IsTrue(testUserItems.Contains(i));
                }
            }
        }

        #endregion
        
    }
}
