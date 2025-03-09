using Iptv.Core.Requests.IdentityRequests;
using Iptv.Core.Responses;

namespace Iptv.Core.Handlers;

public interface IIdentityHandler
{
    Task<BaseResponse<string>> RegisterAsync(RegisterRequest request);
    Task<BaseResponse<string>> LoginAsync(LoginRequest request);
}