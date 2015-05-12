using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterGroceryList.Web.Models
{
    public class SharedBetterList
    {
        public int Id { get; set; }
        public int? UserId { get; set; } //User list shared with
        public string UserEmailAddress { get; set; } //Email address of user getting list shared.
        public int ListId { get; set; } //List user is granted access to.
        public Guid SharingId { get; set; }
        public bool SharingConfirmed { get; set; }

        public virtual UserProfile UserGrantedToList { get; set; }
        public virtual BetterList List { get; set; }
    }
}