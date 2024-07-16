﻿using Application.DTOs.User;
using Application.Models;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class CreateUserCommand : IRequest<Response>
    {
        public CreateUserDto CreateUserDto { get; set; } = null!;
    }
}
