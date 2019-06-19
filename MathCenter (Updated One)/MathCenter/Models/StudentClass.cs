namespace MathCenter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StudentClass
    {
        public int ID { get; set; }

        [Required]
        [StringLength(8)]
        public string VNum { get; set; }

        public int ClassID { get; set; }

        public virtual Class Class { get; set; }

        public virtual Student Student { get; set; }
    }
}
