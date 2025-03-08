namespace Iptv.Core.Models;

public class Address
{
    public long Id { get; set; }
    
    public long UserId { get; set; }

    public List<Order?> Orders { get; set; } = new List<Order?>();
    
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}