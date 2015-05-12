using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace BetterGroceryList.Web.Models
{
    public class BetterListMember
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ListMembershipId { get; set; }
        public int ListItemId { get; set; }

        public virtual BetterList ListMembership { get; set; }
        public virtual BetterListItem ListItem { get; set; }
    }
}