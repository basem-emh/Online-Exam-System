using OnlineExamSystem.BLL.Custom_Models.Questions;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.PL.ViewModels.Exam
{
    public class ExamViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Exam Title is required")]
        [MaxLength(100)]
        [Display(Name ="Exam Title")]
        public string Title { get; set; } = null!;
    }
}
