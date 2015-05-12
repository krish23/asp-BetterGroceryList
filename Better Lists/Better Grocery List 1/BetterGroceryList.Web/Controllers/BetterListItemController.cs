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



namespace BetterGroceryList.Web.Controllers
{



    public class BetterListItemController : Controller
    {
        IListRepository _db = null;
        
        //private IListRepository _repository = new ListRepository();
        //private bool disposed;


        public BetterListItemController()
		{
            _db = new ListRepository();
		}

        public BetterListItemController(IListRepository db)
        {
            _db = db;
        }




        public ActionResult ListView(int id = 0)
        {
            if (Session["ListID"] == null)
            {
                Session["ListID"] = id;
            }
            id = ((int?)Session["ListID"] ?? 0);

            ViewBag.ListID = id;
            var items = _db.GetListWithItems(id);

            return View(items);
        }



        public ActionResult Delete(int id = 0)
        {
            var item = _db.GetListItem(id);

            return View(item);
        }

        [HttpPost, ActionName("Delete")]

        public ActionResult ConfirmedDeleteItem(int id)
        {
            int listID = ((int?)Session["ListID"] ?? 0);
            string user = User.Identity.Name;
            int userID = _db.GetUserId(user);
            _db.RemoveItemFromList(listID, id);

            return RedirectToAction("ListView");
        }




       

        public ActionResult Edit(int id = 0)
        {

            BetterListItem item = _db.GetListItem(id);
            //ViewBag.BetterListID = new SelectList(db.GetLists(userID), "ID", "Name");

            return View(item);
        }


        [Authorize]
        [HttpPost, ActionName("Edit")]
        public ActionResult ConfirmEditItem(BetterListItem item)
        {   // Commented out the user ID crap and used the overloaded UpdateListItem that I created, 
            // instead of using a method that doesn't even do anything with the user ID


            //string user = User.Identity.Name;
            // int userID = db.GetUserId(user);
            _db.UpdateListItem(item.Name, item.Id);//userID, item.Id);
            //ViewBag.BetterListID = new SelectList(db.GetLists(userID), "ID", "Name");

            return RedirectToAction("ListView");
        }



        public ActionResult AddQuantityItem(int id = 0, int qty = 0)
        {
            int listID = ((int?)Session["ListID"] ?? 0);
            string user = User.Identity.Name;
            int userID = _db.GetUserId(user);
            //ViewBag.BetterListID = new SelectList(db.GetLists(userID), "ID", "Name");
            // ViewBag.BetterListItemID = new SelectList(db.GetUserListItems(userID), "ID", "Name");
            _db.IncrementListItemQuantity(listID, id, qty);

            return RedirectToAction("ListView");
        }



        public ActionResult CreateItem(int id = 0)
        {

            string user = User.Identity.Name;
            int userID = _db.GetUserId(user);
            //ViewBag.BetterListID = new SelectList(db.GetLists(userID), "ID", "Name");
            ViewBag.BetterListItemID = new SelectList(_db.GetUserListItems(userID), "ID", "Name");
            return View();
        }

        [HttpPost]

        public ActionResult CreateItem(BetterListItem item)
        {
            string user = User.Identity.Name;
            int userID = _db.GetUserId(user);
            int listID = ((int?)Session["ListID"] ?? 0);
            int listItemID = item.Id;

            if (ModelState.IsValid)
            {
                _db.CreateListItem(item.Name, userID, listID);
                return RedirectToAction("ListView");
            }

            return View(item);
        }

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
        //        // db.UpdateListItemQuantity(listItem.Id, listItem.Quantity);
        //        return RedirectToAction("ListView");
        //    }
        //    else
        //    {
        //        return HttpNotFound();
        //    }
        //}

        [Authorize]
        public ActionResult EditQuantity(BetterListMember member = null)
        {
            BetterListMember rtn = new BetterListMember();
            rtn = member;
            if (rtn == null)
            {
                return HttpNotFound();
            }
            return View(rtn);
        }

        [Authorize]
        [HttpPost, ActionName("EditQuantity")]
        public ActionResult VerifyEditListItemQuantity(BetterListMember member)
        {

            if (member != null)
            {
                //db.UpdateListItem(member.Name, member.UserId, member.Id);
                //db.UpdateListItem(listItem.ListItem.Name, listItem.ListItem.UserId, listItem.ListItem.Id);
                _db.UpdateListItemQuantity(member.Id, member.Quantity);
                return RedirectToAction("ListView");
            }
            else
            {
                return HttpNotFound();
            }
        }
        
    }
}
