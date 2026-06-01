using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Domain.Interface.Entity
{
    public interface IDateAudited : IHasCreationDate
    {
      
        DateTime? ModifiedDate { get; set; }
    }
}
