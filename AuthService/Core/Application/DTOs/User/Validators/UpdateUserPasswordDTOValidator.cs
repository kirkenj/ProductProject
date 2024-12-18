﻿using Application.DTOs.User.Validators.Shared;
using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class UpdateUserPasswordDTOValidator : AbstractValidator<UpdateUserPasswordDto>
    {
        public UpdateUserPasswordDTOValidator()
        {
            Include(new IPasswordDtoValidator());
            Include(new IIdDtoValidator<Guid>());
        }
    }
}
