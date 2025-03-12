using Iptv.Core.Models;
using Iptv.Core.Requests.IdentityRequests;
using Iptv.Core.Responses;

namespace Iptv.Core.Handlers;

public interface IIdentityHandler
{
    Task<BaseResponse<string>> RegisterAsync(RegisterRequest request);
    Task<BaseResponse<string>> LoginAsync(LoginRequest request);
    Task<BaseResponse<UserInfo>> UserInfo(string userId);
}