using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BetterGroceryList.Web.Controllers;
using BetterGroceryList.Web.Models;
using System.Web.Security;
using System.Web.Mvc;
using System.Collections.Generic;


namespace BetterGroceryList.Web.Tests.Controllers
{


	[TestClass]
	public class BetterListControllerTests
	{

		[TestMethod]
		public void Index()
		{
            // arrange
			BetterListController blc = new BetterListController();
            ListRepository repo = new ListRepository();
            IList<BetterList> betterlist;
            betterlist = repo.GetLists(1);

          //act
            var result = blc.Index() as ContentResult;

          // Assert
         Assert.IsNotNull(result);
         Assert.AreEqual("Testing List", result.Content);


		}

		[TestMethod]
		public void ShowEdit()
		{
			Assert.AreEqual(1, 1);
		}

		[TestMethod]
        public void CreateList()
        {
            BetterListController blc = new BetterListController();
            ListRepository repo = new ListRepository();
            BetterList bl = new BetterList();


            string testIt = "Test It";
      
            bl.Id = 9999;
            bl.Name = testIt;


            blc.CreateList(bl);

            Assert.AreEqual(testIt, repo.GetListWithItems(9999).Name);


        }

		[TestMethod]
		public void SaveEdit()
		{
			// arrange
			BetterListController blc = new BetterListController();
			ListRepository repo = new ListRepository();
			BetterList bl = repo.GetListWithItems(1);
			String NewName = DateTime.Now.ToString() + "_test";
			bl.Name = NewName;


			// act
			blc.Edit(bl);

			// assert
			Assert.AreEqual(NewName, repo.GetListWithItems(1).Name);

		}


       
        

	}
}
