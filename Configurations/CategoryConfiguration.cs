using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant_Management__System.Models;

namespace Restaurant_Management__System.Configurations
{
    public class CategoryConfiguration :
          IEntityTypeConfiguration<Category>
    {
        public void Configure(
            EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(x => x.CategoryId);

            builder.Property(x => x.CategoryName)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
