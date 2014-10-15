using Combo.Models;
using Combo.Properties;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Combo.Controllers
{
    public class StudentController : Controller
    {
        
        private IStudentsRepository Context;
        public StudentController(IStudentsRepository _Context)
        {
            
            Context = _Context;
        }
        
        public ActionResult Index()
        {
            return View("Index", Context.GetAllStudents());
        }

        
        public ActionResult Create()
        {
            StudentModel model = new StudentModel();
            Context.PrepareGroup(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(StudentModel model)
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

            StudentModel model = Context.GetStudentById(id);
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
