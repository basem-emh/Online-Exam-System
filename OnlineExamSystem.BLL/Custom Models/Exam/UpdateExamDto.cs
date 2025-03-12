using OnlineExamSystem.BLL.Custom_Models.Questions;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.BLL.Custom_Models.Exam
{
    public class UpdateExamDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Exam Title is required")]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Question is required")]

        public List<UpdateQuestionDto> Questions { get; set; } = new List<UpdateQuestionDto>(); 
    }
}
