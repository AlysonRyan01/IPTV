using Iptv.Core.Requests.MelhorEnvio;
using Iptv.Core.Responses;
using Iptv.Core.Responses.MelhorEnvio;

namespace Iptv.Core.Services;

public interface IMelhorEnvioService
{
    Task<BaseResponse<List<CalculoFreteResponse>>> CalcularFreteAsync(CalcularFreteRequest request);

}
