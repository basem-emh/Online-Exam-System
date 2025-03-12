using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineExamSystem.DAL.Entities.Exam;

namespace OnlineExamSystem.DAL.Persistence.Data.Configurations
{
    public class ExamConfigurations : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.Property(e => e.CreatedAt)
               .IsRequired()
               .HasDefaultValueSql("GETUTCDATE()");

            builder.HasMany(e => e.Questions)
              .WithOne(q => q.Exam)
              .HasForeignKey(q => q.ExamId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.UserExams)
              .WithOne(ue => ue.Exam)
              .HasForeignKey(ue => ue.ExamId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
