using FluentValidation;
using NTTUserApp.Data.Entities;

namespace NTTUserApp.Service.Models.Users;
public class UpdateUserRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Mail { get; set; }
    public string TRNumber { get; set; }
}

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100).MinimumLength(2);
        RuleFor(x => x.Mail).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.TRNumber).NotEmpty().Length(11);
    }
}
