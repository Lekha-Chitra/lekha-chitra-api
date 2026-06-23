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

namespace LekhaChitra.Application.Features.Transaction.Query.FilterTransaction
{
    public class FilterTransactionQueryHandler : IRequestHandler<FilterTransactionQuery, ServiceResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ITenantService _tenantService;

        public FilterTransactionQueryHandler(
            IUnitOfWork uow,
            ITenantService tenantService)
        {
            _uow = uow;
            _tenantService = tenantService;
        }
        public async Task<ServiceResponse> Handle(FilterTransactionQuery request, CancellationToken cancellationToken)
        {

            var filter = request.Filter;
            var tenantId = _tenantService.GetTenantId;
            if (tenantId == Guid.Empty)
            {
                return ServiceResponse<object>.Unauthorized("Access denied.");
            }

            var pageNumber = filter.PageNumber < 1 ? 1 : filter.PageNumber;
            var pageSize = filter.PageSize < 1 ? 10 : filter.PageSize;
            pageSize = pageSize > 100 ? 100 : pageSize;

            var query = _uow.AsyncRepositories<ClientTransaction>()
                            .GetQueryable()
                            .AsNoTracking()
                            .Where(x => x.TenantId == tenantId &&
                                        !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(filter.TransactionType))
            {
                query = query.Where(x => x.TransactionType == filter.TransactionType);
            }

            if (!string.IsNullOrWhiteSpace(filter.PaymentType))
            {
                query = query.Where(x => x.Payment.PaymentType == filter.PaymentType);
            }

            if (!string.IsNullOrWhiteSpace(filter.PaymentMethod))
            {
                query = query.Where(x => x.Payment.PaymentMethod == filter.PaymentMethod);
            }

            if (filter.ClientId.HasValue)
            {
                query = query.Where(x => x.ClientId == filter.ClientId.Value);
            }

            if (filter.ToId.HasValue)
            {
                query = query.Where(x => x.TO == filter.ToId.Value);
            }

            if (filter.FromId.HasValue)
            {
                query = query.Where(x => x.From == filter.FromId.Value);
            }

            if (filter.MinAmount.HasValue)
            {
                query = query.Where(x => x.Payment.Amount >= filter.MinAmount.Value);
            }

            if (filter.MaxAmount.HasValue)
            {
                query = query.Where(x => x.Payment.Amount <= filter.MaxAmount.Value);
            }

            if (filter.FromDate.HasValue)
            {
                query = query.Where(x => x.AddedDate >= filter.FromDate.Value);
            }

            if (filter.ToDate.HasValue)
            {
                query = query.Where(x => x.AddedDate <= filter.ToDate.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(x =>
                    x.Remarks.Contains(filter.Search) ||
                    x.TransactionType.Contains(filter.Search));
            }
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
                                               ModifiedDate = x.ModifiedDate
                                           })
                                           .ToListAsync(cancellationToken);

            var clientIds = transactions.SelectMany(x => new[] { x.ToId, x.FromId, x.ClientId })
                                        .Where(x => x != Guid.Empty)
                                        .Distinct()
                                        .ToList();

            var clients = await _uow.AsyncRepositories<Client>()
                .GetQueryable()
                .AsNoTracking()
                .Where(x => clientIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, x => x.Name, cancellationToken);

            foreach (var transaction in transactions)
            {
                transaction.To = clients.GetValueOrDefault(transaction.ToId);
                transaction.From = clients.GetValueOrDefault(transaction.FromId);
                transaction.Client = clients.GetValueOrDefault(transaction.ClientId);
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
