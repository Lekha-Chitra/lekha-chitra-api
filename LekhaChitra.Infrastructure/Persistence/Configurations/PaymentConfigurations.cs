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
    public class PaymentConfigurations : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount)
                   .HasPrecision(18, 2);


            builder.Property(x => x.PaymentMethod)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.PaymentType)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.TenantId)
                   .IsRequired();

            builder.Property(x => x.AddedDate);

            builder.Property(x => x.ModifiedDate);

            builder.HasIndex(x => x.TenantId);
            builder.HasIndex(x => x.AddedDate);

            builder.HasOne(x => x.Transaction)
                   .WithOne(x => x.Payment)
                   .HasForeignKey<ClientTransaction>(x => x.PaymentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
