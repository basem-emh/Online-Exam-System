using OnlineExamSystem.BLL.Custom_Models.Exam;

namespace OnlineExamSystem.BLL.Services.Admin
{
    public interface IAdminServices
    {
        Task<IEnumerable<ExamDto>> GetAllExamsAsync();
        Task<ExamDetailsDto?> GetExamByIdAsync(int id);
        Task<int> CreateExamAsync(CreateExamDto exam);
        Task<int> CreateAnotherAsync(UpdateExamDto exam);
        Task<int> UpdateExamAsync(UpdateExamDto exam);
        Task<bool> DeleteExamAsync(int id);
    }
}
