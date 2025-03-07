using System.Reflection;
using Iptv.Api.Models;
using Iptv.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Iptv.Api.Data;

public class IptvDbContext(DbContextOptions<IptvDbContext> options)
    : IdentityDbContext<User,
        IdentityRole<long>,
        long,
        IdentityUserClaim<long>,
        IdentityUserRole<long>,
        IdentityUserLogin<long>,
        IdentityRoleClaim<long>,
        IdentityUserToken<long>>(options)
{
    public DbSet<Address> Categories { get; set; } = null!;
    public DbSet<Order> Transactions { get; set; } = null!;
    public DbSet<TvBox> Vouchers { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}