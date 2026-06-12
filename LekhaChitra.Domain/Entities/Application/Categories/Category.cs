using LekhaChitra.Domain.Entities.Base;
using LekhaChitra.Domain.Interface.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Domain.Entities.Application.Categories
{
    public class Category : Entity<int>, ITenantEntity
    {
        public string Name { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; }
                 = new List<SubCategory>();
        public Guid TenantId { get; set; }
    }
}
