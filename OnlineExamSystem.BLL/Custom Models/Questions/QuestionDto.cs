namespace OnlineExamSystem.BLL.Custom_Models.Questions
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Choice1 { get; set; } = null!;
        public string Choice2 { get; set; } = null!;
        public string Choice3 { get; set; } = null!;
        public string Choice4 { get; set; } = null!;
        public string CorrectAnswer { get; set; } = null!;
    }
}
