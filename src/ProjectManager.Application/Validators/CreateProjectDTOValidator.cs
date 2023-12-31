﻿using FluentValidation;
using ProjectManager.Application.DTOs.ProjectDTOs;

namespace ProjectManager.Application.Validators;

public class CreateProjectDTOValidator : AbstractValidator<CreateProjectDTO>
{
    public CreateProjectDTOValidator()
    {
        RuleFor(d => d.OwnerId).NotEmpty();
        RuleFor(d => d.Title).Length(3, 255);
        RuleFor(d => d.Description).MaximumLength(2000);
    }
}
