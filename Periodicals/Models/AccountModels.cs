using System.ComponentModel.DataAnnotations;

namespace Periodicals.Models
{
    /// <summary>
    /// Class creates logIn model
    /// </summary>
    public class LoginModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid adress")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}