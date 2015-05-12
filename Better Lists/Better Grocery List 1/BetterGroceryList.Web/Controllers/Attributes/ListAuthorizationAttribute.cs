using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BetterGroceryList.Web.Models;


namespace BetterGroceryList.Web.Controllers.Attributes
{
    public class ListAuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.RequestContext.RouteData.Values.Keys.Contains("listId") ||
                httpContext.Request.QueryString["listId"] != null ||
                httpContext.Request.Form["listId"] != null)
            {
                int accessingListId = 0;
                if (httpContext.Request.RequestContext.RouteData.Values.Keys.Contains("listId"))
                {
                    accessingListId = (int)httpContext.Request.RequestContext.RouteData.Values["listId"];
                }
                else if (httpContext.Request.Form["listId"] != null) {
                    accessingListId = Convert.ToInt32(httpContext.Request.Form["listId"]);
                }
                else if (httpContext.Request.QueryString["listId"] != null)
                {
                    accessingListId = Convert.ToInt32(httpContext.Request.QueryString["listId"]);
                }
                //The user is accessing a list specific action
                //Check to see if the user is accessing an owned or shared list
                using (IListRepository r = new ListRepository())
                {
                    int currentUserId = r.GetUserId(httpContext.User.Identity.Name);
                    IList<BetterList> accessibleLists = r.GetLists(currentUserId, true);
                    IList<int> accessibleListIds = accessibleLists.Select(l => l.Id).ToList();
                    if (!accessibleListIds.Contains(accessingListId))
                    {
                        //user is not authorized
                        return false;
                    }
                }
            }
            return true;
        }
    }
}