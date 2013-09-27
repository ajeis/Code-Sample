using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CourseRegistration44.Models;

namespace CourseRegistration44.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private CourseRegistrationDBContext db = new CourseRegistrationDBContext();

        //
        // GET: /Course/

        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }




        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}