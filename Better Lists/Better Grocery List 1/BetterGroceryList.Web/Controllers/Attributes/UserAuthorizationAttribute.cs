using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net;

namespace BetterGroceryList.Web.Controllers.Attributes
{
    public class UserAuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.RequestContext.RouteData.Values.Keys.Contains("userId"))
            {
                int accessingUserId = Convert.ToInt32(httpContext.Request.RequestContext.RouteData.Values["userId"]);
                //The user is accessing a user specific action
                //Check to see if the user is using their own UserId
                string username = httpContext.User.Identity.Name;
                using (IListRepository r = new ListRepository())
                {
                    int userId = r.GetUserId(username);
                    if (userId != accessingUserId)
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