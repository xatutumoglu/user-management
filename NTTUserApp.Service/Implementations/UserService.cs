using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NTTUserApp.Data.Entities;
using NTTUserApp.Service.Abstractions;
using NTTUserApp.Service.Models.Users;

namespace NTTUserApp.Service.Implementations;
public class UserService : IUserService
{
    private readonly MyDbContext _context;
    private List<string> UserRoles = new List<string>()
    {
        "Create",
        "Update",
        "Delete",
        "Read"
    };

    public UserService(MyDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserModel> CreateUserAsync(CreateUserRequest request)
    {
        AuthenticateCreateUser(request);

        CreateUserRequestValidator _validator = new();
        await _validator.ValidateAsync(request, options => options.ThrowOnFailures());

        await ValidateBusinessCreateUser(request);

        var item = request.ToEntity();

        _context.Users.Add(item);
        await _context.SaveChangesAsync();

        return new UserModel(item);        
    }

    public async Task<bool> DeleteUserAsync(DeleteUserRequest request)
    {        
        AuthenticateDeleteUser(request);
                
        DeleteUserRequestValidator _validator = new();
        _validator.Validate(request);
                
        await ValidateBusinessDeleteUser(request);

        var item = await _context.Users
            .Where(x => x.Id == request.Id)            
            .SingleOrDefaultAsync();        
             
        if (item == default)
        {
            throw new Exception("Item not found!");
        }

        _context.Users.Remove(item);
        await _context.SaveChangesAsync();
        return true;        
    }

    public async Task<UserModel?> GetUserByIdAsync(GetUserByIdRequest request)
    {        
        AuthenticateGetRequests();
        
        GetUserByIdRequestValidator _validator = new();
        _validator.Validate(request);

        return await _context.Users.AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new UserModel
            {
                Id = x.Id,
                Name = x.Name,
                Mail = x.Mail,
                TRNumber = x.TRNumber
            })
            .SingleOrDefaultAsync();
    }

    public async Task<List<UserModel>> GetUsersAsync()
    {
        AuthenticateGetRequests();

        return await _context.Users.AsNoTracking()
            .Select(x => new UserModel()
            {
                Id = x.Id,
                Name = x.Name,
                Mail = x.Mail,
                TRNumber = x.TRNumber
            })
            .ToListAsync();
    }

    public async Task<UserModel> UpdateUserAsync(UpdateUserRequest request)
    {        
        AuthenticateUpdateUser(request);
                 
        UpdateUserRequestValidator _validator = new();
        _validator.Validate(request);
                
        await ValidateBusinessUpdateUser(request);

        var item = await _context.Users
            .Where(x => x.Id == request.Id)            
            .SingleOrDefaultAsync();        
                
        item.Name = request.Name;
        item.Mail = request.Mail;
        item.TRNumber = request.TRNumber;

        await _context.SaveChangesAsync();
        return new UserModel(item);        
    }

    private async Task AuthenticateGetRequests()
    {
        bool isAuthorized = UserRoles.Contains("Read");
        if (!isAuthorized)
        {
            throw new Exception("Forbidden!");
        }
    }


    private async Task AuthenticateCreateUser(CreateUserRequest request)
    {
        bool isAuthorized = UserRoles.Contains("Create");
        if (!isAuthorized)
        {
            throw new Exception("Forbidden!");
        }
    }

    private async Task ValidateBusinessCreateUser(CreateUserRequest request)
    {
        bool isEmailInUse = await _context.Users.AsNoTracking().Where(x => x.Mail == request.Mail).AnyAsync();
        if (isEmailInUse)
        {
            throw new Exception("E-mail already in use!");
        }

        bool isTRNumberInUse = await _context.Users.AsNoTracking().Where(x => x.TRNumber == request.TRNumber).AnyAsync();
        if (isTRNumberInUse)
        {
            throw new Exception("TR Number already in use!");
        }
    }

    private async Task AuthenticateUpdateUser(UpdateUserRequest request)
    {
        bool isAuthorized = UserRoles.Contains("Update");
        if (!isAuthorized)
        {
            throw new Exception("Forbidden!");
        }
    }

    private async Task ValidateBusinessUpdateUser(UpdateUserRequest request)
    {
        bool isIdInUse = await _context.Users.AsNoTracking().Where(x => x.Id == request.Id).AnyAsync();
        if (!isIdInUse)
        {
            throw new Exception("User ID's not match!");
        }

        bool isEmailInUse = await _context.Users.AsNoTracking().Where(x => x.Mail == request.Mail && x.Id != request.Id).AnyAsync();
        if (isEmailInUse)
        {
            throw new Exception("E-mail already in use!");
        }
        
        bool isTRNumberInUse = await _context.Users.AsNoTracking().Where(x => x.TRNumber == request.TRNumber && x.Id != request.Id).AnyAsync();
        if (isTRNumberInUse)
        {
            throw new Exception("TRNumber already in use!");
        }
    }

    private async Task AuthenticateDeleteUser(DeleteUserRequest request)
    {
        bool isAuthorized = UserRoles.Contains("Delete");
        if (!isAuthorized)
        {
            throw new Exception("Forbidden!");
        }
    }

    private async Task ValidateBusinessDeleteUser(DeleteUserRequest request)
    {        
        bool isIdInUse = await _context.Users.AsNoTracking().Where(x => x.Id == request.Id).AnyAsync();
        if (!isIdInUse)
        {
            throw new Exception("User ID's not found!");
        }
    }
}