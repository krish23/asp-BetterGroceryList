using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterGroceryList.Web.Controllers
{
    public interface IListRepository : IDisposable
    {
        //User Profiles
        int GetUserId(string UserName);

        //Better Lists
        void CreateList(string ListName, int UserId);
        IList<Models.BetterList> GetLists();
        IList<Models.BetterList> GetLists(int UserId, bool IncludeShared = false);
        Models.BetterList GetListWithItems(int ListId);
        void SaveList(Models.BetterList BetterList);

        //Better List Items
        void CreateListItem(string ListItemName, int UserId, int ListId);
        void UpdateListItem(string ListItemName, int UserId, int ItemId);
        void UpdateListItem(string ListItemName, int ItemId);
        Models.BetterListItem GetListItem(int ItemId);
        IList<Models.BetterListItem> GetUserListItems(int UserId);
        IList<Models.BetterListItem> GetUserListItems(int UserId, string QueryText);
        IList<Models.BetterListItem> GetListItems(int ListId);

        //Better List Item Memberships
        void AddItemToList(int ListId, int ItemId);
        void AddItemToList(int ListId, Models.BetterListItem Item);
        void AddItemToList(int ListId, int UserId, string ItemName);
        void RemoveItemFromList(int MembershipId);
        void RemoveItemFromList(int ListId, int ItemId);
        void UpdateListItemQuantity(int MembershipId, int Quantity);
        void UpdateListItemQuantity(int ListId, int ItemId, int Quantity);
        void IncrementListItemQuantity(int ListId, int ItemId, int Increment);

        //Better List Sharing
        IList<Models.SharedBetterList> GetSharedBetterLists(int UserId = 0);
        void ShareList(int ListId, string UserEmailAddress, Guid SharingId);
        Models.SharedBetterList GetSharedBetterList(int ListId, Guid SharingId);
        void ConfirmListSharing(int ListId, int UserId, Guid SharingId);
        void RemoveListSharing(int ListId, int UserId);
        void SaveSharedList(Models.SharedBetterList List);



        
    }
}
