using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.PL.ViewModels.Identity
{
	public class ResetPasswordViewModel
	{

		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;

		[Display(Name = "Cofirm Password")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Cofirm Password doesn't match with password")]
		public string ConfirmPassword { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Token { get; set; } = null!;
    }
}
