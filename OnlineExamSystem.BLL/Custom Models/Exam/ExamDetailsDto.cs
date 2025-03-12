using OnlineExamSystem.BLL.Custom_Models.Questions;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.BLL.Custom_Models.Exam
{
    public class ExamDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        
        [Display(Name ="Created At")]
        public DateTime CreatedAt { get; set; }
        public List<QuestionDto>? Questions { get; set; }
    }
}
