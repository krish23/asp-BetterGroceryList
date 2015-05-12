namespace BetterGroceryList.Web.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<BetterGroceryList.Web.Models.BetterListContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BetterGroceryList.Web.Models.BetterListContext context)
        {
            //Add Seed Data
            context.UserProfiles.AddOrUpdate(
                u => u.UserName,
                new Models.UserProfile { UserName = "UserName1" },
                new Models.UserProfile { UserName = "UserName2" },
                new Models.UserProfile { UserName = "UserName3" },
                new Models.UserProfile { UserName = "UserName4" },
                new Models.UserProfile { UserName = "UserName5" }
                );

            context.SaveChanges();

            ICollection<Models.BetterList> betterLists = new List<Models.BetterList>();

            context.UserProfiles.Where(u => u.UserName != "keg7271@gmail.com").ToList().ForEach(
                u => betterLists.Add(new Models.BetterList { Name = "TestList" + u.UserId.ToString(), UserId = u.UserId })
                );

            context.UserProfiles.Where(u => u.UserName != "keg7271@gmail.com").ToList().ForEach(
                u => betterLists.Add(new Models.BetterList { Name = "TestList" + (u.UserId + 1).ToString(), UserId = u.UserId })
                );

            context.BetterLists.AddOrUpdate(
                l => new { l.Name, l.UserId },
                betterLists.ToArray()
                );

            context.SaveChanges();

            ICollection<Models.BetterListItem> betterListItems = new List<Models.BetterListItem>();

            context.UserProfiles.Where(u => u.UserName != "keg7271@gmail.com").ToList().ForEach(
                u => betterListItems.Add(new Models.BetterListItem { Name = "TestListItem" + u.UserId.ToString(), UserId = u.UserId })
                );

            context.UserProfiles.Where(u => u.UserName != "keg7271@gmail.com").ToList().ForEach(
                u => betterListItems.Add(new Models.BetterListItem { Name = "TestListItem" + (u.UserId + 1).ToString(), UserId = u.UserId })
                );

            context.BetterListItems.AddOrUpdate(
                i => new { i.Name, i.UserId },
                betterListItems.ToArray()
                );

            context.SaveChanges();

            ICollection<Models.BetterListMember> betterListMemberships = new List<Models.BetterListMember>();

            context.UserProfiles.ToList().ForEach(
                u => context.BetterLists.Where(l => l.UserId == u.UserId).ToList().ForEach(
                    l => context.BetterListItems.Where(i => i.UserId == u.UserId).ToList().ForEach(
                        i => betterListMemberships.Add(new Models.BetterListMember { ListItemId = i.Id, ListMembershipId = l.Id, Quantity = 1 })
                        )
                    )
                );

            context.BetterListMemberships.AddOrUpdate(
                m => new { m.ListItemId, m.ListMembershipId },
                betterListMemberships.ToArray()
                );

            context.SaveChanges();
        }
    }
}
