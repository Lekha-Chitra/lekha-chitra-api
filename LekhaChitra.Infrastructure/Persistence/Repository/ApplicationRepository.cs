using LekhaChitra.Application.Interfaces.Data;
using LekhaChitra.Infrastructure.Persistence.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Infrastructure.Persistence.Repository
{
    public class ApplicationRepository<T> : GenericRepository<T>, IApplicationRepository<T>
        where T : class
    {
        public ApplicationRepository(ApplicationDbContext context)
            : base(context) { }
    }
}
