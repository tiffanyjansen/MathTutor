namespace MathCenter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SignIn
    {
        public int ID { get; set; }

        [Display(Name = "Week Number")]
        public int Week { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Hour")]
        public int Hour { get; set; }

        [Display(Name = "Minute")]
        public int Min { get; set; }

        [Required]
        [StringLength(8)]
        public string StudentID { get; set; }

        public int ClassID { get; set; }

        public virtual Class Class { get; set; }

        public virtual Student Student { get; set; }
    }
}
