using LekhaChitra.Domain.Entities.Application.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Infrastructure.Persistence.Configurations
{
    public class ClientConfigurations : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            // Table name (optional but recommended)
            builder.ToTable("Clients");

            // Primary key
            builder.HasKey(x => x.Id);

            // Decimal precision
            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.EstimatedBudget)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(200);

            builder.Property(x => x.Status)
                    .HasMaxLength(50);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.Remarks)
                .HasMaxLength(2000);

            builder.HasOne(x => x.SubCategory)
           .WithMany(x => x.Clients)
           .HasForeignKey(x => x.SubCategoryId)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.SubCategoryId);


        }
    }
}
