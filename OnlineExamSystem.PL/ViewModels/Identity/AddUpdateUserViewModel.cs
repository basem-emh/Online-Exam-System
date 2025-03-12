using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.PL.ViewModels.Identity
{
    public class AddUpdateUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [Display(Name = "User Name")]
        public string UserName { get; set; } = null!;

        [EmailAddress]
        public string Email { get; set; } = null!;

        [Display(Name = "Phone Number")]
        [Phone]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Cofirm Password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Cofirm Password doesn't match with password")]
        public string ConfirmPassword { get; set; } = null!;

        public DateTime AddedAt { get; set; }
    }
}
