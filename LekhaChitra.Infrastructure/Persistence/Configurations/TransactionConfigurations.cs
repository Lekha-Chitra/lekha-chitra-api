using LekhaChitra.Domain.Entities.Application.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Infrastructure.Persistence.Configurations
{
    public class TransactionConfigurations : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TO)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.From)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.TransactionType)
                   .IsRequired()
                   .HasMaxLength(30);

            builder.Property(x => x.Remarks)
                   .HasMaxLength(1000);

            builder.Property(x => x.TenantId)
                   .IsRequired();

            builder.Property(x => x.PaymentId)
                   .IsRequired();

            builder.HasIndex(x => x.TenantId);

            builder.HasIndex(x => x.PaymentId)
                   .IsUnique();

            builder.HasOne(x => x.Payment)
                   .WithOne(x => x.Transaction)
                   .HasForeignKey<Transaction>(x => x.PaymentId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
