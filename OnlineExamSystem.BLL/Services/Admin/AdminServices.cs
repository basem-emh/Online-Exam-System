using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.BLL.Custom_Models.Exam;
using OnlineExamSystem.BLL.Custom_Models.Questions;
using OnlineExamSystem.DAL.Entities.Exam;
using OnlineExamSystem.DAL.Persistence.Unit_Of_Work;

namespace OnlineExamSystem.BLL.Services.Admin
{
    public class AdminServices : IAdminServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ExamDto>> GetAllExamsAsync()
        {
            var exams = await _unitOfWork.Repository<Exam>().GetAllAsync();
            return exams.Select(e => new ExamDto
            {
                Id = e.Id,
                Title = e.Title,
                CreatedAt = e.CreatedAt
            }).ToList();
        }

        public async Task<ExamDetailsDto?> GetExamByIdAsync(int id)
        {
            var exam = await _unitOfWork.Repository<Exam>().GetAsync(id);
            if (exam is null)
                throw new Exception("Exam not found");

            var questions = await _unitOfWork.Repository<Question>()
                .GetAllAsQueryable()
                .Where(q => q.ExamId == id)
                .ToListAsync();

            var result = new ExamDetailsDto
            {
                Id = exam.Id,
                Title = exam.Title,
                CreatedAt = exam.CreatedAt,
                Questions = questions.Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Choice1 = q.Choice1,
                    Choice2 = q.Choice2,
                    Choice3 = q.Choice3,
                    Choice4 = q.Choice4,
                    CorrectAnswer = q.CorrectAnswer
                }).ToList() 
            };

            return result;
        }


        public async Task<int> CreateExamAsync(CreateExamDto examDto)
        {
            var exam = new Exam
            {
                Title = examDto.Title,
                CreatedAt = DateTime.UtcNow,
                Questions = examDto.Questions.Select(q => new Question
                {
                    Title = q.Title,
                    Choice1 = q.Choice1,
                    Choice2 = q.Choice2,
                    Choice3 = q.Choice3,
                    Choice4 = q.Choice4,
                    CorrectAnswer = q.CorrectAnswer
                }).ToList()
            };

            await _unitOfWork.Repository<Exam>().AddAsync(exam);
            await _unitOfWork.CompleteAsync();
         
            return exam.Id;
        }

        public async Task<int> CreateAnotherAsync(UpdateExamDto examDto)
        {
            var exam = await _unitOfWork.Repository<Exam>().GetAsync(examDto.Id);

            if (exam is null)
                throw new Exception("Exam not found");

            exam.Title = examDto.Title;

            foreach (var questionDto in examDto.Questions)
            {
                var newQuestion = new Question
                {
                    Title = questionDto.Title,
                    Choice1 = questionDto.Choice1,
                    Choice2 = questionDto.Choice2,
                    Choice3 = questionDto.Choice3,
                    Choice4 = questionDto.Choice4,
                    CorrectAnswer = questionDto.CorrectAnswer,
                    ExamId = exam.Id 
                };

                exam.Questions.Add(newQuestion); 
            }

            _unitOfWork.Repository<Exam>().Update(exam);
            await _unitOfWork.CompleteAsync();

            return exam.Id;
        }

        public async Task<int> UpdateExamAsync(UpdateExamDto examDto)
        {
            var exam = await _unitOfWork.Repository<Exam>().GetAsync(examDto.Id);
            if (exam == null)
                throw new Exception("Exam not found");

            exam.Title = examDto.Title;

            foreach (var questionDto in examDto.Questions)
            {
                if (questionDto.Id > 0) 
                {
                    var question = exam.Questions.FirstOrDefault(q => q.Id == questionDto.Id);
                    if (question != null)
                    {
                        question.Title = questionDto.Title;
                        question.Choice1 = questionDto.Choice1;
                        question.Choice2 = questionDto.Choice2;
                        question.Choice3 = questionDto.Choice3;
                        question.Choice4 = questionDto.Choice4;
                        question.CorrectAnswer = questionDto.CorrectAnswer;
                    }
                }
                else 
                {
                    exam.Questions.Add(new Question
                    {
                        Title = questionDto.Title,
                        Choice1 = questionDto.Choice1,
                        Choice2 = questionDto.Choice2,
                        Choice3 = questionDto.Choice3,
                        Choice4 = questionDto.Choice4,
                        CorrectAnswer = questionDto.CorrectAnswer
                    });
                }
            }

            _unitOfWork.Repository<Exam>().Update(exam);
            await _unitOfWork.CompleteAsync();

            return exam.Id;
        }

        public async Task<bool> DeleteExamAsync(int id)
        {
            var exam = await _unitOfWork.Repository<Exam>().GetAsync(id);
            if (exam is { })
                _unitOfWork.Repository<Exam>().Delete(exam);
            return await _unitOfWork.CompleteAsync() > 0;
        }

    }
}
