﻿using Application.DTOs.User;
using MediatR;

namespace Application.Features.User.Requests.Queries
{
    public class LoginDto : IRequest<string?>
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
