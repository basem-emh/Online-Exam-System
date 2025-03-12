using OnlineExamSystem.BLL.Custom_Models.Questions;

namespace OnlineExamSystem.BLL.Custom_Models.Exam
{
    public class ExamDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public List<QuestionDto>? Questions { get; set; }

    }
}
