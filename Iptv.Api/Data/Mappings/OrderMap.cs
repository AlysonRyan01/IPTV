using Iptv.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iptv.Api.Data.Mappings;

public class OrderMap : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");
        
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.Id)
            .ValueGeneratedOnAdd();
        
        builder.HasOne(o => o.Address)
            .WithMany(a => a.Orders)
            .HasForeignKey(o => o.AddressId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        builder.Property(o => o.Number)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(o => o.ExternalReference)
            .HasMaxLength(100)
            .IsRequired(false);
        
        builder.Property(o => o.TvBoxId)
            .IsRequired();
        
        builder.HasOne(o => o.TvBox)
            .WithMany(t => t.Orders)
            .HasForeignKey(o => o.TvBoxId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(o => o.Quantity)
            .HasColumnType("INT")
            .IsRequired();
        
        builder.Property(o => o.CreatedAt)
            .HasColumnType("DATETIME2")
            .IsRequired();
        
        builder.Property(o => o.UpdatedAt)
            .HasColumnType("DATETIME2")
            .IsRequired();
        
        builder.Property(o => o.ShippingCost)
            .HasColumnType("MONEY")
            .IsRequired();
        
        builder.Property(o => o.PaymentGateway)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(o => o.OrderStatus)
            .HasConversion<int>()
            .IsRequired();
        
        builder.Ignore(o => o.Amount);
    }
}