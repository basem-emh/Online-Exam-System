using OnlineExamSystem.BLL.Custom_Models.Questions;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.BLL.Custom_Models.Exam
{
    public class CreateExamDto
    {
        [Required(ErrorMessage = "Exam Title is required")]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Question is required")]
        public List<CreateQuestionsDto> Questions { get; set; } = null!;
    }
}
