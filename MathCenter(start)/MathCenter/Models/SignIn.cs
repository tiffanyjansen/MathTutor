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

        public int Week { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public int Hour { get; set; }

        public int Min { get; set; }

        [Required]
        [StringLength(8)]
        public string StudentID { get; set; }

        public virtual Student Student { get; set; }
    }
}
