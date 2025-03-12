using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.PL.ViewModels.Identity
{
    public class UserViewModel
    {
        public string? Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;
        
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
        public string? Email { get; set; } 

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; } 
        public IEnumerable<string> Roles { get; set; } 
    }
}
