using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MathCenter.Models.ViewModels
{
    public class WelcomeViewModel
    {
        [Key]
        [StringLength(8)]
        [RegularExpression(@"^\d{8}$")]
        public string VNum { get; set; }
    }
}