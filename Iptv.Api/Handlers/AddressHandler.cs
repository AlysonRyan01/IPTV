using Iptv.Api.Data;
using Iptv.Core.Handlers;
using Iptv.Core.Models;
using Iptv.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Iptv.Api.Handlers;

public class AddressHandler(IptvDbContext context) : IAddressHandler
{
    public async Task<BaseResponse<Address>> GetAddress(string userId)
    {
        try
        {
            long userIdLong = long.Parse(userId);
            
            var address = await context.Addresses.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userIdLong);
            
            if (address == null)
                return new BaseResponse<Address>(null, 404, "Endereco não encontrado");
            
            return new BaseResponse<Address>(address, 200, "Endereco encontrado com sucesso!");
        }
        catch (Exception e)
        {
            return new BaseResponse<Address>(null, 500, e.Message);
        }
    }

    public async Task<BaseResponse<string>> UpdateAddress(string userId, Address address)
    {
        try
        {
            long userIdLong = long.Parse(userId);
            
            var result = await context.Addresses.FirstOrDefaultAsync(x => x.UserId == userIdLong);

            if (result == null)
                return new BaseResponse<string>("Endereco nao encontrado", 404, "Endereco nao encontrado");
            
            result.Street = address.Street;
            result.Number = address.Number;
            result.City = address.City;
            result.State = address.State;
            result.ZipCode = address.ZipCode;
            result.Neighborhood = address.Neighborhood;
            
            context.Addresses.Update(result);
            
            await context.SaveChangesAsync();
            
            return new BaseResponse<string>("Endereco atualizado com sucesso!", 200, "Endereco atualizado com sucesso!");
        }
        catch (Exception e)
        {
            return new BaseResponse<string>(null, 500, e.Message);
        }
    }
}