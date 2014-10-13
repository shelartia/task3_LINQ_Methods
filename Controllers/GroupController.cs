using Combo.Models;
using Combo.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Combo.Controllers
{
    public class GroupController : Controller
    {
        //public GroupMSRepository Context = new GroupMSRepository();
        //public GroupMongoRepository Context = new GroupMongoRepository();
        private IGroupsRepository Context;

        public GroupController()
        {
            if (!string.IsNullOrEmpty(Settings.Default.CustomProvider) && Settings.Default.CustomProvider == "MSSQL")
            {
                Context = new GroupMSRepository();

            }
            else
            {
                Context = new GroupMongoRepository();
            }
        }

        public ActionResult Index()
        {
            return View("Index", Context.GetAllGroups());
        }       

        public ActionResult Create()
        {
            GroupModel model = new GroupModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(GroupModel model)
        {
            if (ModelState.IsValid)
                {
                    Context.Add(model);
                    
                    return RedirectToAction("Index");
                }
            return View(model);
            
        }

        public ActionResult Delete(string id="")
        {

            GroupModel model = Context.GetGroupById(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            Context.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
