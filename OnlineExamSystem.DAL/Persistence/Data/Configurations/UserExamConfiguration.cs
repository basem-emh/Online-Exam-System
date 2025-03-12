using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineExamSystem.DAL.Entities.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.DAL.Persistence.Data.Configurations
{
    public class UserExamConfiguration : IEntityTypeConfiguration<UserExam>
    {
        public void Configure(EntityTypeBuilder<UserExam> builder)
        {

            builder.Property(uexam => uexam.SubmissionDate)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");
            
            builder.HasOne(uexam => uexam.User)
           .WithMany(u => u.UserExams)
           .HasForeignKey(ue => ue.UserId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(uexam => uexam.Exam)
             .WithMany(e => e.UserExams)
             .HasForeignKey(ue => ue.ExamId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
