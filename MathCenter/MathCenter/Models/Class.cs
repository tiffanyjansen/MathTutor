namespace MCv1._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Class
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Class()
        {
            Students = new HashSet<Student>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CRN { get; set; }

        [Required]
        [StringLength(10)]
        public string DeptPrefix { get; set; }

        public int ClassNum { get; set; }

        [Required]
        public string Instructor { get; set; }

        [Required]
        [StringLength(10)]
        public string Days { get; set; }

        public int? StartTime { get; set; }

        public string Other { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student> Students { get; set; }
    }
}
