using LekhaChitra.Application.Constants.Enums;
using LekhaChitra.Application.Helpers.TenantService;
using LekhaChitra.Application.Interfaces.Data;
using LekhaChitra.Application.Interfaces.TransactionService;
using LekhaChitra.Application.Response;
using LekhaChitra.Application.Services.TransactionService;
using LekhaChitra.Domain.Entities.Application.Clients;
using LekhaChitra.Domain.Entities.Application.Transactions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Transaction.Command.DeleteTransaction
{
    public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, ServiceResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ITenantService _tenantService;
        private readonly ITransactionService _transactionService;

        public DeleteTransactionCommandHandler(
            IUnitOfWork uow,
            ITenantService tenantService,
            ITransactionService transactionService)
        {
            _uow = uow;
            _tenantService = tenantService;
            _transactionService = transactionService;
        }
        public async Task<ServiceResponse> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _tenantService.GetTenantId;
            if (tenantId == Guid.Empty)
            {
                return ServiceResponse.Unauthorized("Access denied.");
            }
            var transaction = await _uow.AsyncRepositories<ClientTransaction>()
                                        .GetQueryable()
                                        .Where(x => x.Id == request.TransactionId
                                                && x.TenantId == tenantId
                                                && x.IsDeleted == false)
                                        .Include(x => x.Payment)
                                        .FirstOrDefaultAsync(cancellationToken);
            if(transaction is null)
            {
                return ServiceResponse.NotFound("No transaction found.");
            }
            if (transaction.TransactionType == TransactionTypeEnums.Internal.ToString())
            {
              var revertTransaction = await _transactionService.RevertInternalTransaction
                                                                    (transaction.TO,
                                                                    transaction.From,
                                                                    transaction.Payment.Amount);
              if(!revertTransaction)
              {
                 return ServiceResponse.BadRequest();
              }
            }
            if (transaction.TransactionType == TransactionTypeEnums.External.ToString())
            {
                var revertTransaction = await _transactionService.RevertExternalTransaction
                                                                        (transaction.ClientId,
                                                                        transaction.Payment.PaymentType,
                                                                        transaction.Payment.Amount);
                if (!revertTransaction)
                {
                    return ServiceResponse.BadRequest();
                }
            }
            transaction.IsDeleted = true;
            transaction.DeletedDate = DateTime.UtcNow;
            await _uow.AsyncRepositories<ClientTransaction>().UpdateAsync(transaction);
            await _uow.Save();
            return ServiceResponse.Success("Transaction Deleted Successfully.");

        }
      
    }
}
