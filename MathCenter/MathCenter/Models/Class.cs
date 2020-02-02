namespace MathCenter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Text.RegularExpressions;

    public partial class Class
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Class()
        {
            SignIns = new HashSet<SignIn>();
            StudentClasses = new HashSet<StudentClass>();
        }

        public int ClassID { get; set; }

        public int? CRN { get; set; }

        [StringLength(10)]
        [Display(Name = "Department")]
        public string DeptPrefix { get; set; }

        [Display(Name = "Class Number")]
        public int? ClassNum { get; set; }

        [Display(Name = "Instructor")]
        public string Instructor { get; set; }

        [StringLength(10)]
        [Display(Name = "Days")]
        public string Days { get; set; }

        [StringLength(25)]
        [Display(Name = "Starting Time")]
        public string Time { get; set; }

        [RegularExpression(@"^[A-Z]{1,3}\s[\d]{2,3}$")]
        public string Other { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SignIn> SignIns { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentClass> StudentClasses { get; set; }

        public override string ToString()
        {
            if(this.Other == null)
            {
                return this.DeptPrefix + " " + this.ClassNum;
            }
            else
            {
                return this.Other;
            }
        }

        public string ToLongString()
        {
            if(this.Other == null)
            {
                if(CCCollegeStrings.Values.Contains(this.Instructor))
                {
                    var Institution = CCCollegeStrings.FirstOrDefault(x => x.Value == this.Instructor).Key;
                    return this.DeptPrefix + " " + this.ClassNum + ": " + Institution;
                }
                else
                {
                    return this.DeptPrefix + " " + this.ClassNum + ": " + this.Instructor;
                }
            }
            else
            {
                return this.Other;
            }
        }

        public string getInstructor()
        {
            if (CCCollegeStrings.Values.Contains(this.Instructor))
            {
                return CCCollegeStrings.FirstOrDefault(x => x.Value == this.Instructor).Key;
            }
            else
            {
                return this.Instructor;
            }
        }

        public string getDepartment()
        {
            if (this.DeptPrefix != null)
            {
                return this.DeptPrefix;
            }
            else
            {
                if(this.Other != null)
                {
                    Regex rx = new Regex(@"([A-Z]{1,3})");
                    var match  = rx.Match(this.Other);
                    return match.Value;
                }
                else
                {
                    return "";
                }
            }
        }

        public int getClassNumber()
        {
            if (this.ClassNum != null)
            {
                return (int) this.ClassNum;
            }
            else
            {
                if (this.Other != null)
                {
                    Regex rx = new Regex(@"([\d]{2,3})");
                    var match = rx.Match(this.Other);
                    return Int32.Parse(match.Value);
                }
                else
                {
                    return 0;
                }
            }
        }

        public static Dictionary<string, string> CCCollegeStrings = new Dictionary<string, string>()
        {
            { "Portland Community College" , "Portland" },
            { "Chemeketa Community College", "Chemeketa" },
            { "Clackamas Community College", "Clackamas" },
            { "Mt. Hood Community College", "Mt. Hood" },
            { "Linn Benton Community College", "Linn Benton" }
        };
    }
}
