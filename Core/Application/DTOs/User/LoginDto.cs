﻿using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class LoginDto
    {
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
