using LekhaChitra.Domain.Interface.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Domain.Entities.Base
{
    public abstract class SoftDeleteEntity<TPrimaryKey> : Entity<TPrimaryKey>, ISoftDelete
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }
}
