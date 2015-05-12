using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterGroceryList.Web.Models
{
    public interface IBetterListContext : IDisposable
    {
        //Account Models
        IDbSet<UserProfile> UserProfiles { get; }
        IDbSet<ExternalUserInformation> ExternalUsers { get; }

        //List Models
        IDbSet<BetterList> BetterLists { get; }
        IDbSet<BetterListItem> BetterListItems { get; }
        IDbSet<BetterListMember> BetterListMemberships { get; }
        IDbSet<SharedBetterList> SharedBetterLists { get; }

        int SaveChanges();
    }
}
