using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScienceCenter.Models.ViewModels
{
    public class CountDay
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Number of Students")]
        public int NumStudents { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Week Number")]
        public int WeekNum { get; set; }

        [Display(Name = "Hour")]
        public int Hour { get; set; }

        [Display(Name = "Average Number of Students")]
        public int AverageNum { get; set; }
        
    }
}