﻿using System.ComponentModel.DataAnnotations;

namespace JobsCandidateRecords.Models.Input
{
    public class CreateUserModel
    {
        public string Username { get; set; }
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
        public string ConfirmPassword { get; set; }
    }
}
