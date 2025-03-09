using System.Data.Common;
using Iptv.Api.Data;
using Iptv.Api.Models;
using Iptv.Api.Services;
using Iptv.Core.Handlers;
using Iptv.Core.Models;
using Iptv.Core.Requests.IdentityRequests;
using Iptv.Core.Responses;
using Microsoft.AspNetCore.Identity;

namespace Iptv.Api.Handlers;

public class IdentityHandler(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IptvDbContext context,
    TokenService tokenService)
    : IIdentityHandler
{
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly TokenService _tokenService = tokenService;

    public async Task<BaseResponse<string>> RegisterAsync(RegisterRequest request)
    {
        using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email,
                Email = request.Email,
                IsAdmin = false
            };

            var result = await userManager.CreateAsync(user, request.Password);
            
            if (!result.Succeeded)
            {
                await transaction.RollbackAsync();
                return new BaseResponse<string>("Erro ao registrar usuário", 400, "Verifique os erros nos campos fornecidos.");
            }

            var address = new Address
            {
                UserId = user.Id
            };
            
            context.Addresses.Add(address);
            var addressResult = await context.SaveChangesAsync();

            if (addressResult == 0)
            {
                await transaction.RollbackAsync();
                return new BaseResponse<string>("Erro ao criar o endereco", 400, "Erro ao associar endereco ao usuário.");
            }
            
            await transaction.CommitAsync();
            return new BaseResponse<string>("Usuário registrado com sucesso", 200, "Usuário registrado com sucesso");
        }
        catch (DbException e)
        {
            await transaction.RollbackAsync();
            return new BaseResponse<string>(e.Message, 500, "Erro no servidor");
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return new BaseResponse<string>(e.Message, 500, "Erro no servidor");
        }
    }
    
    public async Task<BaseResponse<string>> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return new BaseResponse<string>("Senha ou email incorretos", 401, "Senha ou email incorretos");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
                return new BaseResponse<string>("Senha ou email incorretos", 401, "Senha ou email incorretos");

            var token = await _tokenService.Generate(user);

            return new BaseResponse<string>(token, 200, "Login realizado com sucesso");
        }
        catch (Exception e)
        {
            return new BaseResponse<string>(e.Message, 500, "Erro no login");
        }
    }
}