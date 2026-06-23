using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Interfaces.TransactionService
{
    public interface ITransactionService
    {
        Task<bool> RevertInternalTransaction(Guid to, Guid from, decimal amount);
        Task<bool> RevertExternalTransaction(Guid clientId, string paymentType, decimal amount);
    }
}
