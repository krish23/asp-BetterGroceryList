using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BetterGroceryList.Web.Tests.Fakes
{
    //public class ContextMocks
    //{
    //    public Moq.Mock<HttpContextBase> HttpContext { get; set; }
    //    public Moq.Mock<HttpRequestBase> Request { get; set; }
    //    public RouteData RouteData { get; set; }

    //    public ContextMocks(Controller controller)
    //    {
    //        //define context objects
    //        HttpContext = new Moq.Mock<HttpContextBase>();
    //        HttpContext.Setup(x => x.Request).Returns(Request.Object);
    //        //you would setup Response, Session, etc similarly with either mocks or fakes

    //        //apply context to controller
    //        RequestContext rc = new RequestContext(HttpContext.Object, new RouteData());
    //        controller.ControllerContext = new ControllerContext(rc, controller);
    //    }
    //}
}
