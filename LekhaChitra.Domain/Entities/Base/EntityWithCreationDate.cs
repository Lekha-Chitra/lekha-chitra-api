using LekhaChitra.Domain.Interface.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Domain.Entities.Base
{
    public abstract class EntityWithCreationDate<TPrimaryKey> : Entity<TPrimaryKey>, IHasCreationDate
    {
        public DateTime? AddedDate { get; set; }
    }
}
