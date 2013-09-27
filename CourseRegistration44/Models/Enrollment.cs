using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseRegistration44.Models
{
    [Table("Enrollments")]
    public class Enrollment
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }

        public string Notes { get; set; }
        public string Resume { get; set; }

        public bool? Status { get; set; }

        public virtual User Student { get; set; }
        public virtual Course Course { get; set; }

    }
}