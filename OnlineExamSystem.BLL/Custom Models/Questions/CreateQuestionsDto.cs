using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.BLL.Custom_Models.Questions
{
    public class CreateQuestionsDto
    {
        [Required(ErrorMessage = "Question title is required")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Choice 1 is required")]
        [Display(Name = "Option 1")]
        [MaxLength(100)]
        public string Choice1 { get; set; } = null!;

        [Required(ErrorMessage = "Choice 2 is required")]
        [Display(Name = "Option 2")]
        [MaxLength(100)]
        public string Choice2 { get; set; } = null!;

        [Required(ErrorMessage = "Choice 3 is required")]
        [Display(Name = "Option 3")]
        [MaxLength(100)]
        public string Choice3 { get; set; } = null!;

        [Required(ErrorMessage = "Choice 4 is required")]
        [Display(Name = "Option 4")]
        [MaxLength(100)]
        public string Choice4 { get; set; } = null!;
        
        [Required(ErrorMessage = "Correct Answer is required")]
        [Display(Name = "Correct Answer")]
        [MaxLength(100)]
        public string CorrectAnswer { get; set; } = null!;
    }
}
