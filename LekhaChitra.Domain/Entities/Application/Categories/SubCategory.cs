using LekhaChitra.Domain.Entities.Base;
using LekhaChitra.Domain.Interface.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Domain.Entities.Application.Categories
{
    public class SubCategory : Entity<Guid>, ITenantEntity
    {
        public string SubCategoryName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public Guid TenantId { get; set; }
    }
}
