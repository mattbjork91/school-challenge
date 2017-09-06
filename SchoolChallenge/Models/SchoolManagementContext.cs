using Microsoft.EntityFrameworkCore;


namespace SchoolChallenge.Models
{
    public partial class SchoolManagementContext : DbContext
    {
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Teacher> Teacher { get; set; }

        public SchoolManagementContext(DbContextOptions<SchoolManagementContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.StudentId)
                    .HasColumnName("Student ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("First Name")
                    .HasMaxLength(50);

                entity.Property(e => e.HasScholarship)
                    .IsRequired()
                    .HasColumnName("Has Scholarship")
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("Last Name")
                    .HasMaxLength(50);

                entity.Property(e => e.StudentNumber).HasColumnName("Student Number");

            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.Property(e => e.TeacherId)
                    .HasColumnName("Teacher ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("First Name")
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("Last Name")
                    .HasMaxLength(50);

                entity.Property(e => e.NumberOfStudents)
                    .IsRequired()
                    .HasColumnName("Number of Students");
            });
        }
    }
}
