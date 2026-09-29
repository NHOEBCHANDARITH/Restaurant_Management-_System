using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant_Management__System.Models;

namespace Restaurant_Management__System.Configurations
{
    public class MenuItemConfiguration :
        IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(
            EntityTypeBuilder<MenuItem> builder)
        {
            builder.ToTable("MenuItems");

            builder.HasKey(x => x.CategoryId);

            builder.Property(x => x.ItemId)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.Price)
                .HasPrecision(18, 2);

            builder.Property(x => x.ImageUrl)
                .HasMaxLength(500);
        }
    }
}
