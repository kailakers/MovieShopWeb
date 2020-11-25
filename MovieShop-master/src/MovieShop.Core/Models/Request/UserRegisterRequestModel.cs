using System.ComponentModel.DataAnnotations;
namespace MovieShop.Core.Models.Request
{
    // https://liftcodeplay.files.wordpress.com/2017/07/regexrequirement.png
    // https://liftcodeplay.com/2017/07/15/asp-net-core-password-complexity-validation-using-a-regular-expression-in-a-view-model/
    // ^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$
    // https://stackoverflow.com/questions/48635152/regex-for-default-asp-net-core-identity-password
    public class UserRegisterRequestModel
    {
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Password Should have minimum 8 with at least one upper, lower, number and special character")]
        public string Password { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }
    }
}