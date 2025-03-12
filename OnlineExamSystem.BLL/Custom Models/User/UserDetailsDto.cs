using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.BLL.Custom_Models
{
    public class UserDetailsDto
    {
        public string Id { get; set; } = null!;

        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;
        public string? Email { get; set; }

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
        public IEnumerable<string> Roles { get; set; }
        
        [Display(Name = "Added At")]
        public DateTime AddedAt { get; set; }

    }
}
