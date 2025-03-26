using Iptv.Core.Models;
using Iptv.Core.Responses;

namespace Iptv.Core.Handlers;

public interface IAddressHandler
{
    Task<BaseResponse<Address>> GetAddress(string userId);
    Task<BaseResponse<string>> UpdateAddress(string userId, Address address);
}