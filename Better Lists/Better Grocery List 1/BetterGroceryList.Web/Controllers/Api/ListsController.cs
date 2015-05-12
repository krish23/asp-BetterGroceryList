using BetterGroceryList.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BetterGroceryList.Web.Controllers.Api
{
    /**
     * Lists Controller
     * 
     * 
     * 
     **/
    public class ListsController : ApiController
    {
        IListRepository db;

        ListsController()
        {
            db = new ListRepository();
        }

        /**
         * Add
         * @method POST
         * @param list  a string that is to be the name of the new list
         * @return
         **/
        [HttpPost]
        public String Add(String list)
        {
            // code to add a list
            return "success";
        }

        [HttpGet]
        public IList<BetterList> List()
        {
            return db.GetLists( db.GetUserId(User.Identity.Name) );
        }

        [HttpGet]
        public IList<BetterListItem> UserItems(String query)
        {
            ListRepository db = new ListRepository();
            return db.GetUserListItems(db.GetUserId(User.Identity.Name), query);

        }
    }
}
