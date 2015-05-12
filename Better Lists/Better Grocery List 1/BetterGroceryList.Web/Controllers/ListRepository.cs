using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BetterGroceryList.Web.Models;

namespace BetterGroceryList.Web.Controllers
{
    public class ListRepository : IListRepository, IDisposable
    {
        private IBetterListContext db = null;
        private bool disposed;

        public ListRepository()
        {
            this.db = new BetterListContext();
            disposed = false;
        }

        public ListRepository(IBetterListContext context)
        {
            this.db = context;
            disposed = false;
        }

        #region User Profiles

        public int GetUserId(string UserName)
        {
            int userId = 0;
            UserProfile userProfile = db.UserProfiles.Where(u => u.UserName == UserName).FirstOrDefault();
            if (userProfile != null)
            {
                userId = userProfile.UserId;
            }
            return userId;
        }

        #endregion

        #region Better Lists

        public void CreateList(string ListName, int UserId)
        {
            BetterList list = db.BetterLists.Create();
            list.Name = ListName;
            list.UserId = UserId;
            db.BetterLists.Add(list);
            db.SaveChanges();
        }

        public IList<Models.BetterList> GetLists()
        {
            IList<Models.BetterList> lists;
            lists = db.BetterLists.ToList();            
            return lists;
        }

        public IList<Models.BetterList> GetLists(int UserId, bool IncludeShared = false)
        {
            IList<Models.BetterList> usersLists;
            usersLists = db.BetterLists.Where(l => l.UserId == UserId).ToList();
            if (IncludeShared)
            {
                IList<SharedBetterList> sharedLists = db.SharedBetterLists.Include(l => l.List).Where(l => l.UserId == UserId && l.SharingConfirmed).ToList();
                foreach (SharedBetterList sharedList in sharedLists)
                {
                    usersLists.Add(sharedList.List);
                }
            }
            return usersLists;
        }

        public BetterList GetListWithItems(int ListId)
        {
            BetterList list = null;
            list = db.BetterLists
                        .Where(l => l.Id == ListId)
                        .Include(l => l.ListMembers)
                        .Include("ListMembers.ListItem")
                        .FirstOrDefault();
            return list;
        }

        public void SaveList(Models.BetterList BetterList)
        {
            BetterList list = db.BetterLists.Where(l => l.Id == BetterList.Id).SingleOrDefault();
            if (list == null)
            {
                //Must be a new list
                list = new BetterList();
                db.BetterLists.Add(list);
            }

            list.Name = BetterList.Name;
            list.UserId = BetterList.UserId;
                
            db.SaveChanges();
        }

        #endregion

        #region Better List Items

        public void CreateListItem(string ListItemName, int UserId, int ListId)
        {
            BetterListItem item = null;
            var existingItem = db.BetterListItems.Where(i => i.Name.ToLower() == ListItemName.ToLower()).FirstOrDefault();
            if (existingItem == null)
            {
                //item doesn't already exist create it
                item = db.BetterListItems.Create();
                item.Name = ListItemName;
                item.UserId = UserId;
                db.BetterListItems.Add(item);
                db.SaveChanges();
            }
            else
            {
                //item exists
                item = existingItem;
            }
            //now add it to the list
            AddItemToList(ListId, item.Id);
        }

        public void UpdateListItem(string ListItemName, int UserId, int ItemId)
        {
            var existingItem = db.BetterListItems.Where(i => i.Id == ItemId).FirstOrDefault();
            if (existingItem == null)
            {
                throw new Exception("Item not found with id: " + ItemId.ToString());
            }

            existingItem.Name = ListItemName;
            db.SaveChanges();
        }

        /* I'm Christopher Howe, and I approve this method. 
        * (The original method required a userID, with which the function did nothing, 
        *  so I created an overloaded function that does the same exact thing except 
        *  I took out the user ID parameter. This facilitated user testing) */
        public void UpdateListItem(string ListItemName, int ItemId)
        {
            var existingItem = db.BetterListItems.Where(i => i.Id == ItemId).FirstOrDefault();
            if (existingItem == null)
            {
                throw new Exception("Item not found with id: " + ItemId.ToString());
            }

            existingItem.Name = ListItemName;
            db.SaveChanges();
        }

        public Models.BetterListItem GetListItem(int ItemId)
        {
            BetterListItem item = db.BetterListItems.SingleOrDefault(i => i.Id == ItemId);
            return item;
        }

        public IList<BetterListItem> GetUserListItems(int UserId)
        {
            IList<BetterListItem> items = null;
            
            items = db.BetterListItems
                .Include(i => i.ListItemOwner)
                .Include(i => i.ListMemberships)
                .Where(i => i.UserId == UserId)
                .ToList();
            
            return items;
        }

        public IList<BetterListItem> GetUserListItems(int UserId, string QueryText)
        {
            IList<BetterListItem> items = null;
            items = db.BetterListItems.Where(i => i.UserId == UserId && i.Name.ToLower().Contains(QueryText.ToLower())).ToList();
            return items;
        }

        public IList<BetterListItem> GetListItems(int ListId)
        {
            IList<BetterListItem> items = null;

            items = db.BetterListMemberships.Include(m => m.ListItem).Where(m => m.ListMembershipId == ListId).Select(m => m.ListItem).ToList();

            return items;
        }

        #endregion

        #region Better List Memberships

        public void AddItemToList(int ListId, int ItemId)
        {
            if (ListId > 0 && ItemId > 0)
            {
                AddListItemMember(ListId, ItemId);
            }
        }

        public void AddItemToList(int ListId, BetterListItem Item)
        {
            if (ListId > 0 && Item.Id > 0)
            {
                AddListItemMember(ListId, Item.Id);
            }
        }

        public void AddItemToList(int ListId, int UserId, string ItemName)
        {
            CreateListItem(ItemName, UserId, ListId);
        }

        public void RemoveItemFromList(int MembershipId)
        {
            BetterListMember member = db.BetterListMemberships.Where(m => m.Id == MembershipId).FirstOrDefault();
            if (member != null)
            {
                db.BetterListMemberships.Remove(member);
                db.SaveChanges();
            }
        }

        public void RemoveItemFromList(int ListId, int ItemId)
        {
            BetterListMember member = db.BetterListMemberships.Where(m => m.ListMembershipId == ListId && m.ListItemId == ItemId).FirstOrDefault();
            if (member != null)
            {
                db.BetterListMemberships.Remove(member);
                db.SaveChanges();
            }
        }

        public void UpdateListItemQuantity(int MembershipId, int Quantity)
        {
            if (MembershipId > 0)
            {
                BetterListMember member = db.BetterListMemberships.Where(m => m.Id == MembershipId).FirstOrDefault();
                if (member != null)
                {
                    if (Quantity > 0)
                    {
                        member.Quantity = Quantity;
                        db.SaveChanges();
                    }
                    else
                    {
                        RemoveItemFromList(member.Id);
                    }
                }
            }
        }

        public void UpdateListItemQuantity(int ListId, int ItemId, int Quantity)
        {
            if (ListId > 0 && ItemId > 0)
            {
                BetterListMember member = db.BetterListMemberships.Where(m => m.ListMembershipId == ListId && m.ListItemId == ItemId).FirstOrDefault();
                if (member != null)
                {
                    if (Quantity > 0)
                    {
                        member.Quantity = Quantity;
                        db.SaveChanges();
                    }
                    else
                    {
                        RemoveItemFromList(member.Id);
                    }
                }
            }
        }

        public void IncrementListItemQuantity(int ListId, int ItemId, int Increment)
        {
            if (ListId > 0 && ItemId > 0)
            {
                BetterListMember member = db.BetterListMemberships.Where(m => m.ListMembershipId == ListId && m.ListItemId == ItemId).FirstOrDefault();
                if (member != null)
                {
                    if (member.Quantity + Increment > 0)
                    {
                        member.Quantity += Increment;
                        db.SaveChanges();
                    }
                    else
                    {
                        RemoveItemFromList(member.Id);
                    }
                }
            }
        }

        #endregion

        #region Better List Sharing

        public IList<SharedBetterList> GetSharedBetterLists(int UserId = 0)
        {
            IList<SharedBetterList> lists = null;
            if (UserId > 0)
            {
                //Get shared lists for a specific user
                lists = db.SharedBetterLists.Where(l => l.UserId == UserId).ToList();
            }
            else
            {
                //Get all shared lists
                lists = db.SharedBetterLists.ToList();
            }
            return lists;
        }

        public void ShareList(int ListId, string UserEmailAddress, Guid SharingId)
        {
            SharedBetterList sharedList = db.SharedBetterLists.Create();
            sharedList.ListId = ListId;
            sharedList.UserEmailAddress = UserEmailAddress;
            sharedList.SharingId = SharingId;
            sharedList.SharingConfirmed = false;
            db.SharedBetterLists.Add(sharedList);
            db.SaveChanges();
        }

        public SharedBetterList GetSharedBetterList(int ListId, Guid SharingId)
        {
            SharedBetterList list = db.SharedBetterLists.Include(l => l.List).Where(l => l.ListId == ListId && l.SharingId == SharingId).FirstOrDefault();
            return list;
        }

        public void ConfirmListSharing(int ListId, int UserId, Guid SharingId)
        {
            SharedBetterList sharedList = db.SharedBetterLists.Where(l => l.ListId == ListId && l.SharingId == SharingId).FirstOrDefault();
            if (sharedList != null)
            {
                sharedList.UserId = UserId;
                sharedList.SharingConfirmed = true;
                db.SaveChanges();
            }
        }

        public void RemoveListSharing(int ListId, int UserId)
        {
            SharedBetterList sharedList = db.SharedBetterLists.Where(l => l.UserId == UserId && l.ListId == ListId).FirstOrDefault();
            if (sharedList != null)
            {
                db.SharedBetterLists.Remove(sharedList);
                db.SaveChanges();
            }
        }

        public void SaveSharedList(Models.SharedBetterList List)
        {
            if (List != null)
            {
                SharedBetterList sharedList = db.SharedBetterLists.Where(l => l.ListId == List.ListId && l.SharingId == List.SharingId).FirstOrDefault();
                if (sharedList != null)
                {
                    sharedList.SharingConfirmed = List.SharingConfirmed;
                    sharedList.UserId = List.UserId.HasValue && List.UserId > 0 ? List.UserId : null;
                    db.SaveChanges();
                }
            }
        }

        #endregion

        #region Helpers and General Interface Implementations

        private void AddListItemMember(int ListId, int ItemId)
        {
            BetterListMember member = db.BetterListMemberships.Create();
            member.ListItemId = ItemId;
            member.ListMembershipId = ListId;
            member.Quantity = 1;
            db.BetterListMemberships.Add(member);
            db.SaveChanges();
        }

        //Dispose Pattern
        //http://msdn.microsoft.com/en-us/library/fs2xkftw(v=vs.110).aspx
        public void Dispose()
        {
            Dispose(true);
            // Call SupressFinalize in case a subclass implements a finalizer.
            GC.SuppressFinalize(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (db != null)
                    {
                        db.Dispose();
                    }
                }
            }
            db = null;
            disposed = true;
        }

        #endregion

    }
}
