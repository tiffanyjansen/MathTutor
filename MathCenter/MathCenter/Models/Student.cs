namespace MathCenter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            SignIns = new HashSet<SignIn>();
            StudentClasses = new HashSet<StudentClass>();
        }

        [Key]
        [StringLength(8)]
        [RegularExpression(@"^\d{8}$")]
        [Display(Name = "V Number")]
        public string VNum { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z-\.\s]+$")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z-\.\s]+$")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SignIn> SignIns { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentClass> StudentClasses { get; set; }

        public override string ToString()
        {
            return this.FirstName + " " + this.LastName;
        }

        public Student CapitalizeNames()
        {
            this.FirstName = CapitalizeName(this.FirstName);
            this.LastName = CapitalizeName(this.LastName);
            return this;
        }

        public string CapitalizeName(string name)
        {
            name = char.ToUpper(name[0]) + name.Substring(1);
            int index = 0;
            while (name.IndexOf(' ', index) != -1)
            {
                index = name.IndexOf(' ', index) + 1;
                name = name.Substring(0, index) + char.ToUpper(name[index]) + name.Substring(index + 1);
            }
            if (name.IndexOf('-') != -1)
            {
                index = name.IndexOf('-', index) + 1;
                name = name.Substring(0, index) + char.ToUpper(name[index]) + name.Substring(index + 1);
            }
            return name;
        }

        public Student Update(Student student)
        {
            this.FirstName = student.CapitalizeName(student.FirstName);
            this.LastName = student.CapitalizeName(student.LastName);
            return this;
        }
    }
}
