namespace OnlineExamSystem.DAL.Entities.Exam
{
    public class Question : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Choice1 { get; set; } = null!; 
        public string Choice2 { get; set; } = null!; 
        public string Choice3 { get; set; } = null!; 
        public string Choice4 { get; set; } = null!; 
        public string CorrectAnswer { get; set; } = null!;

        public int ExamId { get; set; } 
        public virtual Exam Exam { get; set; } = null!;
    }
}