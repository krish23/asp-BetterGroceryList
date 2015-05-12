using BetterGroceryList.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace BetterGroceryList.Web.Controllers.Api
{
    public class ItemsController : ApiController
    {
        IListRepository db;
        int user_id;

        ItemsController()
        {
            db = new ListRepository();

            if (String.IsNullOrEmpty(User.Identity.Name))
            {
                user_id = 0;
            }
            else
            {
                user_id = db.GetUserId(User.Identity.Name);
            }
        }

        /**
         * method to add a list item
         * @method POST
         * @param string item
         * @param int list_id a list id that is owned and controlled by the current user
         * @return void
         * 
         * example post string
         * item=Tomatoes&list_id=12
         **/
        [HttpPost]
        public void Add(String item, int list_id)
        {
            db.CreateListItem(item, user_id, list_id);
        }

        /**
         * method to retrieve list items
         * @method GET
         * @param list_id id of a list that the user is able to control
         * @return IList<BetterListItem> user's list items
         **/
        [HttpGet]
        public IList<BetterListItem> List(int list_id)
        {
            return db.GetListItems(list_id);
        }

        /**
         * alias for the Search function
         * @deprecated
         * @params query
         * @return result of Search() function
         **/
        [HttpGet]
        public IList<BetterListItem> UserItems(String query)
        {
            return this.Search(query);
        }

        /**
         * searches the User's list items for the query
         * @method GET
         * @params query the string to search for in the user's list items
         * @return IList<BetterListItem> of list items and details
         **/
        [HttpGet]
        public IList<BetterListItem> Search(String query)
        {
            return db.GetUserListItems(user_id);
        }
    }
}