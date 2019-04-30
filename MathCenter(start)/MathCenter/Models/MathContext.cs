namespace MathCenter.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MathContext : DbContext
    {
        public MathContext()
            : base("name=MathContext")
        {
        }

        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<SignIn> SignIns { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>()
                .HasMany(e => e.SignIns)
                .WithRequired(e => e.Class)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.SignIns)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.StudentID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Classes)
                .WithMany(e => e.Students)
                .Map(m => m.ToTable("StudentClasses").MapLeftKey("VNum").MapRightKey("ClassID"));
        }
    }
}
