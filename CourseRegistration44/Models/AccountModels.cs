using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using System.Linq;
using System.Collections;

namespace CourseRegistration44.Models
{
    public class CourseRegistrationDBContext : DbContext
    {
        public CourseRegistrationDBContext()
            : base("CourseRegistrationDBContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        public IQueryable<CourseModel> GetEnrolledCourses(int UserId)
        {
            var b = from enroll in Enrollments
                    where enroll.UserId == UserId
                    select enroll.CourseId;

            var c = from course in Courses
                    where b.Contains(course.CourseId)
                    select course;

            var d = from course in c
                    join user in Users on course.UserId equals user.UserId
                    select new CourseModel
                    {
                        CourseId = course.CourseId,
                        CourseName = course.Name,
                        ProfessorFirstName = user.FirstName,
                        ProfessorLastName = user.LastName,
                        ProfessorEmail = user.Email
                    };

            return d;
        }

        public IQueryable<CourseModel> GetNonEnrolledCourses(int UserId)
        {
            var b = from enroll in Enrollments
                    where enroll.UserId == UserId
                    select enroll.CourseId;

            var c = from course in Courses
                    where !b.Contains(course.CourseId)
                    select course;

            var d = from course in c
                    join user in Users on course.UserId equals user.UserId
                    select new CourseModel
                    {
                        CourseId = course.CourseId,
                        CourseName = course.Name,
                        ProfessorFirstName = user.FirstName,
                        ProfessorLastName = user.LastName,
                        ProfessorEmail = user.Email
                    };

            return d;
        }

        public IQueryable<PendingRequestsModel> GetPendingRequests(int profUserId)
        {
            var a = (from course in Courses
                    where course.UserId == profUserId
                    select course.CourseId).Distinct();

            var b = from enroll in Enrollments
                    where (enroll.Status == null && a.Contains(enroll.CourseId))
                    select enroll;

            var c = from student in Users
                    join enroll in b on student.UserId equals enroll.UserId
                    select new PendingRequestsModel
                    {
                        EnrollmentId = enroll.EnrollmentId,
                        UserId = student.UserId,
                        FirstName = student.FirstName,
                        LastName = student.LastName, 
                        Email = student.Email,
                        UGpa = student.UGpa,
                        Notes = enroll.Notes,
                        Resume = enroll.Resume
                    };

            return c;
        }
    }

    public class CourseModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string ProfessorFirstName { get; set; }
        public string ProfessorLastName { get; set; }
        public string ProfessorEmail { get; set; }
    }

    public class PendingRequestsModel
    {
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public float UGpa { get; set; }

        public string Notes {get;set;}
        public string Resume {get;set;}
        public int SelectedEnroll { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        //[Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Undergraduate GPA")]
        public float UGpa { get; set; }

        public bool Type { get; set; }
    }

    public class PendingRequestProfile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public float UGpa { get; set; }

        public PendingRequestProfile(PendingRequestsModel model)
        {
            FirstName = model.FirstName;
            LastName = model.LastName;
            Email = model.LastName;
            UGpa = model.UGpa;
        }
    }

    public class PendingRequestInfo
    {
        public string Notes { get; set; }
        public string Resume { get; set; }

        public PendingRequestInfo(PendingRequestsModel model)
        {
            Notes = model.Notes;
            Resume = model.Resume;

        }
    }

}
