using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.PL.ViewModels.Identity
{
    public class ForgetPasswordViewModel
    {
        [EmailAddress]
        public string Email { get; set; } = null!; 
    }
}
