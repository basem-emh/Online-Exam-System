using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineExamSystem.DAL.Entities.Exam;

namespace OnlineExamSystem.DAL.Persistence.Data.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasOne(q => q.Exam)
               .WithMany(e => e.Questions)
               .HasForeignKey(q => q.ExamId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
