using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http;
using BetterGroceryList.Web.Models;
using System.Threading;
using System.Net.Http;

namespace BetterGroceryList.Web.Controllers.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class UserAuthorizationAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            if (actionContext.ControllerContext.RouteData.Values.Keys.Contains("userId"))
            {
                int accessingUserId = Convert.ToInt32(actionContext.ControllerContext.RouteData.Values["userId"]);
                //The user is accessing a user specific action
                //Check to see if the user is using their own UserId
                string username = Thread.CurrentPrincipal.Identity.Name;
                using (IListRepository r = new ListRepository())
                {
                    int userId = r.GetUserId(username);
                    if (userId != accessingUserId)
                    {
                        //user is not authorized
                        actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                    }
                }
            }
            else
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.PreconditionFailed);
            }
        }
    }
}