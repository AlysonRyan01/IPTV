namespace Iptv.Core.Models;

public class UserInfo
{
    public long UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => string.Concat(FirstName, ' ', LastName);
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Address Address { get; set; } = new();
    public bool IsAdmin { get; set; } = false;
}