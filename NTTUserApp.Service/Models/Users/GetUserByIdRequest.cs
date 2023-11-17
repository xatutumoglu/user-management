using FluentValidation;

namespace NTTUserApp.Service.Models.Users;
public class GetUserByIdRequest
{
    public int Id { get; set; }
}

public class GetUserByIdRequestValidator : AbstractValidator<GetUserByIdRequest>
{
    public GetUserByIdRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
    }
}