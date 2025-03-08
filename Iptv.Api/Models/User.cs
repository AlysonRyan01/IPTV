using Iptv.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Iptv.Api.Models;

public class User : IdentityUser<long>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Address? Address { get; set; }
}
