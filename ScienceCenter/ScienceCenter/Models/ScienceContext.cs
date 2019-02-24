namespace ScienceCenter.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ScienceContext : DbContext
    {
        public ScienceContext()
            : base("name=ScienceContext")
        {
        }

        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<SignIn> SignIns { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>()
                .HasMany(e => e.Students)
                .WithRequired(e => e.Class1)
                .HasForeignKey(e => e.Class)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.SignIns)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.StudentID)
                .WillCascadeOnDelete(false);
        }

        public System.Data.Entity.DbSet<ScienceCenter.Models.ViewModels.ProfData> ProfDatas { get; set; }

        public System.Data.Entity.DbSet<ScienceCenter.Models.ViewModels.CountDay> CountDays { get; set; }
    }
}
