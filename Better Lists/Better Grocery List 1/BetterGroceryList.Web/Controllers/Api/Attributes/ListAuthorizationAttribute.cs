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
    public class ListAuthorizationAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            if (actionContext.ControllerContext.RouteData.Values.Keys.Contains("listId"))
            {
                int accessingListId = (int)actionContext.ControllerContext.RouteData.Values["listId"];
                //The user is accessing a list specific action
                //Check to see if the user is accessing an owned or shared list
                using (IListRepository r = new ListRepository())
                {
                    int currentUserId = r.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    IList<BetterList> accessibleLists = r.GetLists(currentUserId, true);
                    IList<int> accessibleListIds = accessibleLists.Select(l => l.Id).ToList();
                    if (!accessibleListIds.Contains(accessingListId))
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