using LekhaChitra.Domain.Entities.Base;
using LekhaChitra.Domain.Interface.Entity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Domain.Entities.Application.Transactions
{
    public class Payment : Entity<Guid> , IDateAudited, ITenantEntity
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }  
        public string PaymentType { get; set; }
        public Guid TenantId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? AddedDate { get; set; }
        public ClientTransaction Transaction { get; set; } 

    }
}
