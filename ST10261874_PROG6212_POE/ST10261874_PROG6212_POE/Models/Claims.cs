using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10261874_PROG6212_POE.Models
{
    public class Claims
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Lecturer ID")]
        public string LecturerID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Claims Periods Start")]
        public DateTime ClaimsPeriodsStart { get; set; }

        [Required]
        [Display(Name = "Claims Periods End")]
        public DateTime ClaimsPeriodsEnd { get; set; }

        [Required]
        [Display(Name = "Hours Worked")]
        public double HoursWorked { get; set; }

        [Required]
        [Display(Name = "Rate Per Hour")]
        public double RateHour { get; set; }

        [Required]
        [Display(Name = "Total Amount")]
        public double TotalAmount { get; set; }

        [Display(Name = "Description Of Work")]
        public string DescriptionOfWork { get; set; }

        public List<string> SupportingDocumentFileNames { get; set; } = new List<string>();

        //properties added for verification and approval
        public bool IsVerified { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public bool IsRejected { get; set; } = false;

        // Not mapped property for file uploads
        [NotMapped]
        [Display(Name = "Supporting Documents")]
        public List<IFormFile> SupportingDocuments { get; set; } = new List<IFormFile>();
    }
}
