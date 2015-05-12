using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace BetterGroceryList.Web.Models
{
    public class BetterListContext : DbContext, IBetterListContext
    {
        public BetterListContext()
            : base("name=DefaultConnection")
        {
            //Lazy loading and circular references
            //http://code.msdn.microsoft.com/Loop-Reference-handling-in-caaffaf7
            //http://johnnycode.com/2012/04/10/serializing-circular-references-with-json-net-and-entity-framework/
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public BetterListContext(string ConnectionStringName)
            : base(ConnectionStringName)
        {
            //Lazy loading and circular references
            //http://code.msdn.microsoft.com/Loop-Reference-handling-in-caaffaf7
            //http://johnnycode.com/2012/04/10/serializing-circular-references-with-json-net-and-entity-framework/
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        //Account Models
        public IDbSet<UserProfile> UserProfiles { get; set; }
        public IDbSet<ExternalUserInformation> ExternalUsers { get; set; }

        //List Models
        public IDbSet<BetterList> BetterLists { get; set; }
        public IDbSet<BetterListItem> BetterListItems { get; set; }
        public IDbSet<BetterListMember> BetterListMemberships { get; set; }
        public IDbSet<SharedBetterList> SharedBetterLists { get; set; }

    }
}
