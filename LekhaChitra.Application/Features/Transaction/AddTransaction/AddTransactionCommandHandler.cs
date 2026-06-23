using Azure.Core;
using LekhaChitra.Application.Constants.Enums;
using LekhaChitra.Application.Helpers.TenantService;
using LekhaChitra.Application.Interfaces.Data;
using LekhaChitra.Application.Response;
using LekhaChitra.Domain.Entities.Application.Clients;
using LekhaChitra.Domain.Entities.Application.Transactions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LekhaChitra.Application.Features.Transaction.AddTransaction
{
    public class AddTransactionCommandHandler : IRequestHandler<AddTransactionCommand, ServiceResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ITenantService _tenantService;

        public AddTransactionCommandHandler(
            IUnitOfWork uow,
            ITenantService tenantService)
        {
            _uow = uow;
            _tenantService = tenantService;
        }
        public async Task<ServiceResponse> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _tenantService.GetTenantId;
            if (tenantId == Guid.Empty)
            {
                return ServiceResponse.Unauthorized("Access denied.");
            }

            if (!Enum.TryParse<TransactionTypeEnums>(
                        request.AddTransaction.TransactionType,
                        true,
                        out var transactionType))
            {
                return ServiceResponse.BadRequest("Invalid transaction type.");
            }

            if (!Enum.TryParse<PaymentTypeEnums>(
                            request.AddTransaction.PaymentType,
                            true,
                            out var paymentType))
            {
                return ServiceResponse.BadRequest("Invalid payment type.");
            }

            if (!Enum.TryParse<PaymentMethodEnums>(
                            request.AddTransaction.PaymentMethod,
                            true,
                            out var paymentMethod))
            {
                return ServiceResponse.BadRequest("Invalid payment method.");
            }

            var payment = new Payment()
            {
                Id = Guid.NewGuid(),
                Amount = request.AddTransaction.Amount,
                PaymentMethod = paymentMethod.ToString(),
                PaymentType = paymentType.ToString(),
                TenantId = tenantId,

            };
            var paymentResult = await _uow.AsyncRepositories<Payment>().AddAsync(payment);
            if (paymentResult is null)
            {
                return ServiceResponse.BadRequest("Failed to create payment.");
            }
            else
            {
                var transactionRequest = new ClientTransaction()
                {
                    Id = Guid.NewGuid(),
                    TransactionType = transactionType.ToString(),
                    Remarks = request.AddTransaction.Remarks,
                    TenantId = tenantId,
                    PaymentId = paymentResult.Id,
                };
            if (transactionType == TransactionTypeEnums.External)
            {
                var clientExists = await GetClient(request.AddTransaction.ClientName, tenantId);
                if (clientExists is null)
                {
                    return ServiceResponse.NotFound("Client not found.");
                }
                transactionRequest.ClientId = clientExists.Id;
                await UpdateClientExternalTransaction(clientExists,
                                            paymentResult.PaymentType,
                                            request.AddTransaction.Amount);
            }
            if (transactionType == TransactionTypeEnums.Internal)
            {
                var toClientExists = await GetClient(request.AddTransaction.TO, tenantId);

                if (toClientExists is null)
                {
                    return ServiceResponse.NotFound("TO Client not found.");
                }
                var fromClientExists = await GetClient(request.AddTransaction.From, tenantId);
                if (fromClientExists is null)
                {
                    return ServiceResponse.NotFound("From Client not found.");
                }
                transactionRequest.From = fromClientExists.Id;
                transactionRequest.TO = toClientExists.Id;
                // transactionRequest.ClientId = null;
                await UpdateClientInternalTransaction(toClientExists,
                                                    fromClientExists,
                                                    paymentResult.PaymentType,
                                                    request.AddTransaction.Amount);
            }
                var transactionResult = await _uow.AsyncRepositories<ClientTransaction>()
                                                    .AddAsync(transactionRequest);
                if (transactionResult is null)
                {
                    return ServiceResponse.BadRequest("Failed to create transaction.");
                }
                else
                {
                    //payment.Status = PaymentStatusEnum.Completed.ToString();
                    //await _uow.AsyncRepositories<Payment>().UpdateAsync(payment);
                    await _uow.Save();
                    return ServiceResponse.Success("Transaction created successfully.");
                }
            }
            
        }

        public async Task<Client> GetClient(string clientName, Guid tenantId)
        {
            var clientExists = await _uow.AsyncRepositories<Client>()
                                            .GetQueryable()
                                            .AsNoTracking()
                                            .Where(x => x.Name == clientName
                                                      && x.TenantId == tenantId)
                                            .FirstOrDefaultAsync();
            if (clientExists is null)
            {
                return null;
            }
            else
            {
                return clientExists;
            }

        }

        public async Task UpdateClientInternalTransaction(Client to, Client From, string paymentType, decimal amount)
        {
            if (to is not null && From is not null)
            {
                to.Balance += amount;
                From.Balance -= amount;

               var toResult = await UpdateClientStatus(to);
               var fromResult = await UpdateClientStatus(From);

                if (toResult is not null && fromResult is not null)
                {
                    await _uow.AsyncRepositories<Client>().UpdateAsync(toResult);
                    await _uow.AsyncRepositories<Client>().UpdateAsync(fromResult);
                }
                
            }
        }
        public async Task UpdateClientExternalTransaction(Client client, string paymentType, decimal amount)
        {
            if (client is not null)
            {
                if (paymentType == PaymentTypeEnums.Credit.ToString())
                {
                  client.Balance += amount; 
                }

                if (paymentType == PaymentTypeEnums.Debit.ToString())
                {
                    client.Balance -= amount;
                }
                var result = await UpdateClientStatus(client);
                if (result is not null)
                {
                    await _uow.AsyncRepositories<Client>().UpdateAsync(result);
                }
            }
        }

        public async Task<Client> UpdateClientStatus(Client client)
        { 
            if(client.Balance < 0)
            {
                client.Status = ClientStatusEnums.Debit.ToString();
            }
            else
            {
                client.Status = ClientStatusEnums.Credit.ToString();
            }
            return client;
          
        }
    }
}