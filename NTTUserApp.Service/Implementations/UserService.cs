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
        "Update"
    };

    public UserService(MyDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserModel> CreateUserAsync(CreateUserRequest request)
    {
        CreateUserRequestValidator _validator = new();
        await _validator.ValidateAsync(request, options => options.ThrowOnFailures());

        AuthenticateCreateUser(request);
        ValidateBusinessCreateUser(request);

        var item = request.ToEntity();

        _context.Users.Add(item);
        await _context.SaveChangesAsync();

        return new UserModel(item);        
    }

    public async Task<bool> DeleteUserAsync(DeleteUserRequest request)
    {
        //TODO: Model Validation yapılacak.
        var item = await _context.Users
            .Where(x => x.Id == request.Id)            
            .SingleOrDefaultAsync();
        //TODO: Authorization Validation yapılacak.
        //TODO: Business Validation yapılacak.
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
        //TODO: Model Validation yapılacak.
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
        //var list = await _context.Users.AsNoTracking()
        //    .ToListAsync();

        //return list.Select(x => new UserModel()
        //{
        //    Id = x.Id,
        //    Name = x.Name,
        //    Mail = x.Mail,
        //    TRNumber = x.TRNumber
        //}).ToList();

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
        // TODO: Model Validation yapılacak. 
        UpdateUserRequestValidator _validator = new();
        _validator.Validate(request);

        var item = await _context.Users
            .Where(x => x.Id == request.Id)            
            .SingleOrDefaultAsync();
        // TODO: Authorization yapılacak. 
        // TODO: Business Validation yapılacak.
        if (item == default)
        {
            throw new Exception("Item not found!");
        } 

        item.Name = request.Name;
        item.Mail = request.Mail;
        item.TRNumber = request.TRNumber;

        await _context.SaveChangesAsync();
        return new UserModel(item);        
    }
    private async void AuthenticateCreateUser(CreateUserRequest request)
    {
        bool isAuthorized = UserRoles.Contains("Create");
        if (!isAuthorized)
        {
            throw new Exception("Forbidden!");
        }
    }

    private async void ValidateBusinessCreateUser(CreateUserRequest request)
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

    private async void AuthenticateUpdateUser(UpdateUserRequest request)
    {
        bool isAuthorized = UserRoles.Contains("Update");
        if (!isAuthorized)
        {
            throw new Exception("Forbidden!");
        }
    }

    private async void ValidateBusinessUpdateUser(UpdateUserRequest request)
    {
        bool isEmailInUse = await _context.Users.AsNoTracking().Where(x => x.Mail == request.Mail).AnyAsync();
        if (isEmailInUse)
        {
            throw new Exception("E-mail already in use!");
        }

        bool isIdInUse = await _context.Users.AsNoTracking().Where(x => x.Id == request.Id).AnyAsync();
        if (!isIdInUse)
        {
            throw new Exception("User ID's not match!");
        }

    }  
}