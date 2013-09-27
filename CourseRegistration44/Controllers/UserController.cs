using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CourseRegistration44.Models;
using CourseRegistration44.ViewModel;
using System.Net.Mail;

namespace CourseRegistration44.Controllers
{
    public class UserController : Controller
    {
        private CourseRegistrationDBContext db = new CourseRegistrationDBContext();

        //
        // GET: /User/

        public ActionResult Index()
        {
            return View(db.Users.Where(u => u.Type == false).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();

                var add1 = new MailAddress("Registration@usc.edu");
                var add2 = new MailAddress(user.Email);

                SendEmail(add1, add2, "Registration", "Thanks");


                return RedirectToAction("Index");
            }

            return View(user);
        }

        internal static void SendEmail(MailAddress fromAddress, MailAddress toAddress, string subject, string body)
        {
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };

            var client = new SmtpClient("Course Registration Portal");
            client.Send(message);
        }


        public ActionResult Edit()
        {
            User user = db.Users.Where(u => u.Email == User.Identity.Name).First();

            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [Authorize]
        public ActionResult Courses()
        {
            CoursesViewModel viewModel = new CoursesViewModel();

            IQueryable<User> user = db.Users.Where(u => u.Email == User.Identity.Name);
            if (user.Any())
            {
                var userId = user.First().UserId;
                if (user.First().Type == true)
                {
                    return RedirectToAction("Requests");
                }
                viewModel.EnrolledCourses = db.GetEnrolledCourses(userId);
                viewModel.NonEnrolledCourses = db.GetNonEnrolledCourses(userId);

                return View(viewModel);
            }

            return RedirectToAction("Index","Home");
        }

        [Authorize]
        public ActionResult Requests()
        {
            IQueryable<User> user = db.Users.Where(u => u.Email == User.Identity.Name);
            if (user.Any())
            {
                var userId = user.First().UserId;
                if (user.First().Type == true)
                {
                    var data = db.GetPendingRequests(userId);

                    return View(data.ToList());
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult PendingRequests(string sidx, string sord, int page, int rows)
        {
            IQueryable<User> user = db.Users.Where(u => u.Email == User.Identity.Name);
            if (user.Any())
            {
                var userId = user.First().UserId;
                var pendingRequests = db.GetPendingRequests(userId).ToList();

                var jsonData = new
                {
                    total = 1, //todo: calculate
                    page = page,
                    records = pendingRequests.Count(),
                    rows = (
                      from request in pendingRequests
                      select new
                      {
                          id = request.EnrollmentId,
                          cell = new string[]
                          { 
                            request.LastName, request.FirstName
                          }
                      }).ToArray()
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        
        public PartialViewResult PendingRequestProfile(int Id)
        {
            IQueryable<User> user = db.Users.Where(u => u.Email == User.Identity.Name);
            if (user.Any())
            {
                var userId = user.First().UserId;
                PendingRequestsModel pendingRequest = db.GetPendingRequests(userId).Where(r => r.EnrollmentId == Id).First();

                return PartialView("PendingRequestProfile", new PendingRequestProfile(pendingRequest)) ;
            }
            return null;
        }

        public PartialViewResult PendingRequestInfo(int Id)
        {
            IQueryable<User> user = db.Users.Where(u => u.Email == User.Identity.Name);
            if (user.Any())
            {
                var userId = user.First().UserId;
                PendingRequestsModel pendingRequest = db.GetPendingRequests(userId).Where(r => r.EnrollmentId == Id).First();

                return PartialView("PendingRequestInfo", new PendingRequestInfo(pendingRequest));
            }
            return null;
        }

        public ActionResult RequestStatus(int Id, bool Status)
        {
            var entry = db.Enrollments.Where(e => e.EnrollmentId == Id).First();
            entry.Status = Status;
            db.Entry(entry).State = EntityState.Modified;
            db.SaveChanges();

            var userId = db.Enrollments.Where(e => e.EnrollmentId == Id).First().UserId;
            var email = db.Users.Where(u => u.UserId == userId).First().Email;
            var add1 = new MailAddress("Registration@usc.edu");
            var add2 = new MailAddress(email);

            SendEmail(add1, add2, "Course status change", Status.ToString() + " is status");

            return null;
        }

        public ActionResult CoursesDataNonEnrolled(string sidx, string sord, int page, int rows)
        {
            int i = 0;
            IQueryable<User> user = db.Users.Where(u => u.Email == User.Identity.Name);
            if (user.Any())
            {
                var userId = user.First().UserId;
                var nonEnrolledCourses = db.GetNonEnrolledCourses(userId).OrderBy( or => or.ProfessorLastName).ToList();

                var jsonData = new
                {
                    total = 1, //todo: calculate
                    page = page,
                    records = nonEnrolledCourses.Count(),
                    rows = (
                      from course in nonEnrolledCourses
                      select new
                      {
                          id = course.CourseId,
                          cell = new string[]
                          { 
                  course.CourseId.ToString(), course.CourseName, course.ProfessorFirstName, course.ProfessorLastName, course.ProfessorEmail
                            }
                      }).ToArray()
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            return View();
        }

        public ActionResult CoursesDataEnrolled(string sidx, string sord, int page, int rows)
        {
            int i = 0;
            IQueryable<User> user = db.Users.Where(u => u.Email == User.Identity.Name);
            if (user.Any())
            {
                var userId = user.First().UserId;
                var enrolledCourses = db.GetEnrolledCourses(userId).OrderBy(or => or.ProfessorLastName).ToList();

                var jsonData = new
                {
                    total = 1, //todo: calculate
                    page = page,
                    records = enrolledCourses.Count(),
                    rows = (
                      from course in enrolledCourses
                      select new
                      {
                          id = course.CourseId,
                          cell = new string[]
                          { 
                  course.CourseId.ToString(), course.CourseName, course.ProfessorFirstName, course.ProfessorLastName, course.ProfessorEmail
                            }
                      }).ToArray()
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            return View();
        }

        [HttpPost]
        public bool Register2(int courseId, string notes, string resume)
        {
            //TODO Validate user input 

            Enrollment enroll = new Enrollment();
            IQueryable<User> user = db.Users.Where(u => u.Email == User.Identity.Name);
            if (user.Any())
            {
                enroll.UserId = user.First().UserId;
                enroll.CourseId = courseId;
                enroll.Notes = notes;
                enroll.Resume = resume;

                db.Enrollments.Add(enroll);
                db.SaveChanges();

                return true;
            }
            return false;
        }

        [HttpPost]
        public string Register(int courseId)
        {
            //TODO Validate user input 

            Enrollment enroll = new Enrollment();
            IQueryable<User> user = db.Users.Where(u => u.Email == User.Identity.Name);
            if (user.Any())
            {
                enroll.UserId = user.First().UserId;
                enroll.CourseId = courseId;

                }
            

            return string.Empty;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}