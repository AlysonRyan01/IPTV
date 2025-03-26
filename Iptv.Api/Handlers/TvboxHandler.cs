using Iptv.Api.Data;
using Iptv.Core.Handlers;
using Iptv.Core.Models;
using Iptv.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Iptv.Api.Handlers;

public class TvboxHandler(IptvDbContext context) : ITvboxHandler
{
    public async Task<BaseResponse<TvBox>> GetTvBoxById(string id)
    {
        try
        {
            var longId = long.Parse(id);
            
            var tvbox = await context
                .TvBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == longId);
            
            if (tvbox == null)
                return new BaseResponse<TvBox>(null, 404, "Tvbox Not Found");
            
            return new BaseResponse<TvBox>(tvbox, 200, "Tvbox Found");
        }
        catch (Exception e)
        {
            return new BaseResponse<TvBox>(null, 500, e.Message);
        }
    }
}