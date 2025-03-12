using Microsoft.AspNetCore.Identity;
using OnlineExamSystem.DAL.Entities.Exam;

namespace OnlineExamSystem.DAL.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime AddedAt { get; set; } = DateTime.UtcNow ;
        public virtual ICollection<UserExam> UserExams { get; set; }
    }   
}
