using FluentValidation;
using NTTUserApp.Data.Entities;

namespace NTTUserApp.Service.Models.Users;
public class CreateUserRequest
{
    public string Name { get; set; }
    public string Mail { get; set; }
    public string TRNumber { get; set; }
    public User ToEntity()
    {
        return new User
        {
            Name = Name,
            Mail = Mail,
            TRNumber = TRNumber
        };
    }
}

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100).MinimumLength(2);
        RuleFor(x => x.Mail).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.TRNumber).NotEmpty().Length(11);

    }
}