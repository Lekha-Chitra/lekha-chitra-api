using LekhaChitra.Domain.Entities.Application.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Infrastructure.Persistence.Configurations
{
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // for table name
            builder.ToTable("Categories");
            builder.HasKey(c => c.Id);

            // column properties
            builder.Property(x => x.Name)
                     .IsRequired()
                     .HasMaxLength(100);

            // 1:M relationship with SubCategory
            builder.HasMany(x => x.SubCategories)
                     .WithOne(x => x.Category)
                     .HasForeignKey(x => x.CategoryId)
                     .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
