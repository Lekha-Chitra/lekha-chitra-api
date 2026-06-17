using LekhaChitra.Domain.Entities.Application.Categories;
using LekhaChitra.Domain.Entities.Base;
using LekhaChitra.Domain.Interface.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Domain.Entities.Application.Clients
{
    public class Client : Entity<Guid>, IFullAudited, ITenantEntity
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public decimal EstimatedBudget { get; set; }
        public int Quantity { get; set; }
        public Guid SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
        public string? AddedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? AddedDate { get; set; }
        public Guid TenantId { get; set; }
    }
}
