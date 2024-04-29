using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace WebApiApp.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("students");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(n => n.Name).IsRequired();
            builder.Property(n => n.Name).HasMaxLength(250);
            builder.Property(n => n.Email).IsRequired().HasMaxLength(250);
            builder.Property(n => n.Address).IsRequired(false).HasMaxLength(500);


            builder.HasData(new List<Student>()
            {
                new Student
                {
                    Id = 1,
                    Name = "Test",
                    Address = "India",
                    DOB = new DateTime(2000,01,01),
                    Email = "test@gmail.com"
                },

                new Student
                {
                    Id = 2,
                    Name = "Test 2",
                    Address = "India",
                    DOB = new DateTime(2000,02,02),
                    Email = "test2@gmail.com"
                }

            });
        }
    }
}
