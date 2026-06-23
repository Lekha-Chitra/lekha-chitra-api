using LekhaChitra.Application.Constants.Enums;
using LekhaChitra.Application.Interfaces.Data;
using LekhaChitra.Application.Interfaces.TransactionService;
using LekhaChitra.Domain.Entities.Application.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Services.TransactionService
{
    public class TransactionService: ITransactionService
    {
        private readonly IUnitOfWork _uow;

        public TransactionService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<bool> RevertInternalTransaction(Guid to, Guid from, decimal amount)
        {
            var toClient = await GetClient(to);
            var fromClient = await GetClient(from);
            if (fromClient is null || toClient is null)
            {
                return false;
            }
            toClient.Balance = toClient.Balance - amount;
            fromClient.Balance = fromClient.Balance + amount;

            var statusUpdatedToClient = await RevertClientStatus(toClient);
            var statusUpdatedFromClient = await RevertClientStatus(fromClient);

            await _uow.AsyncRepositories<Client>().UpdateAsync(statusUpdatedToClient);
            await _uow.AsyncRepositories<Client>().UpdateAsync(statusUpdatedFromClient);
            return true;
        }

        public async Task<bool> RevertExternalTransaction(Guid clientId, string paymentType, decimal amount)
        {
            var client = await GetClient(clientId);

            if (client is null)
            {
                return false;
            }

            if (paymentType == PaymentTypeEnums.Credit.ToString())
            {
                client.Balance = client.Balance - amount;
            }
            if (paymentType == PaymentTypeEnums.Debit.ToString())
            {
                client.Balance = client.Balance + amount;
            }

            var statusUpdatedClient = await RevertClientStatus(client);
            await _uow.AsyncRepositories<Client>().UpdateAsync(statusUpdatedClient);
            return true;
        }

        public async Task<Client> GetClient(Guid clientId)
        {
            var client = await _uow.AsyncRepositories<Client>().GetByPrimaryKey(clientId);
            if (client is null)
            {
                return null;
            }
            return client;
        }

        public async Task<Client> RevertClientStatus(Client client)
        {
            if (client.Balance < 0)
            { client.Status = ClientStatusEnums.Debit.ToString(); }
            else
            {
                client.Status = ClientStatusEnums.Credit.ToString();
            }
            return client;
        }

    }
}
