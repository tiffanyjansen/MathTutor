using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScienceCenter.Models.ViewModels
{
    public class PersonWeek
    {
        [Key]
        [StringLength(8)]
        public string VNum { get; set; }

        [Required]
        public int Week { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}