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
    IptvDbContext context,
    TokenService tokenService,
    IAddressHandler addressHandler)
    : IIdentityHandler
{
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

            var token = await tokenService.Generate(user);

            return new BaseResponse<string>(token, 200, "Login realizado com sucesso");
        }
        catch (Exception e)
        {
            return new BaseResponse<string>(e.Message, 500, "Erro no login");
        }
    }

    public async Task<BaseResponse<UserInfo>> UserInfo(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                return new BaseResponse<UserInfo>(null, 400, "UserId não pode ser nulo ou vazio.");

            var user = await userManager.FindByIdAsync(userId);
            
            if (user == null)
                return new BaseResponse<UserInfo>(null, 404, "Usuario não encontrado");

            var addressResult = await addressHandler.GetAddress(userId);

            var address = addressResult.IsSuccess ? addressResult.Data : new Address();

            var userInfo = new UserInfo
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                Phone = user.PhoneNumber ?? string.Empty,
                Address = address ?? new (),
                IsAdmin = user.IsAdmin
            };
            
            return new BaseResponse<UserInfo>(userInfo, 200, "Usuario encontrado com sucesso");
        }
        catch
        {
            return new BaseResponse<UserInfo>(null, 500, "Erro ao retornar informações do cliente");
        }
    }
}