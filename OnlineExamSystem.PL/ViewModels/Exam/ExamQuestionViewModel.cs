namespace OnlineExamSystem.PL.ViewModels.Exam
{
    public class ExamQuestionViewModel
    {
        public ExamViewModel Exam { get; set; } = new ExamViewModel();
        public List<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();
    }
}
