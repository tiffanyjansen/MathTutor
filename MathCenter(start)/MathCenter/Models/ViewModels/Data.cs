using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MathCenter.Models.ViewModels
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

        [StringLength(8)]
        [Display(Name = "VNumber")]
        public string VNum { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public Class SignedClass { get; set; }
    }
}