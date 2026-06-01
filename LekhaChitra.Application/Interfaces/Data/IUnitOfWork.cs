using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Interfaces.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> AsyncRepositories<T>()
            where T : class;
        Task<int> Save();
    }
}
