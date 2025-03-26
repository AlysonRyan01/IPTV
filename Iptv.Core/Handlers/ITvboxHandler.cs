using Iptv.Core.Models;
using Iptv.Core.Responses;

namespace Iptv.Core.Handlers;

public interface ITvboxHandler
{
    Task<BaseResponse<TvBox>> GetTvBoxById(string id);
}