using BetterGroceryList.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net;
using System.Net.Mail;

using BetterGroceryList.Web.Controllers.Attributes;
using System.Text;

namespace BetterGroceryList.Web.Controllers
{
    public class BetterListController : Controller
    {
		IListRepository db = null;

        private BetterListContext _blc = new BetterListContext();
        //
        // GET: /List/

		public BetterListController()
		{
			db = new ListRepository();
		}


        [Authorize]
        public ActionResult ShareListConformation()
        {
            if (string.IsNullOrEmpty(User.Identity.Name))
            {
                return HttpNotFound();
            }
            return View();
        }


        [Authorize]
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ShareListConformation(int id, Guid guidin, string submitButton)
        {
            if (submitButton == "Yes")
            {
                if (ModelState.IsValid && guidin != null)
                {
                    int temp = db.GetUserId(User.Identity.Name);

                    if (string.IsNullOrEmpty(User.Identity.Name))
                    {
                        return HttpNotFound();
                    }

                    db.ConfirmListSharing(id, temp, guidin);
                    return RedirectToAction("Index");
                }
            }
            ViewBag.msg1 = "*******Your list has not been shared due to not accepting the terms!";



            return View();
        }

        [Authorize]
        public ActionResult Index()
        {
            // need this for listView
            Session["ListID"] = null;

            //Get the username and user id
            string user = User.Identity.Name;
            int userID = db.GetUserId(user);

            IList<BetterList> betterlist;
            betterlist = db.GetLists(userID);
            return View(betterlist);
        }

   [Authorize]
        public ActionResult CreateList()
        {
            if (string.IsNullOrEmpty(User.Identity.Name))
            {
                return HttpNotFound();
            }
           return View();
        }


    [Authorize]
    [HttpPost]
   public ActionResult CreateList(BetterList List)
        {

            if (ModelState.IsValid && List != null)
			{
               List.UserId = db.GetUserId(User.Identity.Name);
               
                if (string.IsNullOrEmpty( User.Identity.Name))
			{
                   return HttpNotFound();
               }
                db.CreateList(List.Name, List.UserId);
                return RedirectToAction("Index");
            }

            return View(List);
        }

        public ActionResult Details(int id)
        {
            BetterList bList = db.GetListWithItems(id);
            if (bList == null)
            {
                return HttpNotFound();
            }
            return View(bList);
        }      

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            BetterList rtn = db.GetListWithItems(id);

            if (rtn == null)
            {
                return HttpNotFound();
            }

            return View(rtn); 
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(BetterList list)
        {
            if (list != null)
            {
                /*
                    here we are pulling down the old and updating the fields and then saving
                 */
                BetterList old = db.GetListWithItems(list.Id);

                old.Name = list.Name;

                // do not allow this to change owner
                //old.UserId = db.GetUserId(User.Identity.Name);

                db.SaveList(old);

                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }

        [Authorize]
        public ActionResult ShareList(int id = 0)
        {
            if (string.IsNullOrEmpty(User.Identity.Name))
            {
                return HttpNotFound();
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult ShareList(SharedBetterList list)
        {
            if (list != null)
            {
                #region Validation
                ViewBag.msg1 = string.Empty;
                if (string.IsNullOrEmpty(list.UserEmailAddress))
                {
                     ViewBag.msg1 = "Please enter an email address";
                     return View();
                }
                #endregion

                try
                {
                    #region Save new shared list
                    Guid g = Guid.NewGuid();
                    db.ShareList(list.Id, list.UserEmailAddress, g);
                    #endregion

                    #region Build Email
                    string emailTo = list.UserEmailAddress;
                    string messageBody = string.Empty;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Please click on the link below to confirm the shared list \n\n");
                    //sb.Append("http://localhost:53853/BetterList/ShareListConformation/"); //this is to be used locally
                    sb.Append("http://uwcoscbetterlist.azurewebsites.net/BetterList/ShareListConformation/"); //this is to be used when site is live
                    sb.Append(list.Id + "?guidin=" + g);
                    messageBody = sb.ToString();
                    #endregion

                    #region Send email

                    SmtpClient client = new SmtpClient();
                    client.Port = 587;
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = true;
                    client.Timeout = 10000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("TestUser4220@gmail.com", "Password4220");
                    client.Timeout = 20000;

                    MailMessage mm = new MailMessage(User.Identity.Name, emailTo, "List Sharing Request", messageBody);
                    mm.BodyEncoding = UTF8Encoding.UTF8;
                    mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                    client.Send(mm);
                    #endregion

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.msg1 = "An error has occrued.  See exception: " + ex.ToString();
                    return View();
                }
            }
            else
            {
                return HttpNotFound();
            }
        }
        //[Authorize]
        //public ActionResult Delete(int id)
        //{
        //    BetterList bl = db.GetListWithItems(id);
        //    if (bl == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(bl);
        //}

        //[Authorize]
        //[HttpPost, ActionName("Delete")]
        //public ActionResult ConfirmDelete(int id)
        //{
        //    BetterList bl = _blc.BetterLists.Find(id);
        //    _blc.BetterLists.Remove(bl);
        //    _blc.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        // Added joshua litItem Stuff



        //public ActionResult ListView(int id = 0)
        //{
        //    if (Session["ListID"] == null)
        //    {
        //        Session["ListID"] = id;
        //    }
        //    id = ((int?)Session["ListID"] ?? 0);

        //    ViewBag.ListID = id;
        //    var items = db.GetListWithItems(id);

        //    return View(items);
        //}



        //public ActionResult DeleteItem(int id = 0)
        //{
        //    var item = db.GetListItem(id);

        //    return View(item);
        //}

        //[HttpPost, ActionName("DeleteItem")]

        //public ActionResult ConfirmedDeleteItem(int id)
        //{
        //    int listID = ((int?)Session["ListID"] ?? 0);
        //    string user = User.Identity.Name;
        //    int userID = db.GetUserId(user);
        //    db.RemoveItemFromList(listID, id);

        //    return RedirectToAction("ListView");
        //}

        


        //public ActionResult AddItem(int id = 0)
        //{

        //    string user = User.Identity.Name;
        //    int userID = db.GetUserId(user);
        //    //ViewBag.BetterListID = new SelectList(db.GetLists(userID), "ID", "Name");
        //    ViewBag.BetterListItemID = new SelectList(db.GetUserListItems(userID), "ID", "Name");
        //    return View();
        //}

        //public ActionResult EditItem(int id = 0)
        //{

        //    BetterListItem item = db.GetListItem(id);
        //    //ViewBag.BetterListID = new SelectList(db.GetLists(userID), "ID", "Name");
            
        //    return View(item);
        //}


        //[Authorize]
        //[HttpPost, ActionName("EditItem")]
        //public ActionResult ConfirmEditItem(BetterListItem item)
        //{   // Commented out the user ID crap and used the overloaded UpdateListItem that I created, 
        //    // instead of using a method that doesn't even do anything with the user ID


        //    //string user = User.Identity.Name;
        //    // int userID = db.GetUserId(user);
        //    db.UpdateListItem(item.Name, item.Id);//userID, item.Id);
        //    //ViewBag.BetterListID = new SelectList(db.GetLists(userID), "ID", "Name");

        //    return RedirectToAction("ListView");
        //}


        
        //public ActionResult AddQuantityItem( int id = 0, int qty = 0)
        //{
        //    int listID = ((int?)Session["ListID"] ?? 0);
        //    string user = User.Identity.Name;
        //    int userID = db.GetUserId(user);
        //    //ViewBag.BetterListID = new SelectList(db.GetLists(userID), "ID", "Name");
        //   // ViewBag.BetterListItemID = new SelectList(db.GetUserListItems(userID), "ID", "Name");
        //    db.IncrementListItemQuantity(listID, id, qty);

        //    return RedirectToAction("ListView");
        //}

       


        //[HttpPost]

        //public ActionResult AddItem(BetterListItem item)
        //{
        //    string user = User.Identity.Name;
        //    int userID = db.GetUserId(user);
        //    int listID = ((int?)Session["ListID"] ?? 0);
        //    int listItemID = item.Id;

        //    if (ModelState.IsValid)
        //    {
        //        db.CreateListItem(item.Name, userID, listID);
        //        return RedirectToAction("ListView");
        //    }

        //    return View(item);
        //}

        //[Authorize]
        //public ActionResult EditListItem(BetterListItem listItem = null)
        //{
        //    BetterListItem rtn = listItem;
        //    if (rtn == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(rtn); 
        //}

        //[Authorize]
        //[HttpPost, ActionName("EditListItem")]
        //public ActionResult VerifyEditListItem(BetterListItem listItem)
        //{
            
        //    if (listItem != null)
        //    {
        //        db.UpdateListItem(listItem.Name, listItem.UserId, listItem.Id);
        //        //db.UpdateListItem(listItem.ListItem.Name, listItem.ListItem.UserId, listItem.ListItem.Id);
        //       // db.UpdateListItemQuantity(listItem.Id, listItem.Quantity);
        //        return RedirectToAction("ListView");
        //    }
        //    else
        //    {
        //        return HttpNotFound();
        //    }
        //}

        //[Authorize]
        //public ActionResult EditListItemQuantity(BetterListMember member = null)
        //{
        //    BetterListMember rtn = member;
        //    if (rtn == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(rtn); 
        //}

        //[Authorize]
        //[HttpPost, ActionName("EditListItemQuantity")]
        //public ActionResult VerifyEditListItemQuantity(BetterListMember member)
        //{
            
        //    if (member != null)
        //    {
        //        //db.UpdateListItem(member.Name, member.UserId, member.Id);
        //        //db.UpdateListItem(listItem.ListItem.Name, listItem.ListItem.UserId, listItem.ListItem.Id);
        //        db.UpdateListItemQuantity(member.Id, member.Quantity);
        //        return RedirectToAction("ListView");
        //    }
        //    else
        //    {
        //        return HttpNotFound();
        //    }
        //}
    }
}
