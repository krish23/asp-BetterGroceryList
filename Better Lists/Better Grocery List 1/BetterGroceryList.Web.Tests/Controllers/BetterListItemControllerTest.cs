using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BetterGroceryList.Web.Controllers;
using BetterGroceryList.Web.Models;
using System.Web.Security;
using System.Web.Mvc;
using System.Collections.Generic;

using BetterGroceryList.Web.Tests.Fakes;


namespace BetterGroceryList.Web.Tests.Controllers
{
    class BetterListItemControllerTest
    {

        //Christopher Howe
        [TestMethod]
        public void EditListItem()
        {
            //Arrange
            BetterListController blc = new BetterListController();
            ListRepository repo = new ListRepository();
            BetterListItem testItem = new BetterListItem();
            testItem = repo.GetListItem(1);
            String newName = DateTime.Now.ToString() + "_testing_";
            testItem.Name = newName;

            //Act
            //blc.ConfirmEditItem(testItem);

            //Assert
            Assert.AreEqual(newName, repo.GetListItem(1).Name); //This uses actual database entries....we need to create a fake database.
        }


        // added by joshua tests for adding and deleting a list item
        [TestMethod]
        public void AddItem()
        {







            // arrange
            var db = new FakeIBetterListContext();
            db.AddSet(TestData.Users);

            db.UserProfiles.Create();

            BetterListController blc = new BetterListController();
            ListRepository repo = new ListRepository();
            BetterList bl = new BetterList();
            BetterListItem bli = new BetterListItem();
            String NewNameItem = DateTime.Now.ToString() + "_test List Item";
            String NewNameList = DateTime.Now.ToString() + "_test List";
            bli.Name = NewNameItem;
            bl.Name = NewNameList;

            //repo.CreateList("Test List", userProfile.UserId);

            //blc.AddItem(bli);

            // assert
            Assert.AreEqual(NewNameItem, repo.GetListItems(bl.Id));

        }

    }
}
