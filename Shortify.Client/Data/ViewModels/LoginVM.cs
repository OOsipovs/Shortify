﻿using Microsoft.AspNetCore.Authentication;
using Shortify.Client.Helpers.Validators;
using System.ComponentModel.DataAnnotations;

namespace Shortify.Client.Data.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage ="Email address is required")]
        [CustomEmailValidator(ErrorMessage = "Email address is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters")]
        public string Password { get; set; }

        public IEnumerable<AuthenticationScheme> Schemes { get; set; }
    }
}
