namespace Iptv.Core.Models;

public class TvBox
{
    public long Id { get; set; }
    public string Brand { get; set; } = String.Empty;
    public int Quantity  { get; set; }
    public decimal Amount { get; set; }
    
    public List<Order> Orders { get; set; } = new ();
}