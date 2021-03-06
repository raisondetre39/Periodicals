﻿using System.ComponentModel.DataAnnotations;

namespace Periodicals.Models
{
    /// <summary>
    /// Class cretes registration model
    /// </summary>
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email couldn't be empty")]
        [EmailAddress]
        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid adress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name couldn't be empty")]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public int Wallet { get; set; }

        [Required(ErrorMessage = "Password couldn't be empty")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Field couldn't be empty")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}