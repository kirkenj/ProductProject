﻿using Application.DTOs.User;
using Application.Models.Response;
using MediatR;

namespace Application.Features.User.Requests.Commands
{
    public class UpdateNotSensitiveUserInfoComand : IRequest<Response<string>>
    {
        public UpdateUserAddressDto UpdateUserAddressDto { get; set; } = null!;
    }
}