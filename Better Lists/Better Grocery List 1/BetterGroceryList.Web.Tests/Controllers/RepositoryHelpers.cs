using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterGroceryList.Web.Controllers;
using BetterGroceryList.Web.Models;

namespace BetterGroceryList.Web.Tests.Controllers
{
    public static class RepositoryHelpers
    {
        #region Repository Helpers

        //public static IListRepository GetDefaultRepository()
        //{
        //    return new ListRepository(getTestContext());
        //}

        //public static IListRepository GetSpecificRepository(IBetterListContext context)
        //{
        //    return new ListRepository(context);
        //}

        #endregion

        //#region Context Helpers

        //public static IBetterListContext getFakeContext()
        //{
        //    IBetterListContext context = null;
        //    context = new FakeBetterListContext
        //    {
        //        UserProfiles = 
        //                {
        //                    new UserProfile { UserId = 1, UserName = "username1@domain.com", FullName = "Fullname1" },
        //                    new UserProfile { UserId = 2, UserName = "username2@domain.com", FullName = "Fullname2" },
        //                    new UserProfile { UserId = 3, UserName = "username3@domain.com", FullName = "Fullname3" },
        //                    new UserProfile { UserId = 4, UserName = "username4@domain.com", FullName = "Fullname4" }
        //                },
        //        BetterLists =
        //                {
        //                    new BetterList { Id = 1, Name = "TestList1", UserId = 1 },
        //                    new BetterList { Id = 2, Name = "TestList2", UserId = 2 },
        //                    new BetterList { Id = 3, Name = "TestList3", UserId = 3 },
        //                    new BetterList { Id = 4, Name = "TestList4", UserId = 4 }
        //                },
        //        BetterListItems =
        //                {
        //                    new BetterListItem { Id = 1, Name = "TestListItem1", UserId = 1 },
        //                    new BetterListItem { Id = 2, Name = "TestListItem2", UserId = 2 },
        //                    new BetterListItem { Id = 3, Name = "TestListItem3", UserId = 3 },
        //                    new BetterListItem { Id = 4, Name = "TestListItem4", UserId = 4 }
        //                },
        //        BetterListMemberships =
        //                {
        //                    new BetterListMember { Id = 1, ListMembershipId = 1, ListItemId = 1, Quantity = 1 },
        //                    new BetterListMember { Id = 2, ListMembershipId = 2, ListItemId = 2, Quantity = 1 },
        //                    new BetterListMember { Id = 3, ListMembershipId = 3, ListItemId = 3, Quantity = 1 },
        //                    new BetterListMember { Id = 4, ListMembershipId = 4, ListItemId = 4, Quantity = 1 },
        //                    new BetterListMember { Id = 5, ListMembershipId = 2, ListItemId = 3, Quantity = 1 },
        //                    new BetterListMember { Id = 6, ListMembershipId = 2, ListItemId = 4, Quantity = 1 },
        //                }
        //    };

        //    //Setup BetterListItems->ListMembers
        //    ICollection<BetterListMember> listMembers = new List<BetterListMember>();

        //    //Members for list item 1
        //    listMembers.Add(context.BetterListMemberships.Find(1));

        //    context.BetterListItems.Find(1).ListMemberships = listMembers;

        //    //Members for list item 2
        //    listMembers = new List<BetterListMember>();
        //    listMembers.Add(context.BetterListMemberships.Find(2));

        //    context.BetterListItems.Find(2).ListMemberships = listMembers;

        //    //Members for list item 3
        //    listMembers = new List<BetterListMember>();
        //    listMembers.Add(context.BetterListMemberships.Find(3));
        //    listMembers.Add(context.BetterListMemberships.Find(5));

        //    context.BetterListItems.Find(3).ListMemberships = listMembers;

        //    //Members for list item 4
        //    listMembers = new List<BetterListMember>();
        //    listMembers.Add(context.BetterListMemberships.Find(4));
        //    listMembers.Add(context.BetterListMemberships.Find(6));

        //    context.BetterListItems.Find(4).ListMemberships = listMembers;

        //    //Setup BetterList->ListMembers
        //    listMembers = new List<BetterListMember>();

        //    //Members for list 1
        //    listMembers.Add(context.BetterListMemberships.Find(1));

        //    context.BetterLists.Find(1).ListMembers = listMembers;

        //    //Members for list 2
        //    listMembers = new List<BetterListMember>();

        //    listMembers.Add(context.BetterListMemberships.Find(2));
        //    listMembers.Add(context.BetterListMemberships.Find(5));
        //    listMembers.Add(context.BetterListMemberships.Find(6));

        //    context.BetterLists.Find(2).ListMembers = listMembers;

        //    //Members for list 3
        //    listMembers = new List<BetterListMember>();

        //    listMembers.Add(context.BetterListMemberships.Find(3));

        //    context.BetterLists.Find(3).ListMembers = listMembers;

        //    //Members for list 4
        //    listMembers = new List<BetterListMember>();
        //    listMembers.Add(context.BetterListMemberships.Find(4));

        //    context.BetterLists.Find(4).ListMembers = listMembers;

        //    return context;
        //}

        //public static IBetterListContext getTestContext()
        //{
        //    IBetterListContext context = null;

        //    context = new Models.BetterListContext("TestConnection");

        //    return context;
        //}

        //#endregion
    }
}
