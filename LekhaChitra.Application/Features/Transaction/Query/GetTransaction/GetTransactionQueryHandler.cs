using LekhaChitra.Application.DTO.Transactions;
using LekhaChitra.Application.Helpers.TenantService;
using LekhaChitra.Application.Interfaces.Data;
using LekhaChitra.Application.Response;
using LekhaChitra.Domain.Entities.Application.Clients;
using LekhaChitra.Domain.Entities.Application.Transactions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LekhaChitra.Application.Features.Transaction.Query.GetTransaction
{
    public class GetTransactionQueryHandler : IRequestHandler<GetTransactionQuery, ServiceResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ITenantService _tenantService;

        public GetTransactionQueryHandler(IUnitOfWork uow,
            ITenantService tenantService)
        {
            _uow = uow;
            _tenantService = tenantService;
        }
        public async Task<ServiceResponse> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
        {

            var tenantId = _tenantService.GetTenantId;
            if (tenantId == Guid.Empty)
            {
                return ServiceResponse<object>.Unauthorized("Access denied.");
            }
            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 10 : request.PageSize;
            pageSize = pageSize > 100 ? 100 : pageSize;
            var query = _uow.AsyncRepositories<ClientTransaction>()
                                            .GetQueryable()
                                            .AsNoTracking()
                                            .Where(x => x.TenantId == tenantId
                                                    && x.IsDeleted == false);

            var totalRecords = await query.CountAsync(cancellationToken);
            var transactions = await query.OrderByDescending(x => x.AddedDate)
                                           .Skip((pageNumber - 1) * pageSize)
                                           .Take(pageSize)
                                           .Select(x => new GetTransactionDTO
                                           { 
                                                TransactionId = x.Id,
                                                TransactionType = x.TransactionType,
                                                PaymentType = x.Payment.PaymentType,
                                                ToId = x.TO,
                                                FromId = x.From,
                                                ClientId = x.ClientId,
                                                Amount = x.Payment.Amount,
                                                PaymentMethod = x.Payment.PaymentMethod,
                                                Remarks = x.Remarks,
                                                AddedDate = x.AddedDate,
                                                ModifiedDate = x.ModifiedDate,
                                           })
                                            .ToListAsync();

            if (!transactions.Any())
            {
                return ServiceResponse.NotFound();
            }
            var clientIds = transactions
           .SelectMany(x => new[] { x.ToId, x.FromId, x.ClientId })
           .Distinct()
           .ToList();
            var clients = await _uow.AsyncRepositories<Client>()
                                    .GetQueryable()
                                     .AsNoTracking()
                                     .Where(x => clientIds.Contains(x.Id))
                                     .ToListAsync(cancellationToken);
            var clientDict = clients.ToDictionary(x => x.Id, x => x.Name);

            foreach (var transaction in transactions)
            {
                transaction.To = clientDict.GetValueOrDefault(transaction.ToId);
                transaction.From = clientDict.GetValueOrDefault(transaction.FromId);
                transaction.Client = clientDict.GetValueOrDefault(transaction.ClientId);
            }

          return ServiceResponse<object>.Success(new
            {
                Transactions = transactions,
                Pagination = new
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = totalRecords,
                    TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                    HasNext = pageNumber * pageSize < totalRecords,
                    HasPrevious = pageNumber > 1
                }
            });

        }

      
    }
}
