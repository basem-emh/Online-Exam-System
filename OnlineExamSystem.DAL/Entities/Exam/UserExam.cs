using OnlineExamSystem.DAL.Entities.Identity;

namespace OnlineExamSystem.DAL.Entities.Exam
{
    public class UserExam : BaseEntity
    {
        public double Score { get; set; }
        public bool IsPassed { get; set; } 
        public DateTime SubmissionDate { get; set; }
        public string? UserId { get; set; } 
        public int ExamId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Exam Exam { get; set; }
    }
}