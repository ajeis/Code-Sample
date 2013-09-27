using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CourseRegistration44;
using CourseRegistration44.Models;

namespace CourseRegistration44.ViewModel
{
    public class CoursesViewModel
    {
        public IQueryable<CourseModel> EnrolledCourses { get; set; }
        public IQueryable<CourseModel> NonEnrolledCourses { get; set; }
    }
}