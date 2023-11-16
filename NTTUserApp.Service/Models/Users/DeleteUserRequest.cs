using FluentValidation;

namespace NTTUserApp.Service.Models.Users;
public class DeleteUserRequest
{
    public int Id { get; set; }
}

public class DeleteUserRequestValidator : AbstractValidator<DeleteUserRequest> 
{
    public DeleteUserRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        
    }
}