namespace MathCenter.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using MathCenter.Models;

    public partial class MathContext : DbContext
    {
        public MathContext()
            : base("name=MathContext")
        {
        }

        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<SignIn> SignIns { get; set; }
        public virtual DbSet<StudentClass> StudentClasses { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>()
                .HasMany(e => e.SignIns)
                .WithRequired(e => e.Class)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Class>()
                .HasMany(e => e.StudentClasses)
                .WithRequired(e => e.Class)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.SignIns)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.StudentID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.StudentClasses)
                .WithRequired(e => e.Student)
                .WillCascadeOnDelete(false);
        }
    }
}
