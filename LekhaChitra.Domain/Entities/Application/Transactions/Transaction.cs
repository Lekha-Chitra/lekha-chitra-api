using LekhaChitra.Domain.Entities.Base;
using LekhaChitra.Domain.Interface.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Domain.Entities.Application.Transactions
{
    public class Transaction : Entity<Guid>, ITenantEntity, IDateAudited, ISoftDelete
    {
        public Guid TO { get; set; }
        public Guid From { get; set; }
        public string TransactionType { get; set; }
        public string Remarks { get; set; }
        public Guid ClientId { get; set; }
        public Guid TenantId { get; set; }
        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; }
        public bool IsDeleted { get; set; } 
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? AddedDate { get; set; }
    }
}
