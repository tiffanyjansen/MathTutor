using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScienceCenter.Models.ViewModels
{
    public class Data
    {
        [Display(Name = "Week")]
        public int Week { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Hour")]
        public int Hour { get; set; }

        [Display(Name = "Minute")]
        public int Min { get; set; }

        [Display(Name = "Second")]
        public int Sec { get; set; }

        [StringLength(8)]
        [Display(Name = "V-Number")]
        public string VNum { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "CRN")]
        public int? CRN { get; set; }

        [StringLength(10)]
        [Display(Name = "Class Prefix")]
        public string DeptPrefix { get; set; }

        [Display(Name = "Class Number")]
        public string ClassNum { get; set; }

        [Display(Name = "Instructor")]
        public string Instructor { get; set; }

        [StringLength(10)]
        [Display(Name = "Days")]
        public string Days { get; set; }

        [StringLength(25)]
        [Display(Name = "Start Time")]
        public string StartTime { get; set; }
    }
}