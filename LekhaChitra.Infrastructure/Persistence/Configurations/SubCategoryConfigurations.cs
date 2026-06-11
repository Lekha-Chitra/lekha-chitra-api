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
    public class SubCategoryConfigurations : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
           builder.ToTable("SubCategories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SubCategoryName)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(x => x.CategoryId)
                         .IsRequired();

            builder.HasIndex(x => x.CategoryId);

        }
    }
}
