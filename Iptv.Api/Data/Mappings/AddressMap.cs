using Iptv.Api.Models;
using Iptv.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iptv.Api.Data.Mappings;

public class AddressMap : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {

        builder.ToTable("Address");
        
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();
        
        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<Address>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(a => a.Street)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(a => a.Number)
            .HasMaxLength(10)
            .IsRequired(false);

        builder.Property(a => a.Neighborhood)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(a => a.City)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(a => a.State)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(a => a.ZipCode)
            .HasMaxLength(20)
            .IsRequired(false);
        
    }
}