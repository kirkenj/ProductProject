﻿using Application.Contracts.Persistence;
using Application.DTOs.User.Validators;
using FluentValidation;

namespace Application.Features.User.Requests.Queries.Validators
{
    public class SendTokenToUpdateUserEmailRequestValidator : AbstractValidator<SendTokenToUpdateUserEmailRequest>
    {
        public SendTokenToUpdateUserEmailRequestValidator(IUserRepository userRepository)
        {
            RuleFor(r => r.UpdateUserEmailDto).SetValidator(new UpdateUserEmailDtoValidator(userRepository));
        }
    }
}