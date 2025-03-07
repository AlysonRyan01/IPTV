using Iptv.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iptv.Api.Data.Mappings;

public class TvBoxMap : IEntityTypeConfiguration<TvBox>
{
    public void Configure(EntityTypeBuilder<TvBox> builder)
    {
        builder.ToTable("TvBox");
        
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(t => t.Brand)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(t => t.Quantity)
            .IsRequired();
        
        builder.Property(t => t.Amount)
            .HasColumnType("MONEY")
            .IsRequired();
    }
}