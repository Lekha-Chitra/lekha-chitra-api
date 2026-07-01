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
    public class TransactionConfigurations : IEntityTypeConfiguration<ClientTransaction>
    {
        public void Configure(EntityTypeBuilder<ClientTransaction> builder)
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

            builder.HasIndex(x => x.AddedDate);

            builder.HasOne(x => x.Payment)
                   .WithOne(x => x.Transaction)
                   .HasForeignKey<ClientTransaction>(x => x.PaymentId)
                   .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(x => x.ToClient)
            //        .WithMany()
            //        .HasForeignKey(x => x.TO)
            //        .OnDelete(DeleteBehavior.NoAction);

            //builder.HasOne(x => x.FromClient)
            //        .WithMany()
            //        .HasForeignKey(x => x.From)
            //        .OnDelete(DeleteBehavior.NoAction);

            //builder.HasOne(x => x.Client)
            //        .WithMany()
            //        .HasForeignKey(x => x.ClientId)
            //        .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
