﻿using Application.DTOs.User.Interfaces;

namespace Application.DTOs.User
{
    public class LoginDto : IEmailDto, IPasswordDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
