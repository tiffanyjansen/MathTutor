using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MathCenter.Models.ViewModels
{
    public class WeekVNum
    {
        [Required]
        public int WeekNum { get; set; }

        [Required]
        public string VNum { get; set; }
    }
}