namespace Iptv.Core.Responses.MelhorEnvio;

public class MelhorEnvioResponse
{
    public List<CalculoFreteResponse> Data { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}