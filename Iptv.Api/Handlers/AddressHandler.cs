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
}