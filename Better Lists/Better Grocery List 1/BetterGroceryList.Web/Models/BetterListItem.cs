using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterGroceryList.Web.Models
{

    public class BetterListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }

        public virtual UserProfile ListItemOwner { get; set; }
        public virtual ICollection<BetterListMember> ListMemberships { get; set; }
    }
}