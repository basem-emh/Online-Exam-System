namespace OnlineExamSystem.DAL.Entities.Exam
{
    public class Exam : BaseEntity
    {    
        public string Title { get; set; } = null!;

        public DateTime CreatedAt{ get; set; } 

        public virtual ICollection<Question> Questions { get; set; }

        public virtual ICollection<UserExam> UserExams { get; set; }

    }
}
