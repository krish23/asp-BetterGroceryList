using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterGroceryList.Web.Models
{
    public class BetterList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }

        public virtual UserProfile ListOwner { get; set; }
        public virtual ICollection<BetterListMember> ListMembers { get; set; }
    }
}
