namespace Iptv.Core.Responses.MelhorEnvio;

public class CalculoFreteResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public int DeliveryTime { get; set; }
    public DeliveryRange DeliveryRange { get; set; } = new ();
    public ShippingCompany Company { get; set; } = new ();
}